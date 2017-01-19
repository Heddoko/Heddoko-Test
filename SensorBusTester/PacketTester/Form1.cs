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

        // Data board emulator part
        public bool toggleDbPort = false;
        public bool dbPortOpen = false;
        public int dbDataRate = 20;
        public Int32 setSensorMask = 0, rxSensorMask = 0;


        public ConcurrentQueue<RawPacket> dataMonitorQueue;
        public UInt32 sensorDataRate = 0, sensorAvgRate = 0;
        public UInt32 curSensorFrameTick = 0, preSensorFrameTick = 0;
        public int dbSensorFrameCount = 0, debugCount = 0;
        public ConcurrentQueue<byte> dbDataReceiveQueue;



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
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["X"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Y"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Z"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["W"].Points.Clear()));

            ImuFrame sensorframe = new ImuFrame();
            RawPacket framePacket = new RawPacket();
            bool receivedPacket = false; 
            graphIndex = 0;
            int failedFrameCount = 0;
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
                        else
                        {
                            failedFrameCount++; 
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

            debugMessageQueue.Enqueue(String.Format("Stream Closed received: {0} Failed frames\r\n", failedFrameCount));
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
            saveToNvm,
            togglePassthrough,
            readEepromPacket,
            eepromPacket,
            writeEepromPacket,
            writeEepromResponse    
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
            //Algorithm Status
            for (int i = 0; i < 5; i++)
            {
                int index = i; 
                if ((imuStatus & (1 << (i+24))) > 0)
                {
                    this.BeginInvoke((MethodInvoker)delegate () { clb_algorithmStatus.SetItemCheckState(index, CheckState.Checked); });
                   
                }
                else
                {
                    this.BeginInvoke((MethodInvoker)delegate () { clb_algorithmStatus.SetItemCheckState(index, CheckState.Unchecked); });                    
                }
            }
            //Sentral Status 3rd byte, starts in list at 12
            for (int i = 0; i < 5; i++)
            {
                int index = i + 12;
                if ((imuStatus & (1 << (i + 16))) > 0)
                {
                    this.BeginInvoke((MethodInvoker)delegate () { clb_sensorStatus.SetItemCheckState(index, CheckState.Checked); });

                }
                else
                {
                    this.BeginInvoke((MethodInvoker)delegate () { clb_sensorStatus.SetItemCheckState(index, CheckState.Unchecked); });
                }
            }
            //Sensor Status 2nd, starts in list at 6
            for (int i = 0; i < 6; i++)
            {
                int index = i + 6;
                if ((imuStatus & (1 << (i + 8))) > 0)
                {
                    this.BeginInvoke((MethodInvoker)delegate () { clb_sensorStatus.SetItemCheckState(index, CheckState.Checked); });

                }
                else
                {
                    this.BeginInvoke((MethodInvoker)delegate () { clb_sensorStatus.SetItemCheckState(index, CheckState.Unchecked); });
                }
            }
            //Event Status 1st byte, starts in list at 0
            for (int i = 0; i < 6; i++)
            {
                int index = i;
                if ((imuStatus & (1 << (i))) > 0)
                {
                    this.BeginInvoke((MethodInvoker)delegate () { clb_sensorStatus.SetItemCheckState(index, CheckState.Checked); });

                }
                else
                {
                    this.BeginInvoke((MethodInvoker)delegate () { clb_sensorStatus.SetItemCheckState(index, CheckState.Unchecked); });
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
                    //this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData1.Text = frame.Quaternion_x.ToString(); ; });
                    this.BeginInvoke((MethodInvoker)delegate () 
                    {
                        lbl_valueData1.Text = frame.Quaternion_x.ToString();
                        lbl_valueData2.Text = frame.Quaternion_y.ToString();
                        lbl_valueData3.Text = frame.Quaternion_z.ToString();
                        lbl_valueData4.Text = frame.Quaternion_w.ToString();
                    });
                    //this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData2.Text = frame.Quaternion_y.ToString(); ; });
                    //this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData3.Text = frame.Quaternion_z.ToString(); ; });
                    //this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData4.Text = frame.Quaternion_w.ToString(); ; });
                    break;
                case 1:
                    //this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData1.Text = frame.Magnetic_x.ToString(); ; });
                    //this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData2.Text = frame.Magnetic_y.ToString(); ; });
                    //this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData3.Text = frame.Magnetic_z.ToString(); ; });
                    this.BeginInvoke((MethodInvoker)delegate ()
                    {
                        lbl_valueData1.Text = frame.Magnetic_x.ToString();
                        lbl_valueData2.Text = frame.Magnetic_y.ToString();
                        lbl_valueData3.Text = frame.Magnetic_z.ToString();
                        lbl_valueData4.Text = "0";
                    });
                    break;
                case 2:
                    //this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData1.Text = frame.Acceleration_x.ToString(); ; });
                    //this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData2.Text = frame.Acceleration_y.ToString(); ; });
                    //this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData3.Text = frame.Acceleration_z.ToString(); ; });
                    this.BeginInvoke((MethodInvoker)delegate ()
                    {
                        lbl_valueData1.Text = frame.Acceleration_x.ToString();
                        lbl_valueData2.Text = frame.Acceleration_y.ToString();
                        lbl_valueData3.Text = frame.Acceleration_z.ToString();
                        lbl_valueData4.Text = "0";
                    });
                    break;
                case 3:
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData1.Text = frame.Rotation_x.ToString(); ; });
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData2.Text = frame.Rotation_y.ToString(); ; });
                    this.BeginInvoke((MethodInvoker)delegate () { lbl_valueData3.Text = frame.Rotation_z.ToString(); ; });
                    this.BeginInvoke((MethodInvoker)delegate ()
                    {
                        lbl_valueData1.Text = frame.Rotation_x.ToString();
                        lbl_valueData2.Text = frame.Rotation_y.ToString();
                        lbl_valueData3.Text = frame.Rotation_z.ToString();
                        lbl_valueData4.Text = "0";
                    });
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
                    //this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["X"].Points.AddY((double)frame.Quaternion_x)));
                    //this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Y"].Points.AddY((double)frame.Quaternion_y)));
                    //this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Z"].Points.AddY((double)frame.Quaternion_z)));
                    //this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["W"].Points.AddY((double)frame.Quaternion_w)));
                    this.BeginInvoke((MethodInvoker)delegate ()
                    {
                        chrt_dataChart.Series["X"].Points.AddY((double)frame.Quaternion_x);
                        chrt_dataChart.Series["Y"].Points.AddY((double)frame.Quaternion_y);
                        chrt_dataChart.Series["Z"].Points.AddY((double)frame.Quaternion_z);
                        chrt_dataChart.Series["W"].Points.AddY((double)frame.Quaternion_w);
                    });

                }
                catch (OverflowException)
                {
                    tb_Console.AppendText("Overflow exception caused here.\r\n");
                }
            }
            else if(selectedDataType == 1) //Magnetic
            {
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    chrt_dataChart.Series["X"].Points.AddY(System.Convert.ToDouble(frame.Magnetic_x));
                    chrt_dataChart.Series["Y"].Points.AddY(System.Convert.ToDouble(frame.Magnetic_y));
                    chrt_dataChart.Series["Z"].Points.AddY(System.Convert.ToDouble(frame.Magnetic_z));
                });
                
            }
            else if (selectedDataType == 2) //Acceleration
            {
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    chrt_dataChart.Series["X"].Points.AddY(System.Convert.ToDouble(frame.Acceleration_x));
                    chrt_dataChart.Series["Y"].Points.AddY(System.Convert.ToDouble(frame.Acceleration_y));
                    chrt_dataChart.Series["Z"].Points.AddY(System.Convert.ToDouble(frame.Acceleration_z));
                });
                
            }
            else if (selectedDataType == 3) //Rotation
            {
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    chrt_dataChart.Series["X"].Points.AddY(System.Convert.ToDouble(frame.Rotation_x));
                    chrt_dataChart.Series["Y"].Points.AddY(System.Convert.ToDouble(frame.Rotation_y));
                    chrt_dataChart.Series["Z"].Points.AddY(System.Convert.ToDouble(frame.Rotation_z));
                });
            }

            if (chrt_dataChart.Series["X"].Points.Count > graphMaxSize)
            {
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    chrt_dataChart.Series["X"].Points.RemoveAt(0);
                    chrt_dataChart.Series["Y"].Points.RemoveAt(0);
                    chrt_dataChart.Series["Z"].Points.RemoveAt(0);
                    if (selectedDataType == 0)
                    {
                        chrt_dataChart.Series["W"].Points.RemoveAt(0);
                    }
                });

                //this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["X"].Points.RemoveAt(0)));
                //this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Y"].Points.RemoveAt(0)));
                //this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Z"].Points.RemoveAt(0)));
                //if (selectedDataType == 0)
                //{
                //    this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["W"].Points.RemoveAt(0)));
                //}

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
            tmr_transferTimer.Stop(); //stop the timeout timer
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
                        ImuFrame frame = new ImuFrame(packet);
                        updateChart(frame);
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
                        debugMessageQueue.Enqueue("Received Config Response!\r\nConfig Reg=" +packet.Payload[3].ToString("x")  +"\r\n");
                        processGetConfigResponse(packet);
                        break;
                    case CommandIds.writeEepromResponse:
                        processWriteEepromResponse(packet); 
                        break;
                    case CommandIds.eepromPacket:
                        processEepromPacket(packet);
                        break;
                    default:
                        break;
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
            string[] baudrates = { "110", "150", "300", "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200", "230400"
                    , "460800","500000", "921600","1000000","2000000"};
            cb_BaudRate.Items.AddRange(baudrates);
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
            string[] dataTypes = { "Quaternion", "Magnetic","Acceleration", "Rotation" };
            cb_dataType.Items.AddRange(dataTypes);
            cb_dataType.SelectedIndex = 0;

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


            try
            {
                serialPort.PortName = cb_serialPorts.Items[cb_serialPorts.SelectedIndex].ToString();
                serialPort.BaudRate = int.Parse(cb_BaudRate.Items[cb_BaudRate.SelectedIndex].ToString());
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
            streamDataToChartEnabled = false;
            Thread.Sleep(400);
            processPacketQueueEnabled = false; 
            streamDataEnabled = false;     
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
        private void sendCommandToSensorId(byte commandId, byte sensorId, byte payload)
        {
            if (serialPort.IsOpen)
            {
                byte[] commandBytes = new byte[4];
                commandBytes[0] = 0x01;
                commandBytes[1] = commandId;
                commandBytes[2] = sensorId;
                commandBytes[3] = payload;
                sendPacket(commandBytes, 4);
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
        private void btn_startStream_Click(object sender, EventArgs e)
        {
            if (!streamDataEnabled)
            {
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
        }

        private void cb_dataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //clear the data
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["X"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Y"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Z"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["W"].Points.Clear()));
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

        private void btn_refreshComPorts_Click(object sender, EventArgs e)
        {
            cb_serialPorts.Items.Clear();
            cb_serialPorts.Items.AddRange(SerialPort.GetPortNames());

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void sfd_saveFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
        
        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        
        private void label7_Click(object sender, EventArgs e)
        {

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
            if(streamDataEnabled || streamDataToChartEnabled)
            {
                MessageBox.Show("Cannot configure sensor while streaming.\r\nDisable Streaming to continue", "ERROR!!!", MessageBoxButtons.OK);
                return; 
            }

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

            Buffer.BlockCopy(BitConverter.GetBytes(accelRange), 0, configBytes, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(magRange), 0, configBytes, 6, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(gyroRange), 0, configBytes, 10, 2);
            
          
            if (serialPort.IsOpen)
            {
                sendPacket(configBytes, 14);
            }
        }
        void sendEepromPacket(UInt16 address, byte[] data)
        {
            byte[] outputPacket = new byte[69];
            outputPacket[0] = 0x01;
            outputPacket[1] = 0x2E; //write eeprom command code
            outputPacket[2] = (byte)nud_SelectedImu.Value; //selected IMU
            //copy the address
            Buffer.BlockCopy(BitConverter.GetBytes(address), 0, outputPacket, 3, 2);
            //copy the data
            Buffer.BlockCopy(data, 0, outputPacket, 5, data.Length);
            sendPacket(outputPacket, 69);

        }
        void sendGetEepromPacket(UInt16 address)
        {
            byte[] outputPacket = new byte[5];
            outputPacket[0] = 0x01;
            outputPacket[1] = 0x2C; //write eeprom command code
            outputPacket[2] = (byte)nud_SelectedImu.Value; //selected IMU
            //copy the address
            Buffer.BlockCopy(BitConverter.GetBytes(address), 0, outputPacket, 3, 2);
            sendPacket(outputPacket, 5);

        }
        const int eepromSize = 32000; 
        byte[] eepromData = new byte[eepromSize];
        long eepromDataLength = 0;
        long eepromDataTransferedLength = 0;
        bool eepromTransferInProgress = false;
        string eepromFilename = "";
        private void btn_sendEepromFile_Click(object sender, EventArgs e)
        {

            if (eepromTransferInProgress)
            {
                debugMessageQueue.Enqueue("Error: Transfer In Progress\r\n");
                return; 
            }
            //open the file
            if(ofd_openFile.ShowDialog() != DialogResult.OK)
            {
                return; 
            }
            FileStream eepromFile = File.Open(ofd_openFile.FileName, FileMode.Open); 
            if(eepromFile.Length > eepromSize)
            {
                debugMessageQueue.Enqueue("Error: EEPROM File too large\r\n");
                eepromFile.Close();
                return;
            }
            eepromFile.Read(eepromData, 0, eepromData.Length);
            eepromDataLength = eepromFile.Length;
            eepromDataTransferedLength = 0;
            //turn on the passthrough functionality
            sendCommandToSensorId(0x2B, (byte)nud_SelectedImu.Value, 0x01);
            Thread.Sleep(1000);
            eepromTransferInProgress = true; 
            //send the first packet
            byte[] packet = new byte[64];
            Buffer.BlockCopy(eepromData, 0, packet, 0, 64);
            sendEepromPacket(0, packet);
            //TODO: start timeout timer... add this in next. 
            tmr_transferTimer.Interval = 1000; //set the interval for 1 second
            tmr_transferTimer.Start(); 
        }
        private void processWriteEepromResponse(RawPacket packet)
        {
            byte[] eepromPacket = new byte[64];
            int address = (packet.Payload[3] << 8) + packet.Payload[2];
            int transferSize = 0;
            tmr_transferTimer.Stop(); //stop the timeout timer
            if (packet.Payload[4] == 0x01) //the last packet was written correctly
            {
                //calculate if we need to write more bytes
                eepromDataTransferedLength += 64;
                if (eepromDataTransferedLength >= eepromDataLength)
                {
                    debugMessageQueue.Enqueue("Transfer Complete\r\n");
                    sendCommandToSensorId(0x2B, (byte)nud_SelectedImu.Value, 0x00);
                    eepromTransferInProgress = false;
                    return;
                }
                if (eepromDataLength < (eepromDataTransferedLength + 64))
                {
                    transferSize = (int)(eepromDataLength - eepromDataTransferedLength);
                }
                else
                {
                    transferSize = 64;
                }
                Buffer.BlockCopy(eepromData, (int)eepromDataTransferedLength, eepromPacket, 0, transferSize);
                sendEepromPacket((UInt16)eepromDataTransferedLength, eepromPacket);
                tmr_transferTimer.Start(); //start the timeout timer again
            }
            else
            {
                //the transfer failed... turn off the passthrough. 
                sendCommandToSensorId(0x2B, (byte)nud_SelectedImu.Value, 0x00);
                eepromTransferInProgress = false;
                debugMessageQueue.Enqueue("Transfer packet failed\r\n");
            }
        }
        private void processEepromPacket(RawPacket packet)
        {
            int address = (packet.Payload[3] << 8) + packet.Payload[2];
            int transferSize = 64;
            tmr_transferTimer.Stop(); //stop the timeout timer
            //copy the eeprom packet into the buffer at the given address. 
            if(packet.PayloadSize != 68 && address > eepromSize)
            {
                //the transfer failed... turn off the passthrough. 
                sendCommandToSensorId(0x2B, (byte)nud_SelectedImu.Value, 0x00);
                eepromTransferInProgress = false;
                debugMessageQueue.Enqueue("Transfer Failed: invalid packet size\r\n");
            }
            Buffer.BlockCopy(packet.Payload, 4, eepromData, address, transferSize);
           
            eepromDataTransferedLength += transferSize; 
            if(eepromDataTransferedLength == eepromSize)
            {
                //the transfer is complete... turn off the passthrough. 
                sendCommandToSensorId(0x2B, (byte)nud_SelectedImu.Value, 0x00);
                eepromTransferInProgress = false;
                debugMessageQueue.Enqueue("Transfer Complete\r\n");
                try
                {
                    FileStream outputFile = File.Open(eepromFilename, FileMode.CreateNew);
                    outputFile.Write(eepromData, 0, eepromSize);
                    outputFile.Close(); 
                }
                catch
                {
                    debugMessageQueue.Enqueue("Failed to write file\r\n");
                }
            }
            else
            {
                //now ask for the next packet
                sendGetEepromPacket((UInt16)eepromDataTransferedLength);
                tmr_transferTimer.Interval = 1000; //set the timeout for 1 second
                tmr_transferTimer.Start(); //start the timeout timer again
            }
            
        }
        private void btn_cancelTransfer_Click(object sender, EventArgs e)
        {
            //disable passthrough
            sendCommandToSensorId(0x2B, (byte)nud_SelectedImu.Value, 0x00);
            eepromTransferInProgress = false;
            debugMessageQueue.Enqueue("Transfer Cancelled\r\n");
            eepromDataTransferedLength = 0;
        }

        private void btn_getEepromFile_Click(object sender, EventArgs e)
        {
            if (eepromTransferInProgress)
            {
                debugMessageQueue.Enqueue("Error: Transfer In Progress\r\n");
                return;
            }
            sfd_saveFileDialog.Filter = "Binary Files (*.bin)|*.bin|All Files (*.*)|*.*";
            if (sfd_saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            eepromDataTransferedLength = 0;
            eepromFilename = sfd_saveFileDialog.FileName;
            
            //enable passthrough
            sendCommandToSensorId(0x2B, (byte)nud_SelectedImu.Value, 0x01);
            Thread.Sleep(1000); 
            //send get file
            sendGetEepromPacket(0);
            eepromTransferInProgress = true;
            tmr_transferTimer.Interval = 1000; //set the timeout for 1 second
            tmr_transferTimer.Start(); //start the timeout timer again
        }

        private void tmr_transferTimer_Tick(object sender, EventArgs e)
        {
            tmr_transferTimer.Stop(); //stop the timer
            //disable passthrough
            sendCommandToSensorId(0x2B, (byte)nud_SelectedImu.Value, 0x00);
            eepromTransferInProgress = false;
            debugMessageQueue.Enqueue("Transfer Timed out!\r\n");
            eepromDataTransferedLength = 0;
        }

        private void btn_getConfig_Click(object sender, EventArgs e)
        {
            if (streamDataEnabled || streamDataToChartEnabled)
            {
                MessageBox.Show("Cannot get sensor config while streaming.\r\nDisable Streaming to continue", "ERROR!!!", MessageBoxButtons.OK);
                return;
            }
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



        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }




        private void label7_Click_1(object sender, EventArgs e)
        {

        }



    }
}
