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

namespace Miriam
{

    

    public partial class Control : Form
    {
        private int duration;
        private string arrayNames;
        private int maximumValue = 0;
        private string port = "";
        private Boolean started = false;

        public Control()
        {
            InitializeComponent();

            string[] ports = SerialPort.GetPortNames();
            
            for(int i = 0; i <ports.Length;i++)
            {
                COM.Items.Add(ports[i]);
            }

            if(ports.Length != 0)
            {
                COM.SelectedIndex = 0;
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

            Results.Visible = true;


        }
        
        private void ButtonHeat_Click(object sender, EventArgs e)
        {
            // Set upper temperature

            SerialPort serialPort = null;

            // Create a new SerialPort object with default settings.
            serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            serialPort.PortName = COM.Text;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.BaudRate = 9600;

            try
            {
                serialPort.Open();
                serialPort.DiscardOutBuffer();
                serialPort.DiscardInBuffer();

                String ReceivedData;
                //RecievedData = serialPort.ReadLine();
                //serialPort.DataReceived += new SerialDataReceivedEventHandler(responseHandler);
                serialPort.Write("M " + CboxTempM.Text + "\r\n");

                Boolean conti = true;
                do
                {
                    ReceivedData = serialPort.ReadLine();
                    if (ReceivedData.Contains('$'))
                    {
                        conti = false;
                    }
                } while (conti);


                ReceivedData = "";

                serialPort.Write("U " + CboxTempU.Text + "\r\n");

                conti = true;
                do
                {
                    ReceivedData = serialPort.ReadLine();
                    if (ReceivedData.Contains('$'))
                    {
                        conti = false;
                    }
                } while (conti);


                ReceivedData = "";

                serialPort.Write("H\r\n");

                conti = true;
                do
                {
                    ReceivedData = serialPort.ReadLine();
                    if (ReceivedData.Contains('$'))
                    {
                        conti = false;
                    }
                } while (conti);


                ReceivedData = "";

                serialPort.Write("i\r\n");

                conti = true;
                do
                {
                    ReceivedData = serialPort.ReadLine();
                    if (ReceivedData.Contains('$'))
                    {
                        conti = false;
                    }
                } while (conti);


                ReceivedData = ReceivedData.Replace("$", "");
                ReceivedData = ReceivedData.Replace("\r", "");
                ReceivedData = ReceivedData.Replace("\n", "");

                AppendHeatLabel("Temperature U:" + ReceivedData.Split(',')[4] + "," + "Temperature M:" + ReceivedData.Split(',')[5]);


                serialPort.Close();

            }
            catch (Exception exc)
            {
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
            SerialPort serialPort = null;

            // Create a new SerialPort object with default settings.
            serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            serialPort.PortName = COM.Text;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.BaudRate = 9600;

            try
            {
                serialPort.Open();
                serialPort.DiscardOutBuffer();
                serialPort.DiscardInBuffer();

                String ReceivedData;
                //RecievedData = serialPort.ReadLine();
                //serialPort.DataReceived += new SerialDataReceivedEventHandler(responseHandler);
                serialPort.Write("i" + "\r\n");

                Boolean conti = true;
                do
                {
                    ReceivedData = serialPort.ReadLine();
                    if (ReceivedData.Contains('$'))
                    {
                        conti = false;
                    }
                } while (conti);


                ReceivedData = ReceivedData.Replace("$", "");
                ReceivedData = ReceivedData.Replace("\r", "");
                ReceivedData = ReceivedData.Replace("\n", "");

                AppendHeatLabel("Temperature U:" + ReceivedData.Split(',')[4] + "," + "Temperature M:" + ReceivedData.Split(',')[5]);

                serialPort.Close();

            }
            catch (Exception exc)
            {
            }
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
            if(started == false)
            {
                started = true;
                Results.Visible = true;
                port = COM.Text;

                DateTime localDate = DateTime.Now;


                duration = localDate.Hour * 60 * 60 + localDate.Minute * 60 + localDate.Second +
                    Convert.ToInt32(CboxDuration.Text) * 60;

                List<int> list = new List<int>();
                int counter = 3;
                string msg = "Time,U,M,";

                for (int i = 0; i < Plate.RowCount; i++)
                {
                    for (int j = 0; j < Plate.ColumnCount; j++)
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

                for (int i = 0; i < list.Count; i++)
                {
                    Results.Series.Add(msg.Split(',')[list[i]]);
                    Results.Series[msg.Split(',')[list[i]]].ChartType =
                                    SeriesChartType.FastLine;
                    Results.Series[msg.Split(',')[list[i]]].Color = clr[clrsUsed];

                    clrsUsed += 1;
                    if (clrsUsed > 9)
                    {
                        clrsUsed = 0;
                    }
                }


                Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(doAssay));

                // put to background to force to close if program exit
                t.IsBackground = true;


                t.Start();
            }
            


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
                

                SerialPort serialPort = null;

                // Create a new SerialPort object with default settings.
                serialPort = new SerialPort();

                // Allow the user to set the appropriate properties.
                serialPort.PortName = port;
                serialPort.DataBits = 8;
                serialPort.Parity = Parity.None;
                serialPort.StopBits = StopBits.One;
                serialPort.BaudRate = 9600;

                try
                {
                    serialPort.Open();
                    serialPort.DiscardOutBuffer();
                    serialPort.DiscardInBuffer();

                    String ReceivedData;
                    //RecievedData = serialPort.ReadLine();
                    //serialPort.DataReceived += new SerialDataReceivedEventHandler(responseHandler);
                    serialPort.Write("R" + "\r\n");

                    Boolean conti = true;
                    do
                    {
                        ReceivedData = serialPort.ReadLine();
                        if (ReceivedData.Contains('$'))
                        {
                            conti = false;
                        }
                    } while (conti);


                    conti = true;

                    serialPort.Write("i" + "\r\n");
                    String ReceivedData1;
                    do
                    {
                        ReceivedData1 = serialPort.ReadLine();
                        if (ReceivedData1.Contains('$'))
                        {
                            conti = false;
                        }
                    } while (conti);


                    conti = true;


                    Thread.Sleep(2000);

                    ReceivedData = ReceivedData.Replace("$", "");
                    ReceivedData1 = ReceivedData1.Replace("$", "");
                    ReceivedData = ReceivedData.Replace("\r", "");
                    ReceivedData1 = ReceivedData1.Replace("\r", "");
                    ReceivedData = ReceivedData.Replace("\n", "");
                    ReceivedData1 = ReceivedData1.Replace("\n", "");

                    AppendHeatLabel("Temperature U:" + ReceivedData1.Split(',')[4] + "," + "Temperature M:" + ReceivedData1.Split(',')[5]);



                    AppendData(loop.ToString() + "," + ReceivedData1.Split(',')[4] + "," +
                    ReceivedData1.Split(',')[5] + "," + ReceivedData);

                    string resToAppend = loop.ToString() + ",U,M,";
                
                    AppendResult(loop.ToString() + "," + ReceivedData1.Split(',')[4] + "," +
                        ReceivedData1.Split(',')[5] + "," + ReceivedData);

                    Thread.Sleep(2000);

                    serialPort.Close();

                }
                catch (Exception exc)
                {
                }


                Boolean timeRunning = true;



                do
                {
                    DateTime wait = DateTime.Now;
                    if (endCycle + 120 < wait.Hour * 60 * 60 + wait.Minute * 60 + wait.Second)
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
            //before your loop
            var csv = new StringBuilder();

            for(int i = 0; i<Data.Items.Count;i++)
            {
                
                var newLine = string.Format(Data.Items[i].ToString() + Environment.NewLine);
                csv.Append(newLine);
            }


            //after your loop
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/data.csv", csv.ToString());
        }



    }
}
