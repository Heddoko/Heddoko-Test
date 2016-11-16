// /**
// * @file UdpConnectionSend.cs
// * @brief Contains the UdpConnectionSend class
// * @author Mohammed Haider( mohammed @heddoko.com)
// * @date November 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.Net;
using System.Net.Sockets;

namespace WindowsBPEmulator.Communication
{
    /// <summary>
    /// A udp connection to send data over
    /// </summary>
    public class UdpConnectionSend
    {
        private Socket mSocket;
        private int mPort;
        private string mIpAddress;
        private UdpClient mClient;

        public bool Start(string vIpAddress, int vPort)
        {
            bool vResult = true;
            mIpAddress = vIpAddress;
            mPort = vPort;
            IPAddress vAddress = IPAddress.Parse(mIpAddress);
            mClient = new UdpClient();
            IPEndPoint vEndPoint = new IPEndPoint(vAddress, mPort);
            try
            {
                mClient.Connect(vEndPoint);
            }
            catch (Exception vE)
            {
                Console.WriteLine(vE);
                vResult = false;
            }
            return vResult;
        }

        /// <summary>
        /// Send data over the client
        /// </summary>
        /// <param name="vBuffer"></param>
        /// <param name="vLength"></param>
        public void Send(byte[] vBuffer, int vLength)
        {
            try
            {
                mClient.Send(vBuffer, vLength);
            }
            catch (Exception vE)
            {
                Console.WriteLine(vE);

            }
        }

        /// <summary>
        /// Shuts down the client
        /// </summary>
        public void CloseClient()
        {
             mClient.Close();
        }
    }
}