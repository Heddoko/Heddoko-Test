// /**
// * @file FirmwareDownload.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 10 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */


using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading; 
using Tftp.Net;

namespace WindowsBPEmulator.Communication
{

    public class FirmwareDownload
    {

        private TftpClient mTftpClient;
        private static AutoResetEvent TransferFinishedEvent = new AutoResetEvent(false);
         public void RequestFirmwareFromEndPoint(IPEndPoint vEndPoint, string vFileName)
        {
            Stopwatch vwatch = new Stopwatch();

            var client = new TftpClient(vEndPoint);
            var transfer = client.Download(vFileName);
            transfer.OnFinished += new TftpEventHandler(OnFinishedEvent);
            //Start the transfer and write the data that we're downloading into a memory stream
            FileStream stream = new FileStream("C:\\downl\\firmware.bin", FileMode.CreateNew);
            vwatch.Start();
            UpdateUi("Starting Firmware file transfer");
            transfer.Start(stream);
            TransferFinishedEvent.WaitOne();
            FileInfo vInfo = new FileInfo("C:\\downl\\firmware.bin");
            long vLength= vInfo.Length;
          
            vwatch.Stop();
            UpdateUi("finished file transfer of "+ vLength + " bytes in "+ (vwatch.ElapsedMilliseconds  /1000.0) + "seconds");
            stream.Close();

          
        }

        private void OnFinishedEvent(ITftpTransfer vTransfer)
        {
            TransferFinishedEvent.Set();
        }

      
        private void UpdateUi(string vMsg)
        {
             

        }
    }
}