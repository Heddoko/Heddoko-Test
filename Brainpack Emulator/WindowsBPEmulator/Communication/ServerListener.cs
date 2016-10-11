// /**
// * @file ServerListener.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 10 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using WindowsBPEmulator.Controller;
using WindowsBPEmulator.Properties;

namespace WindowsBPEmulator.Communication
{
    public class ServerListener
    {
        private List<StateObject> mStateobjects = new List<StateObject>();
        private ConsoleTextBlockController mConsoleTextBlockController;
        private Socket mServerSocket;
        public int PortNumber = 8844;
        private int mBacklog = 10;
        public event EventHandler<byte[]> DataReceived;

        public ServerListener(ConsoleTextBlockController vController)
        {
            mConsoleTextBlockController = vController;
        }

        private void UpdateUi(string vMsg)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, 
                new Action(() => mConsoleTextBlockController.AddMsg(vMsg)));
        }
        public bool StartServer()
        {
            UpdateUi("Starting Server");
            IPHostEntry vLocalHost = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            System.Net.IPEndPoint vServerEndPoint;
            try
            {
                vServerEndPoint = new IPEndPoint(vLocalHost.AddressList[0], PortNumber);
            }
            catch (ArgumentOutOfRangeException vArgument)
            {
                throw new ArgumentOutOfRangeException(Resources.ServerListener_StartServer_Port_number_is_invalid, vArgument);
            }
            try
            {
                mServerSocket = new Socket(vServerEndPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (SocketException vException)
            {
                throw new ApplicationException("Could not create socket, check to make sure that port is not being used by another socket", vException);
            }
            try
            {
                UpdateUi("Binding sockets...");
                mServerSocket.Bind(vServerEndPoint); 
                mServerSocket.Listen(mBacklog);
            }
            catch (Exception vException)
            {
                throw new ApplicationException("Error occured while binding socket, check inner exception", vException);
            }
            try
            {
                mServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), mServerSocket);
            }
            catch (Exception vE)
            {
                throw new ApplicationException("Error occured starting listener, check inner exception", vE);
            }
            return true;
        }

        private void AcceptCallback(IAsyncResult vAr)
        {
            StateObject vIncomingConnection = new StateObject();
            try
            {
                UpdateUi("AcceptCallback");

                //finish accepting the connection
                Socket vSocket = (Socket)vAr.AsyncState;
                vIncomingConnection = new StateObject();
                vIncomingConnection.Socket = vSocket.EndAccept(vAr);
                vIncomingConnection.Buffer = new byte[1024];
                lock (mStateobjects)
                {
                    mStateobjects.Add(vIncomingConnection);
                }
                vIncomingConnection.Socket.BeginReceive(vIncomingConnection.Buffer, 0, vIncomingConnection.Buffer.Length,
                    SocketFlags.None, new AsyncCallback(ReceiveCallback), vIncomingConnection);
                mServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), mServerSocket);
            }
            catch (SocketException vException)
            {
                CloseAndRemoveStateObject(vIncomingConnection);
                mServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), mServerSocket);
            }
            catch (Exception vE)
            {
                CloseAndRemoveStateObject(vIncomingConnection);
                mServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), mServerSocket);
            }

        }

        public bool Send(StateObject vConnection, byte[] vMsg)
        {
            UpdateUi("Sending byte count+ "+vMsg.Length);
            if (vConnection != null && vConnection.Socket.Connected)
            {
                lock (vConnection.Socket)
                {
                    vConnection.Socket.Send(vMsg, vMsg.Length, SocketFlags.None);
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        private void CloseAndRemoveStateObject(StateObject vObject)
        {
            if (vObject.Socket != null)
            {
                UpdateUi("closing and removing state object");
                vObject.Socket.LingerState = new LingerOption(true,0);
                vObject.Socket.Shutdown(SocketShutdown.Both);
                vObject.Socket.Close();
                lock (mStateobjects)
                {
                    mStateobjects.Remove(vObject);
                }
            }

        }

        /// <summary>
        /// Receive data handler
        /// </summary>
        /// <param name="vAr"></param>
        private void ReceiveCallback(IAsyncResult vAr)
        {
            //get connection from the callback
            StateObject vIncomingConnection = (StateObject)vAr.AsyncState;
            try
            {
                int vBytesRead = vIncomingConnection.Socket.EndReceive(vAr);
                if (vBytesRead > 0)
                {
                    UpdateUi("received byte count+ " + vBytesRead);

                    string vData = Encoding.ASCII.GetString(vIncomingConnection.Buffer);
                    if (vData.Contains("<EOL>"))
                    {
                        if (vData.Contains("closeConnection"))
                        {
                            CloseAndRemoveStateObject(vIncomingConnection);
                        }
                    }
                    else
                    {
                        //handle code here
                        DataReceived?.Invoke(vIncomingConnection, vIncomingConnection.Buffer);
                        vIncomingConnection.Socket.BeginReceive(vIncomingConnection.Buffer, 0,
                            vIncomingConnection.Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback),
                            vIncomingConnection);
                    }
                    
                }
                //else
                //{
                //    //callback run but there is no data, so close the connection
                //    //perhaps its a disconnection
                //    CloseAndRemoveStateObject(vIncomingConnection);

                //}
            }
            catch (SocketException vE)
            {
                CloseAndRemoveStateObject(vIncomingConnection);

            }
        }
    }
}