using heddoko;
using ProtoBuf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
        public BrainpackControllerForm()
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
                    strBuilder.Append("\r\nStatus Response\r\n");                    
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

        private void BrainpackControllerForm_Load(object sender, EventArgs e)
        {
            //initialize the message queue
            debugMessageQueue = new ConcurrentQueue<string>();
            //start the debug message thread
            processDebugThreadEnabled = true;
            Thread debugMessageThread = new Thread(processDebugMessagesThread);
            debugMessageThread.Start();
            outputPacketQueue = new ConcurrentQueue<RawPacket>();
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
                MemoryStream stream = new MemoryStream();
                stream.WriteByte(0x04);
                Serializer.Serialize(stream, protoPacket);
                RawPacket outputPacket = new RawPacket(stream.ToArray(), (ushort)stream.Length);
                outputPacketQueue.Enqueue(outputPacket);
            }
        }
    }
}
