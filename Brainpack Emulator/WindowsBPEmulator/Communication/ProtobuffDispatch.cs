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
using heddoko;
using HeddokoLib.heddokoProtobuff.Decoder;
using ProtoBuf;


namespace WindowsBPEmulator.Communication
{
    /// <summary>
    /// Dispatches protobuff packets 
    /// </summary>
    public class ProtobuffDispatch
    {

        private ServerListener mListener;
        private ProtobuffDispatchRouter mDispatchRouter;
        private StreamToRawPacketDecoder mDecoder;
        private StateObject mCurrStateObject;

        public ProtobuffDispatch(ServerListener vListener)
        {
            mListener = vListener;
            mListener.DataReceived += FilterData;
            mDecoder = new StreamToRawPacketDecoder(new MemoryStream());
            mDispatchRouter = new ProtobuffDispatchRouter();
            RegisterActions();
        }

        private void RegisterActions()
        {
            mDispatchRouter.Add(PacketType.UpdateFirmwareRequest, FirmwareUpdateRequest);
        }

        private void FirmwareUpdateRequest(object vSender, object vArgs)
        {
            var vPacket = (Packet)vArgs;
            var vStateObject = (StateObject)vSender; 
            var vEndpoint = vPacket.firmwareUpdate.fwEndpoint;
            long vEndPointInLongForm = long.Parse(vEndpoint.address);
            IPEndPoint vServerEndPoint = new IPEndPoint(vEndPointInLongForm, (int)vEndpoint.port);

            FirmwareDownload vDownload = new FirmwareDownload();
            vDownload.FirmwareReceived += OnCompletion;
            vDownload.RequestFirmwareFromEndPoint(vServerEndPoint, "C:\\firmware.binerino"); 

        }

        private void OnCompletion(object vSender, EventArgs vE)
        {
             FirmwareDownload vDownload = (FirmwareDownload)vSender;
            vDownload.FirmwareReceived -= OnCompletion;
            Packet vPacket = new Packet();

            vPacket.type = PacketType.UpdatedFirmwareResponse;
            MemoryStream vStream = new MemoryStream();
            Serializer.Serialize(vStream, vPacket);
            RawPacket vRawPacket = new RawPacket();

            int vRawSize;
            var vRawBytes = vRawPacket.GetRawPacketByteArray(out vRawSize, vStream);
            mListener.Send(mCurrStateObject,vRawBytes);
        }

        /// <summary>
        /// Filters data and attempts to find a packet
        /// </summary>
        /// <param name="vSender"></param>
        /// <param name="vReceivedBytes"></param>
        private void FilterData(object vSender, byte[] vReceivedBytes)
        {
            mCurrStateObject = (StateObject)vSender;
            MemoryStream vStream = new MemoryStream();
            vStream.Write(vReceivedBytes, 0, vReceivedBytes.Length);
            vStream.Seek(0, SeekOrigin.Begin);
            mDecoder.Stream = vStream;
            mDecoder.StartPacketizeStream(PacketizationCompleted, ExceptionHandler);
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
            RawPacket vRawPacket = mDecoder.OutputBuffer.Dequeue();

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


    }
}