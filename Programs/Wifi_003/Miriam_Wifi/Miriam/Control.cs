using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

/*
 * Miriam control
 *  
 * Miriam is an isothermal amplification unit capable of real time detection that supports a 96 well PCR plate
 * This software allows the control over WIFI via string messages
 *   
 * (c) 2016 Juho Terrijarvi juho@miroculus.com, Miroculus Inc.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *    
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace Miriam
{
    public partial class Control : Form
    {
        private int duration;
        private string arrayNames;
        private int maximumValue = 0;
        private string selectedCom = "";

        public Control()
        {
            InitializeComponent();

            string[] ports = SerialPort.GetPortNames();

            for (int i = 0; i < ports.Length; i++)
            {
                COM.Items.Add(ports[i]);
            }

            for (int i = 55; i < 70; i++)
            {
                CboxTempM.Items.Add(i);
            }
            for (int i = 80; i < 90; i++)
            {
                CboxTempU.Items.Add(i);
            }
            for (int i = 0; i < 150; i++)
            {
                CboxDuration.Items.Add(i);
            }
            CboxTempU.Text = "90";
            CboxTempM.Text = "65";
            CboxDuration.Text = "120";

            Plate.ColumnCount = 12;
            for (int i = 0; i < 12; i++)
            {
                DataGridViewColumn column = Plate.Columns[i];

                column.Name = (i+1).ToString();
                column.Width = 40;
            }

            Plate.AllowUserToAddRows = false;



            //Plate.RowCount = 8;

            String alphabets = "ABCDEFGH";
            for (int i = 0; i<8; i++)
            {
                Plate.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "");
                Plate.Rows[i].HeaderCell.Value = alphabets[i].ToString();
                Plate.RowHeadersWidth = Plate.RowHeadersWidth + 1;
            }

            Results.Visible = false;
            TbNWID.Text = "QB3@953";
            TbNWPass.Text = "QB34innovation!";

        }
        

        private void Debug_Click(object sender, EventArgs e)
        {
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadDebug));

            t.Start();
        }

        public static void ThreadDebug()
        {
            Application.Run(new Debug());
        }

        
        

        private void ButtonHeat_Click(object sender, EventArgs e)
        {
            // Set upper temperature

            TCPIP workerObject = new TCPIP();
            Thread workerThread = new Thread(workerObject.sendANDreceive);

            workerObject.command = "U";
            workerObject.par = CboxTempU.Text;


            // Start the worker thread.
            workerThread.Start();

            // Loop until worker thread activates. 
            while (!workerThread.IsAlive) ;
            // Put the main thread to sleep for 1 millisecond to 
            // allow the worker thread to do some work:
            Thread.Sleep(1);

            workerThread.Join();

            System.Threading.Thread.Sleep(2000);
            // Set middle temperature

            workerThread = new Thread(workerObject.sendANDreceive);

            workerObject.command = "M";
            workerObject.par = CboxTempM.Text;


            // Start the worker thread.
            workerThread.Start();

            // Loop until worker thread activates. 
            while (!workerThread.IsAlive) ;
            // Put the main thread to sleep for 1 millisecond to 
            // allow the worker thread to do some work:
            Thread.Sleep(1);

            workerThread.Join();

            System.Threading.Thread.Sleep(2000);
            // Start heat
            workerThread = new Thread(workerObject.sendANDreceive);

            workerObject.command = "H";
            workerObject.par = "";


            // Start the worker thread.
            workerThread.Start();

            // Loop until worker thread activates. 
            while (!workerThread.IsAlive) ;
            // Put the main thread to sleep for 1 millisecond to 
            // allow the worker thread to do some work:
            Thread.Sleep(1);

            workerThread.Join();

            System.Threading.Thread.Sleep(2000);
            Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(doHeatQuery));

            t.Start();

        }



        private void doHeatQuery()
        {
            TCPIP workerObject = new TCPIP();
            Thread workerThread = new Thread(workerObject.sendANDreceive);

            workerObject.command = "i";
            workerObject.par = "";

            // Start the worker thread.
            workerThread.Start();

            // Loop until worker thread activates. 
            while (!workerThread.IsAlive) ;
            // Put the main thread to sleep for 1 millisecond to 
            // allow the worker thread to do some work:
            Thread.Sleep(1);

            workerThread.Join();

            string[] result = workerObject.res.Split(',');

            if (workerObject.res.Equals("ERROR"))
            {
                MessageBox.Show("Connection error");
            } else
            {
                AppendHeatLabel("Temperature U:" + workerObject.res.Split(',')[4] + "," + "Temperature M:" + workerObject.res.Split(',')[5]);

                System.Threading.Thread.Sleep(10000);
            }

            

        }

        private void AppendHeatLabel(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendHeatLabel), new object[] { value });
                return;
            }
            LabelTempUC.Text = value.Split(',')[0];
            LabelTempMC.Text = value.Split(',')[1];

        }

        private void AskHeat_Click(object sender, EventArgs e)
        {
            Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(doHeatQuery));

            t.Start();
        }

        private void RunAssay()
        {
            
        }

        private void AppendData(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendData), new object[] { value });
                return;
            }
            Data.Items.Add(value);

        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            Results.Visible = true;
            ConnectStatus.Visible = false;
            LabelNWID.Visible = false;
            TbNWID.Visible = false;
            LabelNWPass.Visible = false;
            TbNWPass.Visible = false;
            labelCom.Visible = false;
            COM.Visible = false;

            DateTime localDate = DateTime.Now;
            

            duration = localDate.Hour * 60 * 60 + localDate.Minute * 60 + localDate.Second +
                Convert.ToInt32(CboxDuration.Text) * 60;

            Boolean noneFound = true;
            List<String> dupl = new List<String>();
            // clear duplicates
            for (int i = 0; i < Plate.RowCount; i++)
            {
                for (int j = 0; j < Plate.ColumnCount; j++)
                {
                    if (Plate.Rows[i].Cells[j].Value != null)
                    {
                        if (!Plate.Rows[i].Cells[j].Value.ToString().Equals(""))
                        {
                            for (int k = 0; k < dupl.Count; k++)
                            {
                                if (dupl[k].Equals(Plate.Rows[i].Cells[j].Value.ToString()))
                                {
                                    Plate.Rows[i].Cells[j].Value = Plate.Rows[i].Cells[j].Value.ToString() + "_";
                                }

                            }
                            dupl.Add(Plate.Rows[i].Cells[j].Value.ToString());
                            noneFound = false;
                        }
                    }
                    else
                    {
                        Plate.Rows[i].Cells[j].Value = "";
                    }

                }
            }

            if (noneFound)
            {
                Plate.Rows[0].Cells[0].Value = "TimeTrack";
            }

            List<int> list = new List<int>();
            int counter = 3;
            string msg = "Time,U,M,";

            for(int i = 0;i<Plate.RowCount;i++)
            {
                for (int j = 0;j<Plate.ColumnCount;j++)
                {
                    
                    msg += Plate.Rows[i].Cells[j].Value.ToString() + ",";
                    if (!Plate.Rows[i].Cells[j].Value.ToString().Equals(""))
                    {
                        list.Add(counter);
                    }
                    counter += 1;
                }
            }

            Data.Items.Add(msg);
            arrayNames = msg;

            Color[] clr;

            clr = new Color[10];
            clr[0] = Color.Red;
            clr[1] = Color.Blue;
            clr[2] = Color.Yellow;
            clr[3] = Color.Green;
            clr[4] = Color.Black;
            clr[5] = Color.Aqua;
            clr[6] = Color.DimGray;
            clr[7] = Color.DarkViolet;
            clr[8] = Color.DeepPink;
            clr[9] = Color.Gray;

            int clrsUsed = 0;

            for(int i = 0;i<list.Count;i++)
            {
                Results.Series.Add(msg.Split(',')[list[i]]);
                Results.Series[msg.Split(',')[list[i]]].ChartType =
                                SeriesChartType.FastLine;
                Results.Series[msg.Split(',')[list[i]]].Color = clr[clrsUsed];

                clrsUsed += 1;
                if(clrsUsed > 9)
                {
                    clrsUsed = 0;
                }
            }
            

            Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(doAssay));
            t.IsBackground = true;


            t.Start();


        }


        private void AppendResult(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendResult), new object[] { value });
                return;
            }

            for(int i = 3;i<arrayNames.Split(',').Length;i++)
            {
                if(!arrayNames.Split(',')[i].Equals(""))
                {
                    Results.Series[arrayNames.Split(',')[i]].Points.AddXY
                        (Convert.ToInt32(value.Split(',')[0]), 
                        Convert.ToInt32(value.Split(',')[i]));

                    if (Convert.ToInt32(value.Split(',')[i]) > maximumValue)
                    {
                        maximumValue = Convert.ToInt32(value.Split(',')[i]);
                    }
                    Results.ChartAreas[0].AxisY.Maximum = maximumValue + 10;
                    Results.ChartAreas[0].AxisX.Maximum = Convert.ToInt32(value.Split(',')[0]) + 1;
                    Results.ChartAreas[0].AxisY.Minimum = 0;
                    Results.ChartAreas[0].AxisX.Minimum = 0;
                    
                }
               
            }
            

        }

        private void doAssay()
        {
            Boolean cont = true;

            int loop = 0;

            do
            {
                DateTime current = DateTime.Now;
                int endCycle = current.Hour * 60 * 60 + current.Minute * 60 + current.Second;

                if (duration < endCycle)
                {
                    cont = false;
                    
                }

                TCPIP workerObject1 = new TCPIP();
                Thread workerThread1 = new Thread(workerObject1.sendANDreceive);

                workerObject1.command = "R";
                workerObject1.par = "";


                // Start the worker thread.
                workerThread1.Start();

                // Loop until worker thread activates. 
                while (!workerThread1.IsAlive) ;
                // Put the main thread to sleep for 1 millisecond to 
                // allow the worker thread to do some work:
                Thread.Sleep(1);

                workerThread1.Join();

                Thread.Sleep(2000);

                TCPIP workerObject2 = new TCPIP();
                Thread workerThread2 = new Thread(workerObject2.sendANDreceive);

                workerObject2.command = "i";
                workerObject2.par = "";


                // Start the worker thread.
                workerThread2.Start();

                // Loop until worker thread activates. 
                while (!workerThread2.IsAlive) ;
                // Put the main thread to sleep for 1 millisecond to 
                // allow the worker thread to do some work:
                Thread.Sleep(1);

                workerThread2.Join();

                AppendData(loop.ToString() + "," + workerObject2.res.Split(',')[4] + "," +
                    workerObject2.res.Split(',')[5] + "," + workerObject1.res);

                string resToAppend = loop.ToString() + ",U,M,";
                
                AppendResult(loop.ToString() + "," + workerObject2.res.Split(',')[4] + "," +
                    workerObject2.res.Split(',')[5] + "," + workerObject1.res);

                Thread.Sleep(2000);

                Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(doHeatQuery));

                t.Start();



                Boolean timeRunning = true;



                do
                {
                    DateTime wait = DateTime.Now;
                    if (endCycle + 60 < wait.Hour * 60 * 60 + wait.Minute * 60 + wait.Second)
                    {
                        timeRunning = false;
                    }
                    Thread.Sleep(100);
                } while (timeRunning);
                loop += 1;

            } while (cont);
            MessageBox.Show("Assay ready");
        }

        private void ButtonWrite_Click(object sender, EventArgs e)
        {
            try
            {

                //before your loop
                var csv = new StringBuilder();

                for (int i = 0; i < Data.Items.Count; i++)
                {

                    var newLine = string.Format(Data.Items[i].ToString() + Environment.NewLine);
                    csv.Append(newLine);
                }


                //after your loop
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Miriam_wifi_data.csv", csv.ToString());
            }
            catch (IOException)
            {
                MessageBox.Show("File not writable");
            }
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            Results.Visible = false;
            ConnectStatus.Visible = true;
            LabelNWID.Visible = true;
            TbNWID.Visible = true;
            LabelNWPass.Visible = true;
            TbNWPass.Visible = true;
            selectedCom = COM.SelectedItem.ToString();
            Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(connect));
            t.Start();
        }

        private void writeConnect(String value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(writeConnect), new object[] { value });
                return;
            }

            ConnectStatus.Items.Add(value);
            ConnectStatus.SelectedIndex = ConnectStatus.Items.Count-1;
        }

        private void connect()
        {
            string[] ArrayComPortsNames = null;
            //int index = -1;
            //string ComPortName = null;

            ArrayComPortsNames = SerialPort.GetPortNames();

            SerialPort serialPort = null;

            // Create a new SerialPort object with default settings.
            serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            serialPort.PortName = selectedCom;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.BaudRate = 9600;

            // Set the read/write timeouts
            serialPort.ReadTimeout = 10000;
            serialPort.WriteTimeout = 500;

            try
            {
                serialPort.Open();
                serialPort.DiscardOutBuffer();
                serialPort.DiscardInBuffer();

                String ReceivedData;
                //RecievedData = serialPort.ReadLine();
                //serialPort.DataReceived += new SerialDataReceivedEventHandler(responseHandler);
                serialPort.Write("AT+CWQAP" + "\r\n");

                Boolean cont = true;
                do
                {
                    ReceivedData = serialPort.ReadLine();
                    writeConnect(ReceivedData);
                    if (ReceivedData.Contains('$'))
                    {
                        cont = false;
                    }
                } while (cont);

                serialPort.Write("AT+CWJAP=" + "\"" + "QB3@953" + "\"" + "," + "\"" + "QB34innovation!" + "\"" + "\r\n");

                cont = true;
                do
                {
                    ReceivedData = serialPort.ReadLine();
                    writeConnect(ReceivedData);
                    if (ReceivedData.Contains('$'))
                    {
                        cont = false;
                    }
                } while (cont);
                cont = true;

                serialPort.Write("AT+CIFSR" + "\r\n");

                cont = true;
                do
                {
                    ReceivedData = serialPort.ReadLine();
                    writeConnect(ReceivedData);
                    Console.WriteLine(ReceivedData);
                    if (ReceivedData.Contains('$'))
                    {
                        cont = false;
                        Console.WriteLine(ReceivedData.Split('\"')[5]);
                        TCPIP.ip = ReceivedData.Split('\"')[5];
                    }
                } while (cont);
                cont = true;


                serialPort.Close();
                

            }
            catch (Exception exc)
            {
                MessageBox.Show("Serial could not be opened, please check that the device is correct one");
                serialPort.Close();
            }
        
        }

    }
}
