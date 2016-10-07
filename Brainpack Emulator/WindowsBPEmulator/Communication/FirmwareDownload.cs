// /**
// * @file FirmwareDownload.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 10 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.IO;
using System.Net;
using System.Net.Sockets; 
using System.Text;

namespace WindowsBPEmulator.Communication
{
    
    public class FirmwareDownload
    {
        public IPEndPoint Endpoint;
        private TcpClient mClient;
        public string SavePath;
        public event EventHandler FirmwareReceived;
        public void RequestFirmwareFromEndPoint(IPEndPoint vEndPoint, string vPath)
        {
            int vBufferSize = 1024;
            byte[] vBuffer = new byte[vBufferSize];
            int vBytesRead;
            Endpoint = vEndPoint;
            mClient = new TcpClient(Endpoint);
            SavePath = vPath;
            FileStream vFileStream = File.Create(SavePath);
            NetworkStream vStream = mClient.GetStream();
            string vMsg = "GetLatestFirmware<EOL>";
            var vSendMsg = Encoding.ASCII.GetBytes(vMsg);
            vStream.Write(vSendMsg,0,vSendMsg.Length);

            while ((vBytesRead = vStream.Read(vBuffer, 0 , vBufferSize)) > 0)
            {
                vFileStream.Write(vBuffer,0,vBytesRead);
            }
            vFileStream.Close();
            FirmwareReceived?.Invoke(this, null);
        }
    }
}