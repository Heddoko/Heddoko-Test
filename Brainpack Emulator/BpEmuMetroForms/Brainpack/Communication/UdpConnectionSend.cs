// /**
// * @file UdpConnectionSend.cs
// * @brief Contains the UdpConnectionSend class
// * @author Mohammed Haider( mohammed @heddoko.com)
// * @date November 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using heddoko;
using HeddokoLib.heddokoProtobuff.Decoder;
using ProtoBuf;

namespace WindowsBPEmulator.Communication
{
    /// <summary>
    /// A udp connection to send data over
    /// </summary>
    public class UdpConnectionSend
    {
        private int mPort;
        private string mIpAddress;
        private UdpClient mClient;

        public UdpConnectionSend(string vIpAddress, int vPort)
        {
            SetConnectionInfo(vIpAddress, vPort);
        }

        public string IpAddress
        {
            get
            {
                return mIpAddress;
            }
        }

        public int Port
        {
            get { return mPort; }
        }

        public bool Start()
        {
            bool vResult = true;

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

        public void Send(Packet vPacket)
        {
            MemoryStream vStream = new MemoryStream();
            Serializer.Serialize(vStream, vPacket);
            RawPacket vRawPacket = new RawPacket();
            int vRawSize;
            var vRawBytes = vRawPacket.GetRawPacketByteArray(out vRawSize, vStream);
            Send(vRawBytes, vRawSize);
        }

        /// <summary>
        /// Shuts down the client
        /// </summary>
        public void CloseClient()
        {
            mClient.Close();
        }


        public void SetConnectionInfo(string vAddress, int vPort)
        {
            mIpAddress = vAddress;
            mPort = vPort;
        }
    }
}