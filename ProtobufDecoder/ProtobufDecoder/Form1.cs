using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketTester;
using System.Collections.Concurrent;
using System.IO.Ports;
using System.Threading;
using heddoko;
using ProtoBuf;

namespace ProtobufDecoder
{
    public partial class form_decoder : Form
    {
        private bool processDebugThreadEnabled = false;
        public ConcurrentQueue<string> debugMessageQueue;
        private bool processPacketQueueEnabled = false;
        public ConcurrentQueue<RawPacket> packetQueue;
        enum CommandIds { update = 0x11, getFrame, getFrameResp, setupMode, buttonPress, setImuId, setImuIdResp, getStatus, getStatusResp };
        enum OutputType { legacyFrame, appFormat, individualSensorFiles };
        OutputType decodingOutput = OutputType.individualSensorFiles;
        public form_decoder()
        {
            InitializeComponent();
        }

        private void form_decoder_Load(object sender, EventArgs e)
        {
            cb_serialPorts.Items.AddRange(SerialPort.GetPortNames());
            cb_forwardPorts.Items.AddRange(SerialPort.GetPortNames());
            string[] baudrates = { "110", "150", "300", "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200", "230400"
                    , "460800","500000", "921600","1000000"};
            cb_BaudRate.Items.AddRange(baudrates);
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
        }
        private void processProtoPacket(Packet packet)
        {
            switch (packet.type)
            {
                case PacketType.AdvertisingPacket:
                    if (packet.firmwareVersionSpecified)
                    {
                        debugMessageQueue.Enqueue("Received Advertising Packet:" +
                            packet.firmwareVersion + "\r\n");
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
                    if (packet.fullDataFrame != null)
                    {
                        string convertedFrame = convertProtoBufPacket(packet); 
                        if(sp_foward.IsOpen && convertedFrame.Length > 0)
                        {
                            sp_foward.Write(convertedFrame);
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
                switch (commandId)
                {
                    case CommandIds.buttonPress:
                        debugMessageQueue.Enqueue(String.Format("{0}:Received Button Press for serial#\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));
                        break;
                    case CommandIds.getFrameResp:
                        //debugMessageQueue.Enqueue(String.Format("{0}:Received Frame\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));

                        ImuFrame frame = new ImuFrame(packet);
                        //debugMessageQueue.Enqueue(frame.ToString());
                        break;
                    case CommandIds.setImuIdResp:
                        debugMessageQueue.Enqueue(String.Format("{0}:Received set Imu Resp\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));
                        break;
                    case CommandIds.getStatusResp:
                        debugMessageQueue.Enqueue("Recieved Get status\r\n");
                        break;
                    default:
                        break;
                }
            }
            else if (packet.Payload[0] == 0x04) //this is a protocol buffer file. 
            {
                Stream stream = new MemoryStream(packet.Payload, 1, packet.PayloadSize - 1);
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
            while (processPacketQueueEnabled)
            {
                RawPacket packet;
                if (packetQueue.Count > 0)
                {
                    if (packetQueue.TryDequeue(out packet))
                    {
                        processPacket(packet);
                    }
                }
            }
        }


        private string swapHexBytes(string hexString)
        {
            return hexString[2].ToString() + hexString[3].ToString() + hexString[0].ToString() + hexString[1].ToString();
        }
        private string ConvertFloatToHex(float value)
        {
            Int16 convertedValue = (Int16)(value * 8192);
            string convertedString = convertedValue.ToString("X4");
            return swapHexBytes(convertedString);
        }


        private string convertProtoBufPacket(Packet heddokoPacket)
        {
            StringBuilder retString = new StringBuilder();
            if (heddokoPacket.type == PacketType.DataFrame)
            {
                //hard coding mask for now
                retString.Append( heddokoPacket.fullDataFrame.timeStamp.ToString().PadLeft(10,'0') + ",3ff,");
                for (int i = 0; i < heddokoPacket.fullDataFrame.imuDataFrame.Count; i++)
                {                    
                    //do stuff for each frame received.
                    retString.Append(ConvertFloatToHex(heddokoPacket.fullDataFrame.imuDataFrame[i].quat_z_roll)+";");
                    retString.Append(ConvertFloatToHex(heddokoPacket.fullDataFrame.imuDataFrame[i].quat_y_pitch) + ";");
                    retString.Append(ConvertFloatToHex(heddokoPacket.fullDataFrame.imuDataFrame[i].quat_x_yaw) + ",");
                }
                for (int i = 0; i < 9 - heddokoPacket.fullDataFrame.imuDataFrame.Count; i++)
                {
                    retString.Append("0000;0000;0000,");
                }
                retString.Append("1234;BBBB;CCCC;DDDD;EEEE,\r\n");
                return retString.ToString(); 
            }
            return "";
        }
        private uint startTime = 0;
        private string convertProtoPacketToAppFormat(Packet heddokoPacket)
        {
            StringBuilder retString = new StringBuilder();
            if (heddokoPacket.type == PacketType.DataFrame)
            {
                //hard coding mask for now
                if(startTime == 0)
                {
                    startTime = heddokoPacket.fullDataFrame.timeStamp;
                }
                double timeStamp = (double)(heddokoPacket.fullDataFrame.timeStamp - startTime)/1000; 
                retString.Append(timeStamp.ToString() + ",");
                for (int i = 0; i < heddokoPacket.fullDataFrame.imuDataFrame.Count; i++)
                {
                    //do stuff for each frame received.
                    if(heddokoPacket.fullDataFrame.imuDataFrame[i].imuId != i)
                    {
                        debugMessageQueue.Enqueue("IMU ID: i=" + i.ToString() + " imuId=" + heddokoPacket.fullDataFrame.imuDataFrame[i].imuId.ToString() +  " failure at: " + timeStamp.ToString("F3") + "\r\n");
                    }
                    retString.Append(heddokoPacket.fullDataFrame.imuDataFrame[i].imuId.ToString() + ",");
                    retString.Append(heddokoPacket.fullDataFrame.imuDataFrame[i].quat_z_roll.ToString("F3") + ";");
                    retString.Append(heddokoPacket.fullDataFrame.imuDataFrame[i].quat_y_pitch.ToString("F3") + ";");
                    retString.Append(heddokoPacket.fullDataFrame.imuDataFrame[i].quat_x_yaw.ToString("F3") + ",");
                }
                for (int i = 0; i < 10 - heddokoPacket.fullDataFrame.imuDataFrame.Count; i++)
                {
                    retString.Append("9,0.0;0.0;0.0,");
                }
                retString.Append("1234,BBBB,CCCC,DDDD,EEEE\r\n");
                return retString.ToString();
            }
            return "";
        }


        private string processRawPacket(RawPacket packet)
        {
            if (packet.Payload[0] == 0x04) //this is a protocol buffer file. 
            {
                Stream stream = new MemoryStream(packet.Payload, 1, packet.PayloadSize - 1);
                try
                {
                    Packet protoPacket = Serializer.Deserialize<Packet>(stream);
                    if(protoPacket.type == PacketType.DataFrame)
                    {
                        if (decodingOutput == OutputType.appFormat)
                        {
                            return convertProtoPacketToAppFormat(protoPacket); 
                        }
                        else if(decodingOutput == OutputType.legacyFrame)
                        {
                            return convertProtoBufPacket(protoPacket);
                        }
                        else
                        {
                            //convert to the csv
                        }
                    }
                }
                catch
                {
                    debugMessageQueue.Enqueue("Failed to deserialize packet\r\n");
                }

            }
            return ""; 
        }
        private string getHprStringFromQuaternions(ImuDataFrame dataFrame)
        {
            StringBuilder strBuilder = new StringBuilder();
            float Heading = (float)((180 / Math.PI) * Math.Atan2((2 * dataFrame.quat_x_yaw * dataFrame.quat_y_pitch + 2 * dataFrame.quat_w * dataFrame.quat_z_roll), (2 * dataFrame.quat_w * dataFrame.quat_w + 2 * dataFrame.quat_x_yaw * dataFrame.quat_x_yaw - 1)));
            float Pitch = (float)((180 / Math.PI) * Math.Asin(-(2 * dataFrame.quat_x_yaw * dataFrame.quat_z_roll - 2 * dataFrame.quat_w * dataFrame.quat_y_pitch)));
            float Roll = (float)((180 / Math.PI) * Math.Atan2((2 * dataFrame.quat_y_pitch * dataFrame.quat_z_roll + 2 * dataFrame.quat_w * dataFrame.quat_x_yaw), (2 * dataFrame.quat_w * dataFrame.quat_w + 2 * dataFrame.quat_z_roll * dataFrame.quat_z_roll - 1)));
            if (Heading < 0)
                Heading += 360;
            strBuilder.Append(String.Format(",{0:F4},{1:F4},{2:F4}", Heading, Pitch, Roll));
            return strBuilder.ToString();

        }
        private string getHprStringFromQuaternionsApdx2(ImuDataFrame dataFrame)
        {
            StringBuilder strBuilder = new StringBuilder();
            double qx = dataFrame.quat_x_yaw;
            double qy = dataFrame.quat_y_pitch;
            double qz = dataFrame.quat_z_roll;
            double qw = dataFrame.quat_w;
            double Heading = (180 / Math.PI) * Math.Atan2(Math.Pow(qx, 2) - Math.Pow(qy, 2) - Math.Pow(qz, 2) + Math.Pow(qw, 2), (2 * (qx*qy + qz*qw)) );
            double Pitch = (180 / Math.PI) * Math.Asin(-2*((qx*qz)-(qy*qw)));
            double Roll = (180 / Math.PI) * Math.Atan2((-(Math.Pow(qx, 2))- Math.Pow(qy, 2) + Math.Pow(qz, 2) + Math.Pow(qw, 2)) , (2 * (qx*qw + qy*qz)));
            //if (Heading < 0)
            //    Heading += 360;
            strBuilder.Append(String.Format(",{0:F4},{1:F4},{2:F4}", Heading, Pitch, Roll));
            return strBuilder.ToString();

        }
        private string getHprStringFromAccelAndMag(ImuDataFrame dataFrame)
        {
            StringBuilder strBuilder = new StringBuilder();
            Double Heading = 0.0, Pitch = 0.0, Roll = 0.0;
            Double tempM, tempA;
            Double[] mag = { dataFrame.Mag_x, dataFrame.Mag_y, dataFrame.Mag_z };
            Double[] acc = { ((double)dataFrame.Accel_x/512f)*9.81, ((double)dataFrame.Accel_y / 512f)*9.81, ((double)dataFrame.Accel_z / 512f)*9.81 };
            //heading
            tempM = Math.Pow(mag[0], 2) + Math.Pow(mag[1], 2) + Math.Pow(mag[2], 2);
            tempM = Math.Sqrt(tempM);

            tempA = Math.Pow(acc[0], 2) + Math.Pow(acc[1], 2) + Math.Pow(acc[2], 2);
            tempA = Math.Sqrt(tempA);

            for (int i = 0; i < 3; i++)
            {
                mag[i] = mag[i] / tempM;
                acc[i] = acc[i] / tempA;
            }

            tempM = mag[0] * (Math.Pow(acc[1], 2) + Math.Pow(acc[2], 2))
                    - mag[1] * acc[0] * acc[1]
                    - mag[2] * acc[0] * acc[2];

            if (tempM != 0.0)
            {
                Heading = (180 / Math.PI) * Math.Atan2((mag[2] * acc[1] - mag[1] * acc[2]), tempM);
            }

            if (Heading < 0.0)
            {
                Heading += 360;
            }

            //pitch & roll
            // get pitch and roll angles, convert to an angle in degrees
            tempA = Math.Sqrt(Math.Pow(acc[1], 2) + Math.Pow(acc[2], 2));

            if (tempA == 0.0)
            {
                if (acc[0] > 0.0)
                {
                    Pitch = -90;
                }
                else
                {
                    Pitch = 90;
                }
            }
            else
            {
                Pitch = Math.Atan(-acc[0] / tempA) * (180 / Math.PI);
            }

            if (acc[2] == 0.0)
            {
                if (acc[1] > 0.0)
                {
                    Roll = 90;
                }
                else
                {
                    if (acc[1] < 0.0)
                    {
                        Roll = -90;
                    }
                    else
                    {
                        Roll = 0;
                    }
                }
            }
            else
            {
                Roll = Math.Atan2(acc[1], acc[2]) * (180 / Math.PI);
            }
            strBuilder.Append(String.Format(",{0:F4},{1:F4},{2:F4}", Heading, Pitch, Roll));
            return strBuilder.ToString();

        }
        private void convertImuDataFrameToString(long timeStamp, ImuDataFrame dataFrame, ref FileStream outputFile)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(timeStamp.ToString("D10") + "," + dataFrame.imuId.ToString() + "," );
            strBuilder.Append(String.Format("{0:F4},{1:F4},{2:F4},{3:F4},", dataFrame.quat_x_yaw, dataFrame.quat_y_pitch, dataFrame.quat_z_roll, dataFrame.quat_w));            
            strBuilder.Append(String.Format("{0},{1},{2},", dataFrame.Mag_x, dataFrame.Mag_y, dataFrame.Mag_z));
            strBuilder.Append(String.Format("{0},{1},{2},", dataFrame.Accel_x, dataFrame.Accel_y, dataFrame.Accel_z));
            strBuilder.Append(String.Format("{0},{1},{2}", dataFrame.Rot_x.ToString(), dataFrame.Rot_y.ToString(), dataFrame.Rot_z.ToString()));
            strBuilder.Append(getHprStringFromQuaternions(dataFrame));
            //strBuilder.Append(getHprStringFromQuaternionsApdx2(dataFrame));            
            uint calStable = (dataFrame.sensorMask >> 19) & 0x01; 
            uint magTransient = (dataFrame.sensorMask >> 20) & 0x01;
            strBuilder.Append(String.Format(",{0},{1}", calStable.ToString(), magTransient.ToString()));             
            strBuilder.Append("\r\n");
            if (outputFile.CanWrite)
            {
                outputFile.Write(ASCIIEncoding.ASCII.GetBytes(strBuilder.ToString()), 0, strBuilder.Length);
                //outputFile.Flush();
            }
            return; 
        }
        private void processRawPacketForCSV(RawPacket packet, ref FileStream[] outputFiles)
        {
            if (packet.Payload[0] == 0x04) //this is a protocol buffer file. 
            {
                Stream stream = new MemoryStream(packet.Payload, 1, packet.PayloadSize - 1);
                try
                {
                    Packet protoPacket = Serializer.Deserialize<Packet>(stream);
                    if (protoPacket.type == PacketType.DataFrame)
                    {
                        for(int i = 0; i<protoPacket.fullDataFrame.imuDataFrame.Count; i++)
                        {
                            if(protoPacket.fullDataFrame.imuDataFrame[i].imuId < 9)
                            {
                                convertImuDataFrameToString(protoPacket.fullDataFrame.timeStamp, protoPacket.fullDataFrame.imuDataFrame[i], ref outputFiles[protoPacket.fullDataFrame.imuDataFrame[i].imuId]);
                            }
                        }
                    }
                }
                catch
                {
                    debugMessageQueue.Enqueue("Failed to deserialize packet\r\n");
                }

            }
            return;
        }
        private void btn_decode_Click(object sender, EventArgs e)
        {
            //RawPacket packet = new RawPacket();
            if(bgw_ProcessingWorker.IsBusy)
            {
                debugMessageQueue.Enqueue(String.Format("Cannot decode file, waiting for completion\r\n"));
            }
            if (ofd_OpenFile.ShowDialog() != DialogResult.OK)
            {
                return; 
            }
            if(sfd_SaveFile.ShowDialog() != DialogResult.OK)
            {
                return; 
            }
            bgw_ProcessingWorker.RunWorkerAsync();
            
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
        private void form_decoder_FormClosing(object sender, FormClosingEventArgs e)
        {
            processDebugThreadEnabled = false;
            processPacketQueueEnabled = false; 
        }
        private void bnt_Connect_Click(object sender, EventArgs e)
        {
            //set the serial port to the selected item. 
            //Thread serialThread = new Thread(ReadThread);
            if(cb_serialPorts.SelectedIndex < 0)
            {
                return; 
            }
            if (sp_recieve.IsOpen)
            {
                //do nothing
                return;
            }
            sp_recieve.PortName = cb_serialPorts.Items[cb_serialPorts.SelectedIndex].ToString();
            sp_recieve.BaudRate = int.Parse(cb_BaudRate.Items[cb_BaudRate.SelectedIndex].ToString());
            try
            {
                sp_recieve.Open();
                tb_Console.AppendText("Port: " + sp_recieve.PortName + " Open\r\n");
            }
            catch (Exception ex)
            {
                tb_Console.AppendText("Failed to open Port: " + sp_recieve.PortName + " \r\n");
                tb_Console.AppendText("Exception " + ex.Message + " \r\n");
            }
        }

        RawPacket rawPacket = new RawPacket();
        private void sp_foward_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

        }

        private void sp_recieve_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                while (sp_recieve.BytesToRead > 0)
                {
                    int receivedByte = sp_recieve.ReadByte();
                    if (receivedByte != -1)
                    {
                        //process the byte
                        byte newByte = (byte)receivedByte;
                        int bytesReceived = rawPacket.BytesReceived + 1;
                        PacketStatus status = rawPacket.processByte((byte)receivedByte);
                        switch (status)
                        {
                            case PacketStatus.PacketComplete:
                                debugMessageQueue.Enqueue(String.Format("Packet Received {0} bytes\r\n", rawPacket.PayloadSize));
                                RawPacket packetCopy = new RawPacket(rawPacket);
                                packetQueue.Enqueue(packetCopy);
                                rawPacket.resetPacket();
                                break;
                            case PacketStatus.PacketError:
                                //if (cb_logErrors.Checked)
                                //{
                                debugMessageQueue.Enqueue(String.Format("Packet ERROR! {1} bytes received\r\n", bytesReceived));
                                //}
                                rawPacket.resetPacket();
                                break;
                            case PacketStatus.Processing:
                            case PacketStatus.newPacketDetected:
                                break;
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            if(sp_recieve.IsOpen)
            {
                sp_recieve.Close();
            }
        }

        private void cp_openForwardPort_CheckedChanged(object sender, EventArgs e)
        {
            if(cp_openForwardPort.Checked)
            {
                sp_foward.PortName = cb_forwardPorts.Items[cb_forwardPorts.SelectedIndex].ToString();
                try
                {
                    sp_foward.Open();
                    tb_Console.AppendText("Forward Port: " + sp_foward.PortName + " Open\r\n");
                }
                catch (Exception ex)
                {
                    tb_Console.AppendText("Failed to forward open Port: " + sp_foward.PortName + " \r\n");
                    tb_Console.AppendText("Exception " + ex.Message + " \r\n");
                }
            }
            else
            {
                sp_foward.Close();

            }
        }

        private void cb_serialPorts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!sp_recieve.IsOpen)
            {
                cb_serialPorts.Items.Clear();
                cb_serialPorts.Items.AddRange(SerialPort.GetPortNames());
            }
        }

        private void bgw_ProcessingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            UInt32 packetCount = 0, packetErrors = 0; ;
            RawPacket packet = new RawPacket();
            const UInt16 numberOfSensors = 9;
            try
            {
                FileStream dataFile = File.Open(ofd_OpenFile.FileName, FileMode.Open);
                FileStream outputFile = null;
                FileStream[] outputFiles = new FileStream[numberOfSensors];
                debugMessageQueue.Enqueue(String.Format("Processing File: {0}\r\n", ofd_OpenFile.FileName));
                long percent = 0;
                //initialize the start time of the file. 
                startTime = 0;
                if (decodingOutput == OutputType.appFormat)
                {
                    //have to create header for the file before writting it in. 
                    string line1 = Guid.NewGuid().ToString() + "\r\n";
                    string line2 = Guid.NewGuid().ToString() + "\r\n";
                    string line3 = Guid.NewGuid().ToString() + "\r\n";
                    outputFile.Write(ASCIIEncoding.ASCII.GetBytes(line1), 0, line1.Length);
                    outputFile.Write(ASCIIEncoding.ASCII.GetBytes(line2), 0, line2.Length);
                    outputFile.Write(ASCIIEncoding.ASCII.GetBytes(line3), 0, line3.Length);
                }
                if(decodingOutput == OutputType.individualSensorFiles)
                {
                    //open all the output files. 
                    string header = "Time(ms),Interval,Qx,Qy,Qz,Qw,Mx,My,Mz,Ax,Ay,Az,Rx,Ry,Rz,H1,P1,R1,CalStable,MagTransient\r\n";

                    for (int i = 0; i < numberOfSensors; i++)
                    {
                        string filepath = fbd_outputLocation.SelectedPath + "\\Sensor_" + i.ToString() + ".csv";
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
                    outputFile = File.Open(sfd_SaveFile.FileName, FileMode.Create);
                }
                byte[] fileBytes = new byte[dataFile.Length];
                dataFile.Read(fileBytes, 0, (int)dataFile.Length); 
                for (long i = 0; i < dataFile.Length; i++)
                {
                    long newPercent = (i * 100) / (int)dataFile.Length;
                    float percentageFloat = (i * 100) / dataFile.Length;
                    if (newPercent != percent)
                    {
                        bgw_ProcessingWorker.ReportProgress((int)percentageFloat);
                        percent = newPercent;
                    }
                    int dataByte = fileBytes[i];
                    if (dataByte == -1)
                    {
                        //we've reached the end of the stream
                        break;
                    }
                    byte newByte = (byte)dataByte;
                    int bytesReceived = packet.BytesReceived + 1;
                    PacketStatus status = packet.processByte(newByte);
                    switch (status)
                    {
                        case PacketStatus.PacketComplete:
                            //debugMessageQueue.Enqueue(String.Format("{0} Packet Received {1} bytes\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond), packet.PayloadSize));
                            RawPacket packetCopy = new RawPacket(packet);
                            if (decodingOutput == OutputType.individualSensorFiles)
                            {
                                processRawPacketForCSV(packetCopy, ref outputFiles); 
                            }
                            else
                            {
                                string frameString = processRawPacket(packetCopy);
                                if (frameString.Length > 0)
                                {
                                    outputFile.Write(ASCIIEncoding.ASCII.GetBytes(frameString), 0, frameString.Length);
                                    packetCount++;
                                }
                            }
                            packet.resetPacket();
                            break;
                        case PacketStatus.PacketError:
                            debugMessageQueue.Enqueue(String.Format("Packet ERROR! {0} bytes received\r\n", bytesReceived));
                            packetErrors++;
                            packet.resetPacket();
                            break;
                        case PacketStatus.Processing:
                        case PacketStatus.newPacketDetected:
                            break;
                    }
                }
                debugMessageQueue.Enqueue(String.Format("Processed File Size:{0} Bytes, {1} frames extracted ,{2} Errors \r\n", dataFile.Length, packetCount, packetErrors));
                dataFile.Close();
                if(decodingOutput == OutputType.individualSensorFiles)
                {
                    for (int i = 0; i < outputFiles.Length; i++)
                    {
                        debugMessageQueue.Enqueue(String.Format("Stream {0} Closed Wrote {1} Bytes\r\n", i, outputFiles[i].Length));
                        outputFiles[i].Close();
                    }
                }
                else
                {
                    outputFile.Close();
                }
                
            }
            catch
            {
                debugMessageQueue.Enqueue(String.Format("Error Processing file, {0} errors found\r\n", packetErrors));
            }
            
        }

        private void bgw_ProcessingWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (e.ProgressPercentage > 0 && e.ProgressPercentage <= 100)
            {
                pb_progressBar.Value = e.ProgressPercentage;
            }
        }

        private void bgw_ProcessingWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pb_progressBar.Value = 0;
        }

        private void btn_convertToCsv_Click(object sender, EventArgs e)
        {
            if (bgw_ProcessingWorker.IsBusy)
            {
                debugMessageQueue.Enqueue(String.Format("Cannot decode file, waiting for completion\r\n"));
            }
            if (ofd_OpenFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            if (fbd_outputLocation.ShowDialog() == DialogResult.OK)
            {
                //dataStreamFilePath = fbd_outputLocation.SelectedPath;
                decodingOutput = OutputType.individualSensorFiles;
            }
            decodingOutput = OutputType.individualSensorFiles;
            bgw_ProcessingWorker.RunWorkerAsync();
        }
    }
}
