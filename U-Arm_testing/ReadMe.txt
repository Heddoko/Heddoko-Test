The U-arm contains Arduino Uno on its board and the processor is Mega328.
Compile the example program and burn it to U-arm board. It accepts angles from serial terminal with the format:

Main Axis angle, First arm angle, Second arm angle, Hand rotation angle, \r\n

maintaining the above mentioned format is vital for the U-arm to run. You can skip providing value for the hand
rotation angle. The carriage return and line feed at the end are mandatory and so is each value being seperated
by a comma. 