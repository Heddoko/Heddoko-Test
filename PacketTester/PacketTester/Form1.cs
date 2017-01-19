using System;
using System.Collections.Concurrent;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ProtoBuf;
using heddoko;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace PacketTester
{
    public partial class mainForm : Form
    {
        private bool openSerialPort = false;
        private bool openRobotArmPort = false;
        private bool processDebugThreadEnabled = false;
        public ConcurrentQueue<string> debugMessageQueue;
        private bool processPacketQueueEnabled = false; 
        public ConcurrentQueue<RawPacket> packetQueue;

        private bool streamDataEnabled = false;
        private long startTime = 0;
        private UInt16 graphIndex = 0;
        private const UInt16 graphMaxSize = 100;

        private bool streamDataToChartEnabled = false;
        private bool saveSensorDataToFile = false;
        private string saveSensorDataFilename = "";
        private FileStream outputFileStream;

        //sensor board parameters
        bool setSensorIdFlag = false; 

        enum OutputType { legacyFrame, QuaternionFrame, protoBufFrame, individualSensorFiles};
        OutputType streamOutputType = OutputType.protoBufFrame; 
        string dataStreamFilePath = "";
        int selectedDataType = 0;

        public DataTable uarmTable;
        public UInt16 uarmStreamLoopCount = 1;
        public UInt32 uarmStreamLineDelay = 100;
        public bool requestSensorFrame = false;
        public bool enableArmRecording = false;
        public bool togglePortButton = false;
        public bool toggleRecButton = false;
        private bool processRobotArmQueueEnabled = true;
        public ConcurrentQueue<String> robotArmQueue;
        public String robotArmInputStream;
        public bool robotArmWarningEnable = true;
        public bool robotArmOutFileIsOpen = false;
        FileStream robotArmOutFile;
        public int robotArmLoopCount = 0;

        // Data board emulator part
        public bool toggleDbPort = false;
        public bool dbPortOpen = false;
        public int dbDataRate = 20;
        public Int32 setSensorMask = 0, rxSensorMask = 0;
        public bool dbEnableSen0 = false, dbEnableSen1 = false, dbEnableSen2 = false, dbEnableSen3 = false, dbEnableSen4 = false;
        public bool dbEnableSen5 = false, dbEnableSen6 = false, dbEnableSen7 = false, dbEnableSen8 = false;
        bool dbDataMonitorEnable = false, dbDataReceiveEnable = false;
        private bool dataMonitorQueueEnabled = false;
        public ConcurrentQueue<RawPacket> dataMonitorQueue;
        public UInt32 sensorDataRate = 0, sensorAvgRate = 0;
        public UInt32 curSensorFrameTick = 0, preSensorFrameTick = 0;
        public int dbSensorFrameCount = 0, debugCount = 0;
        public ConcurrentQueue<byte> dbDataReceiveQueue;
        private bool dbDataReceiveQueueEnabled = false;
        private int dbPacketErrorCount = 0;

        // Power board emulator part
        public bool togglePbPort = false, pbPortOpen = false;
        private int pbDetectedSensorMask = 0;
        public bool pbProcessDataEnable = false;
        public ConcurrentQueue<byte> pbDataReceiveQueue;
        private bool pbDataReceiveQueueEnabled = false;
        
        public struct StatusMessage
        {
            public byte chargeLevel;   //battery percentage     
            public byte chargerState; //BatteryLow = 0;   BatteryNominal = 1;  BatteryFull = 2; Charging = 3;
            public byte usbCommState; //0 = no comm detected, 1 = comm detected
            public byte jackDetectState; //mask indicating which jacks are connected.
            public byte streamState; //0 = Idle, 1 = Streaming, 2 = Error
            public UInt32 sensorMask; //mask of which sensors have been detected.
        };
        public struct subProcessorConfig
        {
            public byte dataRate;
            public UInt32 sensorMask;
        };

        public mainForm()
        {
            InitializeComponent();
        }

        public void processDebugMessagesThread()
        {
            while (processDebugThreadEnabled)
            {
                string line;
                if (debugMessageQueue.Count > 0)
                {
                    if (debugMessageQueue.TryDequeue(out line))
                    {
                        this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText(line)));
                    }
                }
            }
        }
        public void saveSensorFrameToFile(ImuFrame frame, ref FileStream outputFile, string interval)
        {
            StringBuilder strBuilder = new StringBuilder();
            //so we need to create a frame compatible with the old system. 
            //0000246665,03ff,A2D1;9707; C11B,311B; 5D14; C6C9,7713; B2E2; 73FF,9D3F; AD1A; 07A5,3B2A; E7D7; 125E,7331; 4AFA; 8B42,2132; 97F8; 4EA1,1E3C; 30FD; C8D3,B337; 28FD; BCAA,1234; BBBB; CCCC; DDDD; EEEE, 
            long timeStamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - startTime;
            strBuilder.Append(timeStamp.ToString("D10") + "," + interval + ",");
            strBuilder.Append(frame.getCsvString());
            try
            {
                if (outputFile.CanWrite)
                {
                    outputFile.Write(ASCIIEncoding.ASCII.GetBytes(strBuilder.ToString()), 0, strBuilder.Length);
                    outputFile.Flush();                   
                }
            }
            catch
            {
                debugMessageQueue.Enqueue(String.Format("Failed to write file\r\n"));
                return;
            }
            
        }
        public void streamDataToChartThread()
        {
            startTime = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qx"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qy"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qz"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qw"].Points.Clear()));

            ImuFrame sensorframe = new ImuFrame();
            RawPacket framePacket = new RawPacket();
            bool receivedPacket = false; 
            graphIndex = 0;
            //open file stream

            FileStream outputFile;
            if (saveSensorDataToFile)
            {
                try
                {
                    debugMessageQueue.Enqueue(String.Format("Openning file: {0}\r\n", saveSensorDataFilename));
                    outputFile = File.Open(saveSensorDataFilename, FileMode.Create);
                    string header = "Time(ms),Interval,Qx,Qy,Qz,Qw,Mx,My,Mz,Ax,Ay,Az,Rx,Ry,Rz\r\n";
                    outputFile.Write(ASCIIEncoding.ASCII.GetBytes(header), 0, header.Length);
                }
                catch
                {
                    debugMessageQueue.Enqueue(String.Format("Failed to create file: {0}\r\n", saveSensorDataFilename));
                    return;
                }
                
            }
            else
            {
                outputFile = null;
            }
            //close the other listenning thread for the queue
            processPacketQueueEnabled = false;

            while (streamDataToChartEnabled)
            {
                if (chb_arm_EnableTracing.Checked)      // if sensor precision testing is on
                {
                    if (requestSensorFrame)     //if there is a new request for frame then fetch one
                    {
                        requestSensorFrame = false;
                    }
                    else        // else put the thread to sleep and continue
                    {
                        //Thread.Sleep((int)nud_updateRate.Value);
                        //continue;
                    }
                }
                sendUpdateCommand();
                Thread.Sleep((int)nud_updateRate.Value);
                sendGetFrameCommand((byte)nud_SelectedImu.Value);
                DateTime start = DateTime.Now;
                receivedPacket = false;
                while ((DateTime.Now - start).Milliseconds < 5)
                {
                    if (packetQueue.TryDequeue(out framePacket))
                    {
                        if (sensorframe.ParseImuFrame(framePacket, (byte)nud_SelectedImu.Value))
                        {
                            //we got the packet.                            
                            receivedPacket = true;
                            break;
                        }
                    }
                    Thread.Yield();
                }
                if(receivedPacket)
                {
                    updateChart(sensorframe);
                    updateTable(sensorframe);
                    if (saveSensorDataToFile)
                    {
                        saveSensorFrameToFile(sensorframe,ref outputFile, "0");
                    }
                }
            }
            if(saveSensorDataToFile)
            {
                if(outputFile != null)
                {
                    outputFile.Close(); 
                }
            }
            //start up other listenning thread again
            processPacketQueueEnabled = true;
            Thread packetProcessorThread = new Thread(processPacketThread);
            packetProcessorThread.Start();


        }
        public string createLegacyFrame(ImuFrame[] frameArray, bool[] receivedFlags)
        {
            StringBuilder strBuilder = new StringBuilder();
            //so we need to create a frame compatible with the old system. 
            //0000246665,03ff,A2D1;9707; C11B,311B; 5D14; C6C9,7713; B2E2; 73FF,9D3F; AD1A; 07A5,3B2A; E7D7; 125E,7331; 4AFA; 8B42,2132; 97F8; 4EA1,1E3C; 30FD; C8D3,B337; 28FD; BCAA,1234; BBBB; CCCC; DDDD; EEEE, 
            long timeStamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - startTime;
            UInt16 receivedMask = 0;
            for (int i = 0; i < receivedFlags.Length; i++)
            {
                if(receivedFlags[i])
                {
                    receivedMask += (UInt16)(1 << i);
                }
            }
            strBuilder.Append(timeStamp.ToString("D10") + ",");
            strBuilder.Append(receivedMask.ToString("x4") + ",");
            for (int i = 0; i< frameArray.Length; i++)
            {
                strBuilder.Append(frameArray[i].getAsciiString() + ",");
            }
            for(int i = 0;i < 9 - frameArray.Length; i++)
            {
                strBuilder.Append("0000;0000;0000,");
            }
            strBuilder.Append("1234;BBBB;CCCC;DDDD;EEEE,\r\n");

            return strBuilder.ToString();
        }
        public string createQuaternionFrame(ImuFrame[] frameArray, bool[] receivedFlags)
        {
            StringBuilder strBuilder = new StringBuilder();
            //so we need to create a frame compatible with the old system.             
            long timeStamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - startTime;
            UInt16 receivedMask = 0;
            for (int i = 0; i < receivedFlags.Length; i++)
            {
                if (receivedFlags[i])
                {
                    receivedMask += (UInt16)(1 << i);
                }
            }
            strBuilder.Append(timeStamp.ToString("D10") + ",");
            strBuilder.Append(receivedMask.ToString("x4") + ",");
            for (int i = 0; i < frameArray.Length; i++)
            {
                strBuilder.Append(frameArray[i].getQuaternionString() + ",");
            }
            for (int i = 0; i < 9 - frameArray.Length; i++)
            {
                strBuilder.Append("0000;0000;0000;0000,");
            }
            strBuilder.Append("\r\n");

            return strBuilder.ToString();
        }
        public void createProtoBufFrame(ImuFrame[] frameArray, bool[] receivedFlags, ref FileStream outputFile)
        {
            StringBuilder strBuilder = new StringBuilder();
                      
            long timeStamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - startTime;
            UInt16 receivedMask = 0;
            for (int i = 0; i < receivedFlags.Length; i++)
            {
                if (receivedFlags[i])
                {
                    receivedMask += (UInt16)(1 << i);
                }
            }
            heddoko.Packet packet = new Packet();
            packet.fullDataFrame = new FullDataFrame();
            packet.fullDataFrame.timeStamp = (UInt32)timeStamp;
            for(int i =0; i < frameArray.Length; i++)
            {
                ImuDataFrame frame = new ImuDataFrame();
                frame.imuId = frameArray[i].ImuId;
                frame.Accel_xSpecified = true;
                frame.Accel_x = frameArray[i].Acceleration_x;
                frame.Accel_ySpecified = true;
                frame.Accel_y = frameArray[i].Acceleration_y;
                frame.Accel_zSpecified = true;
                frame.Accel_z = frameArray[i].Acceleration_y;

                frame.Mag_xSpecified = true;
                frame.Mag_x = frameArray[i].Magnetic_x;
                frame.Mag_ySpecified = true;
                frame.Mag_y = frameArray[i].Magnetic_y;
                frame.Mag_zSpecified = true;
                frame.Mag_z = frameArray[i].Magnetic_z;

                frame.Rot_xSpecified = true;
                frame.Rot_x = frameArray[i].Rotation_x;
                frame.Rot_ySpecified = true;
                frame.Rot_y = frameArray[i].Rotation_y;
                frame.Rot_zSpecified = true;
                frame.Rot_z = frameArray[i].Rotation_z;

                frame.quat_x_yawSpecified = true;
                frame.quat_x_yaw = frameArray[i].Quaternion_x;
                frame.quat_y_pitchSpecified = true;
                frame.quat_y_pitch = frameArray[i].Quaternion_y;
                frame.quat_z_rollSpecified = true;
                frame.quat_z_roll = frameArray[i].Quaternion_z;
                frame.quat_wSpecified = true;
                frame.quat_w = frameArray[i].Quaternion_w;

                packet.fullDataFrame.imuDataFrame.Add(frame); 

            }
            packet.type = PacketType.DataFrame;

            //Serializer.To 
            MemoryStream stream = new MemoryStream();
            Serializer.Serialize<Packet>(stream, packet);
            RawPacket rawPacket = new RawPacket();
            ushort rawSize = 0;
            byte[] rawData = rawPacket.createRawPacket(ref rawSize, stream); 
            if(forwardSerialPort.IsOpen)
            {
                forwardSerialPort.Write(rawData, 0, rawSize);
            }
            try
            {
                if (outputFile.CanWrite)
                {
                    outputFile.Write(rawData, 0, rawSize);
                }
            }
            catch
            {
                debugMessageQueue.Enqueue(String.Format("Failed to write file\r\n"));
                return;
            }

        }
        public void createIndividualSensorFrames(ImuFrame[] frameArray, bool[] receivedFlags, ref FileStream[] outputFiles)
        {

            for(int i = 0; i < frameArray.Length; i++)
            {
                if (receivedFlags[i])
                {
                    saveSensorFrameToFile(frameArray[i], ref outputFiles[i], robotArmLoopCount.ToString());
                }
            }

            
        }
        public void streamDataThread()
        {
            startTime = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
            const UInt16 numberOfSensors = 9;
            graphIndex = 0;
            //close the other listenning thread for the queue
            processPacketQueueEnabled = false;
            ImuFrame[] frameArray = new ImuFrame[numberOfSensors];
            for(int i = 0; i < frameArray.Length; i++)
            {
                frameArray[i] = new ImuFrame(); 
            }
            long maxTime = 0;
            long minTime = 100;
            long interval = 0;
            bool[] frameReceived = new bool[numberOfSensors];
            RawPacket framePacket = new RawPacket();
            //open file stream
            FileStream outputFile = null;
            FileStream[] outputFiles = new FileStream[numberOfSensors];
            if (streamOutputType == OutputType.individualSensorFiles)
            {
                string header = "Time(ms),Interval,Qx,Qy,Qz,Qw,Mx,My,Mz,Ax,Ay,Az,Rx,Ry,Rz\r\n";
                
                for (int i =0; i < numberOfSensors; i++)
                {
                    string filepath = dataStreamFilePath + "\\Sensor_" + i.ToString() + ".csv";
                    try
                    {
                        debugMessageQueue.Enqueue(String.Format("Openning file: {0}\r\n", filepath));
                        outputFiles[i] = File.Open(filepath, FileMode.Create);
                        outputFiles[i].Write(ASCIIEncoding.ASCII.GetBytes(header), 0, header.Length);
                    }
                    catch
                    {
                        debugMessageQueue.Enqueue(String.Format("Failed to create file: {0}\r\n", filepath));
                        return;
                    }
                }
            }
            else
            {
                try
                {
                    debugMessageQueue.Enqueue(String.Format("Openning file: {0}\r\n", dataStreamFilePath));
                    outputFile = File.Open(dataStreamFilePath, FileMode.Create);
                }
                catch
                {
                    debugMessageQueue.Enqueue(String.Format("Failed to create file: {0}\r\n", dataStreamFilePath));
                    return;
                }
            }
            while (streamDataEnabled)
            {
                DateTime startGetFrame = DateTime.Now;
                sendUpdateCommand();
                Thread.Sleep(5);
                for(int i = 0; i < numberOfSensors; i++)
                {
                    sendGetFrameCommand((byte)i);
                    frameReceived[i] = false;
                    frameArray[i].ImuId = (byte)i; //set the imu ID to equal the expected frame IMU
                    DateTime start = DateTime.Now;
                    while ((DateTime.Now - start).Milliseconds < 5)
                    {
                        if (packetQueue.TryDequeue(out framePacket))
                        {
                            if (frameArray[i].ParseImuFrame(framePacket, (byte)i))
                            {
                                //we got the packet.
                                frameReceived[i] = true;
                                break;
                            }
                        }
                        Thread.Yield(); 
                    }
                }
                interval = (DateTime.Now - startGetFrame).Milliseconds;
                if(interval < minTime)
                {
                    debugMessageQueue.Enqueue(String.Format("{0}:New min interval found:{1} ms\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond), interval));
                    minTime = interval;
                }
                if(interval > maxTime)
                {
                    debugMessageQueue.Enqueue(String.Format("{0}:New max interval found:{1} ms\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond), interval));
                    maxTime = interval; 
                }
                //create a frame from all the received data

                string frameString = "";

                if (streamOutputType == OutputType.legacyFrame || streamOutputType == OutputType.QuaternionFrame)
                {

                    if (streamOutputType == OutputType.legacyFrame)
                    {
                        frameString = createLegacyFrame(frameArray, frameReceived);
                    }
                    else if (streamOutputType == OutputType.QuaternionFrame)
                    {
                        frameString = createQuaternionFrame(frameArray, frameReceived);
                    }


                    if (forwardSerialPort.IsOpen)
                    {
                        forwardSerialPort.Write(frameString);
                    }
                    try
                    {
                        if (outputFile.CanWrite)
                        {
                            outputFile.Write(ASCIIEncoding.ASCII.GetBytes(frameString), 0, frameString.Length);
                        }
                    }
                    catch
                    {
                        debugMessageQueue.Enqueue(String.Format("Failed to write file\r\n"));
                        return;
                    }
                }
                else if(streamOutputType == OutputType.protoBufFrame)
                {
                    createProtoBufFrame(frameArray, frameReceived, ref outputFile);
                }
                else if(streamOutputType == OutputType.individualSensorFiles)
                {
                    createIndividualSensorFrames(frameArray, frameReceived, ref outputFiles);
                }
                //Thread.Sleep(1); 
            }
            

            if(streamOutputType == OutputType.individualSensorFiles)
            {
                for(int i = 0; i < outputFiles.Length; i++)
                {
                    debugMessageQueue.Enqueue(String.Format("Stream {0} Closed Wrote {1} Bytes\r\n",i, outputFiles[i].Length));
                    outputFiles[i].Close(); 
                }
            }
            else
            {

                if (outputFile != null)
                {
                    debugMessageQueue.Enqueue(String.Format("Stream Closed Wrote {0} Bytes\r\n", outputFile.Length));
                    outputFile.Close();
                }
            }
             
            //start up other listenning thread again
            processPacketQueueEnabled = true;
            Thread packetProcessorThread = new Thread(processPacketThread);
            packetProcessorThread.Start();
        }
        
        enum CommandIds {update=0x11,
            getFrame,
            getFrameResp,
            setupMode,
            buttonPress,
            setImuId,
            setImuIdResp,
            getStatus,
            getStatusResp,
            updateConfigParam,
            getConfigParam,
            getConfigParamResp,
            updateWarmupParam,
            getWarmupParam,
            getWarmupParamResp,
            resetFakeFrame,
            updateFakeFrame,
            eableHpr,
            changeBaud,
            setRates,
            setWarmupParams,
            setRangeParams,
            setConfig,
            getConfig,
            getConfigResp,
            saveToNvm    
        };
        string processSensorSerialNumber(RawPacket packet)
        {
            StringBuilder strBuilder = new StringBuilder();
            if(packet.PayloadSize != 18)
            {
                return "Failed to parse serial Number";
            }
            strBuilder.Append("0x");
            for(int i = 0; i < 16; i++)
            {
                strBuilder.Append(packet.Payload[i + 2].ToString("X2"));
            }
            //set the sensor ID
            if(setSensorIdFlag)
            {
                byte[] setIdBytes = new byte[19];
                setIdBytes[0] = 0x01;
                setIdBytes[1] = (byte)CommandIds.setImuId;
                Buffer.BlockCopy(packet.Payload, 2, setIdBytes, 2, 16);
                setIdBytes[18] = (byte)nud_setId.Value;
                sendPacket(setIdBytes, 19); 
                setSensorIdFlag = false; 

            }
            return strBuilder.ToString();
        }
        string processGetStatusResponse(RawPacket packet)
        {
            StringBuilder strBuilder = new StringBuilder();
            UInt32 imuStatus = BitConverter.ToUInt32(packet.Payload, 3);
            UInt32 receivedPacketError = BitConverter.ToUInt32(packet.Payload,7);
            UInt32 quatError = BitConverter.ToUInt32(packet.Payload, 11);
            UInt32 magError = BitConverter.ToUInt32(packet.Payload, 15);
            UInt32 accelError = BitConverter.ToUInt32(packet.Payload, 19);
            UInt32 gyroError = BitConverter.ToUInt32(packet.Payload, 23);
            strBuilder.Append(string.Format("IMU Status:{0:x}\r\n", imuStatus));
            strBuilder.Append(string.Format("Received Error Count :{0}\r\n", receivedPacketError));
            strBuilder.Append(string.Format("Quat Error:{0}\r\n", quatError));
            strBuilder.Append(string.Format("Mag Rate:{0}\r\n", magError));
            strBuilder.Append(string.Format("Accel Rate:{0}\r\n", accelError));
            strBuilder.Append(string.Format("Gyro Rate:{0}\r\n", gyroError));
            for (int i = 0; i < 5; i++)
            {
                int index = i; 
                if ((imuStatus & (1 << i)) > 0)
                {
                    this.BeginInvoke((MethodInvoker)delegate () { clb_algorithmStatus.SetItemCheckState(index, CheckState.Checked); });
                   
                }
                else
                {
                    this.BeginInvoke((MethodInvoker)delegate () { clb_algorithmStatus.SetItemCheckState(index, CheckState.Unchecked); });                    
                }
            }

            return strBuilder.ToString();
        }
        string processGetConfigParamResponse(RawPacket packet)
        {
            StringBuilder strBuilder = new StringBuilder();
            UInt32 param1 = BitConverter.ToUInt32(packet.Payload, 3);
            UInt32 param2 = BitConverter.ToUInt32(packet.Payload, 7);
            UInt32 param3 = BitConverter.ToUInt32(packet.Payload, 11);
            UInt32 param4 = BitConverter.ToUInt32(packet.Payload, 15);
            strBuilder.Append(string.Format("Param1:{0:x}\r\n", param1));
            strBuilder.Append(string.Format("Param2:{0:x}\r\n", param2));
            strBuilder.Append(string.Format("Param3:{0:x}\r\n", param3));
            strBuilder.Append(string.Format("Param4:{0:x}\r\n", param4));
            return strBuilder.ToString();
        }
        private void updateTable(ImuFrame frame)
        {
            switch (selectedDataType)
            {
                case 0:
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData1.Text = frame.Quaternion_x.ToString(); ; });
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData2.Text = frame.Quaternion_y.ToString(); ; });
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData3.Text = frame.Quaternion_z.ToString(); ; });
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData4.Text = frame.Quaternion_w.ToString(); ; });
                    break;
                case 1:
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData1.Text = frame.Magnetic_x.ToString(); ; });
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData2.Text = frame.Magnetic_y.ToString(); ; });
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData3.Text = frame.Magnetic_z.ToString(); ; });
                    break;
                case 2:
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData1.Text = frame.Acceleration_x.ToString(); ; });
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData2.Text = frame.Acceleration_y.ToString(); ; });
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData3.Text = frame.Acceleration_z.ToString(); ; });
                    break;
                case 3:
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData1.Text = frame.Rotation_x.ToString(); ; });
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData2.Text = frame.Rotation_y.ToString(); ; });
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData3.Text = frame.Rotation_z.ToString(); ; });
                    break;
                default:
                break;
            }
        }

        private void updateChart(ImuFrame frame)
        {
            if(selectedDataType == 0 ) //Quaternions
            {
                try
                {
                    this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qx"].Points.AddY((double)frame.Quaternion_x)));
                    this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qy"].Points.AddY((double)frame.Quaternion_y)));
                    this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qz"].Points.AddY((double)frame.Quaternion_z)));
                    this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qw"].Points.AddY((double)frame.Quaternion_w)));
                }
                catch (OverflowException)
                {
                    tb_Console.AppendText("Overflow exception caused here.\r\n");
                }
            }
            else if(selectedDataType == 1) //Magnetic
            {
                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qx"].Points.AddY(System.Convert.ToDouble(frame.Magnetic_x))));
                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qy"].Points.AddY(System.Convert.ToDouble(frame.Magnetic_y))));
                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qz"].Points.AddY(System.Convert.ToDouble(frame.Magnetic_z))));
            }
            else if (selectedDataType == 2) //Acceleration
            {
                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qx"].Points.AddY(System.Convert.ToDouble(frame.Acceleration_x))));
                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qy"].Points.AddY(System.Convert.ToDouble(frame.Acceleration_y))));
                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qz"].Points.AddY(System.Convert.ToDouble(frame.Acceleration_z))));
            }
            else if (selectedDataType == 3) //Rotation
            {
                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qx"].Points.AddY(System.Convert.ToDouble(frame.Rotation_x))));
                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qy"].Points.AddY(System.Convert.ToDouble(frame.Rotation_y))));
                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qz"].Points.AddY(System.Convert.ToDouble(frame.Rotation_z))));
            }

            if (chrt_dataChart.Series["Qx"].Points.Count > graphMaxSize)
            {
                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qx"].Points.RemoveAt(0)));
                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qy"].Points.RemoveAt(0)));
                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qz"].Points.RemoveAt(0)));
                if (selectedDataType == 0)
                {
                    this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qw"].Points.RemoveAt(0)));
                }

            }

            for (int i = 0; i < 5; i++)
            {
                int index = i;
                if ((frame.frameStatus & (1 << i)) > 0)
                {
                    this.BeginInvoke((MethodInvoker)delegate () { clb_algorithmStatus.SetItemCheckState(index, CheckState.Checked); });

                }
                else
                {
                    this.BeginInvoke((MethodInvoker)delegate () { clb_algorithmStatus.SetItemCheckState(index, CheckState.Unchecked); });
                }
            }


        }
        private void processProtoPacket(Packet packet)
        {
            switch(packet.type)
            {
                case PacketType.AdvertisingPacket:
                    if (packet.firmwareVersionSpecified)
                    {
                        debugMessageQueue.Enqueue("Received Advertising Packet: " +
                            packet.firmwareVersion + " SN: " + packet.serialNumber + " Port: "
                            + packet.configurationPort.ToString() + "\r\n");
                    }
                    else
                    {
                        debugMessageQueue.Enqueue("Error Version not found\r\n");
                    }
                    break;
                case PacketType.StatusResponse:
                    debugMessageQueue.Enqueue("Received Status Response:" +
                        packet.batteryLevel.ToString() + "\r\n");
                    break;
                case PacketType.DataFrame:
                    if(packet.fullDataFrame != null)
                    {
                        debugMessageQueue.Enqueue("Received Data Frame From Timestamp:" +
                        packet.fullDataFrame.timeStamp.ToString() + "\r\n");
                        for(int i = 0; i < packet.fullDataFrame.imuDataFrame.Count; i++)
                        {
                            //do stuff for each frame received. 
                        }
                    }
                    break;
            }

        }
        byte[,] WarmupParameters = new byte[9,140]; //create array to hold warm up data. 
        void processWarmupResponse(RawPacket packet)
        {
            int sensorIndex = (int)packet.Payload[2];
            if (sensorIndex < 9)
            {
                for(int i = 0; i < packet.PayloadSize - 3; i++)
                {
                    WarmupParameters[sensorIndex, i] = packet.Payload[i + 3];
                }                 
            }
        }
        void processGetConfigResponse(RawPacket packet)
        {

            byte algoConfig = 0x00;
            algoConfig = packet.Payload[3];
            
            int index = 0;
            for (int i =0; i<7; i++)
            {
                //skip 3
                if (i == 4)
                {
                    continue;                   
                }
                int sentIndex = index; 
                if ((algoConfig & (1 << i)) > 0)
                {
                    BeginInvoke((MethodInvoker)delegate () { clb_algoConfig.SetItemChecked(sentIndex, true); });
                }
                else
                {
                    BeginInvoke((MethodInvoker)delegate () { clb_algoConfig.SetItemChecked(sentIndex, false); });
                }
                index++; 
            }
            //process warm up
            if(packet.Payload[4] == 1)
            {

                this.BeginInvoke((MethodInvoker)delegate () { clb_algoConfig.SetItemCheckState(6, CheckState.Checked); });
            }
            else
            {
                this.BeginInvoke((MethodInvoker)delegate () { clb_algoConfig.SetItemCheckState(6, CheckState.Unchecked); });
            }
            //process range on boot
            if (packet.Payload[5] == 1)
            {

                this.BeginInvoke((MethodInvoker)delegate () { clb_algoConfig.SetItemCheckState(7, CheckState.Checked); });
            }
            else
            {
                this.BeginInvoke((MethodInvoker)delegate () { clb_algoConfig.SetItemCheckState(7, CheckState.Unchecked); });
            }
            UInt16 accelRange = BitConverter.ToUInt16(packet.Payload, 8);
            UInt16 magRange = BitConverter.ToUInt16(packet.Payload, 6);
            UInt16 gyroRange = BitConverter.ToUInt16(packet.Payload, 10);
            //accelRange = ReverseBytes(accelRange);
            //magRange = ReverseBytes(magRange);
            //gyroRange = ReverseBytes(gyroRange);
            if(!cb_accelRange.Items.Contains(accelRange.ToString()))
            {
                this.BeginInvoke((MethodInvoker)delegate () {
                    cb_accelRange.Items.Add(accelRange.ToString());
                });

            }
            this.BeginInvoke((MethodInvoker)delegate () {
                cb_accelRange.SelectedIndex = cb_accelRange.Items.IndexOf(accelRange.ToString());
            });


            if (!cb_magRange.Items.Contains(magRange.ToString()))
            {
                this.BeginInvoke((MethodInvoker)delegate () {
                    cb_magRange.Items.Add(magRange.ToString());
                });
            }
            this.BeginInvoke((MethodInvoker)delegate () {
                cb_magRange.SelectedIndex = cb_magRange.Items.IndexOf(magRange.ToString());
            });

            if (!cb_gyroRange.Items.Contains(gyroRange.ToString()))
            {
                this.BeginInvoke((MethodInvoker)delegate () {
                    cb_gyroRange.Items.Add(gyroRange.ToString());
                });
            }
            this.BeginInvoke((MethodInvoker)delegate () {
                cb_gyroRange.SelectedIndex = cb_gyroRange.Items.IndexOf(gyroRange.ToString());
            });
            //cb_accelRange.SelectedValue = accelRange.ToString();
            //cb_magRange.SelectedValue = magRange.ToString();
            //cb_gyroRange.SelectedValue = gyroRange.ToString();



        }
        private void processPacket(RawPacket packet)
        {
            //check that the packet comes from an IMU sensor
            if (packet.Payload[0] == 0x03)
            {
                CommandIds commandId = (CommandIds)packet.Payload[1];
                switch(commandId)
                {
                    case CommandIds.buttonPress:
                        debugMessageQueue.Enqueue(String.Format("{0}:Received Button Press for serial#{1}\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond), processSensorSerialNumber(packet)));
                        break;
                    case CommandIds.getFrameResp:
                        //debugMessageQueue.Enqueue(String.Format("{0}:Received Frame\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));
                        ImuFrame frame = new ImuFrame(packet);
                        updateChart(frame);
                        //debugMessageQueue.Enqueue(frame.ToString());
                        break;
                    case CommandIds.setImuIdResp:
                        debugMessageQueue.Enqueue(String.Format("{0}:Received set Imu Resp\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));
                        break;
                    case CommandIds.getConfigParamResp:
                        debugMessageQueue.Enqueue(processGetConfigParamResponse(packet));
                        break;
                    case CommandIds.getStatusResp:
                        debugMessageQueue.Enqueue(processGetStatusResponse(packet));
                        break;
                    case CommandIds.getWarmupParamResp:
                        debugMessageQueue.Enqueue("Received Warmup Parameters\r\n");
                        processWarmupResponse(packet); 
                        break;
                    case CommandIds.getConfigResp:
                        debugMessageQueue.Enqueue("Received Config response 0x" +packet.Payload[3].ToString("x")  +"\r\n");
                        processGetConfigResponse(packet);
                        break;
                    default:
                        break;
                }
            }
            else if(packet.Payload[0] == 0x01) //this is a databoard packet
            {
                switch (packet.Payload[1])
                {
                    case 0x51:
                        debugMessageQueue.Enqueue(String.Format("{0}:Received get Status from data board\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));
                        break;

                    default:
                        debugMessageQueue.Enqueue(String.Format("{0}:Received get Status from data board\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));
                        break;
                }
            }
            else if(packet.Payload[0] == 0x04) //this is a protocol buffer file. 
            {
                Stream stream = new MemoryStream(packet.Payload,1,packet.PayloadSize-1);
                try
                {
                    Packet protoPacket = Serializer.Deserialize<Packet>(stream);
                    processProtoPacket(protoPacket); 
                }
                catch
                {
                    debugMessageQueue.Enqueue("Failed to deserialize packet\r\n");
                }
                
            }
            
        }

        public void processPacketThread()
        {
            while(processPacketQueueEnabled)
            {
                RawPacket packet;
                if(packetQueue.Count > 0)
                {
                    if(packetQueue.TryDequeue(out packet))
                    {
                        processPacket(packet);
                    }
                }
            }
        }
        private void mainForm_Load(object sender, EventArgs e)
        {
            cb_serialPorts.Items.AddRange(SerialPort.GetPortNames());
            cb_forwardPorts.Items.AddRange(SerialPort.GetPortNames());
            cb_robotPort.Items.AddRange(SerialPort.GetPortNames());
            cb_dbComPorts.Items.AddRange(SerialPort.GetPortNames());
            string[] baudrates = { "110", "150", "300", "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200", "230400"
                    , "460800","500000", "921600","1000000","2000000"};
            cb_BaudRate.Items.AddRange(baudrates);
            cb_fpBaudRate.Items.AddRange(baudrates);
            cb_fpBaudRate.SelectedIndex = 14;
            cb_BaudRate.SelectedIndex = 14;
            //initialize the message queue
            debugMessageQueue = new ConcurrentQueue<string>();
            //start the debug message thread
            processDebugThreadEnabled = true;
            Thread debugMessageThread = new Thread(processDebugMessagesThread);
            debugMessageThread.Start();

            packetQueue = new ConcurrentQueue<RawPacket>();
            processPacketQueueEnabled = true; 
            Thread packetProcessorThread = new Thread(processPacketThread);
            packetProcessorThread.Start();

            string[] frameFormats = { "Legacy Frame", "Quaternion Frame", "Protocol Buffer", "Individual Sensor Files" };
            cb_OutputFormat.Items.AddRange(frameFormats);
            cb_OutputFormat.SelectedIndex = 1;

            string[] dataTypes = { "Quaternion", "Magnetic","Acceleration", "Rotation" };
            cb_dataType.Items.AddRange(dataTypes);
            cb_dataType.SelectedIndex = 0;

            this.cb_robotPort.SelectedItem = this.cb_robotPort.Items[0];
            robotArmQueue = new ConcurrentQueue<String>();
            processRobotArmQueueEnabled = true;
            Thread robotArmDataThread = new Thread(robotArmThread);
            robotArmDataThread.Start();

            this.cb_dbComPorts.SelectedItem = this.cb_dbComPorts.Items[0];
            nud_dbDataRate.Value = dbDataRate;

            dataMonitorQueue = new ConcurrentQueue<RawPacket>();
            dataMonitorQueueEnabled = true;

            dbDataReceiveQueue = new ConcurrentQueue<byte>();
            dbDataReceiveQueueEnabled = true;
            cb_dbBaudRate.Items.AddRange(baudrates);
            cb_dbBaudRate.SelectedIndex = 12;

            cb_pbComPorts.Items.AddRange(SerialPort.GetPortNames());
            this.cb_pbComPorts.SelectedItem = this.cb_pbComPorts.Items[0];
            cb_pbBaudRate.Items.AddRange(baudrates);
            cb_pbBaudRate.SelectedIndex = 12;
            pbDataReceiveQueue = new ConcurrentQueue<byte>();
            pbDataReceiveQueueEnabled = true;
            gb_pbManualEmulation.Enabled = false;
        }

        private void bnt_Connect_Click(object sender, EventArgs e)
        {
            //set the serial port to the selected item. 
            //Thread serialThread = new Thread(ReadThread);
            if (serialPort.IsOpen)
            {
                //do nothing
                return;
            }

            serialPort.PortName = cb_serialPorts.Items[cb_serialPorts.SelectedIndex].ToString();
            serialPort.BaudRate = int.Parse(cb_BaudRate.Items[cb_BaudRate.SelectedIndex].ToString()); 
            try
            {
                openSerialPort = true;
                serialPort.Open();

                tb_Console.AppendText("Port: " + serialPort.PortName + " Open\r\n");
            }
            catch (Exception ex)
            {
                tb_Console.AppendText("Failed to open Port: " + serialPort.PortName + " \r\n");
                tb_Console.AppendText("Exception " + ex.Message + " \r\n");
                openSerialPort = false;
            }
        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                try
                {                    
                    serialPort.Close();
                    openSerialPort = false;
                    tb_Console.AppendText("Port: " + serialPort.PortName + " Closed\r\n");
                }
                catch
                {
                    tb_Console.AppendText("Failed to close Port: " + serialPort.PortName + "\r\n");
                }
            }
        }

        RawPacket packet = new RawPacket();
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (serialPort.BytesToRead > 0)
            {
                int receivedByte = serialPort.ReadByte();
                if (receivedByte != -1)
                {                    
                    //process the byte
                    byte newByte = (byte)receivedByte;
                    int bytesReceived = packet.BytesReceived + 1;
                    PacketStatus status = packet.processByte((byte)receivedByte);
                    switch(status)
                    {
                        case PacketStatus.PacketComplete:                        
                            //debugMessageQueue.Enqueue(String.Format("{0} Packet Received {1} bytes\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond), packet.PayloadSize));
                            RawPacket packetCopy = new RawPacket(packet);  
                            packetQueue.Enqueue(packetCopy); 
                            packet.resetPacket(); 
                            break;
                        case PacketStatus.PacketError:
                            if (cb_logErrors.Checked)
                            {
                                debugMessageQueue.Enqueue(String.Format("{0} Packet ERROR! {1} bytes received\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond), bytesReceived));
                            }
                            packet.resetPacket(); 
                            break;
                        case PacketStatus.Processing:
                        case PacketStatus.newPacketDetected:
                            break;
                    }
                }
            }

        }
        private void sendPacket(byte[] payload, UInt16 size)
        {
            RawPacket packetToSend = new RawPacket(payload, size);
            UInt16 rawPacketSize = 0;
            byte[] rawPacketBytes = packetToSend.createRawPacket(ref rawPacketSize);
            try
            {
                serialPort.Write(rawPacketBytes, 0, rawPacketSize);
            }
            catch
            {
                debugMessageQueue.Enqueue(String.Format("{0} Failed to send packet\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));
            }
        }

        private void sendPacketTo(SerialPort port, byte[] payload, UInt16 size)
        {
            RawPacket packetToSend = new RawPacket(payload, size);
            UInt16 rawPacketSize = 0;
            byte[] rawPacketBytes = packetToSend.createRawPacket(ref rawPacketSize);
            try
            {
                port.Write(rawPacketBytes, 0, rawPacketSize);
            }
            catch
            {
                debugMessageQueue.Enqueue(String.Format("{0} Failed to send packet\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));
            }
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //stop debug thread
            processDebugThreadEnabled = false;
            processPacketQueueEnabled = false;
            streamDataEnabled = false;            
            EnableSocketQueue = false;
            processRobotArmQueueEnabled = false;
            pbProcessDataEnable = false;
            dbDataReceiveEnable = false;
            dbDataMonitorEnable = false;
            streamRawFramesEnabled = false;
            pbProcessDataEnable = false;
            //close the serial port
            if (serialPort.IsOpen)
            {
                serialPort.Close(); 
            }
        }
        private void sendGetFrameCommand(byte sensorId)
        {
            if (serialPort.IsOpen)
            {
                byte[] getFrameBytes = new byte[3];
                getFrameBytes[0] = 0x01;
                getFrameBytes[1] = 0x12;
                getFrameBytes[2] = sensorId;
                sendPacket(getFrameBytes, 3);
            }
        }
        private void sendBroadCastCommand(byte commandId)
        {
            if (serialPort.IsOpen)
            {
                byte[] getFrameBytes = new byte[2];
                getFrameBytes[0] = 0x01;
                getFrameBytes[1] = commandId;
                sendPacket(getFrameBytes, 2);
            }
        }
        private void sendCommandToSensorId(byte commandId, byte sensorId)
        {
            if (serialPort.IsOpen)
            {
                byte[] getFrameBytes = new byte[3];
                getFrameBytes[0] = 0x01;
                getFrameBytes[1] = commandId;
                getFrameBytes[2] = sensorId;
                sendPacket(getFrameBytes, 3);
            }
        }
        private void sendGetStatusCommand(byte sensorId)
        {
            if (serialPort.IsOpen)
            {
                byte[] getFrameBytes = new byte[3];
                getFrameBytes[0] = 0x01;
                getFrameBytes[1] = 0x18;
                getFrameBytes[2] = sensorId;
                sendPacket(getFrameBytes, 3);
            }
        }

        private void btn_getFrame_Click(object sender, EventArgs e)
        {
            sendGetFrameCommand((byte)nud_SelectedImu.Value);
        }
        private void btn_updateCfgParam_Click(object sender, EventArgs e)
        {
            sendBroadCastCommand(0x1A);
        }
        private void btn_getCfgParam_Click(object sender, EventArgs e)
        {
            sendCommandToSensorId(0x1B, (byte)nud_SelectedImu.Value);
        }
        private void btn_SetupMode_Click(object sender, EventArgs e)
        {


        }
        private void btn_setRate_Click(object sender, EventArgs e)
        {
            byte[] setRateBytes = new byte[5];
            setRateBytes[0] = 0x01;
            setRateBytes[1] = 0x24; //set rate command code
            setRateBytes[2] = (byte)nud_magRate.Value;
            setRateBytes[3] = (byte)nud_accelRate.Value;
            setRateBytes[4] = (byte)nud_gyroRate.Value;
            if (serialPort.IsOpen)
            {
                sendPacket(setRateBytes, 5);
            }
        }
        private void cb_ypr_CheckedChanged(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte[] setYPRBytes = new byte[3];
                setYPRBytes[0] = 0x01;
                setYPRBytes[1] = 0x22; //set yaw pitch and roll command
                if(cb_ypr.Checked)
                {
                    setYPRBytes[2] = 0x01;
                }
                else
                {
                    setYPRBytes[2] = 0x00;
                }
                sendPacket(setYPRBytes, 3);
            }
        }
        private void sendUpdateCommand()
        {
            if (serialPort.IsOpen)
            {
                byte[] getFrameBytes = new byte[3];
                getFrameBytes[0] = 0x01;
                getFrameBytes[1] = 0x11;
                sendPacket(getFrameBytes, 2);
            }
        }
        private void btn_sendUpdateCmd_Click(object sender, EventArgs e)
        {
            sendUpdateCommand();
        }

        private void cb_enableStream_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_enableStream.Checked)
            {
                if (cb_saveSensorData.Checked)
                {
                    saveSensorDataToFile = true;
                    if (sfd_saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        saveSensorDataFilename = sfd_saveFileDialog.FileName;
                        tb_dataLogLocation.Text = sfd_saveFileDialog.FileName;
                    }
                    else
                    {
                        saveSensorDataToFile = false;
                        tb_dataLogLocation.Text = "";
                    }
                }
                else
                {
                    saveSensorDataToFile = false;
                    tb_dataLogLocation.Text = "";
                }
                Thread streamThread = new Thread(streamDataToChartThread);
                streamDataToChartEnabled = true;
                streamThread.Start(); 
            }
            else
            {
                chb_arm_EnableTracing.Checked = false;
                streamDataToChartEnabled = false;
            }
        }

        private void tb_y_max_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || (e.KeyChar == '.') ||(e.KeyChar == '-' ) || (e.KeyChar == '\b'));
        }

        private void tb_y_min_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || (e.KeyChar == '.') || (e.KeyChar == '-') || (e.KeyChar == '\b'));
        }

        private void btn_setAxis_Click(object sender, EventArgs e)
        {
            try
            {
                float yaxis_max = 0.0F, yaxis_min = 0.0F;
                if(float.TryParse(tb_y_max.Text,out yaxis_max))
                {
                    chrt_dataChart.ChartAreas[0].AxisY.Maximum = yaxis_max;
                }
                if (float.TryParse(tb_y_min.Text, out yaxis_min))
                {
                    chrt_dataChart.ChartAreas[0].AxisY.Minimum = yaxis_min;
                }
                chrt_dataChart.ChartAreas[0].AxisY.RoundAxisValues();
            }
            catch
            {
                debugMessageQueue.Enqueue(String.Format("{0} Failed to parse Axis values (must be decimal number)\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));
            }
        }

        private void chrt_dataChart_Click(object sender, EventArgs e)
        {

        }

        private void btn_getStatus_Click(object sender, EventArgs e)
        {
            sendGetStatusCommand((byte)nud_SelectedImu.Value);
        }
        private void btn_getWarmUp_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte[] packetBytes = new byte[3];
                packetBytes[0] = 0x01;
                packetBytes[1] = 0x1E; //get warmup response command byte
                packetBytes[2] = (byte)nud_SelectedImu.Value;
                sendPacket(packetBytes, 3);

            }


        }
        private void btn_updateWarmUp_Click(object sender, EventArgs e)
        {
            sendBroadCastCommand(0x1D);
        }
        private void btn_clearScreen_Click(object sender, EventArgs e)
        {
            tb_Console.Clear();
        }
        private bool EnableSocketQueue = false;
        private void udpSocketClientProcess()
        {
            int port = 0;
            if(!int.TryParse(mtb_netPort.Text, out port))
            {
                port = 6668; 
            }
            UdpClient udpClient = new UdpClient(port);
            try
            {
                IPAddress ipAddress = IPAddress.Parse("192.168.2.1");//ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                //udpClient.Connect(ipAddress, 6667);
                //IPEndPoint object will allow us to read datagrams sent from the IP address we want. 
                //IPEndPoint RemoteIpEndPoint = new IPEndPoint(ipAddress, 0);
                debugMessageQueue.Enqueue(String.Format("Socket connected"));
                
                while (EnableSocketQueue)
                {
                    // Blocks until a message returns on this socket from a remote host.
                    byte[] receivedBytes = udpClient.Receive(ref remoteEP);
                    if (receivedBytes.Length > 0)
                    {
                        //process the byte
                        for(int i = 0; i < receivedBytes.Length; i++)
                        {
                            byte newByte = receivedBytes[i];
                            int bytesReceived = packet.BytesReceived + 1;
                            PacketStatus status = packet.processByte(receivedBytes[i]);
                            switch (status)
                            {
                                case PacketStatus.PacketComplete:
                                    //debugMessageQueue.Enqueue(String.Format("{0} Packet Received {1} bytes\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond), packet.PayloadSize));
                                    RawPacket packetCopy = new RawPacket(packet);
                                    packetQueue.Enqueue(packetCopy);
                                    packet.resetPacket();
                                    break;
                                case PacketStatus.PacketError:
                                    if (cb_logErrors.Checked)
                                    {
                                        debugMessageQueue.Enqueue(String.Format("{0} Packet ERROR! {1} bytes received\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond), bytesReceived));
                                    }
                                    packet.resetPacket();
                                    break;
                                case PacketStatus.Processing:
                                case PacketStatus.newPacketDetected:
                                    break;
                            }
                        }
                    }
                }
                debugMessageQueue.Enqueue("Socket Closed\r\n");
                debugMessageQueue.Enqueue("This message was sent from " +
                                            remoteEP.Address.ToString() +
                                            " on their port number " +
                                            remoteEP.Port.ToString());
                udpClient.Close();

            }
            catch (Exception e)
            {
                debugMessageQueue.Enqueue(e.ToString());
                
            }
            udpClient.Close();
        }
        private void socketClientProcess()
        {
            try
            {
                // Establish the remote endpoint for the socket.
                //IPHostEntry ipHostInfo =  Dns.GetHostEntry("192.168.1.1");
                IPAddress ipAddress = IPAddress.Parse(mtb_NetAddress.Text);//ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 6666);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    sender.Connect(remoteEP);
                    debugMessageQueue.Enqueue(String.Format("Socket connected to {0}",
    sender.RemoteEndPoint.ToString()));
                    byte[] receivedByte = new byte[1]; 
                    while(sender.Connected && EnableSocketQueue)
                    {                       
                        int receviedCount = sender.Receive(receivedByte);
                        if (receviedCount != 0)
                        {
                            //process the byte
                            byte newByte = receivedByte[0];
                            int bytesReceived = packet.BytesReceived + 1;
                            PacketStatus status = packet.processByte(receivedByte[0]);
                            switch (status)
                            {
                                case PacketStatus.PacketComplete:
                                    //debugMessageQueue.Enqueue(String.Format("{0} Packet Received {1} bytes\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond), packet.PayloadSize));
                                    RawPacket packetCopy = new RawPacket(packet);
                                    packetQueue.Enqueue(packetCopy);
                                    packet.resetPacket();
                                    break;
                                case PacketStatus.PacketError:
                                    if (cb_logErrors.Checked)
                                    {
                                        debugMessageQueue.Enqueue(String.Format("{0} Packet ERROR! {1} bytes received\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond), bytesReceived));
                                    }
                                    packet.resetPacket();
                                    break;
                                case PacketStatus.Processing:
                                case PacketStatus.newPacketDetected:
                                    break;
                            }
                        }

                    }
                    debugMessageQueue.Enqueue("Socket Closed\r\n");
                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    debugMessageQueue.Enqueue(String.Format("ArgumentNullException : {0}", ane.ToString()));
                }
                catch (SocketException se)
                {
                    debugMessageQueue.Enqueue(String.Format("SocketException : {0}", se.ToString()));
                }
                catch (Exception e)
                {
                    debugMessageQueue.Enqueue(String.Format("Unexpected exception : {0}", e.ToString()));
                }

            }
            catch (Exception e)
            {
                debugMessageQueue.Enqueue(e.ToString());
            }
        }
        private void btn_connectSocket_Click(object sender, EventArgs e)
        {
            EnableSocketQueue = true;
            if(cb_udpSelected.Checked)
            {
                Thread socketClientThread = new Thread(udpSocketClientProcess);
                socketClientThread.Start();
            }
            else
            {
                Thread socketClientThread =new Thread(socketClientProcess);
                socketClientThread.Start();
            }
            
            
        }

        private void btn_disconnectSock_Click(object sender, EventArgs e)
        {
            EnableSocketQueue = false;
        }

        private void btn_startStream_Click(object sender, EventArgs e)
        {
            if (!streamDataEnabled)
            {
                streamOutputType = (OutputType)cb_OutputFormat.SelectedIndex;
                if (streamOutputType == OutputType.individualSensorFiles)
                {
                    if(fbd_folderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        dataStreamFilePath = fbd_folderBrowser.SelectedPath;
                        //TODO: add check for files here...
                        Thread streamThread = new Thread(streamDataThread);
                        streamDataEnabled = true;
                        streamThread.Start();
                    }
                }
                else
                {

                    if (sfd_saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        dataStreamFilePath = sfd_saveFileDialog.FileName;
                        Thread streamThread = new Thread(streamDataThread);


                        streamDataEnabled = true;
                        //open the serial port
                        if (cb_EnableFowardPort.Checked)
                        {
                            forwardSerialPort.PortName = cb_forwardPorts.Items[cb_forwardPorts.SelectedIndex].ToString();
                            forwardSerialPort.BaudRate = int.Parse(cb_fpBaudRate.Items[cb_fpBaudRate.SelectedIndex].ToString());
                            try
                            {
                                forwardSerialPort.Open();
                                tb_Console.AppendText("Forward Port: " + forwardSerialPort.PortName + " Open\r\n");
                            }
                            catch (Exception ex)
                            {
                                tb_Console.AppendText("Failed to forward open Port: " + forwardSerialPort.PortName + " \r\n");
                                tb_Console.AppendText("Exception " + ex.Message + " \r\n");
                            }
                        }
                        streamThread.Start();
                    }
                }
            }
        }

        private void btn_StopStreaming_Click(object sender, EventArgs e)
        {
            //set the flag to disable the streaming thread.
            if (streamDataEnabled)
            {
                streamDataEnabled = false; 

            }
            if(forwardSerialPort.IsOpen)
            {
                forwardSerialPort.Close();
            }
        }

        private void cb_dataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //clear the data
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qx"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qy"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qz"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qw"].Points.Clear()));
            //Change the range to make more sense

            if(cb_dataType.SelectedIndex < 0 || cb_dataType.SelectedIndex > 3)
            {
                return; 
            }
            selectedDataType = cb_dataType.SelectedIndex;
            if (cb_dataType.SelectedIndex == 0) //quaternions
            {
                chrt_dataChart.ChartAreas[0].AxisY.Maximum = 1.1;
                chrt_dataChart.ChartAreas[0].AxisY.Minimum = -1.1;
                chrt_dataChart.ChartAreas[0].AxisY.RoundAxisValues();
            }
            else if(cb_dataType.SelectedIndex == 1) //magnetic
            {
                chrt_dataChart.ChartAreas[0].AxisY.Maximum = 3000;
                chrt_dataChart.ChartAreas[0].AxisY.Minimum = -3000;
                chrt_dataChart.ChartAreas[0].AxisY.RoundAxisValues();
            }
            else if (cb_dataType.SelectedIndex == 2)//Acceleration
            {
                chrt_dataChart.ChartAreas[0].AxisY.Maximum = 10000;
                chrt_dataChart.ChartAreas[0].AxisY.Minimum = -10000;
                chrt_dataChart.ChartAreas[0].AxisY.RoundAxisValues();
            }
            else if (cb_dataType.SelectedIndex == 3)//Gyro
            {
                chrt_dataChart.ChartAreas[0].AxisY.Maximum = 15000;
                chrt_dataChart.ChartAreas[0].AxisY.Minimum = -15000;
                chrt_dataChart.ChartAreas[0].AxisY.RoundAxisValues();
            }


        }

        private void cb_fpBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cb_saveSensorData_CheckedChanged(object sender, EventArgs e)
        {
            //if (cb_saveSensorData.Checked)
            //{
            //    if (sfd_saveFileDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        saveSensorDataFilename = sfd_saveFileDialog.FileName;
            //        tb_dataLogLocation.Text = sfd_saveFileDialog.FileName;
            //    }
            //    else
            //    {
            //        cb_saveSensorData.Checked = false;
            //    }
            //}

        }

        private void cb_serialPorts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cb_robotPort_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_refreshComPorts_Click(object sender, EventArgs e)
        {
            cb_serialPorts.Items.Clear();
            cb_serialPorts.Items.AddRange(SerialPort.GetPortNames());
            cb_forwardPorts.Items.Clear();
            cb_forwardPorts.Items.AddRange(SerialPort.GetPortNames());
            cb_robotPort.Items.Clear();
            cb_robotPort.Items.AddRange(SerialPort.GetPortNames());
            cb_dbComPorts.Items.Clear();
            cb_dbComPorts.Items.AddRange(SerialPort.GetPortNames());
            cb_pbComPorts.Items.Clear();
            cb_pbComPorts.Items.AddRange(SerialPort.GetPortNames());
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void sfd_saveFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void btn_arm_startStreaming_Click(object sender, EventArgs e)
        {
            if (bgw_uarmFileWorker.IsBusy)      // don't send file if the backgound worker is busy
            {
                tb_Console.AppendText("Cannot send file, worker busy\r\n");
                return;
            }
            if (File.Exists(textBox1.Text) == true)     // only start streaming if the file exists
            {
                lb_loopsExecuted.Text = 0.ToString();     // reset the loop executed count
                if (robotArmPort.IsOpen)
                {
                    // stream again, don't open the port
                    btn_arm_startStreaming.BackColor = System.Drawing.Color.Tomato;
                    bgw_uarmFileWorker.RunWorkerAsync(ofd_openUarmFile.FileName);
                    return;
                }
                else
                {
                    tb_Console.AppendText("Cannot send file, Com port closed\r\n");
                }
            }
            else
            {
                tb_Console.AppendText("Invalid U-Arm file location\r\n");
                return;
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        private void btn_arm_browseFile_Click(object sender, EventArgs e)
        {
            ofd_openUarmFile.Filter = "comma seperated values(*.csv)|*.csv|Brain pack data(*.dat)|*.dat|All files (*.*)|*.*";
            ofd_openUarmFile.FilterIndex = 3;
            ofd_openUarmFile.RestoreDirectory = true;

            if (ofd_openUarmFile.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd_openUarmFile.FileName;      // copy the file name the text box
            }
        }

        private DataTable convertToCsv(string fileName)
        {
            try
            {
                return CSVDataAdapter.Fill(fileName, false);
            }
            catch
            {
                tb_Console.AppendText("Exception occured in converting U-Arm file.\r\n");
                return null;
            }
        }
        
        private void bgw_uarmFileWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string fileName = (string) e.Argument;      // get the filename
            string text = null;
            robotArmLoopCount = 0;
            if (fileName.EndsWith(".dat") || fileName.EndsWith(".txt") || fileName.EndsWith(".csv"))    // only move forward if the file has either of these extensions.
            {
                uarmTable = convertToCsv(fileName); // convert to datatable.
                if (uarmTable == null)
                {
                    this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText("Failed to load the file\r\n")));
                    return;
                }
                this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText("Loaded " + uarmTable.Rows.Count.ToString() + " Rows \r\n")));
                this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText("From File: " + ofd_openUarmFile.FileName + " \r\n")));

                for (int count = 0; count < (int)uarmStreamLoopCount;)
                {
                    for (int i = 1; i < uarmTable.Rows.Count; i++)
                    {
                        text = "";
                        for (int column = 0; column < uarmTable.Columns.Count; column++)
                        {
                            text += uarmTable.Rows[i][column].ToString();
                            if (column == (uarmTable.Columns.Count - 1))
                            {
                                text += "\r\n";
                            }
                            else
                            {
                                text += ",";
                            }
                        }
                        
                        if (robotArmPort.IsOpen)
                        {
                            robotArmPort.Write(text);
                        }

                        if (bgw_uarmFileWorker.CancellationPending)
                        {
                            robotArmLoopCount = 0;
                            return;
                        }

                        Thread.Sleep((int)(10*uarmStreamLineDelay));     // specified delay after every line
                    }
                    count++;
                    robotArmLoopCount = count;
                    if (this.lb_loopsExecuted.InvokeRequired)
                    {
                        this.lb_loopsExecuted.BeginInvoke((MethodInvoker)delegate () {this.lb_loopsExecuted.Text = count.ToString(); ;});
                    }
                    requestSensorFrame = true;
                    
                }
                robotArmLoopCount = 0;
                return;
            }
            else
            {
                this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText("Invalid U-Arm source file\r\n")));
                robotArmLoopCount = 0;
                return;
            }

        }

        private void bgw_uarmFileWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

        }

        private void bgw_uarmFileWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            btn_arm_startStreaming.BackColor = System.Drawing.Color.LightGreen;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label6.Text = trackBar1.Value.ToString();
        }

        private void countDelaySel_ValueMemberChanged(object sender, EventArgs e)
        {
            
        }

        private void countDelaySel_TextChanged(object sender, EventArgs e)
        {
            if (countDelaySel.SelectedIndex == 0)       // Loop count
            {
                label6.Text = uarmStreamLoopCount.ToString();
                trackBar1.Value = (int)uarmStreamLoopCount;
            }
            else if (countDelaySel.SelectedIndex == 1)  // Line Delay
            {
                label6.Text = uarmStreamLineDelay.ToString();
                trackBar1.Value = (int)uarmStreamLineDelay;
            }
        }

        private void btn_arm_stopStreaming_Click(object sender, EventArgs e)
        {
            btn_arm_startStreaming.BackColor = System.Drawing.Color.LightGreen;
            tb_Console.AppendText("File streaming to U-Arm stopped.\r\n");
            if (bgw_uarmFileWorker.IsBusy)
            {
                bgw_uarmFileWorker.CancelAsync();      // cancel the background worker.
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBox1_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            if (countDelaySel.SelectedIndex == 0)       // Loop count
            {
                if ((UInt16)trackBar1.Value > 0)
                {
                    uarmStreamLoopCount = (UInt16)trackBar1.Value;
                    label6.Text = uarmStreamLoopCount.ToString();
                }
                else
                {
                    trackBar1.Value = (int)uarmStreamLoopCount;
                    label6.Text = uarmStreamLoopCount.ToString();
                }
            }
            else if (countDelaySel.SelectedIndex == 1)  // Line Delay
            {
                if (trackBar1.Value <= 20)
                {
                    DialogResult result;
                    result = MessageBox.Show("Having line delay less than 20ms can cause robot behave randomly. Continue?", "Caution", MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        uarmStreamLineDelay = 21;
                        trackBar1.Value = (int)uarmStreamLineDelay;
                        label6.Text = uarmStreamLineDelay.ToString();
                        return;
                    }
                }
                uarmStreamLineDelay = (UInt32)trackBar1.Value;
                label6.Text = uarmStreamLineDelay.ToString();
            }
        }

        private void chb_arm_EnableTracing_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_arm_EnableTracing.Checked)
            {
                if (!cb_enableStream.Checked)
                {
                    MessageBox.Show("Stream from 'Debug RS485 sensor' tab needs to be checked first.");
                    chb_arm_EnableTracing.Checked = false;
                }
            }
        }

        private void robotArmPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (robotArmPort.BytesToRead > 0)
            {
                int receivedByte = robotArmPort.ReadByte();
                if (receivedByte != -1)
                {
                    //process the byte
                    char newByte = (char)receivedByte;
                    robotArmInputStream += newByte.ToString();
                    if (newByte == '\n')
                    {
                        robotArmQueue.Enqueue((String)robotArmInputStream);
                        robotArmInputStream = "";
                    }
                }
            }
        }

        public void robotArmThread()
        {
            String inputStr;

            while (processRobotArmQueueEnabled)
            {
                if (robotArmQueue.Count > 0)
                {
                    if (robotArmQueue.TryDequeue(out inputStr))
                    {
                        if (enableArmRecording)
                        {
                            // Save the incoming Data
                            if (inputStr.StartsWith("Reading: "))       // save
                            {
                                saveArmAnglesData(inputStr.ToString());
                            }
                            else if (inputStr.Contains("End of Recording"))
                            {
                                this.btn_arm_recMovement.BeginInvoke((MethodInvoker)delegate () { btn_arm_recMovement.BackColor = System.Drawing.Color.Transparent; ; });
                                this.BeginInvoke((MethodInvoker)delegate () { toggleRecButton = false; ; });
                                this.BeginInvoke((MethodInvoker)(() => closeRobotArmOutFile()));
                                //closeRobotArmOutFile();
                                enableArmRecording = false;
                            }
                            this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText("Arm msg: " + inputStr.ToString())));
                        }
                        else
                        {
                            //Display on Console
                            this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText("Arm msg: " + inputStr.ToString())));
                        }
                    }
                }
            }
        }

        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private void saveArmAnglesData(String str)
        {
                if (enableArmRecording)
                {
                    if (robotArmOutFileIsOpen)
                    {
                        if (robotArmOutFile.CanWrite)
                        {
                            str = str.Remove(0, 9);
                            robotArmOutFile.Write(ASCIIEncoding.ASCII.GetBytes(str), (int)0, (int)(str.Length));
                        }
                    }
                    else
                    {
                        try
                        {
                            debugMessageQueue.Enqueue(String.Format("Openning file: {0}\r\n", textBox1.Text));
                            robotArmOutFile = File.Open(textBox1.Text, FileMode.Create);    // open file.
                            str = str.Remove(0, 9);
                            robotArmOutFile.Write(ASCIIEncoding.ASCII.GetBytes(str), (int)0, (int)(str.Length));    // save the received data
                            robotArmOutFileIsOpen = true;
                        }
                        catch
                        {
                            debugMessageQueue.Enqueue(String.Format("Failed to create file: {0}\r\n", textBox1.Text));
                            return;
                        }
                    }
                }
        }

        private void closeRobotArmOutFile()
        {
            if (robotArmOutFileIsOpen)
            {
                robotArmOutFile.Close();
                robotArmOutFile = null;
                robotArmOutFileIsOpen = false;
            }
        }

        private void btn_arm_togglePort_Click(object sender, EventArgs e)
        {
            if (!togglePortButton)
            {
                robotArmPort.PortName = cb_robotPort.Items[cb_robotPort.SelectedIndex].ToString();
                robotArmPort.BaudRate = 115200;
                try
                {
                    openRobotArmPort = true;
                    robotArmPort.Open();

                    tb_Console.AppendText("Port: " + robotArmPort.PortName + " Open\r\n");
                    btn_arm_togglePort.Text = "Close";
                    togglePortButton = !togglePortButton;
                }
                catch (Exception ex)
                {
                    tb_Console.AppendText("Failed to open Port: " + serialPort.PortName + " \r\n");
                    tb_Console.AppendText("Exception " + ex.Message + " \r\n");
                    openRobotArmPort = false;
                }
            }
            else
            {
                    try
                    {
                        robotArmPort.Close();
                        openRobotArmPort = false;
                        tb_Console.AppendText("Port: " + robotArmPort.PortName + " Closed\r\n");
                        btn_arm_togglePort.Text = "Open";
                        togglePortButton = !togglePortButton;
                        chb_arm_easeMovement.Checked = false;
                    }
                    catch
                    {
                        tb_Console.AppendText("Failed to close Port: " + robotArmPort.PortName + "\r\n");
                    }
            }
        }

        private void btn_arm_recMovement_Click(object sender, EventArgs e)
        {
            if (chb_arm_recPoints.Checked)
            {
                if (robotArmPort.IsOpen)
                {
                    robotArmPort.Write("Rec\r");
                }
            }
            else
            {
                if (!toggleRecButton)       // Start recording (false)
                {
                    if (robotArmWarningEnable)
                    {
                        DialogResult result;
                        result = MessageBox.Show("It will save data to the file specified in the address. Press Yes to continue, No to enter new Address, Cancel to stop displaying this message",
                                                    "Caution", MessageBoxButtons.YesNoCancel);
                        if (result == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                        else if (result == System.Windows.Forms.DialogResult.Cancel)
                        {
                            robotArmWarningEnable = false;
                        }
                    }
                    if (File.Exists(textBox1.Text))
                    {
                        if (robotArmPort.IsOpen)
                        {
                            robotArmPort.Write("Rec\r");
                            btn_arm_recMovement.BackColor = System.Drawing.Color.Tomato;
                            enableArmRecording = true;
                            toggleRecButton = !toggleRecButton;
                        }
                    }
                    else
                    {
                        tb_Console.AppendText("Invalid file address");
                        return;
                    }
                }
                else        // stop recording (true)
                {
                    if (robotArmPort.IsOpen)
                    {
                        robotArmPort.Write("End\r");
                    }
                    btn_arm_recMovement.BackColor = System.Drawing.Color.Transparent;
                    //enableArmRecording = false;
                    toggleRecButton = !toggleRecButton;
                }
            }
        }

        private void btb_arm_playRecording_Click(object sender, EventArgs e)
        {
            if (robotArmPort.IsOpen)
            {
                robotArmPort.Write("Play\r");
            }
        }

        private void chb_arm_easeMovement_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_arm_easeMovement.Checked)
            {
                if (robotArmPort.IsOpen)
                {
                    robotArmPort.Write("EaseEn\r");
                    tb_Console.AppendText("Arm movement easing enabled.\r\n");
                }
            }
            else
            {
                if (robotArmPort.IsOpen)
                {
                    robotArmPort.Write("EaseDis\r");
                    tb_Console.AppendText("Arm movement easing disabled.\r\n");
                }
            }
        }

        private void btb_arm_playRecording_MouseEnter(object sender, EventArgs e)
        {

        }
        UInt32 timestampCounter = 0;

        private void btn_pbFullRawFrame_Click(object sender, EventArgs e)
        {
            FullRawFrame dataFrame = new FullRawFrame(9);
            UInt16 numBytes = 0;
            dataFrame.populateFrameWithTestData();
            dataFrame.setTimestamp(timestampCounter++);
            byte[] serializedBytes = dataFrame.serializeFrame(out numBytes);
            sendPacketTo(powerBoardPort, serializedBytes, numBytes);
        }

        bool streamRawFramesEnabled = false; 

        void sendRawFrames()
        {
            FullRawFrame dataFrame = new FullRawFrame(9);
            UInt16 numBytes = 0;
            dataFrame.populateFrameWithTestData();
            
            while(streamRawFramesEnabled)
            {
                Thread.Sleep(10);
                dataFrame.setTimestamp(timestampCounter++);
                byte[] serializedBytes = dataFrame.serializeFrame(out numBytes);
                sendPacketTo(powerBoardPort, serializedBytes, numBytes);
            }

        }
        private void cb_streamRawFrames_CheckedChanged(object sender, EventArgs e)
        {
            
            if (cb_streamRawFrames.Checked)
            {
                timestampCounter = 0;
                Thread streamThread = new Thread(sendRawFrames);
                streamRawFramesEnabled = true;
                streamThread.Start();
            }
            else
            {
                streamRawFramesEnabled = false;
            }

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        public void dbDataReceive()
        {
            byte receivedByte;
            while (dbDataReceiveEnable)
            {
                if (dbDataReceiveQueue.Count == 0)
                {
                    Thread.Sleep(1);
                    //Thread.Yield();
                }

                if (dbDataReceiveQueue.TryDequeue(out receivedByte))
                {
                    int bytesReceived = dataBoardPacket.BytesReceived + 1;
                    PacketStatus status = dataBoardPacket.processByte((byte)receivedByte);
                    switch (status)
                    {
                        case PacketStatus.PacketComplete:
                            RawPacket packetCopy = new RawPacket(dataBoardPacket);
                            processSubpPacket(dataBoardPacket);
                            dataBoardPacket.resetPacket();
                            break;
                        case PacketStatus.PacketError:
                            if (chb_dbDataMonitorEnable.Checked)
                            {
                                debugMessageQueue.Enqueue(String.Format("{0} Packet ERROR! {1} bytes received\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond), bytesReceived));
                            }
                            dbPacketErrorCount++;
                            this.BeginInvoke((MethodInvoker)(() => lbl_dbPacketErrorCount.Text = dbPacketErrorCount.ToString()));
                            dataBoardPacket.resetPacket();
                            break;
                        case PacketStatus.Processing:
                            break;
                        case PacketStatus.newPacketDetected:
                            break;
                    }
                }
            }
        }

        private void btn_dbTogglePort_Click(object sender, EventArgs e)
        {
            if (!toggleDbPort)  // try to open the port
            {
                dataBoardPort.PortName = cb_dbComPorts.Items[cb_dbComPorts.SelectedIndex].ToString();
                dataBoardPort.BaudRate = int.Parse(cb_dbBaudRate.Items[cb_dbBaudRate.SelectedIndex].ToString()); ;
                try
                {
                    dbPortOpen = true;
                    dataBoardPort.Open();

                    tb_Console.AppendText("Port: " + dataBoardPort.PortName + " Open\r\n");
                    btn_dbTogglePort.Text = "Close";
                    toggleDbPort = !toggleDbPort;
                    Thread dbDataReceiveThread = new Thread(dbDataReceive);
                    dbDataReceiveEnable = true;
                    dbDataReceiveThread.Start();
                }
                catch (Exception ex)
                {
                    tb_Console.AppendText("Failed to open Port: " + dataBoardPort.PortName + " \r\n");
                    tb_Console.AppendText("Exception " + ex.Message + " \r\n");
                    toggleDbPort = false;
                    dbPortOpen = false;
                    dbDataReceiveEnable = false;  // close the data reveive thread
                }
            }

            else    // try to close the port
            {
                try
                {
                    if (btn_dbStream.BackColor == System.Drawing.Color.LightGreen)
                    {
                        btn_dbStream.PerformClick();    // stop the stream if enabled
                    }
                    dataBoardPort.Close();
                    dbPortOpen = false;
                    tb_Console.AppendText("Port: " + dataBoardPort.PortName + " Closed\r\n");
                    btn_dbTogglePort.Text = "Open";
                    toggleDbPort = !toggleDbPort;
                    dataBoardPacket.resetPacket();
                }
                catch
                {
                    tb_Console.AppendText("Failed to close Port: " + dataBoardPort.PortName + "\r\n");
                }
            }
        }

        private void nud_dbDataRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void btn_dbSetConfig_Click(object sender, EventArgs e)
        {
            if (dataBoardPort.IsOpen)
            {
                MemoryStream stream = new MemoryStream();
                byte[] header = { 0x01, 0x53, (byte)nud_dbDataRate.Value};
                stream.Write(header, 0, 3);
                stream.Write(BitConverter.GetBytes(setSensorMask), 0, 4);   // NOTE: Bit converter is little endian
                sendPacketTo(dataBoardPort, stream.ToArray(), 7);
            }
        }

        private int convertFromBcd(int bcdNumber)
        {
            return (((bcdNumber & 0xF0) >> 4) * 10 + (bcdNumber & 0x0F));
        }

        private int convertToBcd(int twoDigitInteger)
        {
            int tens, units;
            tens = twoDigitInteger / 10;
            units = twoDigitInteger % 10;
            return ((tens << 4) | units);
        }

        private void getSystemDateTime(ref Int32 time, ref Int32 date)
        {
            int dateCentury, dateYear, dateMonth, dateDay, dateDate;
            int timeSeconds, timeMinutes, timeHour, timeAmPm;

            DateTime localDateTime = DateTime.Now;
            debugMessageQueue.Enqueue(String.Format("{0}\r\n", localDateTime));

            // convert dateTime format to BCD as NUMBER = TENS(MSB) | UNITS(LSB)
            timeSeconds = convertToBcd(localDateTime.Second);
            timeMinutes = convertToBcd(localDateTime.Minute);
            timeHour = convertToBcd(localDateTime.Hour);        // only supports 24-hour mode
            timeAmPm = 0;

            dateCentury = convertToBcd(localDateTime.Year / 100);
            dateYear = convertToBcd(localDateTime.Year % 100);
            dateMonth = convertToBcd(localDateTime.Month);
            dateDay = (int)localDateTime.DayOfWeek;
            dateDate = convertToBcd(localDateTime.Day);

            // convert the data specific to the data accepted by ATSAM4S2A
            time = timeSeconds | (timeMinutes << 8) | (timeHour << 16) | (timeAmPm << 22);
            date = dateCentury | (dateYear << 8) | (dateMonth << 16) | (dateDay << 21) | (dateDate << 24);
        }

        private void btn_dbSetDateTime_Click(object sender, EventArgs e)
        {
            Int32 time = 0, date = 0;
            getSystemDateTime(ref time, ref date);

            if (dataBoardPort.IsOpen)
            {
                MemoryStream stream = new MemoryStream();
                byte[] header = { 0x01, 0x5a };
                stream.Write(header, 0, 2);
                stream.Write(BitConverter.GetBytes(time), 0, 4);
                stream.Write(BitConverter.GetBytes(date), 0, 4);
                sendPacketTo(dataBoardPort, stream.ToArray(), 10);
            }
        }

        private void btn_dbGetStatus_Click(object sender, EventArgs e)
        {
            if (dataBoardPort.IsOpen)
            {
                byte[] header = { 0x01, 0x51 };
                sendPacketTo(dataBoardPort, header, 2);
            }
        }

        private void btn_dbStream_Click(object sender, EventArgs e)
        {
            if (dataBoardPort.IsOpen)
            {
                if (btn_dbStream.BackColor == System.Drawing.Color.Transparent) // use back color to check the state of the button
                {
                    byte[] header = { 0x01, 0x54, 0x01 };   // Stream enable
                    sendPacketTo(dataBoardPort, header, 3);
                    btn_dbStream.BackColor = System.Drawing.Color.LightGreen;
                    sensorDataRate = 0;
                    sensorAvgRate = 0;
                    dbSensorFrameCount = 0;
                    dbPacketErrorCount = 0;
                }
                else
                {
                    byte[] header = { 0x01, 0x54, 0x00 };   // stream disable
                    sendPacketTo(dataBoardPort, header, 3);
                    btn_dbStream.BackColor = System.Drawing.Color.Transparent;
                }
            }
            else
            {
                tb_Console.AppendText("Com port not open.\r\n");
            }
        }

        private void btn_dbGetDateTime_Click(object sender, EventArgs e)
        {
            if (dataBoardPort.IsOpen)
            {
                byte[] header = { 0x01, 0x58 };
                sendPacketTo(dataBoardPort, header, 2);
            }
        }

        private void btn_dbEnableSen0_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_dbEnableSen0.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                dbEnableSen0 = true;
                btn_dbEnableSen0.BackColor = System.Drawing.Color.LightGreen;
                setSensorMask |= (0x01 << 0);
            }
            else // sensor is enabled (before click)
            {
                dbEnableSen0 = false;
                btn_dbEnableSen0.BackColor = System.Drawing.Color.Transparent;
                setSensorMask &= ~(0x01 << 0);
            }
        }

        private void chb_arm_recPoints_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_arm_recPoints.Checked)
            {
                if (File.Exists(textBox1.Text))
                {
                    if (robotArmPort.IsOpen)
                    {
                        robotArmPort.Write("RecPoints\r");
                        tb_Console.AppendText("Manual Recording started\r\n");
                        enableArmRecording = true;
                    }
                }
                else
                {
                    tb_Console.AppendText("Invalid file address\r\n");
                    chb_arm_recPoints.Checked = false;
                    return;
                }
            }
            else
            {
                if (robotArmPort.IsOpen)
                {
                    robotArmPort.Write("End\r");
                    tb_Console.AppendText("End of Recording\r\n");
                    //enableArmRecording = false;
                }
            }
        }

        private void displaySubpStatus(ref RawPacket packet)
        {
            // charge level
            debugMessageQueue.Enqueue(String.Format("Charge Level: {0}\r\n", packet.Payload[2]));

            // charge state
            if (packet.Payload[3] == 0)
                debugMessageQueue.Enqueue("Charge state: Battery Low\r\n");
            else if (packet.Payload[3] == 1)
                debugMessageQueue.Enqueue("Charge state: Battery Nominal\r\n");
            else if (packet.Payload[3] == 2)
                debugMessageQueue.Enqueue("Charge state: Battery Full\r\n");
            else
                debugMessageQueue.Enqueue("Charge state: Charging\r\n");

            // USB comm state
            if (packet.Payload[4] == 0)
                debugMessageQueue.Enqueue("USB state: USB comm not connected\r\n");
            else
                debugMessageQueue.Enqueue("USB state: USB comm detected\r\n");

            // Jack detect state
            if (packet.Payload[5] == 0)
                debugMessageQueue.Enqueue("Jack detect: No jacks detected\r\n");
            else if (packet.Payload[5] == 3)
                debugMessageQueue.Enqueue("Jack detect: Both jacks detected\r\n");
            else
                debugMessageQueue.Enqueue("Jack detect: One jack detected\r\n");

            // Sensor Stream state
            if (packet.Payload[6] == 0)
                debugMessageQueue.Enqueue("Sensor state: Sensor idle\r\n");
            else if (packet.Payload[6] == 1)
                debugMessageQueue.Enqueue("Sensor state: Sensor streaming\r\n");
            else
                debugMessageQueue.Enqueue("Sensor state: Sensor error\r\n");

            // Sensor Mask
            int presentSensors = 0, absentSensors = 0;
            byte[] sensorPresent = new byte[9];
            byte[] sensorAbsent = new byte[9];
            debugMessageQueue.Enqueue("Sensor present: ");
            for (int i = 0; i < 9; i++) // we only have a max of 9 sensors
            {
                // get the IDs for the sensors present and absent
                if (i < 8)
                {
                    if (((packet.Payload[7] >> i) & 0x01) > 0)      // the sensor is present
                    {
                        sensorPresent[presentSensors] = (byte)i;
                        presentSensors++;
                        debugMessageQueue.Enqueue(String.Format("{0}", i));
                    }
                    else    // the sensor is absent
                    {
                        sensorAbsent[absentSensors] = (byte)i;
                        absentSensors++;
                    }
                }
                else      // check the next byte for sensorID 8
                {
                    if ((packet.Payload[8] & 0x01) > 0)      // the sensor is present
                    {
                        sensorPresent[presentSensors] = (byte)i;
                        presentSensors++;
                        debugMessageQueue.Enqueue(String.Format("{0}", i));
                    }
                    else    // the sensor is absent
                    {
                        sensorAbsent[absentSensors] = (byte)i;
                        absentSensors++;
                    }
                }
            }
            debugMessageQueue.Enqueue("\r\nSensor absent: ");
            for (int i = 0; i < absentSensors; i++)
            {
                debugMessageQueue.Enqueue(String.Format("{0}", sensorAbsent[i]));
            }
            debugMessageQueue.Enqueue("\r\n");
            rxSensorMask = ((int)packet.Payload[7]) | ((int)packet.Payload[8] << 8);
            displaySensorState(setSensorMask, rxSensorMask);
        }
        
        private void displaySensorState(int setMask, int rxMask)
        {
            if (btn_dbStream.BackColor == System.Drawing.Color.LightGreen)  // display the sensor state only if the stream is enabled
            {
                // flash red color on the buttons with sensor error
                for (int i = 0; i < 9; i++)
                {
                    if (((setMask >> i) & 0x01) != 0)   // check if the sensor was requested
                    {
                        if (((rxMask >> i) & 0x01) != 0)    // check if the sensor is in error 
                        {
                            setSensorBtnColor(i, System.Drawing.Color.LightGreen);
                        }
                        else
                        {
                            setSensorBtnColor(i, System.Drawing.Color.Tomato);
                        }
                    }
                }
            }
        }

        private void setSensorBtnColor(int sensorId, System.Drawing.Color color)
        {
            switch (sensorId)
            {
                case 0:
                    btn_dbEnableSen0.BackColor = color;
                    break;
                case 1:
                    btn_dbEnableSen1.BackColor = color;
                    break;
                case 2:
                    btn_dbEnableSen2.BackColor = color;
                    break;
                case 3:
                    btn_dbEnableSen3.BackColor = color;
                    break;
                case 4:
                    btn_dbEnableSen4.BackColor = color;
                    break;
                case 5:
                    btn_dbEnableSen5.BackColor = color;
                    break;
                case 6:
                    btn_dbEnableSen6.BackColor = color;
                    break;
                case 7:
                    btn_dbEnableSen7.BackColor = color;
                    break;
                case 8:
                    btn_dbEnableSen8.BackColor = color;
                    break;
                default:
                    break;
            }
        }

        private void displayDateTime(ref RawPacket packet)
        {
            // date
            int date, dayOfWeek, month, year;
            year = (convertFromBcd(packet.Payload[6]) * 100) + convertFromBcd(packet.Payload[7]);
            month = convertFromBcd(packet.Payload[8] & 0x1F);  // lower 5 bits are for months
            dayOfWeek = convertFromBcd(packet.Payload[8] & 0xE0) >> 4; // higher 3 bits are for day
            date = convertFromBcd(packet.Payload[9]);
            debugMessageQueue.Enqueue(String.Format("{0}/{1}/{2}, ", date, month, year));

            // time
            int hour, minute, second;
            hour = convertFromBcd(packet.Payload[4]);
            minute = convertFromBcd(packet.Payload[3]);
            second = convertFromBcd(packet.Payload[2]);
            debugMessageQueue.Enqueue(String.Format("{0}:{1}:{2}\r\n", hour, minute, second));
        }
        private int findFrameOffsetForId(ref RawPacket packet, int expectedSensorId)
        {
            int i = 0;
            for ( i=7; i < packet.PayloadSize; i += 36)
            {
                if(packet.Payload[i] == (byte)expectedSensorId)
                {                    
                    return i; 
                }
            }
            return -1;
        }
        private void displayFrameData(ref RawPacket packet)
        {
            if (chb_dbDataMonitorEnable.Checked)
            {
                if (!streamDataToChartEnabled)  // make sure no other thread is writing to the table and the chart
                {
                    // get the sensor id from the nud
                    int expectedSensorId = (int)nud_dbSensorId.Value;
                    int frameOffset = findFrameOffsetForId(ref packet, expectedSensorId);
                    if(frameOffset == -1)
                    {
                        return; 
                    }
                        //((expectedSensorId) * 36) + 7;    // location of the data in the frame

                    ImuFrame dataFrame = new ImuFrame();
                    dataFrame.ParseDataFromFullFrame(packet, frameOffset, expectedSensorId);
                    updateChart(dataFrame);
                    updateTable(dataFrame);
                    // calculate data rate
                    curSensorFrameTick = BitConverter.ToUInt32(packet.Payload, 3);
                    sensorDataRate = curSensorFrameTick - preSensorFrameTick;
                    preSensorFrameTick = curSensorFrameTick;
                    this.BeginInvoke((MethodInvoker)(() => lbl_dbDataInterval.Text = sensorDataRate.ToString()));
                }
                else
                {
                    tb_Console.AppendText("Other sensor stream writing to the chart and table\r\n");
                }
            }
        }

        public void DataMonitorThread()
        {
            /* */
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qx"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qy"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qz"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Qw"].Points.Clear()));
            
            RawPacket framePacket = new RawPacket();

            while (dbDataMonitorEnable)
            {
                if (dataMonitorQueue.Count == 0)
                {
                    Thread.Sleep(1);
                }

                if (dataMonitorQueue.TryDequeue(out framePacket))
                {
                    displayFrameData(ref framePacket);
                    framePacket.resetPacket();
                } 
                                   
                //Thread.Yield();                
            }
        }

        private void processSubpPacket(RawPacket packet)
        {
            if (packet.Payload[0] == 0x05)  // verify if the packet is coming from Sub processor
            {
                switch (packet.Payload[1])
                {
                    case 0x52:  // get status response
                        displaySubpStatus(ref packet);
                        break;

                    case 0x55:  // Sensor full frame
                        // handle the complete packet containing data from all sensors.
                        RawPacket packetCopy = new RawPacket(packet);
                        dataMonitorQueue.Enqueue(packetCopy);
                        packet.resetPacket();
                        dbSensorFrameCount++;
                        this.BeginInvoke((MethodInvoker)(() => lbl_dbFrameCount.Text = dbSensorFrameCount.ToString()));
                        break;

                    case 0x56:  // power down request
                        // send power down response
                        debugMessageQueue.Enqueue("Received power down request\r\n");
                        if (dataBoardPort.IsOpen)
                        {
                            byte[] header = { 0x01, 0x57 };
                            this.BeginInvoke((MethodInvoker)(() => sendPacketTo(dataBoardPort, header, 2)));
                        }
                        break;

                    case 0x59:  // get date time response
                        displayDateTime(ref packet);
                        break;

                    case 0x5B:  // set date time response
                        if (packet.Payload[2] == 1)
                        {
                            debugMessageQueue.Enqueue("Time and date set successfully\r\n");
                        }
                        else
                        {
                            debugMessageQueue.Enqueue("Setting date and time failed\r\n");
                        }
                        break;

                    case 0x5C:  // this is debug string. Store it in debug Logs
                        byte[] debugStr = new byte[100];
                        Buffer.BlockCopy(packet.Payload, 2, debugStr, 0, 100);
                        String asciiString = System.Text.Encoding.ASCII.GetString(debugStr);    // TODO: this will print nulls, solve it
                        debugMessageQueue.Enqueue(asciiString);
                        break;

                    default:
                        break;
                }
            }
        }

        RawPacket dataBoardPacket = new RawPacket();
        private void dataBoardPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int bytesToRead = dataBoardPort.BytesToRead;
            while (bytesToRead > 0)
            {
                if (!dataBoardPort.IsOpen)
                {
                    return;
                }
                int receivedByte = dataBoardPort.ReadByte();
                if (receivedByte != -1)
                {
                    //process the byte
                    byte newByte = (byte)receivedByte;

                    dbDataReceiveQueue.Enqueue(newByte);
                    
                }
                bytesToRead = dataBoardPort.BytesToRead;
            }
        }

        private void cb_BaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cb_dbBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cb_logErrors_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chb_pbEnableBridge_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_pbEnableBridge.Checked)
            {
                gb_pbManualEmulation.Enabled = false;   // disable the controls
                // disable any ongoing threads
                pbProcessDataEnable = false;
                streamRawFramesEnabled = false;
                cb_streamRawFrames.Checked = false;
            }
            else
            {
                gb_pbManualEmulation.Enabled = true;    // enable the controls
                // enable any disabled threads if the port it open
                if (powerBoardPort.IsOpen)
                {
                    pbProcessDataEnable = true;
                }
            }
        }

        private void btn_pbSensor0_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_pbSensor0.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                btn_pbSensor0.BackColor = System.Drawing.Color.LightGreen;
                pbDetectedSensorMask |= (0x01 << 0);
            }
            else // sensor is enabled (before click)
            {
                btn_pbSensor0.BackColor = System.Drawing.Color.Transparent;
                pbDetectedSensorMask &= ~(0x01 << 0);
            }
        }

        private void btn_pbSensor1_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_pbSensor1.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                btn_pbSensor1.BackColor = System.Drawing.Color.LightGreen;
                pbDetectedSensorMask |= (0x01 << 1);
            }
            else // sensor is enabled (before click)
            {
                btn_pbSensor1.BackColor = System.Drawing.Color.Transparent;
                pbDetectedSensorMask &= ~(0x01 << 1);
            }
        }

        private void btn_pbSensor2_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_pbSensor2.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                btn_pbSensor2.BackColor = System.Drawing.Color.LightGreen;
                pbDetectedSensorMask |= (0x01 << 2);
            }
            else // sensor is enabled (before click)
            {
                btn_pbSensor2.BackColor = System.Drawing.Color.Transparent;
                pbDetectedSensorMask &= ~(0x01 << 2);
            }
        }

        private void btn_pbSensor3_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_pbSensor3.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                btn_pbSensor3.BackColor = System.Drawing.Color.LightGreen;
                pbDetectedSensorMask |= (0x01 << 3);
            }
            else // sensor is enabled (before click)
            {
                btn_pbSensor3.BackColor = System.Drawing.Color.Transparent;
                pbDetectedSensorMask &= ~(0x01 << 3);
            }
        }

        private void btn_pbSensor4_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_pbSensor4.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                btn_pbSensor4.BackColor = System.Drawing.Color.LightGreen;
                pbDetectedSensorMask |= (0x01 << 4);
            }
            else // sensor is enabled (before click)
            {
                btn_pbSensor4.BackColor = System.Drawing.Color.Transparent;
                pbDetectedSensorMask &= ~(0x01 << 4);
            }
        }

        private void btn_pbSensor5_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_pbSensor5.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                btn_pbSensor5.BackColor = System.Drawing.Color.LightGreen;
                pbDetectedSensorMask |= (0x01 << 5);
            }
            else // sensor is enabled (before click)
            {
                btn_pbSensor5.BackColor = System.Drawing.Color.Transparent;
                pbDetectedSensorMask &= ~(0x01 << 5);
            }
        }

        private void btn_pbSensor6_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_pbSensor6.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                btn_pbSensor6.BackColor = System.Drawing.Color.LightGreen;
                pbDetectedSensorMask |= (0x01 << 6);
            }
            else // sensor is enabled (before click)
            {
                btn_pbSensor6.BackColor = System.Drawing.Color.Transparent;
                pbDetectedSensorMask &= ~(0x01 << 6);
            }
        }

        private void btn_pbSensor7_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_pbSensor7.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                btn_pbSensor7.BackColor = System.Drawing.Color.LightGreen;
                pbDetectedSensorMask |= (0x01 << 7);
            }
            else // sensor is enabled (before click)
            {
                btn_pbSensor7.BackColor = System.Drawing.Color.Transparent;
                pbDetectedSensorMask &= ~(0x01 << 7);
            }
        }

        private void btn_pbSensor8_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_pbSensor8.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                btn_pbSensor8.BackColor = System.Drawing.Color.LightGreen;
                pbDetectedSensorMask |= (0x01 << 8);
            }
            else // sensor is enabled (before click)
            {
                btn_pbSensor8.BackColor = System.Drawing.Color.Transparent;
                pbDetectedSensorMask &= ~(0x01 << 8);
            }
        }

        subProcessorConfig subpConfig = new subProcessorConfig();
        // this is for power board emulator to process the packets from the data board
        private void processDbPacket(RawPacket packet)
        {
            Int32 time = 0, date = 0;

            if (packet.Payload[0] == 0x01)  // verify if the packet is coming from Data board
            {
                switch (packet.Payload[1])
                {
                    case 0x51:  // get status request
                        // send status response
                        computeStatusMessage();
                        if (powerBoardPort.IsOpen)
                        {
                            MemoryStream stream = new MemoryStream();
                            byte[] header = { 0x05, 0x52 };
                            stream.Write(header, 0, 2);
                            stream.Write(BitConverter.GetBytes(pbStatusMessage.chargeLevel), 0, 1);   // NOTE: Bit converter is little endian
                            stream.Write(BitConverter.GetBytes(pbStatusMessage.chargerState), 0, 1);
                            stream.Write(BitConverter.GetBytes(pbStatusMessage.usbCommState), 0, 1);
                            stream.Write(BitConverter.GetBytes(pbStatusMessage.jackDetectState), 0, 1);
                            stream.Write(BitConverter.GetBytes(pbStatusMessage.streamState), 0, 1);
                            stream.Write(BitConverter.GetBytes(pbStatusMessage.sensorMask), 0, 4);
                            sendPacketTo(powerBoardPort, stream.ToArray(), 11);
                        }
                        break;

                    case 0x53:  // Sub Processor Config
                        // Display and store the received configuration 
                        subpConfig.dataRate = packet.Payload[2];
                        subpConfig.sensorMask = (UInt32) (packet.Payload[3] | (packet.Payload[4] << 8));
                        debugMessageQueue.Enqueue("Recived sub-processor configuration.\r\n");
                        debugMessageQueue.Enqueue(String.Format("Data rate: {0}, Sensor Mask: 0x{1:X}\r\n", subpConfig.dataRate, subpConfig.sensorMask));
                        break;

                    case 0x54:  // Streaming enable / disable
                        // Start / stop the sensor streaming
                        if (packet.Payload[2] == 0)
                        {
                            debugMessageQueue.Enqueue("Sreaming stopped\r\n");
                            if (cb_streamRawFrames.InvokeRequired)
                            {
                                this.BeginInvoke((MethodInvoker)(() => cb_streamRawFrames.Checked = false));
                            }
                            cb_streamRawFrames_CheckedChanged(cb_streamRawFrames, null);
                        }
                        else
                        {
                            debugMessageQueue.Enqueue("Sreaming started\r\n");
                            if (cb_streamRawFrames.InvokeRequired)
                            {
                                this.BeginInvoke((MethodInvoker)(() => cb_streamRawFrames.Checked = true));
                            }
                            cb_streamRawFrames_CheckedChanged(cb_streamRawFrames, null);
                        }
                        break;

                    case 0x57:  // Power down response
                        // the data board is ready to power down. Display it
                        debugMessageQueue.Enqueue("Received power down response\r\n");
                        break;

                    case 0x58:  // Get date-time request
                        // send current date and time
                        debugMessageQueue.Enqueue("Received get date time request\r\n");
                        getSystemDateTime(ref time, ref date);
                        if (powerBoardPort.IsOpen)
                        {
                            MemoryStream stream = new MemoryStream();
                            byte[] header = { 0x05, 0x59 };
                            stream.Write(header, 0, 2);
                            stream.Write(BitConverter.GetBytes(time), 0, 4);
                            stream.Write(BitConverter.GetBytes(date), 0, 4);
                            sendPacketTo(powerBoardPort, stream.ToArray(), 10);
                        }
                        break;

                    case 0x5a:  // set date-time request
                        // display the received date and time
                        debugMessageQueue.Enqueue("Received set date time request\r\n");
                        displayDateTime(ref packet);
                        if (powerBoardPort.IsOpen)
                        {
                            MemoryStream stream = new MemoryStream();
                            byte[] header = { 0x05, 0x5b, 0x01 };
                            stream.Write(header, 0, 3);
                            sendPacketTo(powerBoardPort, stream.ToArray(), 3);
                        }
                        break;

                    case 0x5c:  // output data
                        // display the received message on the console
                        byte[] debugStr = new byte[100];
                        Buffer.BlockCopy(packet.Payload, 2, debugStr, 0, 100);
                        String asciiString = System.Text.Encoding.ASCII.GetString(debugStr);    // TODO: this will print nulls, solve it
                        debugMessageQueue.Enqueue(asciiString);
                        break;

                    case 0x5d:  // auto power down request
                        // send power down request
                        debugMessageQueue.Enqueue("Received auto power down request.\r\n");
                        btn_sendPwrDwnReq_Click(null, null);
                        break;

                    default:
                        break;
                }
            }
        }

        StatusMessage pbStatusMessage = new StatusMessage();
        private void computeStatusMessage()
        {
            // check the number of sensor available
            if (btn_pbSensor0.BackColor == System.Drawing.Color.LightGreen)
            {
                pbStatusMessage.sensorMask |= (0x01 << 0);        // sensor present
            }
            else
            {
                pbStatusMessage.sensorMask &= Convert.ToUInt32(~(0x01 << 0) & 0xffffffff);    //sensor absent
            }

            if (btn_pbSensor1.BackColor == System.Drawing.Color.LightGreen)
            {
                pbStatusMessage.sensorMask |= (0x01 << 1); // sensor present
            }
            else
            {
                pbStatusMessage.sensorMask &= Convert.ToUInt32(~(0x01 << 1) & 0xffffffff);    //sensor absent
            }

            if (btn_pbSensor2.BackColor == System.Drawing.Color.LightGreen)
            {
                pbStatusMessage.sensorMask |= (0x01 << 2); // sensor present
            }
            else
            {
                pbStatusMessage.sensorMask &= Convert.ToUInt32(~(0x01 << 2) & 0xffffffff);    //sensor absent
            }

            if (btn_pbSensor3.BackColor == System.Drawing.Color.LightGreen)
            {
                pbStatusMessage.sensorMask |= (0x01 << 3); // sensor present
            }
            else
            {
                pbStatusMessage.sensorMask &= Convert.ToUInt32(~(0x01 << 3) & 0xffffffff);    //sensor absent
            }

            if (btn_pbSensor4.BackColor == System.Drawing.Color.LightGreen)
            {
                pbStatusMessage.sensorMask |= (0x01 << 4); // sensor present
            }
            else
            {
                pbStatusMessage.sensorMask &= Convert.ToUInt32(~(0x01 << 4) & 0xffffffff);    //sensor absent
            }

            if (btn_pbSensor5.BackColor == System.Drawing.Color.LightGreen)
            {
                pbStatusMessage.sensorMask |= (0x01 << 5); // sensor present
            }
            else
            {
                pbStatusMessage.sensorMask &= Convert.ToUInt32(~(0x01 << 5) & 0xffffffff);    //sensor absent
            }

            if (btn_pbSensor6.BackColor == System.Drawing.Color.LightGreen)
            {
                pbStatusMessage.sensorMask |= (0x01 << 6); // sensor present
            }
            else
            {
                pbStatusMessage.sensorMask &= Convert.ToUInt32(~(0x01 << 6) & 0xffffffff);    //sensor absent
            }

            if (btn_pbSensor7.BackColor == System.Drawing.Color.LightGreen)
            {
                pbStatusMessage.sensorMask |= (0x01 << 7); // sensor present
            }
            else
            {
                pbStatusMessage.sensorMask &= Convert.ToUInt32(~(0x01 << 7) & 0xffffffff);    //sensor absent
            }

            if (btn_pbSensor8.BackColor == System.Drawing.Color.LightGreen)
            {
                pbStatusMessage.sensorMask |= (0x01 << 8); // sensor present
            }
            else
            {
                pbStatusMessage.sensorMask &= Convert.ToUInt32(~(0x01 << 8) & 0xffffffff);    //sensor absent
            }

            pbStatusMessage.streamState = (byte) nud_pbStreamState.Value;   // stream state
            pbStatusMessage.jackDetectState = (byte)((chb_pbJcDc1.Checked ? 1:0) | ((chb_pbJcDc2.Checked ? 1 : 0) << 1));   // jack detect state
            pbStatusMessage.usbCommState = (byte)(chb_pbUsbComDetected.Checked ? 1 : 0);    // usb comm detect state
            pbStatusMessage.chargerState = (byte)nud_pbChrgState.Value; // charge state
            pbStatusMessage.chargeLevel = (byte)nud_pbChrgLvl.Value;    // charge percentage
        }

        int oldTickValue = 0;
        int newTickValue = Environment.TickCount;
        // thread for the power board emulator to process the process incoming and outgoing messages
        // NOTE: the stream for the full frame data is a different thread
        private void pbProcessData()
        {
            byte receivedByte;
            // send data to the data board and process received bytes
            while (pbProcessDataEnable)
            {
                // dequeue the received data and process it
                if (pbDataReceiveQueue.Count == 0)
                {
                    Thread.Sleep(1);
                    //Thread.Yield();
                }

                if (pbDataReceiveQueue.TryDequeue(out receivedByte))
                {
                    int bytesReceived = powerBoardPacket.BytesReceived + 1;
                    PacketStatus status = powerBoardPacket.processByte((byte)receivedByte);
                    switch (status)
                    {
                        case PacketStatus.PacketComplete:
                            RawPacket packetCopy = new RawPacket(powerBoardPacket);
                            processDbPacket(powerBoardPacket);  // process the incoming data from data board
                            powerBoardPacket.resetPacket();
                            break;
                        case PacketStatus.PacketError:
                            if (chb_dbDataMonitorEnable.Checked)
                            {
                                debugMessageQueue.Enqueue(String.Format("{0} Packet ERROR! {1} bytes received\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond), bytesReceived));
                            }
                            powerBoardPacket.resetPacket();
                            break;
                        case PacketStatus.Processing:
                            break;
                        case PacketStatus.newPacketDetected:
                            break;
                    }
                }

                // if it is more than 5 seconds, send status message
                newTickValue = Environment.TickCount;
                if ((newTickValue - oldTickValue) > 5000)
                {
                    oldTickValue = newTickValue;
                    computeStatusMessage();
                    if (powerBoardPort.IsOpen)
                    {
                        MemoryStream stream = new MemoryStream();
                        byte[] header = { 0x05, 0x52 };
                        stream.Write(header, 0, 2);
                        stream.Write(BitConverter.GetBytes(pbStatusMessage.chargeLevel), 0, 1);   // NOTE: Bit converter is little endian
                        stream.Write(BitConverter.GetBytes(pbStatusMessage.chargerState), 0, 1);
                        stream.Write(BitConverter.GetBytes(pbStatusMessage.usbCommState), 0, 1);
                        stream.Write(BitConverter.GetBytes(pbStatusMessage.jackDetectState), 0, 1);
                        stream.Write(BitConverter.GetBytes(pbStatusMessage.streamState), 0, 1);
                        stream.Write(BitConverter.GetBytes(pbStatusMessage.sensorMask), 0, 4);
                        sendPacketTo(powerBoardPort, stream.ToArray(), 11);
                    }
                }

                // send the requested output data

            }
        }

        RawPacket powerBoardPacket = new RawPacket();
        private void powerBoardPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int bytesToRead = powerBoardPort.BytesToRead;
            while (bytesToRead > 0)
            {
                if (!powerBoardPort.IsOpen)
                {
                    return;
                }
                int receivedByte = powerBoardPort.ReadByte();
                if (receivedByte != -1)
                {
                    //process the byte
                    byte newByte = (byte)receivedByte;
                    // equeue the packet and process it later in other thread
                    pbDataReceiveQueue.Enqueue(newByte);

                }
                bytesToRead = powerBoardPort.BytesToRead;
            }
        }

        private void btn_sendPwrDwnReq_Click(object sender, EventArgs e)
        {
            byte[] header = { 0x05, 0x56 };
            sendPacketTo(powerBoardPort, header, 2);
        }

        private void clb_algorithmStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cb_SetupModeEn_CheckedChanged(object sender, EventArgs e)
        {
            byte[] setupModeBytes = new byte[3];
            setupModeBytes[0] = 0x01;
            setupModeBytes[1] = 0x14;
            if (cb_SetupModeEn.Checked)
            {
                setupModeBytes[2] = 0x01;
            }
            else
            {
                setupModeBytes[2] = 0x00;
            }
            if (serialPort.IsOpen)
            {
                sendPacket(setupModeBytes, 3);
            }
        }

        private void cb_gyroRange_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public static UInt16 ReverseBytes(UInt16 value)
        {
            return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }
        private void btn_SendConfig_Click(object sender, EventArgs e)
        {
            byte[] configBytes = new byte[14];
            configBytes[0] = 0x01;
            configBytes[1] = 0x27; //send config command byte
            configBytes[2] = (byte)nud_SelectedImu.Value; 
            byte algoControl = 0x00;
            //cannot loop through this since there is hole in the middle of the register. 
            
            //check standby checked state
            if (clb_algoConfig.GetItemCheckState(0) == CheckState.Checked)
            {
                algoControl |= (1 << 0); 
            }
            //check raw data state
            if (clb_algoConfig.GetItemCheckState(1) == CheckState.Checked)
            {
                algoControl |= (1 << 1);
            }
            //check HPR checked state
            if (clb_algoConfig.GetItemCheckState(2) == CheckState.Checked)
            {
                algoControl |= (1 << 2);
            }
            //check 6axis checked state
            if (clb_algoConfig.GetItemCheckState(3) == CheckState.Checked)
            {
                algoControl |= (1 << 3);
            }
            //check ENU checked state
            if (clb_algoConfig.GetItemCheckState(4) == CheckState.Checked)
            {
                algoControl |= (1 << 5);
            }
            //check gyro off when still state
            if (clb_algoConfig.GetItemCheckState(5) == CheckState.Checked)
            {
                algoControl |= (1 << 6);
            }
            configBytes[3] = algoControl;
            //check warm start state
            if (clb_algoConfig.GetItemCheckState(6) == CheckState.Checked)
            {
                configBytes[4] = 1;
            }
            else
            {
                configBytes[4] = 0;
            }
            //check the range setting
            if (clb_algoConfig.GetItemCheckState(7) == CheckState.Checked)
            {
                configBytes[5] = 1;
            }
            else
            {
                configBytes[5] = 0;
            }
            //setup the range values; 
            //if no range is selected use the default. 
            if(cb_accelRange.SelectedIndex < 0)
            {
                cb_accelRange.SelectedIndex = 0;
            }
            if(cb_magRange.SelectedIndex < 0)
            {
                cb_magRange.SelectedIndex = 0;
            }
            if(cb_gyroRange.SelectedIndex < 0)
            {
                cb_gyroRange.SelectedIndex = 0;
            }

            UInt16 accelRange = UInt16.Parse(cb_accelRange.Items[cb_accelRange.SelectedIndex].ToString());
            UInt16 magRange = UInt16.Parse(cb_magRange.Items[cb_magRange.SelectedIndex].ToString());            
            UInt16 gyroRange = UInt16.Parse(cb_gyroRange.Items[cb_gyroRange.SelectedIndex].ToString());
            //accelRange = ReverseBytes(accelRange);
            //magRange = ReverseBytes(magRange);
            //gyroRange = ReverseBytes(gyroRange); 

            Buffer.BlockCopy(BitConverter.GetBytes(accelRange), 0, configBytes, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(magRange), 0, configBytes, 6, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(gyroRange), 0, configBytes, 10, 2);
            
          
            if (serialPort.IsOpen)
            {
                sendPacket(configBytes, 14);
            }
        }

        private void btn_getConfig_Click(object sender, EventArgs e)
        {
            sendCommandToSensorId((byte)CommandIds.getConfig, (byte)nud_SelectedImu.Value);
        }

        private void btn_SaveConfig_Click(object sender, EventArgs e)
        {
            sendCommandToSensorId((byte)CommandIds.saveToNvm, (byte)nud_SelectedImu.Value);
        }

        private void btn_setSensorId_Click(object sender, EventArgs e)
        {
            if(cb_SetupModeEn.Checked)
            {
                //set the flag for processing the incoming button press. 
                debugMessageQueue.Enqueue("Waiting for Button press event from sensor\r\n");
                setSensorIdFlag = true; 
            }
            else
            {
                debugMessageQueue.Enqueue("Sensor Must be in setup mode\r\n");
                //the sensor has to be in setup mode for this to work. 
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void btn_pbTogglePort_Click(object sender, EventArgs e)
        {
            if (!togglePbPort)  // try to open the port
            {
                powerBoardPort.PortName = cb_pbComPorts.Items[cb_pbComPorts.SelectedIndex].ToString();
                powerBoardPort.BaudRate = int.Parse(cb_pbBaudRate.Items[cb_pbBaudRate.SelectedIndex].ToString());
                try
                {
                    pbPortOpen = true;
                    powerBoardPort.Open();

                    tb_Console.AppendText("Port: " + powerBoardPort.PortName + " Open\r\n");
                    btn_pbTogglePort.Text = "Close";
                    togglePbPort = !togglePbPort;
                    Thread pbTransmitThread = new Thread(pbProcessData);
                    pbProcessDataEnable = true;
                    pbTransmitThread.Start();
                    gb_pbManualEmulation.Enabled = true;
                }
                catch (Exception ex)
                {
                    tb_Console.AppendText("Failed to open Port: " + powerBoardPort.PortName + " \r\n");
                    tb_Console.AppendText("Exception " + ex.Message + " \r\n");
                    togglePbPort = false;
                    pbPortOpen = false;
                    pbProcessDataEnable = false;  // close the data reveive thread
                }
            }

            else    // try to close the port
            {
                try
                {
                    powerBoardPort.Close();
                    pbPortOpen = false;
                    pbProcessDataEnable = false;
                    tb_Console.AppendText("Port: " + powerBoardPort.PortName + " Closed\r\n");
                    btn_pbTogglePort.Text = "Open";
                    togglePbPort = !togglePbPort;
                    powerBoardPacket.resetPacket();
                    gb_pbManualEmulation.Enabled = false;
                }
                catch
                {
                    tb_Console.AppendText("Failed to close Port: " + powerBoardPort.PortName + "\r\n");
                }
            }
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void chb_dbDataMonitorEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_dbDataMonitorEnable.Checked)
            {
                if (cb_enableStream.Checked)
                {
                    tb_Console.AppendText("Failed: Debug 485 thread writing to chart and table\r\n");
                    chb_dbDataMonitorEnable.Checked = false;
                    return;
                }
                Thread dbDataMonitorThread = new Thread(DataMonitorThread);
                dbDataMonitorEnable = true;
                dbDataMonitorThread.Start();
            }
            else
            {
                dbDataMonitorEnable = false;
            }
        }

        private void cb_dbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedDataType = cb_dbDataType.SelectedIndex;
        }

        private void label7_Click_1(object sender, EventArgs e)
        {

        }

        private void btn_dbEnableSen1_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_dbEnableSen1.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                dbEnableSen1 = true;
                btn_dbEnableSen1.BackColor = System.Drawing.Color.LightGreen;
                setSensorMask |= (0x01 << 1);
            }
            else // sensor is enabled (before click)
            {
                dbEnableSen1 = false;
                btn_dbEnableSen1.BackColor = System.Drawing.Color.Transparent;
                setSensorMask &= ~(0x01 << 1);
            }
        }

        private void btn_dbEnableSen2_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_dbEnableSen2.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                dbEnableSen2 = true;
                btn_dbEnableSen2.BackColor = System.Drawing.Color.LightGreen;
                setSensorMask |= (0x01 << 2);
            }
            else // sensor is enabled (before click)
            {
                dbEnableSen2 = false;
                btn_dbEnableSen2.BackColor = System.Drawing.Color.Transparent;
                setSensorMask &= ~(0x01 << 2);
            }
        }

        private void btn_dbEnableSen3_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_dbEnableSen3.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                dbEnableSen3 = true;
                btn_dbEnableSen3.BackColor = System.Drawing.Color.LightGreen;
                setSensorMask |= (0x01 << 3);
            }
            else // sensor is enabled (before click)
            {
                dbEnableSen3 = false;
                btn_dbEnableSen3.BackColor = System.Drawing.Color.Transparent;
                setSensorMask &= ~(0x01 << 3);
            }
        }

        private void btn_dbEnableSen4_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_dbEnableSen4.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                dbEnableSen4 = true;
                btn_dbEnableSen4.BackColor = System.Drawing.Color.LightGreen;
                setSensorMask |= (0x01 << 4);
            }
            else // sensor is enabled (before click)
            {
                dbEnableSen4 = false;
                btn_dbEnableSen4.BackColor = System.Drawing.Color.Transparent;
                setSensorMask &= ~(0x01 << 4);
            }
        }

        private void btn_dbEnableSen5_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_dbEnableSen5.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                dbEnableSen5 = true;
                btn_dbEnableSen5.BackColor = System.Drawing.Color.LightGreen;
                setSensorMask |= (0x01 << 5);
            }
            else // sensor is enabled (before click)
            {
                dbEnableSen5 = false;
                btn_dbEnableSen5.BackColor = System.Drawing.Color.Transparent;
                setSensorMask &= ~(0x01 << 5);
            }
        }

        private void btn_dbEnableSen6_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_dbEnableSen6.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                dbEnableSen6 = true;
                btn_dbEnableSen6.BackColor = System.Drawing.Color.LightGreen;
                setSensorMask |= (0x01 << 6);
            }
            else // sensor is enabled (before click)
            {
                dbEnableSen6 = false;
                btn_dbEnableSen6.BackColor = System.Drawing.Color.Transparent;
                setSensorMask &= ~(0x01 << 6);
            }
        }

        private void btn_dbEnableSen7_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_dbEnableSen7.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                dbEnableSen7 = true;
                btn_dbEnableSen7.BackColor = System.Drawing.Color.LightGreen;
                setSensorMask |= (0x01 << 7);
            }
            else // sensor is enabled (before click)
            {
                dbEnableSen7 = false;
                btn_dbEnableSen7.BackColor = System.Drawing.Color.Transparent;
                setSensorMask &= ~(0x01 << 7);
            }
        }

        private void btn_dbEnableSen8_Click(object sender, EventArgs e)
        {
            // We will use system colors to detect the state of the button instead of a bool variable
            if (btn_dbEnableSen8.BackColor == System.Drawing.Color.Transparent)  // the sensor is not enabled (before click)
            {
                dbEnableSen8 = true;
                btn_dbEnableSen8.BackColor = System.Drawing.Color.LightGreen;
                setSensorMask |= (0x01 << 8);
            }
            else // sensor is enabled (before click)
            {
                dbEnableSen8 = false;
                btn_dbEnableSen8.BackColor = System.Drawing.Color.Transparent;
                setSensorMask &= ~(0x01 << 8);
            }
        }
    }
}
