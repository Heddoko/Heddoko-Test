// /**
// * @file FileStreamer.cs
// * @brief Contains the 
// * @author Mohammed Haider(mohammed@heddoko.com)
// * @date November 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System.IO;
using System.Threading;

namespace WindowsBPEmulator.Communication
{
    public class FileStreamer
    {
        private FileInfo mFileToSend;
         private bool mIsWorking = false;
        private UdpConnectionSend mConnection;
        private Thread mWorkerThread;
        private int mPointer = 0;
        private readonly int mBufferLength = 1024;
        public byte[] ItemToInterleave;
        /// <summary>
        /// Sets the connection for the file stream
        /// </summary>
        /// <param name="vUdpConnection"></param>
        public void SetConnection(UdpConnectionSend vUdpConnection)
        {
            mConnection = vUdpConnection;
        }
        public FileInfo FileToSend
        {
            get { return mFileToSend; }
            set { mFileToSend = value; }
        }

        /// <summary>
        /// Begin File send on a loop
        /// </summary>
        /// <param name="vInfo"></param>
        public void BeginFileSend(FileInfo vInfo)
        {
            mFileToSend = vInfo;
            mIsWorking = true;
            mWorkerThread = new Thread(WorkingFunc);
            mWorkerThread.IsBackground = true;
            mWorkerThread.Start();
        }

        /// <summary>
        /// The working functions
        /// </summary>
        private void WorkingFunc()
        { 
            using (System.IO.FileStream vStream = File.OpenRead(mFileToSend.FullName))
            {
                long vFileSize = mFileToSend.Length;
                while (mIsWorking)
                {
                    //check if the remaining file exceeds buffer length. Set the reset flag
                    int vTake = mBufferLength;
                    bool vResetFlag = false;
                    if (vFileSize - mPointer < mBufferLength)
                    {
                        vTake = (int) (vFileSize - mPointer);
                        vResetFlag = true;
                    }
                    byte[] vBuffer = new byte[vTake];
                    vStream.Read(vBuffer, mPointer, vTake);
                    mConnection.Send(vBuffer, vTake);
                    if (ItemToInterleave != null)
                    {
                        mConnection.Send(ItemToInterleave,ItemToInterleave.Length);
                    }
                    mPointer = vResetFlag? 0 :mPointer + vTake ;
                }
            }
        }

        /// <summary>
        /// Switches the current file
        /// </summary>
        /// <param name="vFileInfo"></param>
        public void SwitchFile(FileInfo vFileInfo)
        {
            //Kill thread and make a new instance
            if (mWorkerThread.IsAlive)
            {
                mIsWorking = false;
                mWorkerThread.Join();
            }
            BeginFileSend(vFileInfo);
        }

        public void Stop()
        {
            mIsWorking = false;
        }
    }
}