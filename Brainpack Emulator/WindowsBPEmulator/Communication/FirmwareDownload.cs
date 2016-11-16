// /**
// * @file FirmwareDownload.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 10 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */


using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using WindowsBPEmulator.Controller;
using HeddokoLib.networking;
using Tftp.Net;

namespace WindowsBPEmulator.Communication
{

    public class FirmwareDownload
    {

        private TftpClient mTftpClient;
        private static AutoResetEvent TransferFinishedEvent = new AutoResetEvent(false);
        public ConsoleTextBlockController ConsoleTextBlockController;
        public void RequestFirmwareFromEndPoint(IPEndPoint vEndPoint, string vFileName)
        {
            Stopwatch vwatch = new Stopwatch();

            var client = new TftpClient(vEndPoint);
            var transfer = client.Download(vFileName);
            transfer.OnFinished += new TftpEventHandler(OnFinishedEvent);
            //Start the transfer and write the data that we're downloading into a memory stream
            FileStream vStreamer = new FileStream("C:\\downl\\firmware.bin", FileMode.CreateNew);
            vwatch.Start();
            UpdateUi("Starting Firmware file transfer");
            transfer.Start(vStreamer);
            TransferFinishedEvent.WaitOne();
            FileInfo vInfo = new FileInfo("C:\\downl\\firmware.bin");
            long vLength= vInfo.Length;
          
            vwatch.Stop();
            UpdateUi("finished file transfer of "+ vLength + " bytes in "+ (vwatch.ElapsedMilliseconds  /1000.0) + "seconds");
            vStreamer.Close();

          
        }

        private void OnFinishedEvent(ITftpTransfer vTransfer)
        {
            TransferFinishedEvent.Set();
        }

        // public IPEndPoint Endpoint;
        // private Socket mSocket;
        // private TcpClient mClient;
        // public string SavePath;
        // public event EventHandler FirmwareReceived;
        // private int mBufferSize = 1024;
        // private byte[] byterion = new byte[1024];
        // private List<byte[]> BufferList = new List<byte[]>();
        // private static ManualResetEvent connectDone = new ManualResetEvent(false);
        // private static ManualResetEvent receiveDone = new ManualResetEvent(false);
        // public ConsoleTextBlockController ConsoleTextBlockController;
        // public void RequestFirmwareFromEndPoint(IPEndPoint vEndPoint, string vPath)
        // {

        //     Endpoint = vEndPoint;
        //     SavePath = vPath;

        //     // Create a TCP/IP socket.
        //     mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //     UpdateUi("FirmwareDownload: connect...");
        //     //  mSocket.Connect(Endpoint);
        //     mSocket.BeginConnect(Endpoint,
        //        new AsyncCallback(ConnectCallback), mSocket);
        //     //          UpdateUi("FirmwareDownload: connectDone.WaitOne...");
        //     connectDone.WaitOne();

        //     UpdateUi("FirmwareDownload: send message: GetLatestFirmware");
        //     string vMsg = "GetLatestFirmware<EOL>";
        //     var vSendMsg = Encoding.ASCII.GetBytes(vMsg);
        //   //  mSocket.Connect(Endpoint);
        //     UpdateUi("FirmwareDownload: sendmessage");
        // //     mSocket.Send(vSendMsg, vSendMsg.Length, 0);
        //   mSocket.BeginSend(vSendMsg, 0, vSendMsg.Length, 0,
        //             new AsyncCallback(SendCallback), mSocket);
        //  //   sendDone.WaitOne();
        //     //StateObject state = new StateObject();
        //     // state.Buffer = new byte[state.BufferSize];
        //     // state.Socket = mSocket;

        //     ////   mSocket.Receive(state.Buffer, state.Buffer.Length, 0);
        //     ////   int size = state.Buffer.Length;
        //     //   mSocket.BeginReceive(state.Buffer, 0, state.BufferSize,SocketFlags.None, new AsyncCallback(ReceiveCallback), state);
        //     //  int mBufferSize = 1024;
        //     //  byte[] vBuffer = new byte[mBufferSize];
        //     //   
        //     //  mClient = new TcpClient();
        //     //  mClient.Connect(Endpoint);
        //     //  
        //     //  NetworkStream vStream = mClient.GetStream();


        //     //  mSocket.).Socket.BeginReceive(vIncomingConnection.Buffer, 0, vIncomingConnection.Buffer.Length,
        //     //      SocketFlags.None, new AsyncCallback(ReceiveCallback), vIncomingConnection);
        //     //mSocket.BeginReceive()
        //     //vStream.Write(vSendMsg,0,vSendMsg.Length);
        //     //List<byte[]> vBufferList = new List<byte[]>(); 

        //     // while(true)
        //     // while (vStream.DataAvailable)
        //     //{
        //     //    byte[] vBuffers = new byte[1024];
        //     //    vStream.Read(vBuffers, 0, 1024);
        //     //    vBufferList.Add(vBuffers);
        //     //}
        //     ////vStream.BeginRead(byterion, 0, mBufferSize, new AsyncCallback(ReadCallback), vStream);
        //     //int count = vBufferList.Count;
        // }
        // private static ManualResetEvent sendDone =
        //new ManualResetEvent(false);
        // private void SendCallback(IAsyncResult vAr)
        // {
        //     try
        //     {
        //         UpdateUi("SendCallback");
        //         // Retrieve the socket from the state object.
        //         Socket client = (Socket)vAr.AsyncState;

