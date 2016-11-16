// /**
// * @file BrainpackModel.cs
// * @brief Contains the 
// * @author Mohammed Haider(mohammed@heddoko.com) 
// * @date 11 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.Collections;
using System.IO;

namespace BpEmuMetroForms.Brainpack
{
    public delegate void ModelChangedEvent(BrainpackModel brainpackModel);
    public class BrainpackModel : IEqualityComparer
    {
        public string SerialNum { get; set; }
        public Version FirmwareVersion { get; set; }
        public int ConfigurationPort { get; set; }

        private int mAdvertisingTickRate;
        private int mConcernReportTickRate;
        public int AdvertisingTickRate
        {
            get
            {
                return mAdvertisingTickRate;
            }
            set
            {
                mAdvertisingTickRate = value;
                ModelChangedEvent?.Invoke(this);
            }
        }

        public int ConcernReportTickRate
        {
            get { return mConcernReportTickRate; }
            set
            {
                mConcernReportTickRate = value;
                ModelChangedEvent?.Invoke(this);
            }
        }

        public FileInfo FirmwarePath;
        public FileInfo RecordingFile;
        public Sensor[] Sensors = new Sensor[9];
        public int UdpTransmitPort;
        public event ModelChangedEvent ModelChangedEvent;
        public BrainpackModel()
        {
            for (int i = 0; i < Sensors.Length; i++)
            {
                var vSensor = new Sensor();
                vSensor.CurrentState = Sensor.State.Enabled;
                vSensor.Index = i;
                vSensor.IsCalibrated = true;
                vSensor.IsInMagneticTransience = false;
                Sensors[i] = vSensor;
            }
        }
        public new bool Equals(object vX, object vY)
        {
            if (vX == null || vY == null)
            {
                return false;
            }
            if (vX.GetType() != typeof(BrainpackModel) || vY.GetType() != typeof(BrainpackModel))
            {
                return false;
            }
            BrainpackModel vModelX = (BrainpackModel)vX;
            BrainpackModel vModelY = (BrainpackModel)vY;
            return vModelY.SerialNum.Equals(vModelX.SerialNum);
        }

        public int GetHashCode(object vObj)
        {
            BrainpackModel vModelObj = (BrainpackModel)vObj;
            return vModelObj.SerialNum.GetHashCode();
        }
    }
}