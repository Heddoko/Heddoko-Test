// /**
// * @file Client.cs
// * @brief Contains the 
// * @author Mohammed Haider( mohammed@heddoko.com)
// * @date October 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using HeddokoLib.heddokoProtobuff.Decoder;

namespace WindowsBPEmulator.Communication
{
    public class Client  :IDisposable
    {
        private Sender mSender; 
        private Receiver mReceiver;
        public event EventHandler<byte[]> DataReceived;
 
        public void SendData(byte[] vData)
        {
            mSender.SendData(vData);
        }

        public Client()
        {
           


            //  mTcpClient = new TcpClient(vLocalEndpoint);

         //   mStream = mTcpClient.GetStream();
            mReceiver = new Receiver();
         //   mSender = new Sender(mStream);
         //   mReceiver.DataReceivedEvent += OnDataReceivedHandler;
            
        }

        /// <summary>
        /// Callback on data received. 
        /// </summary>
        /// <param name="vSender"></param>
        /// <param name="vE"></param>
        public void OnDataReceivedHandler(object vSender, byte[] vE)
        {
            var vHandler = DataReceived;
            if (vHandler != null)
            {
                DataReceived(vSender, vE);
            }
        }

        public void Dispose()
        {
             
        }
 
    }

   
}