/**
 * File: U-ARM program for Heddoko Testing
 * @author: Hriday Mehta
 * @copyright: Heddoko 2016.
 */

// headers should must include these four headers
#include "uarm_library.h"  //name of uArm Library
#include <string.h>

#define FAIL 1
#define PASS 0

#define MAX_DECIMAL_PLACES 2
#define MAX_REC_COUNT 50
#define INTERPOLATE_RES 10    // NOTE: this count should always be less than MAX_REC_COUNT

bool strReady = 0, recEn = 0, playEn = 0, easeEn = 0;
int strStatus = PASS;
String str;
double int1, int2, int3, int4;
double ang1_old, ang2_old, ang3_old;
double int1Array[MAX_REC_COUNT], int2Array[MAX_REC_COUNT], int3Array[MAX_REC_COUNT], int4Array[MAX_REC_COUNT];
String word1, word2, word3, word4;
double x, y, z;
int readCount = 0;

String getValue(String data, char separator, int index)
{
  int found = 0;
  int strIndex[] = {0, -1};
  int maxIndex = data.length()-1;

  for(int i=0; i<=maxIndex && found<=index; i++){
    if(data.charAt(i)==separator || i==maxIndex){
        found++;
        strIndex[0] = strIndex[1]+1;
        strIndex[1] = (i == maxIndex) ? i+1 : i;
    }
  }

  return found>index ? data.substring(strIndex[0], strIndex[1]) : "";
}

int validateStr(String data)
{
  int len;
  int stat = PASS;

  len = data.length();
  if (len > 3)
  {
      return FAIL;
  }
  for (int i=0; i < len; i++)
  {
    if ((data.charAt(i) > 47) && (data.charAt(i) < 58))   // check if the char represents a number
      stat |= PASS;
    else if ((data.charAt(i) == 10) || (data.charAt(i) == 13) || (data.charAt(i) == 46))    // accept CR and LF
      stat |= PASS;
    else
      stat |= FAIL;
  }
    
  return stat;
}

void setup() 
{
        Serial.begin(115200);
        //Serial.setTimeout(500);
        Serial.write("System Initialized\r\n");  
        uarm.init();
        int1 = uarm.read_servo_offset(SERVO_ROT_NUM);
        int2 = uarm.read_servo_offset(SERVO_LEFT_NUM);
        int3 = uarm.read_servo_offset(SERVO_RIGHT_NUM);
        Serial.print("Servo Offsets: ");
        Serial.print(int1, DEC);
        Serial.print(int2, DEC);
        Serial.print(int3, DEC);
        Serial.print("\r\n");
}

//void serialEvent()
//{
//  int temp;
//  if (!strReady)
//  {
//    temp = Serial.read();
//    str.concat(temp);
//    if (temp == '\n')
//    {
//      strReady = 1;
//    }
//  }
//}

void convertToCoordinate(double theta_1, double theta_2, double theta_3)
{
        double l5 = (MATH_L2 + MATH_L3*cos(theta_2 / MATH_TRANS) + MATH_L4*cos(theta_3 / MATH_TRANS));

        x = -cos(abs(theta_1 / MATH_TRANS))*l5;
        y = -sin(abs(theta_1 / MATH_TRANS))*l5;
        z = MATH_L1 + MATH_L3*sin(abs(theta_2 / MATH_TRANS)) - MATH_L4*sin(abs(theta_3 / MATH_TRANS));
}

