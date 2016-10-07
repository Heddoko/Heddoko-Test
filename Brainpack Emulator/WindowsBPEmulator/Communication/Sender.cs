// /**
// * @file Sender.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 10 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using HeddokoLib.adt;

namespace WindowsBPEmulator.Communication
{
    public class Sender
    {
        private NetworkStream mStream;
        private Thread mThread;
        private CircularQueue<byte[]> mSendBuffer = new CircularQueue<byte[]>(100,true);
        public ManualResetEvent ShutDownEvent = new ManualResetEvent(false);
        public void SendData(byte[] vData)
        {
            mSendBuffer.Enqueue(vData);
        }

        internal Sender(NetworkStream vStream)
        {
            mStream = vStream;
            mThread = new Thread(Run);
            mThread.Start();
        }

        private void Run()
        {
            byte[] vBuffer = new byte[1024];
            try
            {
                while (!ShutDownEvent.WaitOne(0))
                {
                    try
                    {
                        if (mSendBuffer.Count == 0)
                        {
                            Thread.Sleep(1);
                        }
                        else
                        {
                            var vItem = mSendBuffer.Dequeue();
                           mStream.Write(vItem,0,vItem.Length);
                        }
                        
                    }
                    catch (IOException vException)
                    {

                    }
                }
            }
            catch (Exception vException)
            {
            }
            finally
            {
                mStream.Close();
            }
        }

        public void Stop()
        {
            try
            {
                ShutDownEvent.Set();
            }
            catch (Exception)
            { 
            }
        }
    }
}