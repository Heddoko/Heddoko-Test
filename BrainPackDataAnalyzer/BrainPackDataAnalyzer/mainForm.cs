using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using ProtoBuf;
using heddoko;

namespace BrainPackDataAnalyzer
{
    public partial class mainForm : Form
    {
        public Thread readThread;

        public FileStream dataFile;
        public DataTable analysisData;
        public DataTable sensorStats;
        public ConcurrentQueue<string> incomingDataQueue;
        public imu[] imuArray = new imu[9];
        private bool openSerialPort = false;
        private bool processDataTheadEnabled = true;
        private bool processDebugThreadEnabled = false;
        public ConcurrentQueue<string> debugMessageQueue;
        private bool processPacketQueueEnabled = false;
        public ConcurrentQueue<RawPacket> packetQueue;
        private bool mIsBatchModeSelected = false;
        public mainForm()
        {
            InitializeComponent();
        }
        public void processDebugMessagesThread()
        {
            while (processDebugThreadEnabled)
            {
                string line;
                if (debugMessageQueue.Count > 0)
                {
                    if (debugMessageQueue.TryDequeue(out line))
                    {
                        this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText(line)));
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }
        public void ReadThread()
        {
            while (true)
            {
                if (serialPort.IsOpen)
                {
                    try
                    {
                        string line = serialPort.ReadLine();
                        lock (serialPortPassThrough)
                        {
                            if (serialPortPassThrough.IsOpen)
                            {
                                serialPortPassThrough.WriteLine(line);
                            }
                        }
                        if (line.Length == 176)
                        {
                            processEntry(line);
                            if (dataFile != null)
                            {
                                lock (dataFile)
                                {
                                    if (dataFile.CanWrite)
                                    {
                                        line += "\r\n";
                                        dataFile.Write(System.Text.Encoding.ASCII.GetBytes(line), 0, line.Length);
                                    }
                                }
                            }
                        }
                        else if (line.Length == 14 && line.Contains("&"))
                        {
                            //process the quintic frame
                            processImuEntry(line);

                        }
                        else
                        {
                            this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText(line + "\r\n")));

                        }

                    }
                    catch
                    {
                        //do nothing, this is alright
                    }
                    if (!openSerialPort)
                    {
                        this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText("Serial Port Closed\r\n")));
                        serialPort.Close();
                        return;
                    }

                }
            }
        }
        public void processDataThread()
        {
            while (processDataTheadEnabled)
            {
                string line;
                if (incomingDataQueue.Count > 0)
                {
                    if (incomingDataQueue.TryDequeue(out line))
                    {
                        if (line.Length == 176 || line.Length == 175)
                        {
                            processEntry(line);
                            if (dataFile != null)
                            {
                                lock (dataFile)
                                {
                                    if (dataFile.CanWrite)
                                    {
                                        line += "\r\n";
                                        dataFile.Write(System.Text.Encoding.ASCII.GetBytes(line), 0, line.Length);
                                    }
                                }
                            }
                        }
                        else if (line.Length == 14 && line.Contains("&"))
                        {
                            //process the quintic frame
                            processImuEntry(line);

                        }
                        else
                        {
                            this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText(line + "\r\n")));
                        }
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }

            }
        }
        private void processProtoPacket(Packet packet)
        {
            switch (packet.type)
            {
                case PacketType.BrainPackVersionResponse:
                    if (packet.brainPackVersionSpecified)
                    {
                        debugMessageQueue.Enqueue("Received Version Response:" +
                            packet.brainPackVersion + "\r\n");
                    }
                    else
                    {
                        debugMessageQueue.Enqueue("Error Version not found\r\n");
                    }
                    break;
                case PacketType.BatteryChargeResponse:
                    debugMessageQueue.Enqueue("Received Battery Charge Response:" +
                        packet.batteryCharge.ToString() + "\r\n");
                    break;
                case PacketType.StateResponse:
                    break;
                case PacketType.DataFrame:
                    if (packet.fullDataFrame != null)
                    {
                        //debugMessageQueue.Enqueue("Received Data Frame From Timestamp:" +
                        //packet.fullDataFrame.timeStamp.ToString() + "\r\n");
                        processFullDataFrame(packet.fullDataFrame);
                    }
                    break;
            }

        }
        private void processFullDataFrame(FullDataFrame dataFrame)
        {
            try
            {
                int selectedImu = System.Convert.ToInt32(nud_SelectedImu.Value);
                UInt32 timeStamp = dataFrame.timeStamp;
                //UInt16 sensorMask = UInt16.Parse(entrySplit[1], System.Globalization.NumberStyles.HexNumber);
                for (int i = 0; i < dataFrame.imuDataFrame.Count; i++)
                {
                    int sensorId = (int)dataFrame.imuDataFrame[i].imuId;
                    //if the frame is valid for this sensor then process it. 
                    if (sensorId < 9 && dataFrame.imuDataFrame[i].sensorMask != 0)
                    {
                        imuArray[dataFrame.imuDataFrame[i].imuId].ProcessEntry(timeStamp, dataFrame.imuDataFrame[i]);
                        imuArray[i].EntryUpdated = true;
                    }
                    else
                    {
                        imuArray[i].EntryUpdated = false;
                        continue;
                    }

                    //DataRow row = sensorStats.NewRow();
                    sensorStats.Rows[sensorId]["Sensor ID"] = sensorId.ToString();
                    sensorStats.Rows[sensorId]["Roll"] = imuArray[sensorId].GetCurrentEntry().Roll.ToString("F3") + convertToArrow(imuArray[sensorId].GetCurrentEntry().Roll);
                    sensorStats.Rows[sensorId]["Pitch"] = imuArray[sensorId].GetCurrentEntry().Pitch.ToString("F3") + convertToArrow(imuArray[sensorId].GetCurrentEntry().Pitch);
                    sensorStats.Rows[sensorId]["Yaw"] = imuArray[sensorId].GetCurrentEntry().Yaw.ToString("F3") + convertToArrow(imuArray[sensorId].GetCurrentEntry().Yaw);
                    sensorStats.Rows[sensorId]["Frame Count"] = imuArray[sensorId].GetTotalEntryCount().ToString();
                    sensorStats.Rows[sensorId]["Interval"] = imuArray[sensorId].GetLastInterval().ToString();
                    sensorStats.Rows[sensorId]["Max Interval"] = imuArray[sensorId].GetMaxInterval().ToString();
                    sensorStats.Rows[sensorId]["Average Interval"] = imuArray[sensorId].GetAverageInterval().ToString();

                    if (selectedImu == sensorId)
                    {
                        this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Roll"].Points.AddY(imuArray[selectedImu].GetCurrentEntry().Roll)));
                        this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Pitch"].Points.AddY(imuArray[selectedImu].GetCurrentEntry().Pitch)));
                        this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Yaw"].Points.AddY(imuArray[selectedImu].GetCurrentEntry().Yaw)));

                        if (chrt_dataChart.Series["Roll"].Points.Count > 150)
                        {
                            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Roll"].Points.RemoveAt(0)));
                            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Pitch"].Points.RemoveAt(0)));
                            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Yaw"].Points.RemoveAt(0)));
                        }
                    }


                }
            }
            catch
            {
                //error
            }
        }

        private void processPacket(RawPacket packet)
        {
            //check that the packet comes from an IMU sensor
            if (packet.Payload[0] == 0x03)
            {

            }
            else if (packet.Payload[0] == 0x04) //this is a protocol buffer file. 
            {
                Stream stream = new MemoryStream(packet.Payload, 1, packet.PayloadSize - 1);
                try
                {
                    Packet protoPacket = Serializer.Deserialize<Packet>(stream);
                    processProtoPacket(protoPacket);
                }
                catch
                {
                    debugMessageQueue.Enqueue("Failed to deserialize packet\r\n");
                }

            }

        }

        public void processPacketThread()
        {
            while (processPacketQueueEnabled)
            {
                RawPacket packet;
                if (packetQueue.Count > 0)
                {
                    if (packetQueue.TryDequeue(out packet))
                    {
                        processPacket(packet);
                    }
                }
                else
                {
                    Thread.Sleep(2);
                }
            }
        }
        public string convertToArrow(double value)
        {
            string retVal = "";
            if (value >= (-Math.PI / 8) && value < (Math.PI / 8))
            {
                //right arrow
                retVal += " \u2192";
            }
            else if (value >= (-(3 * Math.PI) / 8) && value < (-Math.PI / 8))
            {
                //right down
                retVal += " \u2198";
            }
            else if (value >= (-(5 * Math.PI) / 8) && value < (-(3 * Math.PI) / 8))
            {
                //down
                retVal += " \u2193";
            }
            else if (value >= (-(7 * Math.PI) / 8) && value < (-(5 * Math.PI) / 8))
            {
                //left down
                retVal += " \u2199";
            }
            else if ((value >= (7 * Math.PI) / 8) && value <= Math.PI)
            {
                //left
                retVal += " \u2190";
            }
            else if ((value >= -Math.PI) && (value <= -(7 * Math.PI) / 8))
            {
                //left
                retVal += " \u2190";
            }
            else if (value >= ((5 * Math.PI) / 8) && value < ((7 * Math.PI) / 8))
            {
                //left up
                retVal += " \u2196";
            }
            else if (value >= ((3 * Math.PI) / 8) && value < ((5 * Math.PI) / 8))
            {
                //up
                retVal += " \u2191";
            }
            else if (value >= ((Math.PI) / 8) && value < ((3 * Math.PI) / 8))
            {
                //up right
                retVal += " \u2197";
            }
            else
            {
                retVal += "X";
            }
            return retVal;
        }
        public void processImuEntry(string entry)
        {
            string formattedEntry = entry.Substring(2, 4) + ";" + entry.Substring(6, 4) + ";" + entry.Substring(10, 4);
            int i = 0;
            try
            {
                UInt32 timeStamp = 0;
                imuArray[i].ProcessEntry(timeStamp, formattedEntry);
                //DataRow row = sensorStats.NewRow();
                sensorStats.Rows[i]["Sensor ID"] = i.ToString();
                sensorStats.Rows[i]["Roll"] = imuArray[i].GetCurrentEntry().Roll.ToString("F3") + convertToArrow(imuArray[i].GetCurrentEntry().Roll);
                sensorStats.Rows[i]["Pitch"] = imuArray[i].GetCurrentEntry().Pitch.ToString("F3") + convertToArrow(imuArray[i].GetCurrentEntry().Pitch);
                sensorStats.Rows[i]["Yaw"] = imuArray[i].GetCurrentEntry().Yaw.ToString("F3") + convertToArrow(imuArray[i].GetCurrentEntry().Yaw);


                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Roll"].Points.AddY(imuArray[0].GetCurrentEntry().Roll)));
                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Pitch"].Points.AddY(imuArray[0].GetCurrentEntry().Pitch)));
                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Yaw"].Points.AddY(imuArray[0].GetCurrentEntry().Yaw)));

                if (chrt_dataChart.Series["Roll"].Points.Count > 150)
                {
                    this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Roll"].Points.RemoveAt(0)));
                    this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Pitch"].Points.RemoveAt(0)));
                    this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Yaw"].Points.RemoveAt(0)));
                }

            }
            catch
            {
                //error
            }
        }
        public void processEntry(string entry)
        {
            string[] entrySplit = entry.Split(',');
            //there should be 12 columns of data
            int selectedImu = System.Convert.ToInt32(nud_SelectedImu.Value);
            if (entrySplit.Length == 13)
            {
                try
                {
                    UInt32 timeStamp = UInt32.Parse(entrySplit[0]);
                    UInt16 sensorMask = UInt16.Parse(entrySplit[1], System.Globalization.NumberStyles.HexNumber);
                    for (int i = 0; i < 9; i++)
                    {
                        //if the frame is valid for this sensor then process it. 
                        if ((sensorMask & (1 << i)) > 0)
                        {
                            imuArray[i].ProcessEntry(timeStamp, entrySplit[i + 2]);
                            imuArray[i].EntryUpdated = true;
                        }
                        else
                        {
                            imuArray[i].EntryUpdated = false;
                        }
                        //DataRow row = sensorStats.NewRow();
                        sensorStats.Rows[i]["Sensor ID"] = i.ToString();
                        sensorStats.Rows[i]["Roll"] = imuArray[i].GetCurrentEntry().Roll.ToString("F3") + convertToArrow(imuArray[i].GetCurrentEntry().Roll);
                        sensorStats.Rows[i]["Pitch"] = imuArray[i].GetCurrentEntry().Pitch.ToString("F3") + convertToArrow(imuArray[i].GetCurrentEntry().Pitch);
                        sensorStats.Rows[i]["Yaw"] = imuArray[i].GetCurrentEntry().Yaw.ToString("F3") + convertToArrow(imuArray[i].GetCurrentEntry().Yaw);
                        sensorStats.Rows[i]["Frame Count"] = imuArray[i].GetTotalEntryCount().ToString();
                        sensorStats.Rows[i]["Interval"] = imuArray[i].GetLastInterval().ToString();
                        sensorStats.Rows[i]["Max Interval"] = imuArray[i].GetMaxInterval().ToString();
                        sensorStats.Rows[i]["Average Interval"] = imuArray[i].GetAverageInterval().ToString();

                        if (selectedImu == i)
                        {
                            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Roll"].Points.AddY(imuArray[selectedImu].GetCurrentEntry().Roll)));
                            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Pitch"].Points.AddY(imuArray[selectedImu].GetCurrentEntry().Pitch)));
                            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Yaw"].Points.AddY(imuArray[selectedImu].GetCurrentEntry().Yaw)));

                            if (chrt_dataChart.Series["Roll"].Points.Count > 150)
                            {
                                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Roll"].Points.RemoveAt(0)));
                                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Pitch"].Points.RemoveAt(0)));
                                this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Yaw"].Points.RemoveAt(0)));
                            }
                        }


                    }
                    this.BeginInvoke((MethodInvoker)(() => tb_stretchData.Text = entrySplit[11]));
                }
                catch
                {
                    //error
                }
            }
        }

        public void analyzeEntry(string entry)
        {
            string[] entrySplit = entry.Split(',');
            string result = "";
            //there should be 12 columns of data
            if (entrySplit.Length == 13)
            {
                try
                {
                    UInt32 timeStamp = UInt32.Parse(entrySplit[0]);
                    UInt16 sensorMask = UInt16.Parse(entrySplit[1], System.Globalization.NumberStyles.HexNumber);
                    for (int i = 0; i < 9; i++)
                    {
                        //if the frame is valid for this sensor then process it. 
                        if ((sensorMask & (1 << i)) > 0)
                        {
                            imuArray[i].ProcessEntry(timeStamp, entrySplit[i + 2]);
                            imuArray[i].EntryUpdated = true;
                        }
                        else
                        {
                            imuArray[i].EntryUpdated = false;
                        }
                        //DataRow row = sensorStats.NewRow();

                        //save the data to the row

                        //sensorStats.Rows[i]["Sensor ID"] = i.ToString();
                        //sensorStats.Rows[i]["Roll"] = imuArray[i].GetCurrentEntry().Roll.ToString("F3") + convertToArrow(imuArray[i].GetCurrentEntry().Roll);
                        //sensorStats.Rows[i]["Pitch"] = imuArray[i].GetCurrentEntry().Pitch.ToString("F3") + convertToArrow(imuArray[i].GetCurrentEntry().Pitch);
                        //sensorStats.Rows[i]["Yaw"] = imuArray[i].GetCurrentEntry().Yaw.ToString("F3") + convertToArrow(imuArray[i].GetCurrentEntry().Yaw);
                        sensorStats.Rows[i]["Frame Count"] = imuArray[i].GetTotalEntryCount().ToString();
                        sensorStats.Rows[i]["Interval"] = imuArray[i].GetLastInterval().ToString();
                        sensorStats.Rows[i]["Max Interval"] = imuArray[i].GetMaxInterval().ToString();
                        //sensorStats.Rows[i]["Average Interval"] = imuArray[i].GetAverageInterval().ToString();
                    }
                    //this.BeginInvoke((MethodInvoker)(() => tb_stretchData.Text = entrySplit[11]));
                }
                catch
                {
                    //error
                }
            }
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            cb_serialPorts.Items.AddRange(SerialPort.GetPortNames());
            cb_serialPassT.Items.AddRange(SerialPort.GetPortNames());
            string[] baudrates = { "110", "150", "300", "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200", "230400"
                    , "460800","500000", "921600","1000000"};
            cb_Baud.Items.AddRange(baudrates);
            cb_Baud.SelectedIndex = 10;

            serialPort.NewLine = "\r\n";
            serialPortPassThrough.NewLine = "\r\n";
            //start read thread --> automatically put things in list box            
            tb_Console.AppendText("Brain Data Analyzer\r\n");
            sensorStats = new DataTable("Sensor Statistics");
            sensorStats.Columns.Add("Sensor ID", typeof(string));
            sensorStats.Columns.Add("Roll", typeof(string));
            sensorStats.Columns.Add("Pitch", typeof(string));
            sensorStats.Columns.Add("Yaw", typeof(string));
            sensorStats.Columns.Add("Frame Count", typeof(string));
            sensorStats.Columns.Add("Interval", typeof(string));
            sensorStats.Columns.Add("Max Interval", typeof(string));
            sensorStats.Columns.Add("Average Interval", typeof(string));
            for (int i = 0; i < 9; i++)
            {
                DataRow row = sensorStats.NewRow();
                row["Sensor ID"] = i.ToString();
                row["Roll"] = "0";
                row["Pitch"] = "0";
                row["Yaw"] = "0";
                row["Frame Count"] = "0";
                row["Interval"] = "0";
                row["Max Interval"] = "0";
                row["Average Interval"] = "0";
                sensorStats.Rows.Add(row);
            }
            for (int i = 0; i < imuArray.Length; i++)
            {
                imuArray[i] = new imu();
            }
            bindingSource1.DataSource = sensorStats;
            dgv_SensorStats.DataSource = bindingSource1;
            dgv_SensorStats.Update();
            //initialize the message queue
            debugMessageQueue = new ConcurrentQueue<string>();
            //start the debug message thread
            processDebugThreadEnabled = true;
            Thread debugMessageThread = new Thread(processDebugMessagesThread);
            debugMessageThread.Start();

            packetQueue = new ConcurrentQueue<RawPacket>();
            processPacketQueueEnabled = true;
            Thread packetProcessorThread = new Thread(processPacketThread);
            packetProcessorThread.Start();

            //initialize queue
            incomingDataQueue = new ConcurrentQueue<string>();
            //start processor thread
            processDataTheadEnabled = true;
            Thread dataProcessorThread = new Thread(processDataThread);
            dataProcessorThread.Start();

        }

        private void bnt_Connect_Click(object sender, EventArgs e)
        {
            //set the serial port to the selected item. 
            //Thread serialThread = new Thread(ReadThread);
            if (serialPort.IsOpen)
            {
                //do nothing
                return;
            }
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Roll"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Pitch"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Yaw"].Points.Clear()));
            serialPort.PortName = cb_serialPorts.Text;
            try
            {
                serialPort.BaudRate = int.Parse(cb_Baud.Text);
                openSerialPort = true;
                serialPort.Open();
                tb_Console.AppendText("Port: " + serialPort.PortName + " Open\r\n");
            }
            catch (Exception ex)
            {
                tb_Console.AppendText("Failed to open Port: " + serialPort.PortName + " \r\n");
                tb_Console.AppendText("Exception " + ex.Message + " \r\n");
                openSerialPort = false;
            }
        }
        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Close();
                    openSerialPort = false;
                    tb_Console.AppendText("Port: " + serialPort.PortName + " Closed\r\n");
                }
                catch
                {
                    tb_Console.AppendText("Failed to close Port: " + serialPort.PortName + "\r\n");
                }
            }
        }
        private void sendCommmand(string command)
        {
            try
            {
                serialPort.Write(command);
            }
            catch
            {
                tb_Console.AppendText("Send command failed \r\n");
            }
        }
        private void btn_SendCmd_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                if (cb_commands.Text.Length > 0)
                {
                    sendCommmand(cb_commands.Text + "\r\n");
                    //sendCommmand(tb_cmd.Text + "\r\n");
                }
            }
        }
        private void btn_getState_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                sendCommmand("GetState\r\n");
            }
        }
        private void btn_record_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                sendCommmand("Record\r\n");
            }
        }
        private void btn_reset_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                sendCommmand("Reset\r\n");
            }
        }
        private void btn_setTime_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                DateTime time = DateTime.Now;
                int dayOfWeekNumber = (int)time.DayOfWeek;
                string timeCommand = "setTime" + time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString() + "-" +
                 dayOfWeekNumber.ToString() + "-" + time.Hour.ToString() + ":" + time.Minute.ToString() + ":" + time.Second.ToString() + "\r\n";
                sendCommmand(timeCommand);
            }
        }
        private DataTable convertDatFile(string filename)
        {
            string tempFilename = Application.StartupPath + "\\temp.csv";

            //tb_Console.AppendText("Start File Decryption: " + System.DateTime.Now.ToLongTimeString() + " \r\n");  
            try
            {
                if (File.Exists(tempFilename))
                {
                    File.Delete(tempFilename);
                }
                FileStream tempFile = new FileStream(tempFilename, FileMode.Create);
                FileStream rawFile = new FileStream(filename, FileMode.Open);


                //read the first portion of data
                byte[] header = new byte[44];
                rawFile.Read(header, 0, 44);

                string headerString = System.Text.Encoding.ASCII.GetString(header);
                long offset = 0;
                if (headerString.Contains("BPVERSION"))
                {
                    this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText("Header: " + headerString)));
                    offset = headerString.Length;
                }

                Int32 temp = 0x00;
                int errorCount = 0;
                byte[] fileBytes = new byte[rawFile.Length - offset];
                rawFile.Seek(offset, SeekOrigin.Begin);
                rawFile.Read(fileBytes, 0, (int)(rawFile.Length - offset));
                for (long i = 0; i < rawFile.Length - offset; i++)
                {
                    if ((fileBytes[i] & 0x80) > 0)
                    {
                        fileBytes[i] -= 0x80;
                        tempFile.WriteByte((byte)(fileBytes[i]));
                    }
                    else
                    {
                        //error bytes
                        errorCount++;
                    }
                    //temp = rawFile.ReadByte();
                    ////check if the end of file has been reached. 
                    //if (temp == -1)
                    //{
                    //    break; 
                    //}
                    //tempFile.WriteByte((byte)(temp - 0x80)); 
                }
                this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText("Found  " + errorCount.ToString() + " bytes improperly formatted\r\n")));
                //tempFile.Write(fileBytes, 0, (int)(rawFile.Length - offset));
                tempFile.Close();
            }
            catch
            {
                return null;
            }

            return CSVDataAdapter.Fill(tempFilename, false);
        }
        public void processDataRow(DataRow row)
        {
            //there should be 12 columns of data
            int selectedImu = System.Convert.ToInt32(nud_SelectedImu.Value);
            if (row.Table.Columns.Count < 12)
            {
                return;
            }
            try
            {
                UInt32 timeStamp = UInt32.Parse(row[0].ToString());
                UInt16 sensorMask = UInt16.Parse(row[1].ToString(), System.Globalization.NumberStyles.HexNumber);
                for (int i = 0; i < 9; i++)
                {
                    //if the frame is valid for this sensor then process it. 
                    if ((sensorMask & (1 << i)) > 0)
                    {
                        imuArray[i].ProcessEntry(timeStamp, row[i + 2].ToString());
                        imuArray[i].EntryUpdated = true;
                    }
                    else
                    {
                        imuArray[i].EntryUpdated = false;
                    }
                    //DataRow row = sensorStats.NewRow();
                    sensorStats.Rows[i]["Sensor ID"] = i.ToString();
                    sensorStats.Rows[i]["Roll"] = imuArray[i].GetCurrentEntry().Roll.ToString("F3") + convertToArrow(imuArray[i].GetCurrentEntry().Roll);
                    sensorStats.Rows[i]["Pitch"] = imuArray[i].GetCurrentEntry().Pitch.ToString("F3") + convertToArrow(imuArray[i].GetCurrentEntry().Pitch);
                    sensorStats.Rows[i]["Yaw"] = imuArray[i].GetCurrentEntry().Yaw.ToString("F3") + convertToArrow(imuArray[i].GetCurrentEntry().Yaw);
                    sensorStats.Rows[i]["Frame Count"] = imuArray[i].GetTotalEntryCount().ToString();
                    sensorStats.Rows[i]["Interval"] = imuArray[i].GetLastInterval().ToString();
                    sensorStats.Rows[i]["Max Interval"] = imuArray[i].GetMaxInterval().ToString();
                    sensorStats.Rows[i]["Average Interval"] = imuArray[i].GetAverageInterval().ToString();

                    //if (selectedImu == i)
                    //{
                    //    this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Roll"].Points.AddY(imuArray[selectedImu].GetCurrentEntry().Roll)));
                    //    this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Pitch"].Points.AddY(imuArray[selectedImu].GetCurrentEntry().Pitch)));
                    //    this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Yaw"].Points.AddY(imuArray[selectedImu].GetCurrentEntry().Yaw)));

                    //    if (chrt_dataChart.Series["Roll"].Points.Count > 150)
                    //    {
                    //        this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Roll"].Points.RemoveAt(0)));
                    //        this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Pitch"].Points.RemoveAt(0)));
                    //        this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Yaw"].Points.RemoveAt(0)));
                    //    }
                    //}


                }
            }
            catch
            {
                //error
            }
        }

        private void btn_Convert_Click(object sender, EventArgs e)
        {
            if (mIsBatchModeSelected)
            {
                OpenFileDialog mFileDialog = new OpenFileDialog();
                FolderBrowserDialog mFolderBrowser = new FolderBrowserDialog();
                mFileDialog.Multiselect = true;
                mFileDialog.Filter = "dat files|*.dat";
                mFileDialog.Title = "Select files for conversion";
                if (mFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string vFileLoc = mFileDialog.FileNames[0];
                    FileInfo vInfo = new FileInfo(vFileLoc);
                    DirectoryInfo vDirInfo = new DirectoryInfo(vInfo.DirectoryName);
                    //if (mFolderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        // mFolderBrowser.Description = "Select Files output folder";
                        //  string vPath = mFolderBrowser.SelectedPath;
                        var vFiles = vDirInfo.GetFiles("*.dat");
                        foreach (var vFileInfo in vFiles)
                        {
                            analysisData = convertDatFile(vFileInfo.FullName);
                            if (analysisData == null)
                            {
                                tb_Console.AppendText("Failed to load the file\r\n");
                            }
                            else
                            {
                                tb_Console.AppendText("Loaded " + analysisData.Rows.Count.ToString() + " Rows \r\n");
                                tb_Console.AppendText("From File: " + ofd_AnalyzeFile.FileName + " \r\n");
                                Int32 maxTime = 0;
                                DataTable convertedData = new DataTable("Converted_Data");
                                convertedData.Columns.Add("TimeStamp", typeof(float));
                                ImuEntry defaultValue = new ImuEntry(0.0, 0.0, 0.0);
                                //create the columns for the IMUs
                                for (int i = 0; i < 10; i++)
                                {
                                    //Add column with the IMU index. 
                                    convertedData.Columns.Add(i.ToString(), typeof(string));
                                    convertedData.Columns[i.ToString()].DefaultValue = (i + 1).ToString();
                                    string columnName = "IMU" + i.ToString();
                                    convertedData.Columns.Add(columnName, typeof(ImuEntry));
                                    convertedData.Columns[columnName].DefaultValue = defaultValue; //set default to all zeros 
                                }
                                //create the column for the stretch sense, should be 5 columns
                                convertedData.Columns.Add("SS1", typeof(string));
                                convertedData.Columns.Add("SS2", typeof(string));
                                convertedData.Columns.Add("SS3", typeof(string));
                                convertedData.Columns.Add("SS4", typeof(string));
                                convertedData.Columns.Add("SS5", typeof(string));
                                Int32 startTime = Int32.Parse(analysisData.Rows[0][0].ToString());
                                pb_processingProgress.Visible = true;
                                pb_processingProgress.Value = 0;
                                for (int i = 1; i < analysisData.Rows.Count; i++)
                                {
                                    pb_processingProgress.Value = (i * 100) / analysisData.Rows.Count;
                                    try
                                    {
                                        Int32 val1 = Int32.Parse(analysisData.Rows[i][0].ToString());
                                        Int32 val2 = Int32.Parse(analysisData.Rows[i - 1][0].ToString());
                                        Int32 interval = val1 - val2;
                                        if (interval > maxTime)
                                        {
                                            maxTime = interval;
                                        }
                                        DataRow row = convertedData.NewRow();
                                        row[0] = (float)(val1 - startTime) / 1000; //convert to float

                                        for (int j = 1, k = 2; j < 18; j += 2, k++)
                                        {
                                            row[j + 1] = new ImuEntry(analysisData.Rows[i][k].ToString());
                                        }
                                        string[] fabSense = analysisData.Rows[i][11].ToString().Split(';');
                                        if (fabSense.Length == 5)
                                        {
                                            row["SS1"] = fabSense[0];
                                            row["SS2"] = fabSense[1];
                                            row["SS3"] = fabSense[2];
                                            row["SS4"] = fabSense[3];
                                            row["SS5"] = fabSense[4];
                                        }
                                        convertedData.Rows.Add(row);
                                    }
                                    catch
                                    {
                                        tb_Console.AppendText("Failed on Row " + i.ToString() + "\r\n");
                                    }

                                }
                                //analysisDataStream.Close();
                                tb_Console.AppendText("Max Interval" + maxTime.ToString() + "ms \r\n");
                                //    sfd_ConvertedFile.DefaultExt = ".csv";
                                //   sfd_ConvertedFile.AddExtension = true;
                                //   if (sfd_ConvertedFile.ShowDialog() == DialogResult.OK)
                                {
                                    //have to create header for the file before writting it in. 
                                    string line1 = Guid.NewGuid().ToString() + "\r\n";
                                    string line2 = Guid.NewGuid().ToString() + "\r\n";
                                    string line3 = Guid.NewGuid().ToString() + "\r\n";
                                    StreamWriter writer = File.CreateText(vFileInfo.Directory + "\\" + vFileInfo.Name + ".csv");
                                    writer.Write(line1);
                                    writer.Write(line2);
                                    writer.Write(line3);
                                    writer.Close();
                                    CSVDataAdapter.Write(convertedData, false, vFileInfo.Directory + "\\" + vFileInfo.Name + ".csv", true);
                                }
                                pb_processingProgress.Value = 0;
                                pb_processingProgress.Visible = false;
                            }
                            tb_Console.AppendText("End File Conversion: " + System.DateTime.Now.ToLongTimeString() + " \r\n");
                        }
                    }
                }
            }


            else
            {
                ofd_AnalyzeFile.Filter = "comma seperated values(*.csv)|*.csv|Brain pack data(*.dat)|*.dat|All files (*.*)|*.*";
                ofd_AnalyzeFile.FilterIndex = 3;
                ofd_AnalyzeFile.RestoreDirectory = true;


                if (ofd_AnalyzeFile.ShowDialog() == DialogResult.OK)
                {
                    if (ofd_AnalyzeFile.FileName.Contains(".dat"))
                    {
                        analysisData = convertDatFile(ofd_AnalyzeFile.FileName);
                    }
                    else
                    {
                        analysisData = CSVDataAdapter.Fill(ofd_AnalyzeFile.FileName, false);
                    }
                    if (analysisData == null)
                    {
                        tb_Console.AppendText("Failed to load the file\r\n");
                    }
                    tb_Console.AppendText("Loaded " + analysisData.Rows.Count.ToString() + " Rows \r\n");
                    tb_Console.AppendText("From File: " + ofd_AnalyzeFile.FileName + " \r\n");
                    Int32 maxTime = 0;
                    DataTable convertedData = new DataTable("Converted_Data");
                    convertedData.Columns.Add("TimeStamp", typeof(float));
                    ImuEntry defaultValue = new ImuEntry(0.0, 0.0, 0.0);
                    //create the columns for the IMUs
                    for (int i = 0; i < 10; i++)
                    {
                        //Add column with the IMU index. 
                        convertedData.Columns.Add(i.ToString(), typeof(string));
                        convertedData.Columns[i.ToString()].DefaultValue = (i + 1).ToString();
                        string columnName = "IMU" + i.ToString();
                        convertedData.Columns.Add(columnName, typeof(ImuEntry));
                        convertedData.Columns[columnName].DefaultValue = defaultValue; //set default to all zeros 
                    }
                    //create the column for the stretch sense, should be 5 columns
                    convertedData.Columns.Add("SS1", typeof(string));
                    convertedData.Columns.Add("SS2", typeof(string));
                    convertedData.Columns.Add("SS3", typeof(string));
                    convertedData.Columns.Add("SS4", typeof(string));
                    convertedData.Columns.Add("SS5", typeof(string));
                    Int32 startTime = Int32.Parse(analysisData.Rows[0][0].ToString());
                    pb_processingProgress.Visible = true;
                    pb_processingProgress.Value = 0;
                    for (int i = 1; i < analysisData.Rows.Count; i++)
                    {
                        pb_processingProgress.Value = (i * 100) / analysisData.Rows.Count;
                        try
                        {
                            Int32 val1 = Int32.Parse(analysisData.Rows[i][0].ToString());
                            Int32 val2 = Int32.Parse(analysisData.Rows[i - 1][0].ToString());
                            Int32 interval = val1 - val2;
                            if (interval > maxTime)
                            {
                                maxTime = interval;
                            }
                            DataRow row = convertedData.NewRow();
                            row[0] = (float)(val1 - startTime) / 1000; //convert to float

                            for (int j = 1, k = 2; j < 18; j += 2, k++)
                            {
                                row[j + 1] = new ImuEntry(analysisData.Rows[i][k].ToString());
                            }
                            string[] fabSense = analysisData.Rows[i][11].ToString().Split(';');
                            if (fabSense.Length == 5)
                            {
                                row["SS1"] = fabSense[0];
                                row["SS2"] = fabSense[1];
                                row["SS3"] = fabSense[2];
                                row["SS4"] = fabSense[3];
                                row["SS5"] = fabSense[4];
                            }
                            convertedData.Rows.Add(row);
                        }
                        catch
                        {
                            tb_Console.AppendText("Failed on Row " + i.ToString() + "\r\n");
                        }

                    }
                    //analysisDataStream.Close();
                    tb_Console.AppendText("Max Interval" + maxTime.ToString() + "ms \r\n");
                    sfd_ConvertedFile.DefaultExt = ".csv";
                    sfd_ConvertedFile.AddExtension = true;
                    if (sfd_ConvertedFile.ShowDialog() == DialogResult.OK)
                    {
                        //have to create header for the file before writting it in. 
                        string line1 = Guid.NewGuid().ToString() + "\r\n";
                        string line2 = Guid.NewGuid().ToString() + "\r\n";
                        string line3 = Guid.NewGuid().ToString() + "\r\n";
                        StreamWriter writer = File.CreateText(sfd_ConvertedFile.FileName);
                        writer.Write(line1);
                        writer.Write(line2);
                        writer.Write(line3);
                        writer.Close();
                        CSVDataAdapter.Write(convertedData, false, sfd_ConvertedFile.FileName, true);
                    }
                    pb_processingProgress.Value = 0;
                    pb_processingProgress.Visible = false;
                }
                tb_Console.AppendText("End File Conversion: " + System.DateTime.Now.ToLongTimeString() + " \r\n");
                //FileStream dataFile = new FileStream(ofd_AnalyzeFile.FileName, FileMode.Open);
            }

        }

        private void btn_Analyze_Click(object sender, EventArgs e)
        {
            ofd_AnalyzeFile.Filter = "comma seperated values(*.csv)|*.csv|Brain pack data(*.dat)|*.dat|All files (*.*)|*.*";
            ofd_AnalyzeFile.FilterIndex = 3;
            ofd_AnalyzeFile.RestoreDirectory = true;

            if (bgw_AnalysisBackgroundWorker.IsBusy)
            {
                tb_Console.AppendText("Cannot start analysis, file is being processed\r\n");
                return;
            }
            if (ofd_AnalyzeFile.ShowDialog() == DialogResult.OK)
            {

                pb_processingProgress.Visible = true;
                pb_processingProgress.Value = 0;
                for (int i = 0; i < imuArray.Length; i++)
                {
                    imuArray[i].clearStats();
                }
                bgw_AnalysisBackgroundWorker.RunWorkerAsync(ofd_AnalyzeFile.FileName);
                return;

            }
        }
        //experimental encryption functions
        private byte rotlByte(byte value, int count)
        {
            const int CHAR_BIT = 8;
            const int mask = (CHAR_BIT * sizeof(byte) - 1);
            count &= mask;

            return (byte)((value << count) | (value >> ((-count) & mask)));
        }

        private byte rotrByte(byte value, int count)
        {
            const int CHAR_BIT = 8;
            const int mask = (CHAR_BIT * sizeof(byte) - 1);
            count &= mask;
            return (byte)((value >> count) | (value << ((-count) & mask)));
        }
        private void encrypt(ref byte[] data, int size)
        {
            int shift = 0;
            for (int i = 0; i < size; i++)
            {
                shift = i % 7;
                if (shift == 0)
                {
                    shift = 3;
                }
                data[i] = rotlByte(data[i], shift);
            }
        }

        private void decrypt(ref byte[] data, int size)
        {
            int shift = 0;
            for (int i = 0; i < size; i++)
            {
                shift = i % 7;
                if (shift == 0)
                {
                    shift = 3;
                }
                data[i] = rotrByte(data[i], shift);
            }
        }

        private void btn_EncryptSettings_Click(object sender, EventArgs e)
        {
            if (ofd_AnalyzeFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream settingsFs = File.Open(ofd_AnalyzeFile.FileName, FileMode.Open);
                    byte[] data = new byte[500];
                    int totalBytesRead = settingsFs.Read(data, 0, 500);
                    settingsFs.Close();
                    encrypt(ref data, totalBytesRead);

                    //decrypt(ref data, totalBytesRead);  

                    if (sfd_ConvertedFile.ShowDialog() == DialogResult.OK)
                    {
                        FileStream encryptedSettings = File.Open(sfd_ConvertedFile.FileName, FileMode.Create);
                        byte[] header = new byte[2];
                        header[0] = (byte)'e';
                        header[1] = (byte)'e';
                        encryptedSettings.Write(header, 0, 2);
                        encryptedSettings.Write(data, 0, totalBytesRead);
                        encryptedSettings.Close();
                    }
                }
                catch
                {

                }
            }
        }
        private byte reverse(byte c)
        {
            int shift;
            int result = 0;
            for (shift = 0; shift < 8; shift++)
            {
                if ((c & (0x01 << shift)) > 0)
                    result |= ((byte)0x80 >> shift);
            }
            return (byte)result;
        }
        private byte[] bitReverseAllBytes(byte[] data)
        {
            byte[] reversedBytes = new byte[data.Length];

            for (int i = 0, j = 0; j < reversedBytes.Length; i++, j++)
            {
                reversedBytes[j] = reverse(data[i]);
            }
            return reversedBytes;

        }
        private void btn_CreateFwBin_Click(object sender, EventArgs e)
        {
            if (ofd_AnalyzeFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream fwStream = File.Open(ofd_AnalyzeFile.FileName, FileMode.Open);
                    byte[] data = new byte[fwStream.Length];
                    int totalBytesRead = fwStream.Read(data, 0, data.Length);
                    fwStream.Close();

                    CRC_Calculator crcCal = new CRC_Calculator(InitialCrcValue.NonZero1);
                    CRCTool crcTool = new CRCTool();
                    crcTool.Init(CRCTool.CRCCode.CRC32);
                    ulong crcValue1 = crcTool.crcbitbybitfast(data);
                    ushort crcValue2 = crcCal.ComputeChecksum(data);
                    ulong crcValue3 = crcTool.crcbitbybitfast(bitReverseAllBytes(data));
                    tb_Console.AppendText("CRC method 1 Calculated: " + crcValue1.ToString() + " \r\n");
                    tb_Console.AppendText("CRC method 2 Calculated: " + crcValue2.ToString() + " \r\n");
                    tb_Console.AppendText("CRC method 3 Calculated: " + crcValue3.ToString() + " \r\n");
                    //header 0x55AA55AA CRC(16bit) CRC(16bit), Length(32bit)
                    byte[] header = { 0x55, 0xAA, 0x55, 0xAA, 0, 0, 0, 0, 0, 0, 0, 0 };

                    header[4] = (byte)(crcValue1 & 0x00FF);
                    header[5] = (byte)((crcValue1 >> 8) & 0x00FF);
                    header[6] = (byte)((crcValue1 >> 16) & 0x00FF);
                    header[7] = (byte)((crcValue1 >> 24) & 0x00FF);


                    //decrypt(ref data, totalBytesRead);  

                    if (sfd_ConvertedFile.ShowDialog() == DialogResult.OK)
                    {
                        FileStream outputFw = File.Open(sfd_ConvertedFile.FileName, FileMode.Create);
                        outputFw.Write(header, 0, header.Length);
                        outputFw.Write(data, 0, totalBytesRead);
                        outputFw.Close();
                    }
                }
                catch
                {

                }
            }
        }

        private void dgv_SensorStats_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Console.WriteLine("data Error");
        }

        private void btn_clearStats_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < imuArray.Length; i++)
            {
                imuArray[i].clearStats();
            }
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Roll"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Pitch"].Points.Clear()));
            this.BeginInvoke((MethodInvoker)(() => chrt_dataChart.Series["Yaw"].Points.Clear()));
        }

        private void btn_setSaveLocation_Click(object sender, EventArgs e)
        {
            if (sfd_ConvertedFile.ShowDialog() == DialogResult.OK)
            {
                tb_saveLocation.Text = sfd_ConvertedFile.FileName;
            }
        }

        private void cb_saveRecordEntries_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_saveRecordEntries.Checked)
            {
                if (tb_saveLocation.Text.Length > 0)
                {
                    try
                    {
                        dataFile = File.Open(tb_saveLocation.Text, FileMode.Append);
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                try
                {
                    dataFile.Lock(0, dataFile.Length);
                    dataFile.Close();
                }
                catch
                {

                }
            }
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            processDataTheadEnabled = false;
            processDebugThreadEnabled = false;
            processPacketQueueEnabled = false;
        }

        private void cb_serialPassEn_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_serialPassEn.Checked)
            {
                lock (serialPortPassThrough)
                {
                    serialPortPassThrough.PortName = cb_serialPassT.Items[cb_serialPassT.SelectedIndex].ToString();
                    try
                    {
                        serialPortPassThrough.Open();
                        if (serialPortPassThrough.IsOpen)
                        {
                            tb_Console.AppendText("Pass through Port: " + serialPortPassThrough.PortName + "Open\r\n");
                        }

                    }
                    catch
                    {
                        tb_Console.AppendText("Failed to open pass through Port: " + serialPortPassThrough.PortName + "Open\r\n");
                        openSerialPort = false;
                    }
                }

            }
            else
            {
                lock (serialPortPassThrough)
                {
                    serialPortPassThrough.Close();
                }
            }
        }

        private void serialPortPassThrough_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {

                string line = serialPortPassThrough.ReadLine();
                if (serialPort.IsOpen)
                {
                    serialPort.WriteLine(line);
                }
            }
            catch
            {

            }
        }
        RawPacket packet = new RawPacket();
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                if (cb_protobuf.Checked)
                {
                    try
                    {
                        while (serialPort.BytesToRead > 0)
                        {
                            int receivedByte = serialPort.ReadByte();
                            if (receivedByte != -1)
                            {
                                //process the byte
                                byte newByte = (byte)receivedByte;
                                int bytesReceived = packet.BytesReceived + 1;
                                PacketStatus status = packet.processByte((byte)receivedByte);
                                switch (status)
                                {
                                    case PacketStatus.PacketComplete:
                                        // debugMessageQueue.Enqueue(String.Format("Packet Received {0} bytes\r\n", packet.PayloadSize));
                                        RawPacket packetCopy = new RawPacket(packet);
                                        packetQueue.Enqueue(packetCopy);
                                        packet.resetPacket();
                                        break;
                                    case PacketStatus.PacketError:
                                        //if (cb_logErrors.Checked)
                                        //{
                                        //debugMessageQueue.Enqueue(String.Format("Packet ERROR! {1} bytes received\r\n", bytesReceived));
                                        //}
                                        packet.resetPacket();
                                        break;
                                    case PacketStatus.Processing:
                                    case PacketStatus.newPacketDetected:
                                        break;
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                else
                {
                    try
                    {
                        while (serialPort.BytesToRead > 0)
                        {
                            string line = serialPort.ReadLine();
                            lock (serialPortPassThrough)
                            {
                                if (serialPortPassThrough.IsOpen)
                                {
                                    serialPortPassThrough.WriteLine(line);
                                }
                            }
                            incomingDataQueue.Enqueue(line);
                        }

                    }
                    catch
                    {
                        //do nothing, this is alright
                    }
                }

            }
        }

        private void cb_serialPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (!serialPort.IsOpen)
            //{
            //    cb_serialPorts.Items.Clear();
            //    cb_serialPorts.Items.AddRange(SerialPort.GetPortNames());
            //}
        }

        private void mainForm_DoubleClick(object sender, EventArgs e)
        {
            if (!serialPort.IsOpen)
            {
                cb_serialPorts.Items.Clear();
                cb_serialPorts.Items.AddRange(SerialPort.GetPortNames());
            }
        }

        private void cb_serialPorts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!serialPort.IsOpen)
            {
                cb_serialPorts.Items.Clear();
                cb_serialPorts.Items.AddRange(SerialPort.GetPortNames());
            }
        }

        private void bgw_AnalysisBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string filename = (string)e.Argument;
            if (filename.Contains(".dat"))
            {
                analysisData = convertDatFile(filename);
            }
            else
            {
                analysisData = CSVDataAdapter.Fill(filename, false);
            }
            if (analysisData == null)
            {
                this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText("Failed to load the file\r\n")));
                return;
            }
            this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText("Loaded " + analysisData.Rows.Count.ToString() + " Rows \r\n")));
            this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText("From File: " + ofd_AnalyzeFile.FileName + " \r\n")));
            Int32 maxTime = 0;

            Int32 startTime = Int32.Parse(analysisData.Rows[0][0].ToString());
            int errorCount = 0;
            for (int i = 1; i < analysisData.Rows.Count; i++)
            {
                bgw_AnalysisBackgroundWorker.ReportProgress((i * 100) / analysisData.Rows.Count);
                try
                {
                    Int32 val1 = Int32.Parse(analysisData.Rows[i][0].ToString());
                    Int32 val2 = Int32.Parse(analysisData.Rows[i - 1][0].ToString());
                    Int32 interval = val1 - val2;
                    if (interval > maxTime)
                    {
                        maxTime = interval;
                    }
                    processDataRow(analysisData.Rows[i]);
                }
                catch
                {
                    errorCount++;
                }

            }
            this.BeginInvoke((MethodInvoker)(() => tb_Console.AppendText("Failed on " + errorCount.ToString() + " Rows \r\n")));
        }

        private void bgw_AnalysisBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pb_processingProgress.Value = e.ProgressPercentage;
        }

        private void bgw_AnalysisBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pb_processingProgress.Value = 0;
            pb_processingProgress.Visible = false;
        }

        private void btn_EnterBootloader_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to enter the bootloader?\r\nExiting the bootloader can only be done through the SAM-BA program.",
                "Enter Bootloader", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (serialPort.IsOpen)
                {
                    sendCommmand("enterBootloader\r\n");
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }

        }

        private void btn_exitBootloader_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                sendCommmand("W400E1400,A5000005#");
            }
        }

        private void btn_CheckVersion_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                sendCommmand("?\r\n");
                Thread.Sleep(400);
                sendCommmand("pbVersion\r\n");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            mIsBatchModeSelected = checkBox1.Checked;
        }

        
    }
}



