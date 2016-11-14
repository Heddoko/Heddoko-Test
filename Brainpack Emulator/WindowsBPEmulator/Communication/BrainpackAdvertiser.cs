// /**
// * @file BrainpackAdvertiser.cs
// * @brief Contains the 
// * @author Mohammed Haider( mohammed@heddoko)
// * @date 10 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */


using System;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using WindowsBPEmulator.Controller;
using heddoko;
using HeddokoLib.heddokoProtobuff.Decoder;
 using ProtoBuf;

namespace WindowsBPEmulator.Communication
{
    /// <summary>
    /// Sends out an advertising packet over udp, indicating that the brainpack is online and how to connect to its control port
    /// </summary>
    public class BrainpackAdvertiser
    {
        Timer mTimer;
        private Broadcast mBroadcaster;
        public ConsoleTextBlockController ConsoleTextBlockController;
        /// <summary>
        /// Start advertising by providing a port number to advertise over and the heart beat interval in milliseconds
        /// </summary>
        /// <param name="vPort"></param>
        /// <param name="vHeartbeatInterval"></param>
        public void StartAdvertising( double vHeartbeatInterval, int vPort= 6668)
        {
            if (mBroadcaster == null)
            {
                mBroadcaster = new Broadcast();
            }
            mBroadcaster.SetupSocket(vPort);
            mTimer = new Timer(vHeartbeatInterval);
            mTimer.AutoReset = true;
            mTimer.Elapsed += OnTimerElapsed;
            mTimer.Start();
        }

        private void OnTimerElapsed(object vSender, ElapsedEventArgs vE)
        {
            UpdateUi("Sending advertising packet ");
               Packet vPacket=  new Packet();
            vPacket.type = PacketType.AdvertisingPacket;
            vPacket.firmwareVersion = BrainpackModel.BrainpackInfo.Version;
            vPacket.serialNumber = BrainpackModel.BrainpackInfo.Id;
            vPacket.configurationPort = (uint) BrainpackModel.GetTcpControlPort;
            MemoryStream vStream = new MemoryStream();
            Serializer.Serialize(vStream, vPacket);
            RawPacket vRawPacket = new RawPacket();
            int vRawSize;
            var vRawBytes = vRawPacket.GetRawPacketByteArray(out vRawSize, vStream);
            mBroadcaster.Send(vRawBytes);
        }

        private void UpdateUi(string vMsg)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() => ConsoleTextBlockController.AddMsg(vMsg)));
        }
        public void StopHeartBeat()
        {
            mTimer.Stop();
            mTimer.Dispose();
        }
    }
}