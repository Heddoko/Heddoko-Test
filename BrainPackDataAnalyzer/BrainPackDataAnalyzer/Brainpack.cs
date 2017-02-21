using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BrainPackDataAnalyzer
{
    public class Brainpack
    {
        private string serialNumber = "BPXXXXX";
        private string firmwareVersion = "0.0.0.0";
        private uint configurationPort = 0;
        private IPEndPoint remoteEP;
        private bool isRemoteEpSet = false; 
        public Brainpack(string serial,string firmware,uint configPort, IPEndPoint endpoint)
        {
            serialNumber = serial;
            firmwareVersion = firmware;
            configurationPort = configPort;
            remoteEP = endpoint;
            isRemoteEpSet = true;
        }
        public Brainpack()
        {
            serialNumber = "BPXXXXX";
            firmwareVersion = "0.0.0.0";
            configurationPort = 0;
            remoteEP = new IPEndPoint(0,0);
            isRemoteEpSet = false; 
        }


        public IPEndPoint RemoteEP
        {
            get
            {
                return remoteEP;
            }

            set
            {
                remoteEP = value;
                isRemoteEpSet = true; 
            }
        }

        public uint ConfigurationPort
        {
            get
            {
                return configurationPort;
            }

            set
            {
                configurationPort = value;
            }
        }

        public string FirmwareVersion
        {
            get
            {
                return firmwareVersion;
            }

            set
            {
                firmwareVersion = value;
            }
        }

        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }

            set
            {
                serialNumber = value;
            }
        }

        public bool IsRemoteEpSet
        {
            get
            {
                return isRemoteEpSet;
            }

            set
            {
                isRemoteEpSet = value;
            }
        }
    }
}
