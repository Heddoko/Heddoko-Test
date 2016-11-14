// /**
// * @file ProtobuffDispatch.cs
// * @brief Contains the 
// * @author Mohammed Haider( mohammed@heddoko.com)
// * @date 10 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.IO;
using System.Net;
using BpEmuMetroForms.Brainpack.Communication;
using heddoko;
using HeddokoLib.heddokoProtobuff.Decoder;
using ProtoBuf;


namespace WindowsBPEmulator.Communication
{
    /// <summary>
    /// Dispatches protobuff packets 
    /// </summary>
    public class ProtobuffDispatch : IDisposable
    {

        private ServerListener mListener;
        private ProtobuffDispatchRouter mDispatchRouter;
        private RawPacketDecoder mDecoder;
        private StateObject mCurrStateObject;

        public ProtobuffDispatch(ServerListener vListener)
        {
            mListener = vListener;
            mListener.DataReceived += FilterData;
            mDecoder = new RawPacketDecoder();
            mDecoder.PacketizationCompletedEvent += PacketizationCompleted;
            mDispatchRouter = new ProtobuffDispatchRouter();
            RegisterActions();
            mDecoder.Start();
        }

        private void RegisterActions()
        {
            mDispatchRouter.Add(PacketType.UpdateFirmwareRequest, FirmwareUpdateRequest);
            mDispatchRouter.Add(PacketType.StatusRequest, BrainpackStatusResponse);
            mDispatchRouter.Add(PacketType.StartDataStream, StartDataStream);
        }

        private void StartDataStream(object vVsender, object vVargs)
        {
            
        }

        private void BrainpackStatusResponse(object vSender, object vVargs)
        {
           // StateObject vObject = (StateObject) vSender;
            Packet vPacket = new Packet();
            vPacket.type = PacketType.StatusResponse;
            vPacket.firmwareVersion = "1.1.2.3";
            vPacket.serialNumber = "S0123";
            MemoryStream vStream = new MemoryStream();
            Serializer.Serialize(vStream, vPacket);
            RawPacket vRawPacket = new RawPacket();

            int vRawSize;
            var vRawBytes = vRawPacket.GetRawPacketByteArray(out vRawSize, vStream);
            mListener.Send(mCurrStateObject, vRawBytes);
        }

        private void FirmwareUpdateRequest(object vSender, object vArgs)
        {
            var vPacket = (Packet)vArgs;
            var vEndpoint = vPacket.firmwareUpdate.fwEndpoint;
            IPAddress vAddress = IPAddress.Parse(vEndpoint.address);
            IPEndPoint vServerEndPoint = new IPEndPoint(vAddress, (int)vEndpoint.port);
            FirmwareDownload vDownload = new FirmwareDownload();
             vDownload.RequestFirmwareFromEndPoint(vServerEndPoint, vPacket.firmwareUpdate.fwFilename);

        }

        private void OnCompletion(object vSender, EventArgs vE)
        {
            FirmwareDownload vDownload = (FirmwareDownload)vSender;
             Packet vPacket = new Packet();

            vPacket.type = PacketType.UpdatedFirmwareResponse;
            MemoryStream vStream = new MemoryStream();
            Serializer.Serialize(vStream, vPacket);
            RawPacket vRawPacket = new RawPacket();

            int vRawSize;
            var vRawBytes = vRawPacket.GetRawPacketByteArray(out vRawSize, vStream);
            mListener.Send(mCurrStateObject, vRawBytes);
        }

        /// <summary>
        /// Filters data and attempts to find a packet
        /// </summary>
        /// <param name="vSender"></param>
        /// <param name="vReceivedBytes"></param>
        private void FilterData(object vSender, byte[] vReceivedBytes)
        {
            mCurrStateObject = (StateObject)vSender;
            mDecoder.EnqueueRawBytes(vReceivedBytes);

        }

        /// <summary>
        /// Exception handler on stream packetization completion
        /// </summary>
        /// <param name="vObj"></param>
        private void ExceptionHandler(Exception vObj)
        {
            throw vObj;
        }

        /// <summary>
        /// Packetization completed event
        /// </summary>
        private void PacketizationCompleted()
        {
            RawPacket vRawPacket = mDecoder.ConvertedPackets.Dequeue();

            if (vRawPacket.Payload[0] == 0x04)
            {
                MemoryStream vMemoryStream = new MemoryStream();
                //reset the stream pointer, write and reset.
                vMemoryStream.Seek(0, SeekOrigin.Begin);
                vMemoryStream.Write(vRawPacket.Payload, 1, (int)vRawPacket.PayloadSize - 1);
                vMemoryStream.Seek(0, SeekOrigin.Begin);
                Packet vProtoPacket = Serializer.Deserialize<Packet>(vMemoryStream);
                mDispatchRouter.Process(vProtoPacket.type, this, vProtoPacket);
            }
        }


        public void Dispose()
        {
            mDecoder.PacketizationCompletedEvent -= PacketizationCompleted;

        }
    }
}