// /**
// * @file BrainpackAdvertiser.cs
// * @brief Contains the 
// * @author Mohammed Haider( mohammed@heddoko)
// * @date 10 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */


using System.IO;
using System.Timers;
using heddoko;
using HeddokoLib.heddokoProtobuff.Decoder;
using ProtoBuf;

namespace BpEmuMetroForms.Brainpack.Communication
{
    /// <summary>
    /// Sends out an advertising packet over udp, indicating that the brainpack is online and how to connect to its control port
    /// </summary>
    public class BrainpackAdvertiser
    {
        Timer mTimer;
        private Broadcast mBroadcaster;
        private BrainpackModel mModel;

        /// <summary>
        /// Start advertising by providing a port number to advertise over and the heart beat interval in milliseconds
        /// </summary>
        /// <param name="vPort"></param>
        /// <param name="vHeartbeatInterval"></param>
        /// 
        public BrainpackAdvertiser(BrainpackModel vModel)
        {
            mModel = vModel;
        }
        public void StartAdvertising(int vPort = 6668)
        {
            if (mBroadcaster == null)
            {
                mBroadcaster = new Broadcast();
            }
            mBroadcaster.SetupSocket(vPort);
            mTimer = new Timer(mModel.AdvertisingTickRate);
            mTimer.AutoReset = true;
            mTimer.Elapsed += OnTimerElapsed;
            mTimer.Start();

        }

        private void OnTimerElapsed(object vSender, ElapsedEventArgs vE)
        {
            UpdateUi("Sending advertising packet ");
            Packet vPacket = new Packet();
            vPacket.type = PacketType.AdvertisingPacket;
            vPacket.firmwareVersion = mModel.FirmwareVersion.ToString();
            vPacket.serialNumber = mModel.SerialNum;
            vPacket.configurationPort = (uint)mModel.ConfigurationPort;
            MemoryStream vStream = new MemoryStream();
            Serializer.Serialize(vStream, vPacket);
            RawPacket vRawPacket = new RawPacket();
            int vRawSize;
            var vRawBytes = vRawPacket.GetRawPacketByteArray(out vRawSize, vStream);
            mBroadcaster.Send(vRawBytes, vRawSize);
        }

        private void UpdateUi(string vMsg)
        {

        }
        public void StopAdvertising()
        {
            mTimer.Stop();
            mTimer.Dispose();
            mBroadcaster.CloseSocket();
        }

        public void SetTickRate(int vAdvertisingTickRate)
        {
            if (mTimer != null)
            {
                mTimer.Interval = vAdvertisingTickRate;
            }
        }
    }
}