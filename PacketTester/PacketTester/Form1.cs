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
        
        enum CommandIds {update=0x11,getFrame,getFrameResp,setupMode,buttonPress,setImuId,setImuIdResp,getStatus,getStatusResp};
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
        }
        private void processProtoPacket(Packet packet)
        {
            switch(packet.type)
            {
                case PacketType.BrainPackVersionResponse:
                    if (packet.brainPackVersionSpecified)
                    {
                        debugMessageQueue.Enqueue("Received Version Response:" +
                            packet.brainPackVersion + "\r\n");
                    }
                    else
                    {
                        debugMessageQueue.Enqueue("Error Version not found\r\n");
                    }
                    break;
                case PacketType.BatteryChargeResponse:
                    debugMessageQueue.Enqueue("Received Battery Charge Response:" +
                        packet.batteryCharge.ToString() + "\r\n");
                    break;
                case PacketType.StateResponse:
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
                    case CommandIds.getStatusResp:
                        debugMessageQueue.Enqueue(processGetStatusResponse(packet));
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
                    , "460800","500000", "921600","1000000"};
            cb_BaudRate.Items.AddRange(baudrates);
            cb_fpBaudRate.Items.AddRange(baudrates);
            cb_fpBaudRate.SelectedIndex = 14;
            cb_BaudRate.SelectedIndex = 12;
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

        private void btn_SetupMode_Click(object sender, EventArgs e)
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

        private void btn_clearScreen_Click(object sender, EventArgs e)
        {
            tb_Console.Clear();
        }
        private bool EnableSocketQueue = false;
        private void udpSocketClientProcess()
        {
            UdpClient udpClient = new UdpClient(6667);
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
                chrt_dataChart.ChartAreas[0].AxisY.Maximum = 1500;
                chrt_dataChart.ChartAreas[0].AxisY.Minimum = -1500;
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
            sendPacket(serializedBytes, numBytes);
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
                sendPacket(serializedBytes, numBytes);
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

        private void btn_dbTogglePort_Click(object sender, EventArgs e)
        {
            if (!toggleDbPort)
            {
                dataBoardPort.PortName = cb_dbComPorts.Items[cb_dbComPorts.SelectedIndex].ToString();
                dataBoardPort.BaudRate = 115200;
                try
                {
                    dbPortOpen = true;
                    dataBoardPort.Open();

                    tb_Console.AppendText("Port: " + dataBoardPort.PortName + " Open\r\n");
                    btn_dbTogglePort.Text = "Close";
                    toggleDbPort = !toggleDbPort;
                }
                catch (Exception ex)
                {
                    tb_Console.AppendText("Failed to open Port: " + dataBoardPort.PortName + " \r\n");
                    tb_Console.AppendText("Exception " + ex.Message + " \r\n");
                    toggleDbPort = false;
                    dbPortOpen = false;
                }
            }

            else
            {
                try
                {
                    dataBoardPort.Close();
                    dbPortOpen = false;
                    tb_Console.AppendText("Port: " + dataBoardPort.PortName + " Closed\r\n");
                    btn_dbTogglePort.Text = "Open";
                    toggleDbPort = !toggleDbPort;
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

        private int convertToBcd(int twoDigitInteger)
        {
            int tens, units;
            tens = twoDigitInteger / 10;
            units = twoDigitInteger % 10;
            return ((tens << 4) | units);
        }

        private void btn_dbSetDateTime_Click(object sender, EventArgs e)
        {
            int dateCentury, dateYear, dateMonth, dateDay, dateDate;
            int timeSeconds, timeMinutes, timeHour, timeAmPm;
            Int32 time, date;
            
            DateTime localDateTime = DateTime.Now;
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
            
            tb_Console.AppendText(localDateTime.ToString() + "\r\n");

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
                if (btn_dbStream.BackColor == System.Drawing.Color.Transparent)
                {
                    byte[] header = { 0x01, 0x54, 0x01 };
                    sendPacketTo(dataBoardPort, header, 3);
                    btn_dbStream.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    byte[] header = { 0x01, 0x54, 0x00 };
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