        //         // Complete sending the data to the remote device.
        //         int bytesSent = client.EndSend(vAr);
        //         UpdateUi("FirmwareUpdate bytes sent: " + bytesSent);


        //         // Signal that all bytes have been sent.
        //         sendDone.Set();
        //         UpdateUi("FirmwareUpdate :  sendDone.Set() ");
        //         StateObject state = new StateObject();
        //         state.Buffer = new byte[state.BufferSize];
        //         state.Socket = mSocket;


        //       //     mSocket.Receive(state.Buffer, state.Buffer.Length, 0);
        //         //   int size = state.Buffer.Length;
        //        mSocket.BeginReceive(state.Buffer, 0, state.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), state);
        //         // mSocket.Close();
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e.ToString());
        //     }
        // }

        // private void ConnectCallback(IAsyncResult vAr)
        // {
        //     try
        //     {
        //         UpdateUi("FirmwareDownload: in ConnectCallback");
        //         // Retrieve the socket from the state object.
        //         Socket client = (Socket)vAr.AsyncState;

        //         // Complete the connection.
        //         client.EndConnect(vAr);
        //         UpdateUi("FirmwareDownload:    client.EndConnect(vAr);");
        //         // Signal that the connection has been made.
        //         connectDone.Set();
        //         UpdateUi("FirmwareDownload:     connectDone.Set();");

        //     }
        //     catch (Exception vE)
        //     {
        //         UpdateUi("FirmwareDownload: exception : " + vE);
        //     }
        // }

        // private void AcceptCallback(IAsyncResult vAr)
        // {
        //     StateObject vIncomingConnection = new StateObject();
        //     Socket vSocket = (Socket)vAr.AsyncState;
        //     vIncomingConnection = new StateObject();
        //     vIncomingConnection.Socket = vSocket.EndAccept(vAr);
        //     vIncomingConnection.Buffer = new byte[vIncomingConnection.BufferSize];
        //     vIncomingConnection.Socket.BeginReceive(vIncomingConnection.Buffer, 0, vIncomingConnection.Buffer.Length,
        //           SocketFlags.None, new AsyncCallback(ReceiveCallback), vIncomingConnection);

        // }

        // private void ReceiveCallback(IAsyncResult vAr)
        // {
        //     // Retrieve the state object and the client socket 
        //     // from the asynchronous state object.
        //     StateObject state = (StateObject)vAr.AsyncState;
        //     Socket client = state.Socket;

        //     // Read data from the remote device.
        //     int bytesRead = client.EndReceive(vAr);

        //     if (bytesRead > 0)
        //     {
        //         string vMsg = Encoding.ASCII.GetString(state.Buffer);

        //         if (vMsg.Contains("<BoF>"))
        //         {
        //             File.WriteAllBytes(SavePath, state.Buffer);
        //         }
        //         // There might be more data, so store the data received so far.

        //         // Get the rest of the data.
        //         client.BeginReceive(state.Buffer, 0, state.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        //     }
        //     else
        //     {
        //         string vMsg = Encoding.ASCII.GetString(state.Buffer);
        //         if (vMsg.Contains("<BoF>"))
        //         {
        //             File.WriteAllBytes(SavePath, state.Buffer);
        //         }
        //         //string vMsg = Encoding.ASCII.GetString(vIncomingConnection.Buffer);
        //         // All the data has arrived; put it in response.
        //         if (state.Buffer.Length > 1)
        //         {

        //         }
        //         // Signal that all bytes have been received.
        //         receiveDone.Set();
        //     }
        // }
        // //StateObject vIncomingConnection = (StateObject)vAr.AsyncState;

        // //try
        // //{
        // //    int vBytesRead = vIncomingConnection.Socket.EndReceive(vAr);
        // //    if (vBytesRead > 0)
        // //    {
        // //        int vLenght = vIncomingConnection.Buffer.Length;
        // //        int dfafda = vLenght;

        // //    }
        // //}
        // //catch (SocketException vE)
        // //{
        // //   // CloseAndRemoveStateObject(vIncomingConnection);

        // //}
        // //}


        // private void ReadCallback(IAsyncResult vAr)
        // {
        //     NetworkStream vStream = (NetworkStream)vAr.AsyncState;
        //     int mBufferSize = 1024;
        //     byte[] vBuffer = new byte[mBufferSize];
        //     int vByteRead = vStream.EndRead(vAr);
        //     int dafd = byterion.Length;
        //     int vdfadfad = vByteRead;
        //     //FileStream vFileStream = File.Create(SavePath);

        //     //int vBytesRead;
        //     //if (vStream.CanRead)
        //     //{
        //     //    while ((vBytesRead = vStream.Read(vBuffer, 0, mBufferSize)) > 0)
        //     //    {
        //     //        vFileStream.Write(vBuffer, 0, vBytesRead);
        //     //    }
        //     //}
        //     //vStream.Close();
        //     //vFileStream.Close();
        //     //FirmwareReceived?.Invoke(this, null);
        // }
        private void UpdateUi(string vMsg)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() => ConsoleTextBlockController.AddMsg(vMsg)));

        }
    }
}