// /**
// * @file BrainpackModel.cs
// * @brief Contains the 
// * @author Mohammed Haider(  mohammed@heddoko.com)
// * @date 10 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.Net;
using System.Net.Sockets;
using HeddokoLib.HeddokoDataStructs.Brainpack;

namespace WindowsBPEmulator
{
    public static class BrainpackModel
    {
        private static Brainpack mBrainpack;
        public static Brainpack BrainpackInfo
        {
            get
            {
                if (mBrainpack == null)
                {
                    mBrainpack = new Brainpack();
                    mBrainpack.Id = "S00055";
                    mBrainpack.Point = new IPEndPoint(IPAddress.Parse(GetLocalIPAddress()), 8844);
                    mBrainpack.Version = "1.0.0.0";
                    mBrainpack.TcpControlPort = 8844;
                }
                return mBrainpack;
            }
        }

        public static int GetPort
        {
            get
            {
                IPEndPoint vEndpoint = (IPEndPoint)BrainpackInfo.Point;
                return vEndpoint.Port;
            }
        }

        public static uint GetTcpControlPort
        {
            get { return (uint)BrainpackInfo.TcpControlPort; }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

    }
}