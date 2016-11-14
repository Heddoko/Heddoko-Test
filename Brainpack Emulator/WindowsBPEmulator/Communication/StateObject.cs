// /**
// * @file StateObject.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 10 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System.Net.Sockets;

namespace WindowsBPEmulator.Communication
{
    public class StateObject
    {
        public int BufferSize=1024;
        public byte[] Buffer;
        public  Socket Socket;

    }
}