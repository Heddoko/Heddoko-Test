// /**
// * @file Sensor.cs
// * @brief Contains the Sensor class
// * @author Mohammed Haider( mohammed@heddoko.com)
// * @date 11 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */
namespace BpEmuMetroForms.Brainpack
{
    public class Sensor
    {
        public bool IsInMagneticTransience;
        public bool IsCalibrated;
        public int Index;
        public State CurrentState;
        public enum State
        {
            Enabled,
            Disabled
        }
        
    }
}