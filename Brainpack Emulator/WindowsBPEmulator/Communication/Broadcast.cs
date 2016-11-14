// /**
// * @file Broadcast.cs
// * @brief Contains the Broadcast class
// * @author Mohammed Haider( mohammed@heddoko.com)
// * @date 10 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.Net;
using System.Net.Sockets;

namespace WindowsBPEmulator.Communication
{
    public class Broadcast
    {
        public int Port;
        private Socket mUdpBroadcastSocket;
        private IPAddress mBroadcastAddress;
        /// <summary>
        /// Start broadcasting over the following port
        /// </summary>
        /// <param name="vPort"></param>
        public void SetupSocket(int vPort)
        {
            mUdpBroadcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
           ProtocolType.Udp);
            mBroadcastAddress = IPAddress.Broadcast;
            Port = vPort;
            EndPoint vEp = new IPEndPoint(mBroadcastAddress,Port);
            mUdpBroadcastSocket.BeginConnect(vEp, new AsyncCallback(OnConnect), mUdpBroadcastSocket);
        }

        private void OnConnect(IAsyncResult vAr)
        {
            Socket vSocket = (Socket) vAr.AsyncState;
          vSocket.EndConnect(vAr);
        }

        public bool Send(byte[] vBufer)
        {
            if (mUdpBroadcastSocket == null || !mUdpBroadcastSocket.Connected)
            {
                return false;
            }
            else
            {
                try
                {
                    mUdpBroadcastSocket.BeginSend(vBufer, 0, vBufer.Length, 0, new AsyncCallback(OnSend),
                        mUdpBroadcastSocket);
                }
                catch (Exception vE)
                {
                    throw;

                }
            }
            return true;

        }

        private void OnSend(IAsyncResult vAr)
        {
            Socket vObject = (Socket) vAr.AsyncState;
            try
            {
                mUdpBroadcastSocket.EndSend(vAr);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void CloseSocket()
        {
            if (mUdpBroadcastSocket != null && mUdpBroadcastSocket.Connected)
            {
                mUdpBroadcastSocket.Shutdown(SocketShutdown.Both);
                mUdpBroadcastSocket.Close();
            }
        }
    }
}