using heddoko;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrainPackDataAnalyzer
{
    public partial class BrainpackControllerForm : Form
    {
        private bool processDebugThreadEnabled = false;
        public ConcurrentQueue<string> debugMessageQueue;
        public ConcurrentQueue<RawPacket> outputPacketQueue;
        private bool streamRequested = false;


        public BrainpackControllerForm()
        {
            InitializeComponent();

        }
        public BrainpackControllerForm(Brainpack brainapack)
        {
            InitializeComponent();
            mtb_netPort.Text = brainapack.ConfigurationPort.ToString();
            mtb_NetAddress.Text = brainapack.RemoteEP.Address.ToString();
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("Serial Number: " + brainapack.SerialNumber + "\r\n");
            strBuilder.Append("Firmware Version: " + brainapack.FirmwareVersion + "\r\n");
            strBuilder.Append("IP Address: " + brainapack.RemoteEP.Address.ToString() + "\r\n");
            tb_Console.AppendText(strBuilder.ToString());
            this.Text = "Brainpack Configuration " + brainapack.SerialNumber; 
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
                else
                {
                    Thread.Sleep(100);
                }
            }
        }
        private void processPacket(RawPacket packet)
        {
            //check that the packet comes from an IMU sensor
            if (packet.Payload[0] == 0x03)
            {

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
        private void processSensorMask(UInt32 sensorMask)
        {
            for(int i =0; i < 9; i++)
            {
                if((sensorMask &(1<< i)) > 0)
                {
                    setCheckedSensorState(i, CheckState.Checked);
                }
                else
                {
                    setCheckedSensorState(i, CheckState.Unchecked);
                }
                
                this.BeginInvoke((MethodInvoker)(() => pb_bodyPicture.Update()));
            }
            
        }
        private void setCheckedSensorState(int sensor, CheckState checkedState)
        {
            switch(sensor)
            {
                case 0:
                    this.BeginInvoke((MethodInvoker)(() => cb_sensor0.CheckState = checkedState));                    
                    break;
                case 1:
                    this.BeginInvoke((MethodInvoker)(() => cb_sensor1.CheckState = checkedState));
                    break;
                case 2:
                    this.BeginInvoke((MethodInvoker)(() => cb_sensor2.CheckState = checkedState));
                    break;
                case 3:
                    this.BeginInvoke((MethodInvoker)(() => cb_sensor3.CheckState = checkedState));
                    break;
                case 4:
                    this.BeginInvoke((MethodInvoker)(() => cb_sensor4.CheckState = checkedState));
                    break;
                case 5:
                    this.BeginInvoke((MethodInvoker)(() => cb_sensor5.CheckState = checkedState));
                    break;
                case 6:
                    this.BeginInvoke((MethodInvoker)(() => cb_sensor6.CheckState = checkedState));
                    break;
                case 7:
                    this.BeginInvoke((MethodInvoker)(() => cb_sensor7.CheckState = checkedState));
                    break;
                case 8:
                    this.BeginInvoke((MethodInvoker)(() => cb_sensor8.CheckState = checkedState));
                    break;
            }
        }
        int statusMessageReceivedCount = 0;
        private void processProtoPacket(Packet packet)
        {

            StringBuilder strBuilder = new StringBuilder();
            switch (packet.type)
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
                    strBuilder.Append("\r\nStatus Response #" +statusMessageReceivedCount++.ToString() +  "\r\n");                    
                    if(packet.serialNumberSpecified)
                    {
                        strBuilder.Append("Serial Number: " + packet.serialNumber + "\r\n");
                    }                    
                    if(packet.firmwareVersionSpecified)
                    {
                        strBuilder.Append("Version: V" + packet.firmwareVersion + "\r\n");
                    }
                    if(packet.brainpackStateSpecified)
                    {
                        strBuilder.Append("State: " + packet.brainpackState.ToString() + "\r\n");
                    }
                    if(packet.chargeStateSpecified)
                    {
                        strBuilder.Append("Charger State: " + packet.chargeState.ToString() + "\r\n");
                    }
                    strBuilder.Append("Battery Level: " + packet.batteryLevel.ToString() + "\r\n");
                    if(packet.sensorMaskSpecified)
                    {
                        strBuilder.Append("Sensor Mask: " + packet.sensorMask.ToString("X") + "\r\n");
                        processSensorMask(packet.sensorMask);
                    }
                    debugMessageQueue.Enqueue(strBuilder.ToString());
                    break;
                case PacketType.DataFrame:
                    if (packet.fullDataFrame != null)
                    {
                        debugMessageQueue.Enqueue("Received Data Frame From Timestamp:" +
                        packet.fullDataFrame.timeStamp.ToString() + "\r\n");
                    }
                    break;
                case PacketType.MessageStatus:
                    if (packet.messageStatusSpecified)
                    {
                        debugMessageQueue.Enqueue(string.Format("Message Status: {0}\r\n",packet.messageStatus));
                        //if a stream was requested, check the return value and change the button to stop stream. 
                        if (streamRequested)
                        {
                            if(packet.messageStatus)
                            {
                                this.BeginInvoke((MethodInvoker)(() => btn_StreamReq.Text = "Stop Stream"));
                            }
                            streamRequested = false; 
                        }
                    }
                    break;
                case PacketType.UpdatedFirmwareResponse:
                    if (packet.messageStatusSpecified)
                    {
                        debugMessageQueue.Enqueue(string.Format("Firmware Update Result: {0}\r\n", packet.messageStatus));
                    }
                    break;
                default:
                    debugMessageQueue.Enqueue(string.Format("Receiced Packet {0}", packet.type.ToString()));
                    break;
            }

        }

        private bool EnableSocketQueue = false;
        RawPacket packet = new RawPacket();


        private void socketClientProcess()
        {
            try
            {
                // Establish the remote endpoint for the socket.
                //IPHostEntry ipHostInfo =  Dns.GetHostEntry("192.168.1.1");
                IPAddress ipAddress = IPAddress.Parse(mtb_NetAddress.Text);//ipHostInfo.AddressList[0];
                Int32 port = Int32.Parse(mtb_netPort.Text);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
                // Connect the socket to the remote endpoint. Catch any errors.
                sender.SendTimeout = 5000; 
                try
                {
                    sender.Connect(remoteEP);
                    debugMessageQueue.Enqueue(String.Format("Socket connected to {0}",
    sender.RemoteEndPoint.ToString()));
                    byte[] receivedByte = new byte[512];
                    sender.ReceiveTimeout = 100; //wait 100ms between receive requests. 
                    int receviedCount = 0;
                    while (sender.Connected && EnableSocketQueue)
                    {
                        try
                        {
                            receviedCount = sender.Receive(receivedByte);
                        }
                        catch
                        {

                        }
                        if (receviedCount != 0)
                        {
                            int bytesReceived = 0;
                            for (int i = 0; i < receviedCount; i++)
                            {
                                //process the byte
                                bytesReceived = packet.BytesReceived + 1;
                                PacketStatus status = packet.processByte(receivedByte[i]);
                                switch (status)
                                {
                                    case PacketStatus.PacketComplete:
                                        //debugMessageQueue.Enqueue(String.Format("{0} Packet Received {1} bytes\r\n", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond), packet.PayloadSize));
                                        RawPacket packetCopy = new RawPacket(packet);
                                        processPacket(packetCopy);
                                        packet.resetPacket();
                                        break;
                                    case PacketStatus.PacketError:
                                        debugMessageQueue.Enqueue(String.Format("Packet ERROR! {1} bytes received\r\n", bytesReceived));
                                        packet.resetPacket();
                                        break;
                                    case PacketStatus.Processing:
                                    case PacketStatus.newPacketDetected:
                                        break;
                                }
                            }
                            receviedCount = 0;
                        }
                        RawPacket outputPacket;
                        if (outputPacketQueue.Count > 0)
                        {
                            if (outputPacketQueue.TryDequeue(out outputPacket))
                            {
                                //serialize and then send the packet 
                                ushort size = 0;
                                byte[] bytes = outputPacket.createRawPacket(ref size);
                                sender.Send(bytes,size,SocketFlags.None);  

                            }
                        }
                    }
                    EnableSocketQueue = false; 
                    debugMessageQueue.Enqueue("Socket Closed\r\n");
                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    debugMessageQueue.Enqueue(String.Format("ArgumentNullException : {0}\r\n", ane.ToString()));
                }
                catch (SocketException se)
                {
                    debugMessageQueue.Enqueue(String.Format("SocketException : {0}\r\n", se.ToString()));
                }
                catch (Exception e)
                {
                    debugMessageQueue.Enqueue(String.Format("Unexpected exception : {0}\r\n", e.ToString()));
                }
                if (sender.Connected)
                {
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
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
            Thread socketClientThread = new Thread(socketClientProcess);
            socketClientThread.Start();
        }
        public static string GetLocalIpAddress()
        {
            var vHost = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var vIp in vHost.AddressList)
            {
                if (vIp.AddressFamily == AddressFamily.InterNetwork)
                {
                    return vIp.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
        public string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }
        private void BrainpackControllerForm_Load(object sender, EventArgs e)
        {
            //initialize the message queue
            debugMessageQueue = new ConcurrentQueue<string>();
            //start the debug message thread
            processDebugThreadEnabled = true;
            Thread debugMessageThread = new Thread(processDebugMessagesThread);
            debugMessageThread.Start();
            outputPacketQueue = new ConcurrentQueue<RawPacket>();
            try
            {
                mtb_streamAddress.Text = GetLocalIPv4(NetworkInterfaceType.Wireless80211); 
            }
            catch
            {
                mtb_streamAddress.Text = "192.168.11.1";
            }

        }

        private void btn_disconnectSock_Click(object sender, EventArgs e)
        {
            //close the socket
            EnableSocketQueue = false; 

        }

        private void btn_getStatus_Click(object sender, EventArgs e)
        {
            if (EnableSocketQueue)
            {
                Packet protoPacket = new Packet();
                protoPacket.type = PacketType.StatusRequest;
                sendProtoPacket(protoPacket); 
            }
        }
        private void sendProtoPacket(Packet protoPacket)
        {
            if (EnableSocketQueue)
            {
                MemoryStream stream = new MemoryStream();
                stream.WriteByte(0x04);
                Serializer.Serialize(stream, protoPacket);
                RawPacket outputPacket = new RawPacket(stream.ToArray(), (ushort)stream.Length);
                outputPacketQueue.Enqueue(outputPacket);
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {
            debugMessageQueue.Enqueue("don't click there, ya moron!\r\n"); 
        }

        private void btn_StreamReq_Click(object sender, EventArgs e)
        {
            Packet protoPacket = new Packet();
            Endpoint protoEndpoint = new Endpoint();
            protoPacket.type = PacketType.StartDataStream;
            protoPacket.recordingFilename = tb_recordingName.Text;
            protoPacket.recordingFilenameSpecified = true; 
            protoPacket.sensorMask = 0x1FF; //all sensors
            protoPacket.sensorMaskSpecified = true;
            protoPacket.recordingRateSpecified = true;
            protoPacket.recordingRate = (UInt32)nud_dataInterval.Value;
            protoEndpoint.address = mtb_streamAddress.Text;
            protoEndpoint.port = UInt32.Parse(mtb_streamPort.Text); 
            protoPacket.endpoint = protoEndpoint;
            sendProtoPacket(protoPacket);
        }

        private void btn_stopStream_Click(object sender, EventArgs e)
        {
            Packet protoPacket = new Packet();
            protoPacket.type = PacketType.StopDataStream;
            sendProtoPacket(protoPacket);
        }

        private void btn_cfgRecord_Click(object sender, EventArgs e)
        {
            Packet protoPacket = new Packet();
            protoPacket.type = PacketType.ConfigureRecordingSettings;
            protoPacket.recordingFilename = tb_recordingName.Text;
            protoPacket.recordingFilenameSpecified = true;
            protoPacket.sensorMask = 0x1FF; //all sensors
            protoPacket.sensorMaskSpecified = true;
            protoPacket.recordingRateSpecified = true;
            protoPacket.recordingRate = (UInt32)nud_dataInterval.Value;
            sendProtoPacket(protoPacket);
        }

        private void BrainpackControllerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            processDebugThreadEnabled = false;
            EnableSocketQueue = false; 
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btn_FwUpdate_Click(object sender, EventArgs e)
        {
            Packet protoPacket = new Packet();
            Endpoint protoEndpoint = new Endpoint();
            FirmwareUpdate protoFirmwareUpdate = new FirmwareUpdate(); 
            protoPacket.type = PacketType.UpdateFirmwareRequest;
            protoEndpoint.address = mtb_transferIp.Text;
            protoEndpoint.port = UInt32.Parse(mtb_transferPort.Text);
            protoFirmwareUpdate.fwEndpoint = protoEndpoint;
            protoFirmwareUpdate.fwFilename = tb_firmwareFilename.Text;
            protoPacket.firmwareUpdate = protoFirmwareUpdate;                 
            sendProtoPacket(protoPacket);
        }
    }
}
