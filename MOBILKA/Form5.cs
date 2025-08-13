using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MOBILKA
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Shown(object sender, EventArgs e)
        {
            /*if (File.Exists("clear_all_sms.txt"))
            {
                ThreadStart thr1 = new ThreadStart(clear_sms_func);
                Thread clear_sms = new Thread(thr1);
                clear_sms.Start();
            }*/
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }
/*        public void clear_sms_func()
        {
            if (!Program._com_read)
            {
                for (int i = 0; i <= 99; i++)
                {

                    Form1.serialPort1.Write("AT+CMGD="+i.ToString()+" + (char)13 + (char)10");
                    Thread.Sleep(500);
                }
            }

        }*/
    }
}
