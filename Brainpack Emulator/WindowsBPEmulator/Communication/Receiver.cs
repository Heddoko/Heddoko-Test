// /**
// * @file Receiver.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 10 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System; 
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WindowsBPEmulator.Communication
{
    public class Receiver
    {
       
        //internal event EventHandler<byte[]> DataReceivedEvent;
        //private TcpListener mListener;
        //private Thread mThread;
        //public   ManualResetEvent ShutDownEvent =
        //    new ManualResetEvent(false);

        //internal Receiver(int vPortNum=8844)
        //{
        //    IPHostEntry vIpHostEntry = Dns.GetHostEntry("localhost");
        //    IPAddress vIpAddress = vIpHostEntry.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);//.AddressList[0];
        //    IPEndPoint vLocalEndpoint = new IPEndPoint(vIpAddress, vPortNum);
        //    mListener = new TcpListener(vLocalEndpoint);
        //    mListener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpIPCallback), mListener);

        ////    mThread = new Thread(Run);
        //   // mThread.Start();
        //}

        //private void AcceptTcpIPCallback(IAsyncResult vAr)
        //{
             
        //}

        //void Run()
        //{
        //    byte[] vBuffer = new byte[1024];
        //    try
        //    {
        //        while (!ShutDownEvent.WaitOne(0))
        //        {
        //            try
        //            {
        //                if (!mStream.DataAvailable)
        //                {
        //                    Thread.Sleep(1);
        //                }
        //                else if (mStream.Read(vBuffer, 0, 1024) > 0)
        //                {
        //                    //raise data received event here
        //                    DataReceivedEvent?.Invoke(mStream, vBuffer); 
        //                }
        //                else
        //                {
        //                    ShutDownEvent.Set();
        //                }
        //            }
        //            catch (IOException vException)
        //            {

        //            }
        //        }
        //    }
        //    catch (Exception vException)
        //    { 
        //    }
        //    finally
        //    {
        //        mStream.Close();
        //    }
        //}

        //public void Stop()
        //{
        //    try
        //    {
        //        ShutDownEvent.Set();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
    }
}