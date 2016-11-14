// /**
// * @file NetworkUtils.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 11 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.Net;
using System.Net.Sockets;

namespace BpEmuMetroForms.Brainpack.Communication
{
    public class NetworkUtils
    {
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
    }
}