void loop() 
{
  
        if(Serial.available()>0)
        {
          str = Serial.readStringUntil('\n');    // this function can timeout, hence make a check before moving forward
          if (str.compareTo("Rec\r") == 0)
            recEn = 1;
          else if (str.compareTo("Play\r") == 0)
            playEn = 1;
          else if (str.compareTo("EaseEn\r") == 0)    // Enable interpolation of path for smooth movement
            easeEn = 1;
          else if (str.compareTo("EaseDis\r") == 0)   // Disbale interpolation of path.
            easeEn = 0;
          else    // this is input angle data
          {
            if (str.length() > 3) // check if the string has valid length
            {
              strStatus = 0;
            
              word1 = getValue(str, ',', 0);
              //strStatus = validateStr(word1);
              
              word2 = getValue(str, ',', 1);
              //strStatus |= validateStr(word2);
              
              word3 = getValue(str, ',', 2);
              //strStatus |= validateStr(word3);
              
              //word4 = getValue(str, ',', 3);
    
              //if (strStatus == PASS)
                strReady = 1;
            }
          }

          /*  Go to specified destination  */
          if (strReady)
          {
            Serial.print("Moving to: ");
            Serial.println(str);
            strReady = 0;
            strStatus = 0;
            int1 = (double)(word1.toFloat()/1);
            int2 = (double)(word2.toFloat()/1);
            int3 = (double)(word3.toFloat()/1);
            //int4 = word4.toInt();

            if (!easeEn)    // Move to destination directly
              uarm.write_servo_angle(int1,int2,int3,10);
              //uarm.move_to(x, y, z, 0, F_ABSOLUTE, 1, PATH_ANGLES, INTERP_EASE_INOUT, false);
            
            else    // Interpolate the path to the destination
            {
              ang1_old = uarm.read_servo_angle(SERVO_ROT_NUM, true);
              ang2_old = uarm.read_servo_angle(SERVO_LEFT_NUM, true);
              ang3_old = uarm.read_servo_angle(SERVO_RIGHT_NUM, true);

              int1Array[0] = ang1_old + ((int1 - ang1_old)/INTERPOLATE_RES);
              int2Array[0] = ang2_old + ((int2 - ang2_old)/INTERPOLATE_RES);
              int3Array[0] = ang3_old + ((int3 - ang3_old)/INTERPOLATE_RES);
              
              for (int i=0; i<INTERPOLATE_RES; )
              {
                Serial.print(int1Array[i], MAX_DECIMAL_PLACES);
                Serial.print(",");
                Serial.print(int2Array[i], MAX_DECIMAL_PLACES);
                Serial.print(",");
                Serial.print(int3Array[i], MAX_DECIMAL_PLACES);
                Serial.println(",");
                uarm.write_servo_angle(int1Array[i], int2Array[i], int3Array[i], 10);
                i++;
                int1Array[i] = int1Array[i-1] + ((int1 - ang1_old)/INTERPOLATE_RES);
                int2Array[i] = int2Array[i-1] + ((int2 - ang2_old)/INTERPOLATE_RES);
                int3Array[i] = int3Array[i-1] + ((int3 - ang3_old)/INTERPOLATE_RES);
                delay(50);
              }
              uarm.write_servo_angle(int1,int2,int3,10);
            }
            Serial.print("OK\n");
          }
          
          /*  Record a movement */  
          if (recEn)
          {
            Serial.println("Recording: Disonnect Power");
            delay(1000);
            Serial.println("Recording Started");
            readCount = 0;
            uarm.set_servo_status(false, SERVO_ROT_NUM);
            uarm.set_servo_status(false, SERVO_LEFT_NUM);
            uarm.set_servo_status(false, SERVO_RIGHT_NUM);
            uarm.set_servo_status(false, SERVO_HAND_ROT_NUM);

            //Serial.setTimeout(100); // change timeout to change the resolution of mapping
            
            while (1)
            {
              str = Serial.readStringUntil('\n');   // adds set serial timeout. Default is 1000ms
              if (str.compareTo("End\r") == 0)
              {
                Serial.println("End of Recording");
                recEn = 0;
                break;
              }
              //uarm.angle_to_coordinate(uarm.read_servo_angle(SERVO_ROT_NUM), uarm.read_servo_angle(SERVO_LEFT_NUM), uarm.read_servo_angle(SERVO_RIGHT_NUM),\
                                                //int1Array[readCount], int2Array[readCount], int3Array[readCount]);    // store coordinates in array
              
              int1Array[readCount] = uarm.read_servo_angle(SERVO_ROT_NUM, true);    // store angles in the array
              int2Array[readCount] = uarm.read_servo_angle(SERVO_LEFT_NUM, true);
              int3Array[readCount] = uarm.read_servo_angle(SERVO_RIGHT_NUM, true);

              /*  Perform safety adjustments  */
              if(int2Array[readCount] < 43) int2Array[readCount] = 43;    
              if(int2Array[readCount] > 180) int2Array[readCount] = 180;  
              if(int3Array[readCount] < 10) int3Array[readCount] = 10;    
              if(int3Array[readCount] > 120) int3Array[readCount] = 120;  
      
              if(int2Array[readCount] + int3Array[readCount] > 200)   
              {
                      int3Array[readCount] = 200 - int2Array[readCount];
              }

              /*  Send the reading  */
              Serial.print("Reading: ");
              Serial.print((double)(int1Array[readCount]*1), MAX_DECIMAL_PLACES);
              Serial.print(",");
              Serial.print((double)(int2Array[readCount]*1), MAX_DECIMAL_PLACES);
              Serial.print(",");
              Serial.print((double)(int3Array[readCount]*1), MAX_DECIMAL_PLACES);
              Serial.println(",");
              readCount++;
              if (readCount >= MAX_REC_COUNT)
              {
                Serial.println("End of Recording");
                recEn = 0;
                break;
              }
              //delay(100);   // adds up to the delay provided by the Serial timeout
            }
            //Serial.setTimeout(1000); // change back the serial timeout
            uarm.set_servo_status(true, SERVO_ROT_NUM);
            uarm.set_servo_status(true, SERVO_LEFT_NUM);
            uarm.set_servo_status(true, SERVO_RIGHT_NUM);
            uarm.set_servo_status(true, SERVO_HAND_ROT_NUM);
          }

          /*  Playback the recorded movement  */
          if (playEn)
          {
            for (int i=0; i<readCount; i++)
            {
              Serial.print(int1Array[i], MAX_DECIMAL_PLACES);
              Serial.print(",");
              Serial.print(int2Array[i], MAX_DECIMAL_PLACES);
              Serial.print(",");
              Serial.print(int3Array[i], MAX_DECIMAL_PLACES);
              Serial.println(",");
              //uarm.coordinate_to_angle(int1Array[i], int2Array[i], int3Array[i], x, y, z);  //convert coordinates to angles
              uarm.write_servo_angle(int1Array[i], int2Array[i], int3Array[i], 10);  // write the angles
              delay(1000);
            }
            Serial.println("End of playback");
            //uarm.alert(2,50,50);
            playEn = 0;
          }
        }
}

