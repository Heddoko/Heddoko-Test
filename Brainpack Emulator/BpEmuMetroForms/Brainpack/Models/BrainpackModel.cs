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
    public class BrainpackModel: IEqualityComparer
    {
       public string SerialNum { get; set; }  
        public Version FirmwareVersion { get; set; }
        public int ConfigurationPort { get; set; }
        public FileInfo FirmwarePath;
        public Sensor[] Sensors = new Sensor[9];
        public int UdpTransmitPort;
        
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
            if (vX.GetType() != typeof (BrainpackModel) || vY.GetType() != typeof (BrainpackModel))
            {
                return false;
            }
            BrainpackModel vModelX = (BrainpackModel) vX;
            BrainpackModel vModelY = (BrainpackModel) vY;
            return vModelY.SerialNum.Equals(vModelX.SerialNum);
        }

        public int GetHashCode(object vObj)
        {
            BrainpackModel vModelObj = (BrainpackModel)vObj;
            return vModelObj.SerialNum.GetHashCode();
        }
    }
}