using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PacketTester
{
    public class FullRawFrame
    {
        ImuFrame[] imuFrameArray;
        const byte packetType = 0x55;
        byte numberOfSensors;
        UInt32 timestamp;

        public FullRawFrame(int numSensors)
        {
            imuFrameArray = new ImuFrame[numSensors];
            for(int i =0; i<numSensors; i++)
            {
                imuFrameArray[i] = new ImuFrame();
            }
            numberOfSensors = (byte)numSensors;
        }
        public void setTimestamp(UInt32 value)
        {
            timestamp = value; 
        }
        public void populateFrameWithTestData()
        {
            timestamp++;
            for (int i = 0; i < numberOfSensors; i++)
            {

                imuFrameArray[i].ImuId = (byte)i; //this is the IMU ID
                imuFrameArray[i].Quaternion_x = 0.1F;
                imuFrameArray[i].Quaternion_y = 0.2F;
                imuFrameArray[i].Quaternion_z = 0.3F;
                imuFrameArray[i].Quaternion_w = 0.4F;
                imuFrameArray[i].Magnetic_x++;
                imuFrameArray[i].Magnetic_y++;
                imuFrameArray[i].Magnetic_z++;
                imuFrameArray[i].Acceleration_x = 8;
                imuFrameArray[i].Acceleration_y = 9;
                imuFrameArray[i].Acceleration_z = 10;
                imuFrameArray[i].Rotation_x = 11;
                imuFrameArray[i].Rotation_y = 12;
                imuFrameArray[i].Rotation_z = 13;
            }
        }
        public byte[] serializeFrame(out UInt16 numberOfBytes)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            byte source = 0x05;
            //timestamp = (UInt32)DateTime.Now.Ticks;
            byte[] header = { source, packetType, numberOfSensors };
            stream.Write(header, 0, 3);
            stream.Write(BitConverter.GetBytes(timestamp), 0, 4);
            
            for(int i = 0; i < numberOfSensors; i++)
            {
                imuFrameArray[i].serialize(ref stream);
            }

            numberOfBytes = (UInt16)stream.Length;

            return stream.ToArray();
        }
    }

}
