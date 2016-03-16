using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miriam
{
    public partial class Debug : Form
    {
        
        public Debug()
        {
            InitializeComponent();
        }


        private void Read_Click(object sender, EventArgs e)
        {
            Data.Items.Add("R");
            TCPIP workerObject = new TCPIP();
            Thread workerThread = new Thread(workerObject.sendANDreceive);

            workerObject.command = "R";
            workerObject.par = "";


            // Start the worker thread.
            workerThread.Start();

            // Loop until worker thread activates. 
            while (!workerThread.IsAlive) ;
            // Put the main thread to sleep for 1 millisecond to 
            // allow the worker thread to do some work:
            Thread.Sleep(1);

            workerThread.Join();
            Data.Items.Add(workerObject.res);
            Data.SelectedIndex = Data.Items.Count - 1;
        }

        private void info_Click(object sender, EventArgs e)
        {
            Data.Items.Add("i");

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
            Data.Items.Add(workerObject.res);
            Data.SelectedIndex = Data.Items.Count - 1;
        }

        private void SetHeat_Click(object sender, EventArgs e)
        {
            Data.Items.Add("P");

            TCPIP workerObject = new TCPIP();
            Thread workerThread = new Thread(workerObject.sendANDreceive);

            workerObject.command = "P";
            workerObject.par = "";


            // Start the worker thread.
            workerThread.Start();

            // Loop until worker thread activates. 
            while (!workerThread.IsAlive) ;
            // Put the main thread to sleep for 1 millisecond to 
            // allow the worker thread to do some work:
            Thread.Sleep(1);

            workerThread.Join();
            Data.Items.Add(workerObject.res);
            Data.SelectedIndex = Data.Items.Count - 1;
        }

        private void SetU_Click(object sender, EventArgs e)
        {
            Data.Items.Add("U");

            TCPIP workerObject = new TCPIP();
            Thread workerThread = new Thread(workerObject.sendANDreceive);

            workerObject.command = "U";
            workerObject.par = temp.Text;


            // Start the worker thread.
            workerThread.Start();

            // Loop until worker thread activates. 
            while (!workerThread.IsAlive) ;
            // Put the main thread to sleep for 1 millisecond to 
            // allow the worker thread to do some work:
            Thread.Sleep(1);

            workerThread.Join();
            Data.Items.Add(workerObject.res);
            Data.SelectedIndex = Data.Items.Count - 1;

            temp.Text = "";
        }

        private void SetL_Click(object sender, EventArgs e)
        {
            Data.Items.Add("M");

            TCPIP workerObject = new TCPIP();
            Thread workerThread = new Thread(workerObject.sendANDreceive);

            workerObject.command = "M";
            workerObject.par = temp.Text;


            // Start the worker thread.
            workerThread.Start();

            // Loop until worker thread activates. 
            while (!workerThread.IsAlive) ;
            // Put the main thread to sleep for 1 millisecond to 
            // allow the worker thread to do some work:
            Thread.Sleep(1);

            workerThread.Join();
            Data.Items.Add(workerObject.res);
            Data.SelectedIndex = Data.Items.Count - 1;

            temp.Text = "";
        }

        private void Heat_Click(object sender, EventArgs e)
        {
            Data.Items.Add("H");

            TCPIP workerObject = new TCPIP();
            Thread workerThread = new Thread(workerObject.sendANDreceive);

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
            Data.Items.Add(workerObject.res);
            Data.SelectedIndex = Data.Items.Count - 1;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Data.Items.Add("C");

            TCPIP workerObject = new TCPIP();
            Thread workerThread = new Thread(workerObject.sendANDreceive);

            workerObject.command = "C";
            workerObject.par = "";


            // Start the worker thread.
            workerThread.Start();

            // Loop until worker thread activates. 
            while (!workerThread.IsAlive) ;
            // Put the main thread to sleep for 1 millisecond to 
            // allow the worker thread to do some work:
            Thread.Sleep(1);

            workerThread.Join();
            Data.Items.Add(workerObject.res);
            Data.SelectedIndex = Data.Items.Count - 1;
            
        }
    }
}
