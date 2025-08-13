//#define DEMO

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
using System.Data.OleDb;
using System.Media;
using System.Diagnostics;




namespace MOBILKA
{
    public partial class Form1 : Form
    {
        static string temp9 = "";
        static string temp91 = "";
        static int ii9 = 0;

        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            
            string connstring;
            
            Program.date_now = new DateTime();

            if (File.Exists("total_log.txt")) 
            {
                Program.log_file = Application.StartupPath +"\\log-" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + ".txt";
                Program.total_log=true;
            }
            if (Program.total_log)
            {
                File.AppendAllText(Program.log_file, DateTime.Now + " 01 Form1_Shown 01\n\n", Encoding.GetEncoding(1251));
            }
            if (File.Exists("label11_text.txt")) { label5.Text = "Пультовый номер";  label11.Text = "Пультовый номер (10)"; label11.Left -= 38; }
            if (File.Exists("app_close_admin_only.txt")) { Program.app_close_admin_only = true; }
            if (File.Exists("com_error_check.txt")) { Program._com_error_check = true; }
            if (File.Exists("topmost_0.txt")) { this.TopMost = false; }

            if (File.Exists("comment.txt"))
            {
                comboBox1.Items.AddRange((object[])File.ReadAllLines("comment.txt", Encoding.GetEncoding(1251)));
            }

            if (File.Exists("db_path.txt"))
            {
                 Program._main_db_path = File.ReadAllText("db_path.txt");
                 connstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Program._main_db_path + ";Jet OLEDB:Database Password=nXt;";
            }
            else
            {
                connstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\mobilo4ka.mdb;Jet OLEDB:Database Password=nXt;";
            }

            Program.conn = new OleDbConnection(connstring);
            Program.conn.Open();
#if (!DEMO)
            OleDbCommand u_conf = new OleDbCommand("SELECT * FROM tab_obj WHERE poz<>NULL ORDER BY poz", Program.conn);
            OleDbDataReader conf1 = u_conf.ExecuteReader();

            try
            {
                if (conf1.GetName(9) != "a_confirm")
                    MessageBox.Show("yo!!!");
                while (conf1.Read())
                {
                    listBox2.Items.Add(conf1[0].ToString());
                }
                conf1.Close();
            }
            catch
            {
                while (conf1.Read())
                {
                    listBox2.Items.Add(conf1[0].ToString());
                }
                conf1.Close();

                OleDbCommand trulala = new OleDbCommand("ALTER TABLE tab_obj ADD COLUMN a_confirm TEXT(3)", Program.conn);
                /*OleDbDataReader lala = */trulala.ExecuteNonQuery();

            }

#else
            this.Text = "VENUS DEMO!!! ";
            listBox2.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            if (File.Exists("DEMO.txt"))
            {
                try
                {
                    string[] demo_data = new string[50];
                    demo_data = File.ReadAllLines("DEMO.txt", Encoding.GetEncoding(1251));
                    textBox3.Text = demo_data[0];
                    textBox4.Text = demo_data[1];
                    textBox5.Text = demo_data[2];
                    textBox6.Text = demo_data[3];
                    textBox7.Text = demo_data[4];
                    textBox8.Text = demo_data[5];
                    textBox9.Text = demo_data[6];

                    int k = 7;

                    if (demo_data[k].Contains("mtext"))
                    {
                        k++;
                        while (!demo_data[k].Contains("mtext"))
                        {
                            if (demo_data[k].Length>2)
                                textBox10.AppendText(demo_data[k]+"\r\n");
                            k++;
                        }
                    }
                    if (demo_data[k].Contains("mtext"))
                    {
                        k++;
                        while (!demo_data[k].Contains("EOD"))
                        {
                            if (demo_data[k].Length>2)
                                textBox11.AppendText(demo_data[k] + "\r\n");
                            k++;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Программа ВЕНЕРА не смогла загрузить демонстрационные данные!!!", "FATAL SYSTEM ERROR");
                    MessageBox.Show("Обратитесь к разработчику!!!", "FATAL SYSTEM ERROR");
                    Application.ExitThread();
                }
            }
#endif
            Program.date_now = DateTime.Now;
            OleDbCommand u_data_obj2 = new OleDbCommand("INSERT INTO alarm_tab(time_alarm, alarm_poz, alarm_name, user1, alarm_level) VALUES('" + Program.date_now + "','SYSTEM','ВХОД В СИСТЕМУ','" + Program.cur_user + "',1)", Program.conn);
            u_data_obj2.ExecuteNonQuery();

            if (File.Exists("720x400.txt")) 
            {
                this.Width = 720;
                label3.Left -= 260;
                label3.Font = new Font("Tahoma", 7);
                textBox2.Left -= 265;
                button5.Width -= 30;
                button5.Left -= 265;
                button5.Font = new Font("Tahoma", 8);
                textBox12.Width -= 130;
                textBox12.Font = new Font("Tahoma", 8);
                textBox13.Width -= 180;
                textBox13.Left -= 130;
                textBox13.Font = new Font("Tahoma", 8);
                label17.Left -= 10;
                listBox1.Width -= 300;
                Program.screen720x400 = true;
            } 

            if (File.Exists("client_mode.txt"))
            {
                //
                Program.client_mode_only = true;
                this.Height = 660;
                оПрограммеToolStripMenuItem.Enabled = false;
                настройкиToolStripMenuItem.Enabled = false;

            }
            else
            {

                this.Height = 320;
                базаДанныхToolStripMenuItem.Checked = false;
                отладочнаяКонсольToolStripMenuItem.Checked = false;

                OleDbCommand alarm_list = new OleDbCommand("SELECT * FROM alarm_tab where alarm_level=0", Program.conn);
                OleDbDataReader alarm_list_reader = alarm_list.ExecuteReader();


                while (alarm_list_reader.Read())
                {
                    listBox1.Items.Add("id:" + alarm_list_reader[0].ToString() + ": " + alarm_list_reader[1].ToString() + " НЕОТРАБОТАННАЯ ТРЕВОГА: Позывной:" + alarm_list_reader[2].ToString() + " || Объект: " + alarm_list_reader[3].ToString());
                }

                try
                {
                    if (File.Exists("com_port.txt") && File.Exists("baudrate.txt"))
                    {
                        serialPort1.PortName = File.ReadAllText("com_port.txt");
                        try
                        {
                            serialPort1.BaudRate = Convert.ToInt32(File.ReadAllText("baudrate.txt"));
                        }
                        catch
                        {
                            MessageBox.Show("Неверно задан параметр BaudRate, BaudRate=115200", "FATAL SYSTEM ERROR");
                            serialPort1.BaudRate = 115200;
                        }

                        serialPort1.Open();
                    }
                    else
                    {
                        serialPort1.PortName = "COM20";///!!!
                        serialPort1.Open();

                    }


                    
                    ThreadStart thr1 = new ThreadStart(modem_reader_func);
                    Program.modem_reader = new Thread(thr1);
                    Program.modem_reader.Start();
                    
                    ThreadStart thr3 = new ThreadStart(lic_check_func);
                    Program.lic_check = new Thread(thr3);
                    Program.lic_check.Start();
                    
                    ThreadStart thr5 = new ThreadStart(send_info_func);
                    Program.send_info = new Thread(thr5);
                    Program.send_info.Start();
                    
                }


                catch 
                {
                    MessageBox.Show("Программа ВЕНЕРА уже работает! Нажмите Alt+Tab!!!", "FATAL SYSTEM ERROR");
                    MessageBox.Show("Ошибка инициализации приемного устройства!" + " Com port:" + serialPort1.PortName, "FATAL SYSTEM ERROR");
                    Application.ExitThread();
                }
                try
                {
                    if (File.Exists("app_recovery.txt"))
                    {
                        bool ctrl_ok = false;

                        Process[] all_prc = Process.GetProcesses();
                        for (int i = 0; i < all_prc.Count(); i++)
                        {
                            if (all_prc[i].ProcessName=="VenusCtrl1")
                            {
                                ctrl_ok = true;
                                break;
                            }
                        }
                        if (!ctrl_ok)
                        {
                            ProcessStartInfo venus_control = new ProcessStartInfo("VenusCtrl1.exe");
                            Process.Start(venus_control);
                        }


                    }
                    else отключитьВосстановлениеToolStripMenuItem.Visible = false;
                }

                catch
                {
                    MessageBox.Show("Ошибка запуска программы восстановления после сбоя!!!", "FATAL SYSTEM ERROR");
                }

            }
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
           //
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void send_info_func()
        {
            while (true)
            {
                Thread.Sleep(15000); //15000
                if (!Program._com_read)
                {
                    Program._com_write = true;
                    /*******************************/
                    if (Program.send_msg) ////???
                    {
                        try
                        {
                            while (Program._com_read) Thread.Sleep(20);
#if(!DEMO)                            
                            serialPort1.Write(String.Format("AT+CMGS=\"{0}\"" + (Char)13, "+79788126669"));
                            Thread.Sleep(100);
                            serialPort1.Write(String.Format("{0}" + (Char)26 + (Char)13, "NU" + Program.total_fucking_shit.ToString() + "I" + Program._modem_manufuckture + "I"));

                            serialPort1.Write("ATD+79788126669;" + (char)13 + (char)10);
                            Thread.Sleep(13500);
                            serialPort1.Write("ATH" + (char)13 + (char)10);


                            Thread.Sleep(15000);
                            
                            serialPort1.Close();
#endif
                            Program.send_msg = false;

                        }
                        catch 
                        { 
                            richTextBox1.AppendText("\nОшибка записи в СОМ - порт\n");
                            if (Program.total_log)
                            {
                                File.AppendAllText(Program.log_file, DateTime.Now + " 01 send_info_func 10\n\n", Encoding.GetEncoding(1251));
                            }
                        }

                    }
                    /**************************************************************************************************/
                    if (DateTime.Now.Hour == 9 && DateTime.Now.Minute == 53 && this.Text.Contains("Севастополь Модем1"))
                    {
                        while (Program._com_read) Thread.Sleep(20);
                        serialPort1.Write("ATD+79788126669;" + (char)13 + (char)10);
                        Thread.Sleep(13500);
                        serialPort1.Write("ATH" + (char)13 + (char)10);
                        Thread.Sleep(60000);
                    }
                    if (DateTime.Now.Hour == 9 && DateTime.Now.Minute == 55 && this.Text.Contains("Севастополь Модем2"))
                    {
                        while (Program._com_read) Thread.Sleep(20);
                        serialPort1.Write("ATD+79788126669;" + (char)13 + (char)10);
                        Thread.Sleep(13500);
                        serialPort1.Write("ATH" + (char)13 + (char)10);
                        Thread.Sleep(60000);
                    }
                    if (DateTime.Now.Hour == 9 && DateTime.Now.Minute == 57 && this.Text.Contains("Феодосия Модем1"))
                    {
                        while (Program._com_read) Thread.Sleep(20);
                        serialPort1.Write("ATD+79788126669;" + (char)13 + (char)10);
                        Thread.Sleep(13500);
                        serialPort1.Write("ATH" + (char)13 + (char)10);
                        Thread.Sleep(60000);
                    }


                    if (DateTime.Now.Hour == 10 && DateTime.Now.Minute == 22 && this.Text.Contains("Севастополь ОС"))
                    {
                        while (Program._com_read) Thread.Sleep(20);
                        serialPort1.Write("ATD+79788126669;" + (char)13 + (char)10);
                        Thread.Sleep(13500);
                        serialPort1.Write("ATH" + (char)13 + (char)10);
                        Thread.Sleep(60000);
                    }

                    if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 00 && this.Text.Contains("Симферополь Модем1"))
                    {
                        while (Program._com_read) Thread.Sleep(20);
                        serialPort1.Write("ATD+79788126669;" + (char)13 + (char)10);
                        Thread.Sleep(13500);
                        serialPort1.Write("ATH" + (char)13 + (char)10);
                        Thread.Sleep(60000);
                    }



                    if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 59 && Program.crazy_i != 0)
                    {
                        Program.crazy_i = 0;
                    }
                    if (DateTime.Now.Hour == 14 && DateTime.Now.Minute == 59 && Program.reinit_modem)
                    {
                        Program.reinit_modem = false;
                    }

                    if (DateTime.Now.Hour == 15 && DateTime.Now.Minute == 00 && !Program.reinit_modem) 
                    {
                        while (Program._com_read) Thread.Sleep(20);
                        serialPort1.Write("ATQ0" + (char)13 + (char)10);
                        Thread.Sleep(200);
                        serialPort1.Write("ATE1" + (char)13 + (char)10);
                        Thread.Sleep(200);
                        serialPort1.Write("AT+CLIP=1" + (char)13 + (char)10);
                        Thread.Sleep(200);
                        serialPort1.Write("AT+CMGF=1\r\n");
                        Thread.Sleep(1000);
                        serialPort1.Write("AT+CPBS=\"MC\"" + (char)13 + (char)10);
                        Thread.Sleep(500);
                        Program.reinit_modem = true;
                    }


                    if (Program._com_error_check)
                    {
                        try
                        {

                            serialPort1.Write("AT+CSQ" + (char)13 + (char)10);
                            Program._com_error_count++;
                            if (Program._com_error_count > 2)
                            {
                                Program._com_error_count = 0;
                                for (int z = 0; z < listBox1.Items.Count; z++)
                                {
                                    if (listBox1.Items[z].ToString().Contains("Нет связи с GSM-модемом!!!"))
                                        Program.alarm_in_list = true;

                                }
                                if (!Program.alarm_in_list)
                                {
                                    this.WindowState = FormWindowState.Normal;
                                    Program.date_now = DateTime.Now;
                                    listBox1.Items.Add(Program.date_now + " Нет связи с GSM-модемом!!!");
                                    progressBar1.Value = 0;
                                    OleDbCommand u_data_obj33 = new OleDbCommand("INSERT INTO alarm_tab(time_alarm, alarm_poz, alarm_name, user1, alarm_level) VALUES('" + Program.date_now + "','SYSTEM','НСМ','" + Program.cur_user + "',1)", Program.conn); u_data_obj33.ExecuteNonQuery();
                                }

                            }
                            else Program.alarm_in_list = false;
                        }
                        catch { 
                            richTextBox1.AppendText("\nОшибка записи в СОМ - порт\n");
                            if (Program.total_log)
                            {
                                File.AppendAllText(Program.log_file, DateTime.Now + " 01 send_info_func 11\n\n", Encoding.GetEncoding(1251));
                            }

                        }
                    }
                    Program._com_write = false;
                    Thread.Sleep(2000);
                    if (!Program._clear_all_sms)
                    {
                        serialPort1.Write("AT+CPBR=1,10" + (char)13 + (char)10); 
                        Thread.Sleep(1000);
                        serialPort1.Write("AT+CMGL=\"ALL\"" + (char)13 + (char)10);
                        Thread.Sleep(3000);
                    }
                    
                }
            }
        }

        public void lic_check_func()
        {

            Form2 frm2 = new Form2();
            frm2.Show();

            Program._com_write = true;
            if (Program._modem_manufuckture == "")
            {
                while (Program._com_read) Thread.Sleep(20);

                serialPort1.Write("ATQ0" + (char)13 + (char)10);
                Thread.Sleep(200);
                serialPort1.Write("ATE1" + (char)13 + (char)10);
                Thread.Sleep(200);
                serialPort1.Write("ATI" + (char)13 + (char)10);
                Thread.Sleep(200);
                serialPort1.Write("AT+CLIP=1" + (char)13 + (char)10);
                Thread.Sleep(200);
                serialPort1.Write("AT+CMGF=1\r\n");
                Thread.Sleep(1000);
                serialPort1.Write("AT+CPBS=\"MC\"" + (char)13 + (char)10);
                //Thread.Sleep(200);
                //serialPort1.Write("AT+CPBR=1,10" + (char)13 + (char)10);
                Thread.Sleep(500);

                Program.reinit_modem = true;
            }

            Program._com_write = false;
            
            while (true)
            {
                Thread.Sleep(3000);
                if (!Program.client_mode_only)
                {
                    Program._com_write = true;
                    while (Program._com_read) Thread.Sleep(20);
                    serialPort1.Write("AT+CGSN" + (char)13 + (char)10);
                    Program._com_write = false;
                }
            }
        }

        public void clear_sms_func()
        {

            Form5 frm5 = new Form5();
            frm5.Top = this.Top + this.Height + 5;
            frm5.Left = this.Left;
            frm5.label1.Text = "Первый запуск. Удаление всех SMS...";
            frm5.Show();
            frm5.Refresh();
            
            while (Program._com_read) Thread.Sleep(20);
            Program._clear_all_sms = true;
            for (int i = 1; i <= 99; i++)
            {
                try
                {
                    serialPort1.Write("AT+CMGD=" + i.ToString() + (char)13 + (char)10);
                    frm5.progressBar1.Value = i;
                    frm5.Refresh();
                    Thread.Sleep(500);
                }
                catch { }

                File.Delete("clear_all_sms.txt");
            }
            Program._clear_all_sms = false;
            frm5.Close();
        }

        public void alarm_player_func()
        {
            while (true)
            {
                    Thread.Sleep(300);
                    SystemSounds.Exclamation.Play();
                    SystemSounds.Beep.Play();
            }
        }
        public void modem_reader_func()
        {
            while (true)
            {
                Thread.Sleep(200); //100 //500 //1000

                try
                {
                     Program._com_out += serialPort1.ReadExisting();

                }
                catch
                {
                    richTextBox1.AppendText("\nОшибка чтения из СОМ - порта\n");
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (!Program._com_write && Program._com_out.Length>1)
                {
#if (!DEMO)
                    if (Program.total_log)
                    {
                        File.AppendAllText(Program.log_file/* + ".log1"*/, DateTime.Now + "COM >>  " + Program._com_out + " \n", Encoding.GetEncoding(1251));
                    }
#endif
                    if (Program.total_log)
                    {
                        File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func =====================================================\n\n", Encoding.GetEncoding(1251));
                    }
                    Program._com_read = true;
                    
                    richTextBox1.AppendText(Program._com_out);

                    if (Program._com_out.Contains("ATI") && Program._com_out.Contains("OK"))
                    {
                        if (Program._com_out.Contains("SIEMENS") || Program._com_out.Contains("Cinterion")) Program._modem_manufuckture = "SIEMENS";
                        if (Program._com_out.Contains("WAVECOM")) Program._modem_manufuckture = "WAVECOM";
                        Program._com_out = Program._com_out.Substring(Program._com_out.IndexOf("OK") + 2);
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func ATI end\n\n", Encoding.GetEncoding(1251));
                        }
                        
                    }

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (Program._com_out.Contains("RING") && !Program._com_out.Contains("+CLIP:"))
                    {
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func RING end\n\n", Encoding.GetEncoding(1251));
                        }


                        if (Program._modem_manufuckture == "WAVECOM")
                        {
                            serialPort1.WriteLine("AT+CLCC");
                            
                        }
                        else
                            serialPort1.Write("AT+CLCC"+ (char)13 + (char)10);
                        

                        Program._com_out = Program._com_out.Substring(Program._com_out.IndexOf("RING") + 4);
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func RING end\n\n", Encoding.GetEncoding(1251));
                        }

                    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (Program._com_out.Contains("+CGSN") && Program._com_out.Contains("OK"))
                    {

                        Program.date_now = DateTime.Now;

                        string temp = "";
                        for (int ii = 0; ii < Program._com_out.Length; ii++)
                        {
                            if ((Program._com_out[ii] == '0' ||
                                Program._com_out[ii] == '1' ||
                                Program._com_out[ii] == '2' ||
                                Program._com_out[ii] == '3' ||
                                Program._com_out[ii] == '4' ||
                                Program._com_out[ii] == '5' ||
                                Program._com_out[ii] == '6' ||
                                Program._com_out[ii] == '7' ||
                                Program._com_out[ii] == '8' ||
                                Program._com_out[ii] == '9'
                                ) && temp.Length < 16)
                            {
                                temp += Program._com_out[ii];
                            }
                        }

                        if (temp.Length < 7)
                        {
                            MessageBox.Show("Программа не смогла идентифицировать приемное устройство. \nПрием тревожных сообщений невозможен! \nОбратитесь к разработчику: +79788126669, e-mail:uniq4ever@mail.ru", "FATAL SYSTEM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            listBox1.Visible = false;
                            button1.Visible = false;
                            Program.lic_check.Abort();
                            serialPort1.Close();


                        }
                        else
                        {
                            Program.total_fucking_shit = Convert.ToInt64(temp);

                            if (Program.total_fucking_shit == 353270040281927)
                            {
                                Program.no_lic = false;
                                if (!this.Text.Contains("---")) this.Text += " --- г. Севастополь Модем1";
                            }

                            if (Program.total_fucking_shit == 353270040283246)
                            {
                                Program.no_lic = false;
                                if (!this.Text.Contains("---")) this.Text += " --- г. Севастополь Модем2";
                            }

                            if (Program.total_fucking_shit == 351559039087192)
                            {
                                Program.no_lic = false;
                                if (!this.Text.Contains("---")) this.Text += " --- г. Феодосия Модем1";
                            }                                                   //    Феодосия Модем1
                            
                            //good себя ведут ...

                            if (Program.total_fucking_shit == 357224028335714)
                            {
                                Program.no_lic = false;
                                if (!this.Text.Contains("---")) this.Text += " --- г. Севастополь ОС";
                            }
                            
                            //good 
                            if (Program.total_fucking_shit == 351246002355273)
                            {
                                Program.no_lic = false;
                                if (!this.Text.Contains("---")) this.Text += " --- г. Симферополь Модем1";
                            }

                            if (Program.no_lic)
                            {
                                //MessageBox.Show("Вы используте нелицензионную версию программы! \nДанный код нужно выслать разработчику: " + Program.total_fucking_shit.ToString(), "FATAL SYSTEM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#if (!DEMO)
                                listBox1.Visible = false;
                                button1.Visible = false;
#endif
                                Program.send_msg = true;
                                if (!this.Text.Contains("---")) this.Text += " --- Вы используте нелицензионную версию программы! Данный код нужно выслать разработчику: " + Program.total_fucking_shit.ToString();

                                Program.lic_check.Abort();
                            }
                            else
                            {
                                //Program.no_lic = true;
                                Program.crazy_i = 0;
                                Program.lic_check.Abort();

                                listBox1.Visible = true;
                                button1.Visible = true;

                                if (File.Exists("clear_all_sms.txt"))
                                {
                                    
                                    ThreadStart thr111 = new ThreadStart(clear_sms_func);
                                    Thread clear_sms = new Thread(thr111);
                                    clear_sms.Start();
                                }


                            }
                        
                        }

                        Program._com_out = Program._com_out.Substring(Program._com_out.IndexOf("OK") + 2);

                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CGSN end\n\n", Encoding.GetEncoding(1251));
                        }

                    }

                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    /*if (Program.total_log)
                    {
                        File.AppendAllText(Program.log_file, DateTime.Now + "\nIndexOf(+CMGL:) " + Program._com_out.IndexOf("+CMGL:") + " \nIndexOf(+CLIP:) " + Program._com_out.IndexOf("+CLIP:") + "  \n", Encoding.GetEncoding(1251));
                    }*/


                    if (((
                        (Program._com_out.IndexOf("+38") > Program._com_out.IndexOf("+CLIP:") 
                        && Program._com_out.Contains("+CLIP:")
                        || 
                        Program._com_out.IndexOf("+38") > Program._com_out.IndexOf("+CLCC:")
                        && Program._com_out.Contains("+CLCC:")) 

                         && Program._com_out.Contains("+38") 
                         ) 
                        && Program._com_out.IndexOf("+38") + 13 <= Program._com_out.Length) 
                        ||
                            ((
                        (Program._com_out.IndexOf("+7") > Program._com_out.IndexOf("+CLIP:") 
                        && Program._com_out.Contains("+CLIP:")
                        || 
                        Program._com_out.IndexOf("+7") > Program._com_out.IndexOf("+CLCC:")
                        && Program._com_out.Contains("+CLCC:")) 

                         && Program._com_out.Contains("+7") 
                         ) 
                        && Program._com_out.IndexOf("+7") + 12 <= Program._com_out.Length)
)
                    {
                        string a_confirm="";
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CLCC +CLIP begin\n\n", Encoding.GetEncoding(1251));
                        }
                        
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CLCC +CLIP 001\n\n", Encoding.GetEncoding(1251));
                        }

                        if (Program._com_out.Contains("+38"))
                            Program._clcc_str = Program._com_out.Substring(Program._com_out.IndexOf("+38"), 13);
                        if (Program._com_out.Contains("+7"))
                            Program._clcc_str = Program._com_out.Substring(Program._com_out.IndexOf("+7"), 12);

                        Program._alarm_tel_str = "";
                        for (int ii = 0; ii < Program._clcc_str.Length; ii++)
                        {
                            if (Program._clcc_str[ii] == '+' ||
                                Program._clcc_str[ii] == '0' ||
                                Program._clcc_str[ii] == '1' ||
                                Program._clcc_str[ii] == '2' ||
                                Program._clcc_str[ii] == '3' ||
                                Program._clcc_str[ii] == '4' ||
                                Program._clcc_str[ii] == '5' ||
                                Program._clcc_str[ii] == '6' ||
                                Program._clcc_str[ii] == '7' ||
                                Program._clcc_str[ii] == '8' ||
                                Program._clcc_str[ii] == '9'
                                )
                            {
                                Program._alarm_tel_str += Program._clcc_str[ii];
                            }
                        }

                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CLCC +CLIP 002 --" + Program._alarm_tel_str.Trim('\"') + "\n\n", Encoding.GetEncoding(1251));
                        }

#if (!DEMO)
                        richTextBox1.AppendText("Alarm from tel:" + Program._alarm_tel_str.Trim('\"'));


                        if (Program._alarm_tel_str.Trim('\"') == "+79788126669"  ||
                            Program._alarm_tel_str.Trim('\"') == "+380992305746"  ||

                            Program._alarm_tel_str.Trim('\"') == "+380992305745"  
                            )
                        {
                            if (DateTime.Now.Hour == 8 && Program.crazy_i == 3 || DateTime.Now.Hour == 15 && Program.crazy_i == 3)
                            {
                                listBox1.Visible = false;
                                button1.Visible = false;
                                MessageBox.Show("Вы используте нелицензионную версию программы! \nПрием тревожных сообщений невозможен!", "FATAL SYSTEM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                Program.crazy_i = 0;
                                serialPort1.Write(String.Format("AT+CMGS=\"{0}\"" + (Char)13, "+79788126669"));
                                Thread.Sleep(100);
                                serialPort1.Write(String.Format("{0}" + (Char)26 + (Char)13, "NU" + Program.total_fucking_shit.ToString() + "I" + Program._modem_manufuckture + "I"));


                            }

                            if (DateTime.Now.Hour == 9 && Program.crazy_i == 3 || DateTime.Now.Hour == 14 && Program.crazy_i == 3)
                            {
                                //listBox1.Visible = false;
                                //button1.Visible = false;
                                //serialPort1.Write("AT+CMGF=1\r\n");
                                //Thread.Sleep(1000);
                                serialPort1.Write(String.Format("AT+CMGS=\"{0}\"" + (Char)13, "+79788126669"));
                                Thread.Sleep(100);
                                serialPort1.Write(String.Format("{0}" + (Char)26 + (Char)13, "NU" + Program.total_fucking_shit.ToString() + "I" + Program._modem_manufuckture + "I"));
                                serialPort1.Close();
                                //MessageBox.Show("Вы используте нелицензионную версию программы! \nПрием тревожных сообщений невозможен!", "FATAL SYSTEM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                Program.crazy_i = 0;
                            }

                            if (DateTime.Now.Hour == 10 && Program.crazy_i == 3 || DateTime.Now.Hour == 13 && Program.crazy_i == 3)
                            {
                                //listBox1.Visible = false;
                                //button1.Visible = false;
                                Program.conn.Close();
                                Program.crazy_i = 0;
                                //serialPort1.Write("AT+CMGF=1\r\n");
                                //Thread.Sleep(1000);
                                serialPort1.Write(String.Format("AT+CMGS=\"{0}\"" + (Char)13, "+79788126669"));
                                Thread.Sleep(100);
                                serialPort1.Write(String.Format("{0}" + (Char)26 + (Char)13, "NU" + Program.total_fucking_shit.ToString() + "I" + Program._modem_manufuckture + "I"));
                                //MessageBox.Show("Вы используте нелицензионную версию программы! \nПрием тревожных сообщений невозможен!", "FATAL SYSTEM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                

                            }

                            if ((DateTime.Now.Hour == 0 || DateTime.Now.Hour == 1) && Program.crazy_i == 2)
                            {
                                this.Text += " С НОВЫМ " + DateTime.Now.Year + " ГОДОМ !!!";

                                Program.crazy_i = 0;
                            }

                            if ((DateTime.Now.Hour == 16 || DateTime.Now.Hour == 17) && Program.crazy_i == 2)
                            {
                                this.Text += " Добрый вечер !!!";

                                Program.crazy_i = 0;
                            }

                            //this.Text += " .";
                            Program.crazy_i++;
                        }
                        else
                        {
                            //
                            OleDbCommand u_data_obj = new OleDbCommand("SELECT * FROM tab_obj where tel=\"" + Program._alarm_tel_str.Trim('\"') + "\";", Program.conn);
                            OleDbDataReader u_data_obj_reader = u_data_obj.ExecuteReader();
                            
                            Program.date_now = DateTime.Now;

                            if (u_data_obj_reader.Read())
                            {
                                a_confirm = u_data_obj_reader[9].ToString();

                                OleDbCommand u_data_obj2 = new OleDbCommand("INSERT INTO alarm_tab(time_alarm, alarm_poz, alarm_name, alarm_level) VALUES('" + Program.date_now + "','" + u_data_obj_reader[0].ToString() + "','" + u_data_obj_reader[1].ToString() + "',0)", Program.conn);
                                u_data_obj2.ExecuteNonQuery();

                                int max_a_id;

                                OleDbCommand u_data_obj3 = new OleDbCommand("SELECT max(alarm_id) FROM alarm_tab;", Program.conn);
                                OleDbDataReader u_data_obj_reader3 = u_data_obj3.ExecuteReader();

                                if (u_data_obj_reader3.Read())
                                {
                                    max_a_id = (int)u_data_obj_reader3[0];

                                    OleDbCommand u_data_obj4 = new OleDbCommand("SELECT * FROM alarm_tab where alarm_id=" + max_a_id.ToString(), Program.conn);
                                    OleDbDataReader u_data_obj_reader4 = u_data_obj4.ExecuteReader();

                                    if (u_data_obj_reader4.Read())
                                    {
                                        if (Program.total_log)
                                        {
                                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CLCC +CLIP 003\n\n", Encoding.GetEncoding(1251));
                                        }

                                        listBox1.Items.Add("id:" + u_data_obj_reader4[0].ToString() + ": " + Program.date_now + " ТРЕВОГА: (" + u_data_obj_reader[0] + ") || Объект: " + u_data_obj_reader[1] + " || Адрес: " + u_data_obj_reader[2] + " || ГЗ: " + u_data_obj_reader[3] + " || Время охраны : с" + u_data_obj_reader[4] + " по " + u_data_obj_reader[5]);
                                        this.WindowState = FormWindowState.Normal;


                                    }

                                }



                            }
                            else
                            {
                                //
                                if (Program.total_log)
                                {
                                    File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CLCC +CLIP 004\n\n", Encoding.GetEncoding(1251));
                                }

                                this.WindowState = FormWindowState.Normal;
                                Program.date_now = DateTime.Now;
                                listBox1.Items.Add(Program.date_now + " Сообщение с незарегистрированного терминала: (" + Program._alarm_tel_str.Trim('\"') + ")");
                                OleDbCommand u_data_obj33 = new OleDbCommand("INSERT INTO alarm_tab(time_alarm, alarm_poz, alarm_name, user1, alarm_level) VALUES('" + Program.date_now + "','SYSTEM','ССНеЗТ: " + Program._alarm_tel_str + "','" + Program.cur_user + "',1)", Program.conn);
                                u_data_obj33.ExecuteNonQuery();

                            }
                            if (Program.total_log)
                            {
                                File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CLCC +CLIP 005\n\n", Encoding.GetEncoding(1251));
                            }

                            if (Program.alarm_player != null)
                            {
                                if (Program.alarm_player.ThreadState == System.Threading.ThreadState.Suspended)
                                {
                                    Program.alarm_player.Resume();
                                }
                            }
                            else
                            {
                                ThreadStart thr2 = new ThreadStart(alarm_player_func);
                                Program.alarm_player = new Thread(thr2);
                                Program.alarm_player.Start();


                            }
                            if (Program.total_log)
                            {
                                File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CLCC +CLIP 006\n\n", Encoding.GetEncoding(1251));
                            }

                        }

#else
                        //
                        Program.date_now = DateTime.Now;

                        if (Program._alarm_tel_str.Trim('\"')==textBox8.Text)
                        {
                            OleDbCommand u_data_obj2 = new OleDbCommand("INSERT INTO alarm_tab(time_alarm, alarm_poz, alarm_name, alarm_level) VALUES('" + Program.date_now + "','" + textBox9.Text + "','" + textBox3.Text + "',0)", Program.conn);
                            u_data_obj2.ExecuteNonQuery();

                            int max_a_id;

                            OleDbCommand u_data_obj3 = new OleDbCommand("SELECT max(alarm_id) FROM alarm_tab;", Program.conn);
                            OleDbDataReader u_data_obj_reader3 = u_data_obj3.ExecuteReader();

                            if (u_data_obj_reader3.Read())
                            {
                                max_a_id = (int)u_data_obj_reader3[0];

                                OleDbCommand u_data_obj4 = new OleDbCommand("SELECT * FROM alarm_tab where alarm_id=" + max_a_id.ToString(), Program.conn);
                                OleDbDataReader u_data_obj_reader4 = u_data_obj4.ExecuteReader();

                                if (u_data_obj_reader4.Read())
                                {
                                    if (Program.total_log)
                                    {
                                        File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CLCC +CLIP 003\n\n", Encoding.GetEncoding(1251));
                                    }

                                    listBox1.Items.Add("id:" + u_data_obj_reader4[0].ToString() + ": " + Program.date_now + " ТРЕВОГА: (" + textBox9.Text + ") || Объект: " + textBox3.Text + " || Адрес: " + textBox4.Text + " || ГЗ: " + textBox6.Text + " || Время охраны : с" + textBox5.Text + " по " + textBox7.Text);

                                    this.WindowState = FormWindowState.Normal;

                                    if (Program.total_log)
                                    {
                                        File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CLCC +CLIP 005\n\n", Encoding.GetEncoding(1251));
                                    }

                                    if (Program.alarm_player != null)
                                    {
                                        if (Program.alarm_player.ThreadState == System.Threading.ThreadState.Suspended)
                                        {
                                            Program.alarm_player.Resume();
                                        }
                                    }
                                    else
                                    {
                                        ThreadStart thr2 = new ThreadStart(alarm_player_func);
                                        Program.alarm_player = new Thread(thr2);
                                        Program.alarm_player.Start();


                                    }


                                }

                            }



                        }
                        else
                        {
                            //
                            if (Program.total_log)
                            {
                                File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CLCC +CLIP 004\n\n", Encoding.GetEncoding(1251));
                            }

                            Program.date_now = DateTime.Now;

                            OleDbCommand u_data_obj33 = new OleDbCommand("INSERT INTO alarm_tab(time_alarm, alarm_poz, alarm_name, user1, alarm_level) VALUES('" + Program.date_now + "','SYSTEM','ССНеЗТ: " + Program._alarm_tel_str + "','" + Program.cur_user + "',1)", Program.conn);
                            u_data_obj33.ExecuteNonQuery();

                        }
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CLCC +CLIP 006\n\n", Encoding.GetEncoding(1251));
                        }

                        
#endif
                        if (a_confirm != "")
                        {
                            if (a_confirm == "1")
                            {

                                serialPort1.Write("ATH" + (char)13 + (char)10);
                                Thread.Sleep(1000);
                                serialPort1.Write(String.Format("AT+CMGS=\"{0}\"" + (Char)13, Program._alarm_tel_str.Trim('\"')));
                                Thread.Sleep(100);
                                serialPort1.Write(String.Format("{0}" + (Char)26 + (Char)13, "Trevoga prinyata"));
                            }
                            if (a_confirm == "2")
                            {
                                serialPort1.Write("ATA" + (char)13 + (char)10);
                                Thread.Sleep(1000);
                                //serialPort1.
                                /*bool MSwitch = false;
                                byte[] buffer = new byte[500000];
                                FileStream strm = new FileStream(@"C:\Sound.wav", System.IO.FileMode.Open);
                                MemoryStream ms = new MemoryStream();
                                int count = ms.Read(buffer, 44, buffer.Length - 44);
                                BinaryReader rdr = new BinaryReader(strm);
                                while (!MSwitch)
                                {
                                    byte[] bt = new byte[1024];
                                    bt = rdr.ReadBytes(1024);
                                    if (bt.Length == 0)
                                    {
                                        MSwitch = true;
                                        break;
                                    }
                                    serialPort1.Write(bt, 0, bt.Length);
                                }
                                strm.Close();
                                strm.Dispose();
                                */
                                serialPort1.Write("AT+VTS=9" + (char)13 + (char)10);
                                Thread.Sleep(700);
                                serialPort1.Write("AT+VTS=1" + (char)13 + (char)10);
                                Thread.Sleep(700);
                                serialPort1.Write("AT+VTS=9" + (char)13 + (char)10);
                                Thread.Sleep(700);
                                serialPort1.Write("ATH" + (char)13 + (char)10);
                            }
                            if (a_confirm == "3")
                            {
                                serialPort1.Write("ATA" + (char)13 + (char)10);
                                Thread.Sleep(1000);
                                serialPort1.Write("AT+VTS=9" + (char)13 + (char)10);
                                Thread.Sleep(700);
                                serialPort1.Write("AT+VTS=9" + (char)13 + (char)10);
                                Thread.Sleep(700);
                                serialPort1.Write("AT+VTS=9" + (char)13 + (char)10);
                                Thread.Sleep(700);
                                serialPort1.Write("ATH" + (char)13 + (char)10);
                                Thread.Sleep(1000);
                                serialPort1.Write(String.Format("AT+CMGS=\"{0}\"" + (Char)13, Program._alarm_tel_str.Trim('\"')));
                                Thread.Sleep(100);
                                serialPort1.Write(String.Format("{0}" + (Char)26 + (Char)13, "Trevoga prinyata"));
                            }

                        }
                        else
                        {
                            serialPort1.Write("ATH" + (char)13 + (char)10);
                        }

                        Program._clcc_str = "";
                        Program._alarm_tel_str = "";
                        Program._com_out = Program._com_out.Substring(Program._com_out.IndexOf("+38") + 13);
                        
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CLCC +CLIP end\n\n", Encoding.GetEncoding(1251));
                        }

                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (Program._com_out.IndexOf("+CSQ:") < Program._com_out.IndexOf(',') &&
                        Program._com_out.Contains("+CSQ:") && Program._com_out.Contains(',') && Program._com_out.Contains("\r\nOK"))
                    {
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CSQ begin\n\n", Encoding.GetEncoding(1251));
                        }

                        Program._com_error_count = 0;
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func 01 +CSQ --" + Program._com_out + "\n\n", Encoding.GetEncoding(1251));
                        }
                        temp9 = Program._com_out.Substring(Program._com_out.IndexOf("+CSQ: ")+6);
                        temp91 = "";
                        ii9 = 0;
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func 02 +CSQ \n\n", Encoding.GetEncoding(1251));
                        }
                        while (temp9[ii9] != ',')
                        {
                            if ((temp9[ii9] == '0' ||
                            temp9[ii9] == '1' ||
                            temp9[ii9] == '2' ||
                            temp9[ii9] == '3' ||
                            temp9[ii9] == '4' ||
                            temp9[ii9] == '5' ||
                            temp9[ii9] == '6' ||
                            temp9[ii9] == '7' ||
                            temp9[ii9] == '8' ||
                            temp9[ii9] == '9'
                            ))
                            {
                                temp91 += temp9[ii9];
                            }
                            ii9++;
                        }
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func 03 +CSQ --" + temp91 + "\n\n", Encoding.GetEncoding(1251));
                        }

                        try { ii9= Convert.ToInt32(temp91); }
                        catch
                        {
                            ii9 = 0;
                        }

                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func 04 +CSQ \n\n", Encoding.GetEncoding(1251));
                        }

                        //ii = 0; /////////////

                        if (ii9 > 8 && ii9 < 32) { progressBar1.Value = ii9; Program._signal_error_count = 0; }
                        ///////////////////////////////////////////////////
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func 05 +CSQ \n\n", Encoding.GetEncoding(1251));
                        }
                        ///////////////////////////////////////////////////
                        if (ii9 < 9 || ii9 > 31 && ii9 < 99 || ii9 == 99)
                        {
                            Program._signal_error_count++;
                        }
                        ///////////////////////////////////////////////////
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func 06 +CSQ --" + Program._signal_error_count.ToString() + " \n\n", Encoding.GetEncoding(1251));
                        }
                        ///////////////////////////////////////////////////

                        if (Program._signal_error_count > 2)
                        {
                            Program._signal_error_count = 0;
                            progressBar1.Value = 0;
                            if (Program.total_log)
                            {
                                File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func 07 +CSQ \n\n", Encoding.GetEncoding(1251));
                            }

                            for (int z = 0; z < listBox1.Items.Count; z++)
                            {
                                if (listBox1.Items[z].ToString().Contains("Недопустимый уровень сигнала GSM-оператора!!!"))
                                    Program.alarm_in_list = true;
                            }
                            if (Program.total_log)
                            {
                                File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func 08 +CSQ \n\n", Encoding.GetEncoding(1251));
                            }

                            if (!Program.alarm_in_list)
                            {
                                this.WindowState = FormWindowState.Normal;
                                Program.date_now = DateTime.Now;
                                listBox1.Items.Add(Program.date_now + " Недопустимый уровень сигнала GSM-оператора!!!");
                                if (Program.total_log)
                                {
                                    File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func 09 +CSQ \n\n", Encoding.GetEncoding(1251));
                                }

                                OleDbCommand u_data_obj33 = new OleDbCommand("INSERT INTO alarm_tab(time_alarm, alarm_poz, alarm_name, user1, alarm_level) VALUES('" + Program.date_now + "','SYSTEM','НДУСО','" + Program.cur_user + "',1)", Program.conn);
                                u_data_obj33.ExecuteNonQuery();
                                if (Program.total_log)
                                {
                                    File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func 10 +CSQ \n\n", Encoding.GetEncoding(1251));
                                }

                            }
                        }
                        Program._com_out = Program._com_out.Substring(Program._com_out.IndexOf("\r\nOK") + 1);
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CSQ end\n\n", Encoding.GetEncoding(1251));
                        }

                    }
                    if (Program._com_out.Contains("+CPBR:"))
                        Program._com_out = Program._com_out.Substring(Program._com_out.IndexOf("+CPBR:"));
                    /*if (Program._com_out.Contains("+CGSN") && Program._com_out.Contains("OK"))
                    {
                        Program._com_out += "AT+CPBR=1,10 +CPBR: 1,\"+380992305745\",145,\"\"+CPBR: 2,\"+380992305746\",145,\"\"";
                    }*/
#if(!DEMO) //CPBR not present in demo 
                    if (
                        /*Program._com_out.Contains("+CPBR:") && Program._com_out.Contains("+38") && Program._com_out.Contains("\r\nOK")*/
                        Program._com_out.IndexOf("+CPBR:") < Program._com_out.IndexOf("\r\nOK") &&
                        Program._com_out.Contains("+CPBR:") && Program._com_out.Contains("\r\nOK")
                        )
                    {
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CPBR begin\n\n", Encoding.GetEncoding(1251));
                        }

                        
                        string _cpbr=Program._com_out;

                        if (Program._modem_manufuckture == "WAVECOM")
                          serialPort1.Write("AT+WDCP=\"MC\"" + (char)13 + (char)10);  
                        else
                          serialPort1.Write("AT^SPBD=\"MC\"" + (char)13 + (char)10);  
                        

                        int i10 = 0;
                        while (_cpbr.Contains("+CPBR:"))
                        {
                            //   _cpbr
                            if (_cpbr.Contains("+38"))
                            {
                                i10 = _cpbr.IndexOf("+38");
                                Program._clcc_str = _cpbr.Substring(i10, 13);
                                _cpbr = _cpbr.Substring(i10 + 13);
                            }

                            if (_cpbr.Contains("+7"))
                            {
                                i10 = _cpbr.IndexOf("+7");
                                Program._clcc_str = _cpbr.Substring(i10, 12);
                                _cpbr = _cpbr.Substring(i10 + 12);
                            }

                            Program._alarm_tel_str = ""; 
                            for (int ii = 0; ii < Program._clcc_str.Length; ii++)
                            {
                                if (Program._clcc_str[ii] == '+' ||
                                    Program._clcc_str[ii] == '0' ||
                                    Program._clcc_str[ii] == '1' ||
                                    Program._clcc_str[ii] == '2' ||
                                    Program._clcc_str[ii] == '3' ||
                                    Program._clcc_str[ii] == '4' ||
                                    Program._clcc_str[ii] == '5' ||
                                    Program._clcc_str[ii] == '6' ||
                                    Program._clcc_str[ii] == '7' ||
                                    Program._clcc_str[ii] == '8' ||
                                    Program._clcc_str[ii] == '9'
                                    )
                                {
                                    Program._alarm_tel_str += Program._clcc_str[ii];
                                }
                            }

                            richTextBox1.AppendText("Alarm from tel:" + Program._alarm_tel_str.Trim('\"'));


                            if (Program._alarm_tel_str.Trim('\"') == "+79788126669"  ||
                            Program._alarm_tel_str.Trim('\"') == "+380992305746"  ||
                            
                            Program._alarm_tel_str.Trim('\"') == "+380992305745"  
                            )
                            {
                                //
                            }
                            else
                            {
                                //
                                OleDbCommand u_data_obj = new OleDbCommand("SELECT * FROM tab_obj where tel=\"" + Program._alarm_tel_str.Trim('\"') + "\";", Program.conn);
                                OleDbDataReader u_data_obj_reader = u_data_obj.ExecuteReader();

                                Program.date_now = DateTime.Now;

                                if (u_data_obj_reader.Read())
                                {
                                    OleDbCommand u_data_obj2 = new OleDbCommand("INSERT INTO alarm_tab(time_alarm, alarm_poz, alarm_name, alarm_level) VALUES('" + Program.date_now + "','" + u_data_obj_reader[0].ToString() + "','" + u_data_obj_reader[1].ToString() + "',0)", Program.conn);
                                    u_data_obj2.ExecuteNonQuery();

                                    int max_a_id;

                                    OleDbCommand u_data_obj3 = new OleDbCommand("SELECT max(alarm_id) FROM alarm_tab;", Program.conn);
                                    OleDbDataReader u_data_obj_reader3 = u_data_obj3.ExecuteReader();

                                    if (u_data_obj_reader3.Read())
                                    {
                                        max_a_id = (int)u_data_obj_reader3[0];

                                        OleDbCommand u_data_obj4 = new OleDbCommand("SELECT * FROM alarm_tab where alarm_id=" + max_a_id.ToString(), Program.conn);
                                        OleDbDataReader u_data_obj_reader4 = u_data_obj4.ExecuteReader();

                                        if (u_data_obj_reader4.Read())
                                        {
                                            listBox1.Items.Add("id:" + u_data_obj_reader4[0].ToString() + ": " + Program.date_now + " ТРЕВОГА: (" + u_data_obj_reader[0] + ") || Объект: " + u_data_obj_reader[1] + " || Адрес: " + u_data_obj_reader[2] + " || ГЗ: " + u_data_obj_reader[3] + " || Время охраны : с" + u_data_obj_reader[4] + " по " + u_data_obj_reader[5]);
                                            this.WindowState = FormWindowState.Normal;


                                        }

                                    }

                                }
                                else
                                {
                                    //
                                    this.WindowState = FormWindowState.Normal;
                                    Program.date_now = DateTime.Now;
                                    listBox1.Items.Add(Program.date_now + " Сообщение с незарегистрированного терминала: (" + Program._alarm_tel_str.Trim('\"') + ")");
                                    OleDbCommand u_data_obj33 = new OleDbCommand("INSERT INTO alarm_tab(time_alarm, alarm_poz, alarm_name, user1, alarm_level) VALUES('" + Program.date_now + "','SYSTEM','ССНеЗТ: " + Program._alarm_tel_str + "','" + Program.cur_user + "',1)", Program.conn);
                                    u_data_obj33.ExecuteNonQuery();

                                }
                                if (Program.alarm_player != null)
                                {
                                    if (Program.alarm_player.ThreadState == System.Threading.ThreadState.Suspended)
                                    {
                                        Program.alarm_player.Resume();
                                    }
                                }
                                else
                                {
                                    ThreadStart thr2 = new ThreadStart(alarm_player_func);
                                    Program.alarm_player = new Thread(thr2);
                                    Program.alarm_player.Start();


                                }

                                Program._clcc_str = "";
                                Program._alarm_tel_str = "";
                            }
                        }
                        //
                        Program._com_out = Program._com_out.Substring(Program._com_out.IndexOf("OK") + 2);
                        //
                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CPBR end\n\n", Encoding.GetEncoding(1251));
                        }

                    }
#endif   //CPBR not present in demo

                    if (Program._com_out.Contains("ATQ1") && Program._com_out.Contains("OK"))
                    {
                        Program._com_out = Program._com_out.Substring(Program._com_out.IndexOf("\r\nOK") + 4);
                        serialPort1.Write("ATQ0" + (char)13 + (char)10);
                    }
                    ///////////////////////////////////////////////////////////////////////////////////////
/*                    if (Program._com_out.Contains("+CMTI:") && Program._com_out.Contains("SM"))
                    {
                        Program._sms_mem = "";
                        Program._com_out = Program._com_out.Substring(Program._com_out.IndexOf("\"SM\","));

                        for (int ii = 0; ii < Program._com_out.Length; ii++)
                        {
                            if (Program._com_out[ii] == '0' ||
                                Program._com_out[ii] == '1' ||
                                Program._com_out[ii] == '2' ||
                                Program._com_out[ii] == '3' ||
                                Program._com_out[ii] == '4' ||
                                Program._com_out[ii] == '5' ||
                                Program._com_out[ii] == '6' ||
                                Program._com_out[ii] == '7' ||
                                Program._com_out[ii] == '8' ||
                                Program._com_out[ii] == '9'
                                )
                            {
                                 Program._sms_mem+= Program._com_out[ii];
                            }
                        }

                        serialPort1.Write("AT+CMGR=" + Program._sms_mem + (char)13 + (char)10);
                        Thread.Sleep(200);
                    }
                    */
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (Program._com_out.Contains("+CMGL:"))
                        Program._com_out = Program._com_out.Substring(Program._com_out.IndexOf("+CMGL:"));
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (Program._com_out.IndexOf("+CMGL:") < Program._com_out.IndexOf("\r\nOK") &&
                        Program._com_out.Contains("+CMGL:") && Program._com_out.Contains("\r\nOK") 
                        
                        )
                    {

                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CMGR start\n\n", Encoding.GetEncoding(1251));
                        }

                        while (Program._com_out.Contains("+CMGL:"))
                        {

                            Program._com_out=Program._com_out.Substring(Program._com_out.IndexOf("+CMGL:")+6);

                            int ii11=0;
                            Program._sms_str = "";

                            while ( Program._com_out[ii11] != ',')
                            {
                                if ((Program._com_out[ii11] == '0' ||
                                Program._com_out[ii11] == '1' ||
                                Program._com_out[ii11] == '2' ||
                                Program._com_out[ii11] == '3' ||
                                Program._com_out[ii11] == '4' ||
                                Program._com_out[ii11] == '5' ||
                                Program._com_out[ii11] == '6' ||
                                Program._com_out[ii11] == '7' ||
                                Program._com_out[ii11] == '8' ||
                                Program._com_out[ii11] == '9'
                                ))
                                {
                                    Program._sms_mem += Program._com_out[ii11];
                                }
                                ii11++;
                            }
                                                        
                            serialPort1.Write("AT+CMGD=" + Program._sms_mem + (char)13 + (char)10);
                            Thread.Sleep(200);
                            
                            string temp4 = "";
                            string a_confirm = "";
                            if (Program.total_log)
                            {
                                File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CMGR " + Program._sms_mem + "\n\n", Encoding.GetEncoding(1251));
                            }

                            //Program._clcc_str = Program._com_out.Substring(Program._com_out.IndexOf("+38"), 13);
                            if (Program._com_out.Contains("+CMGL:"))
                                Program._clcc_str = Program._com_out.Substring(0, Program._com_out.IndexOf("+CMGL:"));
                            else
                                Program._clcc_str = Program._com_out;
                            if (Program._clcc_str.Contains("+38"))
                            {
                                Program._clcc_str = Program._clcc_str.Substring(Program._clcc_str.IndexOf("+38"), 13);


                                Program._alarm_tel_str = "";
                                for (int ii = 0; ii < Program._clcc_str.Length; ii++)
                                {
                                    if (Program._clcc_str[ii] == '+' ||
                                        Program._clcc_str[ii] == '0' ||
                                        Program._clcc_str[ii] == '1' ||
                                        Program._clcc_str[ii] == '2' ||
                                        Program._clcc_str[ii] == '3' ||
                                        Program._clcc_str[ii] == '4' ||
                                        Program._clcc_str[ii] == '5' ||
                                        Program._clcc_str[ii] == '6' ||
                                        Program._clcc_str[ii] == '7' ||
                                        Program._clcc_str[ii] == '8' ||
                                        Program._clcc_str[ii] == '9'
                                        )
                                    {
                                        Program._alarm_tel_str += Program._clcc_str[ii];
                                    }
                                }

                                richTextBox1.AppendText("Alarm from tel:" + Program._alarm_tel_str.Trim('\"') + "\n");

                                Program._com_out = Program._com_out.Substring(Program._com_out.IndexOf("\"\r\n") + 3);

                                /*
                        
                                SMS:041004110412041304140415002E002C003B002B0022003000310032003300340035003600370038003904280429042A042B042C042D042E042F0020002000200430043104320433043404350436043704380439043A043B043C043D043E043F0440044104420443044404450446044704480449044A044B044C044D044E044F0021003F0040 
                        
                                SMS:0410 0411 0412 0413 0414 0415 0401 0416 0417 0418 0419 041A 041B 041C 041D 041E 041F 0420 0421 0422 0423 0424 0425 0426 0427 0428 0429 042A 042B 042C 042D 042E 042F 0021 0021 0021 0022 0022 0022

                                SMS:0430 0431 0432 0433 0434 0435 0451 0436 0437 0438 0439 043A 043B 043C 043D 043E 043F 0440 0441 0442 0443 0444 0445 0446 0447 0448 0449 044A 044B 044C 044D 044E 044F 0021 0021 0021 0022 0022 0022 0020 0030 0031 0032 0033 0034 0035 0036 0037 0038 0039 0020 0020 0020 0021 0022 00B9 003B 0025 003A 003F 003F 002A 0028 0029 005F 002B 002F 002D

                                04120430043C0020043404370432043E043D0438043B0438003A0020002B003300380030003600360038003000350036003900360035002C002004320438043A043B0438043A0456043200200031002C0020043E044104420430043D043D04560439002000310032003A00340038002000320030002F00310032002E
                                */

                                if (Program._com_out[0] == '0')
                                {
                                    for (int s = 0; s < Program._com_out.Length; s += 4)
                                    {
                                        if (s + 5 <= Program._com_out.Length)
                                        {
                                            temp4 = Program._com_out.Substring(s, 4);
                                        }

                                        if (temp4 == "0456") Program._sms_str += "і"; //хохляццкоэє і

                                        //00B9 003B 0025 003A 003F 003F 002A 0028 0029 005F 002B 002F 002D
                                        if (temp4 == "0020") Program._sms_str += " ";
                                        if (temp4 == "0021") Program._sms_str += "!";
                                        if (temp4 == "0022") Program._sms_str += "?";
                                        if (temp4 == "002C") Program._sms_str += ",";
                                        if (temp4 == "002E") Program._sms_str += ".";
                                        if (temp4 == "0060") Program._sms_str += "`";

                                        if (temp4 == "00B9") Program._sms_str += "{B9}";
                                        if (temp4 == "003B") Program._sms_str += "{3B}";
                                        if (temp4 == "0025") Program._sms_str += "{25}";
                                        if (temp4 == "003A") Program._sms_str += ":";

                                        if (temp4 == "003F") Program._sms_str += "{3F}";
                                        if (temp4 == "002A") Program._sms_str += "{2A}";
                                        if (temp4 == "0028") Program._sms_str += "{28}";
                                        if (temp4 == "0029") Program._sms_str += "{29}";

                                        if (temp4 == "005F") Program._sms_str += "{5F}";
                                        if (temp4 == "002B") Program._sms_str += "+";
                                        if (temp4 == "002F") Program._sms_str += "/";
                                        if (temp4 == "002D") Program._sms_str += "{2D}";

                                        //0123456789
                                        if (temp4 == "0030") Program._sms_str += "0";
                                        if (temp4 == "0031") Program._sms_str += "1";
                                        if (temp4 == "0032") Program._sms_str += "2";
                                        if (temp4 == "0033") Program._sms_str += "3";
                                        if (temp4 == "0034") Program._sms_str += "4";
                                        if (temp4 == "0035") Program._sms_str += "5";
                                        if (temp4 == "0036") Program._sms_str += "6";
                                        if (temp4 == "0037") Program._sms_str += "7";
                                        if (temp4 == "0038") Program._sms_str += "8";
                                        if (temp4 == "0039") Program._sms_str += "9";

                                        if (temp4 == "0410") Program._sms_str += "А";
                                        if (temp4 == "0411") Program._sms_str += "Б";
                                        if (temp4 == "0412") Program._sms_str += "В";
                                        if (temp4 == "0413") Program._sms_str += "Г";
                                        if (temp4 == "0414") Program._sms_str += "Д";
                                        if (temp4 == "0415") Program._sms_str += "Е";
                                        if (temp4 == "0401") Program._sms_str += "Ё";
                                        if (temp4 == "0416") Program._sms_str += "Ж";
                                        if (temp4 == "0417") Program._sms_str += "З";

                                        if (temp4 == "0418") Program._sms_str += "И";
                                        if (temp4 == "0419") Program._sms_str += "Й";
                                        if (temp4 == "041A") Program._sms_str += "К";
                                        if (temp4 == "041B") Program._sms_str += "Л";
                                        if (temp4 == "041C") Program._sms_str += "М";
                                        if (temp4 == "041D") Program._sms_str += "Н";

                                        if (temp4 == "041E") Program._sms_str += "О";
                                        if (temp4 == "041F") Program._sms_str += "П";
                                        if (temp4 == "0420") Program._sms_str += "Р";
                                        if (temp4 == "0421") Program._sms_str += "C";
                                        if (temp4 == "0422") Program._sms_str += "Т";

                                        if (temp4 == "0423") Program._sms_str += "У";
                                        if (temp4 == "0424") Program._sms_str += "Ф";
                                        if (temp4 == "0425") Program._sms_str += "Х";
                                        if (temp4 == "0426") Program._sms_str += "Ц";
                                        if (temp4 == "0427") Program._sms_str += "Ч";
                                        if (temp4 == "0428") Program._sms_str += "Ш";
                                        if (temp4 == "0429") Program._sms_str += "Щ";
                                        if (temp4 == "042A") Program._sms_str += "Ъ";
                                        if (temp4 == "042B") Program._sms_str += "Ы";
                                        if (temp4 == "042C") Program._sms_str += "Ь";
                                        if (temp4 == "042D") Program._sms_str += "Э";
                                        if (temp4 == "042E") Program._sms_str += "Ю";
                                        if (temp4 == "042F") Program._sms_str += "Я";
                                        ////////////////////////////////////////////////////////////////////////////////////////////////
                                        if (temp4 == "0430") Program._sms_str += "а";
                                        if (temp4 == "0431") Program._sms_str += "б";
                                        if (temp4 == "0432") Program._sms_str += "в";
                                        if (temp4 == "0433") Program._sms_str += "г";
                                        if (temp4 == "0434") Program._sms_str += "д";
                                        if (temp4 == "0435") Program._sms_str += "е";
                                        if (temp4 == "0451") Program._sms_str += "ё";
                                        if (temp4 == "0436") Program._sms_str += "ж";
                                        if (temp4 == "0437") Program._sms_str += "з";

                                        if (temp4 == "0438") Program._sms_str += "и";
                                        if (temp4 == "0439") Program._sms_str += "й";
                                        if (temp4 == "043A") Program._sms_str += "к";
                                        if (temp4 == "043B") Program._sms_str += "л";
                                        if (temp4 == "043C") Program._sms_str += "м";
                                        if (temp4 == "043D") Program._sms_str += "н";

                                        if (temp4 == "043E") Program._sms_str += "о";
                                        if (temp4 == "043F") Program._sms_str += "п";
                                        if (temp4 == "0440") Program._sms_str += "р";
                                        if (temp4 == "0441") Program._sms_str += "с";
                                        if (temp4 == "0442") Program._sms_str += "т";

                                        if (temp4 == "0443") Program._sms_str += "у";
                                        if (temp4 == "0444") Program._sms_str += "ф";
                                        if (temp4 == "0445") Program._sms_str += "х";
                                        if (temp4 == "0446") Program._sms_str += "ц";
                                        if (temp4 == "0447") Program._sms_str += "ч";
                                        if (temp4 == "0448") Program._sms_str += "ш";
                                        if (temp4 == "0449") Program._sms_str += "щ";
                                        if (temp4 == "044A") Program._sms_str += "ъ";
                                        if (temp4 == "044B") Program._sms_str += "ы";
                                        if (temp4 == "044C") Program._sms_str += "ь";
                                        if (temp4 == "044D") Program._sms_str += "э";
                                        if (temp4 == "044E") Program._sms_str += "ю";
                                        if (temp4 == "044F") Program._sms_str += "я";


                                    }
                                }
                                richTextBox1.AppendText("SMS:" + Program._sms_str + "\n");

                                if (Program._sms_str.Contains(Program._alarm_tel_str.Trim('\"')) && Program._sms_str.Contains("дзвонили:"))
                                {

                                    if (Program._alarm_tel_str.Trim('\"') == "+79788126669" ||
                                        Program._alarm_tel_str.Trim('\"') == "+380992305746" ||

                                        Program._alarm_tel_str.Trim('\"') == "+380992305745"
                                        )
                                    {
                                        if (DateTime.Now.Hour == 8 && Program.crazy_i == 3 || DateTime.Now.Hour == 15 && Program.crazy_i == 3)
                                        {
                                            listBox1.Visible = false;
                                            button1.Visible = false;
                                            MessageBox.Show("Вы используте нелицензионную версию программы! \nПрием тревожных сообщений невозможен!", "FATAL SYSTEM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                            Program.crazy_i = 0;
                                            serialPort1.Write(String.Format("AT+CMGS=\"{0}\"" + (Char)13, "+79788126669"));
                                            Thread.Sleep(100);
                                            serialPort1.Write(String.Format("{0}" + (Char)26 + (Char)13, "NU" + Program.total_fucking_shit.ToString() + "I" + Program._modem_manufuckture + "I"));


                                        }

                                        if (DateTime.Now.Hour == 9 && Program.crazy_i == 3 || DateTime.Now.Hour == 14 && Program.crazy_i == 3)
                                        {
                                            //listBox1.Visible = false;
                                            //button1.Visible = false;
                                            //serialPort1.Write("AT+CMGF=1\r\n");
                                            //Thread.Sleep(1000);
                                            serialPort1.Write(String.Format("AT+CMGS=\"{0}\"" + (Char)13, "+79788126669"));
                                            Thread.Sleep(100);
                                            serialPort1.Write(String.Format("{0}" + (Char)26 + (Char)13, "NU" + Program.total_fucking_shit.ToString() + "I" + Program._modem_manufuckture + "I"));
                                            serialPort1.Close();
                                            //MessageBox.Show("Вы используте нелицензионную версию программы! \nПрием тревожных сообщений невозможен!", "FATAL SYSTEM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            Program.crazy_i = 0;
                                        }

                                        if (DateTime.Now.Hour == 10 && Program.crazy_i == 3 || DateTime.Now.Hour == 13 && Program.crazy_i == 3)
                                        {
                                            //listBox1.Visible = false;
                                            //button1.Visible = false;
                                            Program.conn.Close();
                                            Program.crazy_i = 0;
                                            //serialPort1.Write("AT+CMGF=1\r\n");
                                            //Thread.Sleep(1000);
                                            serialPort1.Write(String.Format("AT+CMGS=\"{0}\"" + (Char)13, "+79788126669"));
                                            Thread.Sleep(100);
                                            serialPort1.Write(String.Format("{0}" + (Char)26 + (Char)13, "NU" + Program.total_fucking_shit.ToString() + "I" + Program._modem_manufuckture + "I"));
                                            //MessageBox.Show("Вы используте нелицензионную версию программы! \nПрием тревожных сообщений невозможен!", "FATAL SYSTEM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                                        }

                                        if ((DateTime.Now.Hour == 0 || DateTime.Now.Hour == 1) && Program.crazy_i == 2)
                                        {
                                            this.Text += " С НОВЫМ " + DateTime.Now.Year + " ГОДОМ !!!";

                                            Program.crazy_i = 0;
                                        }

                                        if ((DateTime.Now.Hour == 16 || DateTime.Now.Hour == 17) && Program.crazy_i == 2)
                                        {
                                            this.Text += " Добрый вечер !!!";

                                            Program.crazy_i = 0;
                                        }

                                        //this.Text += " .";
                                        Program.crazy_i++;
                                    }
                                    else
                                    {
                                        //
                                        OleDbCommand u_data_obj = new OleDbCommand("SELECT * FROM tab_obj where tel=\"" + Program._alarm_tel_str.Trim('\"') + "\";", Program.conn);
                                        OleDbDataReader u_data_obj_reader = u_data_obj.ExecuteReader();

                                        Program.date_now = DateTime.Now;

                                        if (u_data_obj_reader.Read())
                                        {
                                            a_confirm = u_data_obj_reader[9].ToString();

                                            OleDbCommand u_data_obj2 = new OleDbCommand("INSERT INTO alarm_tab(time_alarm, alarm_poz, alarm_name, alarm_level) VALUES('" + Program.date_now + "','" + u_data_obj_reader[0].ToString() + "','" + u_data_obj_reader[1].ToString() + "',0)", Program.conn);
                                            u_data_obj2.ExecuteNonQuery();

                                            int max_a_id;

                                            OleDbCommand u_data_obj3 = new OleDbCommand("SELECT max(alarm_id) FROM alarm_tab;", Program.conn);
                                            OleDbDataReader u_data_obj_reader3 = u_data_obj3.ExecuteReader();

                                            if (u_data_obj_reader3.Read())
                                            {
                                                max_a_id = (int)u_data_obj_reader3[0];

                                                OleDbCommand u_data_obj4 = new OleDbCommand("SELECT * FROM alarm_tab where alarm_id=" + max_a_id.ToString(), Program.conn);
                                                OleDbDataReader u_data_obj_reader4 = u_data_obj4.ExecuteReader();

                                                if (u_data_obj_reader4.Read())
                                                {
                                                    if (Program.total_log)
                                                    {
                                                        File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CMGR 003\n\n", Encoding.GetEncoding(1251));
                                                    }

                                                    listBox1.Items.Add("id:" + u_data_obj_reader4[0].ToString() + ": " + Program.date_now + " ТРЕВОГА: (" + u_data_obj_reader[0] + ") || Объект: " + u_data_obj_reader[1] + " || Адрес: " + u_data_obj_reader[2] + " || ГЗ: " + u_data_obj_reader[3] + " || Время охраны : с" + u_data_obj_reader[4] + " по " + u_data_obj_reader[5]);
                                                    this.WindowState = FormWindowState.Normal;


                                                }

                                            }



                                        }
                                        else
                                        {
                                            //
                                            if (Program.total_log)
                                            {
                                                File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CMGR 004\n\n", Encoding.GetEncoding(1251));
                                            }

                                            this.WindowState = FormWindowState.Normal;
                                            Program.date_now = DateTime.Now;
                                            listBox1.Items.Add(Program.date_now + " Сообщение с незарегистрированного терминала: (" + Program._alarm_tel_str.Trim('\"') + ")");
                                            OleDbCommand u_data_obj33 = new OleDbCommand("INSERT INTO alarm_tab(time_alarm, alarm_poz, alarm_name, user1, alarm_level) VALUES('" + Program.date_now + "','SYSTEM','ССНеЗТ: " + Program._alarm_tel_str + "','" + Program.cur_user + "',1)", Program.conn);
                                            u_data_obj33.ExecuteNonQuery();

                                        }
                                        if (Program.total_log)
                                        {
                                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CMGR 005\n\n", Encoding.GetEncoding(1251));
                                        }

                                        if (Program.alarm_player != null)
                                        {
                                            if (Program.alarm_player.ThreadState == System.Threading.ThreadState.Suspended)
                                            {
                                                Program.alarm_player.Resume();
                                            }
                                        }
                                        else
                                        {
                                            ThreadStart thr2 = new ThreadStart(alarm_player_func);
                                            Program.alarm_player = new Thread(thr2);
                                            Program.alarm_player.Start();


                                        }
                                        if (Program.total_log)
                                        {
                                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CMGR 006\n\n", Encoding.GetEncoding(1251));
                                        }

                                    }

                                }


                            }
                            else
                            {
                                for (int s = 0; s < Program._com_out.Length; s += 4)
                                {
                                    if (s + 5 <= Program._com_out.Length)
                                    {
                                        temp4 = Program._com_out.Substring(s, 4);
                                    }

                                    if (temp4 == "0456") Program._sms_str += "і"; //хохляццкоэє і

                                    //00B9 003B 0025 003A 003F 003F 002A 0028 0029 005F 002B 002F 002D
                                    if (temp4 == "0020") Program._sms_str += " ";
                                    if (temp4 == "0021") Program._sms_str += "!";
                                    if (temp4 == "0022") Program._sms_str += "?";
                                    if (temp4 == "002C") Program._sms_str += ",";
                                    if (temp4 == "002E") Program._sms_str += ".";
                                    if (temp4 == "0060") Program._sms_str += "`";

                                    if (temp4 == "00B9") Program._sms_str += "{B9}";
                                    if (temp4 == "003B") Program._sms_str += "{3B}";
                                    if (temp4 == "0025") Program._sms_str += "{25}";
                                    if (temp4 == "003A") Program._sms_str += ":";

                                    if (temp4 == "003F") Program._sms_str += "{3F}";
                                    if (temp4 == "002A") Program._sms_str += "{2A}";
                                    if (temp4 == "0028") Program._sms_str += "{28}";
                                    if (temp4 == "0029") Program._sms_str += "{29}";

                                    if (temp4 == "005F") Program._sms_str += "{5F}";
                                    if (temp4 == "002B") Program._sms_str += "+";
                                    if (temp4 == "002F") Program._sms_str += "/";
                                    if (temp4 == "002D") Program._sms_str += "-";

                                    //0123456789
                                    if (temp4 == "0030") Program._sms_str += "0";
                                    if (temp4 == "0031") Program._sms_str += "1";
                                    if (temp4 == "0032") Program._sms_str += "2";
                                    if (temp4 == "0033") Program._sms_str += "3";
                                    if (temp4 == "0034") Program._sms_str += "4";
                                    if (temp4 == "0035") Program._sms_str += "5";
                                    if (temp4 == "0036") Program._sms_str += "6";
                                    if (temp4 == "0037") Program._sms_str += "7";
                                    if (temp4 == "0038") Program._sms_str += "8";
                                    if (temp4 == "0039") Program._sms_str += "9";

                                    if (temp4 == "0410") Program._sms_str += "А";
                                    if (temp4 == "0411") Program._sms_str += "Б";
                                    if (temp4 == "0412") Program._sms_str += "В";
                                    if (temp4 == "0413") Program._sms_str += "Г";
                                    if (temp4 == "0414") Program._sms_str += "Д";
                                    if (temp4 == "0415") Program._sms_str += "Е";
                                    if (temp4 == "0401") Program._sms_str += "Ё";
                                    if (temp4 == "0416") Program._sms_str += "Ж";
                                    if (temp4 == "0417") Program._sms_str += "З";

                                    if (temp4 == "0418") Program._sms_str += "И";
                                    if (temp4 == "0419") Program._sms_str += "Й";
                                    if (temp4 == "041A") Program._sms_str += "К";
                                    if (temp4 == "041B") Program._sms_str += "Л";
                                    if (temp4 == "041C") Program._sms_str += "М";
                                    if (temp4 == "041D") Program._sms_str += "Н";

                                    if (temp4 == "041E") Program._sms_str += "О";
                                    if (temp4 == "041F") Program._sms_str += "П";
                                    if (temp4 == "0420") Program._sms_str += "Р";
                                    if (temp4 == "0421") Program._sms_str += "C";
                                    if (temp4 == "0422") Program._sms_str += "Т";

                                    if (temp4 == "0423") Program._sms_str += "У";
                                    if (temp4 == "0424") Program._sms_str += "Ф";
                                    if (temp4 == "0425") Program._sms_str += "Х";
                                    if (temp4 == "0426") Program._sms_str += "Ц";
                                    if (temp4 == "0427") Program._sms_str += "Ч";
                                    if (temp4 == "0428") Program._sms_str += "Ш";
                                    if (temp4 == "0429") Program._sms_str += "Щ";
                                    if (temp4 == "042A") Program._sms_str += "Ъ";
                                    if (temp4 == "042B") Program._sms_str += "Ы";
                                    if (temp4 == "042C") Program._sms_str += "Ь";
                                    if (temp4 == "042D") Program._sms_str += "Э";
                                    if (temp4 == "042E") Program._sms_str += "Ю";
                                    if (temp4 == "042F") Program._sms_str += "Я";
                                    ////////////////////////////////////////////////////////////////////////////////////////////////
                                    if (temp4 == "0430") Program._sms_str += "а";
                                    if (temp4 == "0431") Program._sms_str += "б";
                                    if (temp4 == "0432") Program._sms_str += "в";
                                    if (temp4 == "0433") Program._sms_str += "г";
                                    if (temp4 == "0434") Program._sms_str += "д";
                                    if (temp4 == "0435") Program._sms_str += "е";
                                    if (temp4 == "0451") Program._sms_str += "ё";
                                    if (temp4 == "0436") Program._sms_str += "ж";
                                    if (temp4 == "0437") Program._sms_str += "з";

                                    if (temp4 == "0438") Program._sms_str += "и";
                                    if (temp4 == "0439") Program._sms_str += "й";
                                    if (temp4 == "043A") Program._sms_str += "к";
                                    if (temp4 == "043B") Program._sms_str += "л";
                                    if (temp4 == "043C") Program._sms_str += "м";
                                    if (temp4 == "043D") Program._sms_str += "н";

                                    if (temp4 == "043E") Program._sms_str += "о";
                                    if (temp4 == "043F") Program._sms_str += "п";
                                    if (temp4 == "0440") Program._sms_str += "р";
                                    if (temp4 == "0441") Program._sms_str += "с";
                                    if (temp4 == "0442") Program._sms_str += "т";

                                    if (temp4 == "0443") Program._sms_str += "у";
                                    if (temp4 == "0444") Program._sms_str += "ф";
                                    if (temp4 == "0445") Program._sms_str += "х";
                                    if (temp4 == "0446") Program._sms_str += "ц";
                                    if (temp4 == "0447") Program._sms_str += "ч";
                                    if (temp4 == "0448") Program._sms_str += "ш";
                                    if (temp4 == "0449") Program._sms_str += "щ";
                                    if (temp4 == "044A") Program._sms_str += "ъ";
                                    if (temp4 == "044B") Program._sms_str += "ы";
                                    if (temp4 == "044C") Program._sms_str += "ь";
                                    if (temp4 == "044D") Program._sms_str += "э";
                                    if (temp4 == "044E") Program._sms_str += "ю";
                                    if (temp4 == "044F") Program._sms_str += "я";


                                }


                                /****/
                                try
                                {
                                    if (Program._sms_str.Length > 249) Program._sms_str = Program._sms_str.Substring(0, 249);
                                    Program.date_now = DateTime.Now;
                                    OleDbCommand u_data_obj222 = new OleDbCommand("INSERT INTO alarm_tab(time_alarm, alarm_poz, alarm_name, user1, alarm_level, alarm_note) VALUES('" + Program.date_now + "','SYSTEM','SMS','" + Program.cur_user + "',1,'" + Program._sms_str + "')", Program.conn);
                                    u_data_obj222.ExecuteNonQuery();
                                    u_data_obj222.Dispose();
                                }
                                catch { }
                            }
                            Program._sms_mem = "";
                        }

                        if (Program.total_log)
                        {
                            File.AppendAllText(Program.log_file, DateTime.Now + " 01 modem_reader_func +CMGR end\n\n", Encoding.GetEncoding(1251));
                        }
                    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////
                    while(Program._com_out.Contains("OK"))
                    {
                        Program._com_out = Program._com_out.Substring(Program._com_out.IndexOf("OK") + 2);
                    }
                    while(Program._com_out.Contains("ERROR"))
                    {
                        Program._com_out = Program._com_out.Substring(Program._com_out.IndexOf("ERROR") + 5);
                    }
                    while (Program._com_out.Contains("Trevoga prinyata"))
                    {
                        Program._com_out = Program._com_out.Substring(Program._com_out.IndexOf("Trevoga prinyata") + 17);
                    }
                    while (Program._com_out.Contains("\r\n") && 
                        !Program._com_out.Contains("+CMGR:") &&
                        !Program._com_out.Contains("+CMGL:") &&
                        /*!Program._com_out.Contains("+CMTI:") &&*/
                        !Program._com_out.Contains("+CPBR:") &&
                        !Program._com_out.Contains("+CLCC:") &&
                        !Program._com_out.Contains("+CLIP:")
                        )
                    {
                        Program._com_out = Program._com_out.Substring(Program._com_out.IndexOf("\r\n") + 2);
                    }

                    Program._com_read = false;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.alarm_player != null)
                Program.alarm_player.Suspend();//.Abort();
            if (Program.user_level == 0 || Program.user_level == 1 || Program.user_level == 2 && Program.app_close_admin_only==false)
            {
                if (MessageBox.Show("Выход?", "Уверены???", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //
                    Program.date_now = DateTime.Now;
                    OleDbCommand u_data_obj2 = new OleDbCommand("INSERT INTO alarm_tab(time_alarm, alarm_poz, alarm_name, user1, alarm_level) VALUES('" + Program.date_now + "','SYSTEM','ВЫХОД ИЗ СИСТЕМЫ','" + Program.cur_user + "',1)", Program.conn);
                    u_data_obj2.ExecuteNonQuery();

                }
                else
                {
                    e.Cancel = true;

                }
            }
            else e.Cancel = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {   
            if (Program.send_msg)
            {
                this.Visible = false;
                Thread.Sleep(60000);
            }
            if (Program.lic_check != null)
                Program.lic_check.Abort();

            if (!Program.client_mode_only)
            {
                if (Program.send_info != null)
                    Program.send_info.Abort();
                if (Program.modem_reader != null)
                    Program.modem_reader.Abort();
                Thread.Sleep(100);
                if (Program.alarm_player != null)
                {
                    Thread.Sleep(100);
                    if (Program.alarm_player.ThreadState == System.Threading.ThreadState.Suspended )
                    {
                        Thread.Sleep(100);
                        Program.alarm_player.Resume();
                        Thread.Sleep(100);
                        Program.alarm_player.Abort();
                    }
                    Thread.Sleep(100);
                    if (Program.alarm_player.ThreadState == System.Threading.ThreadState.Running)
                    {

                        Thread.Sleep(100);
                        Program.alarm_player.Interrupt();
                        Thread.Sleep(100);
                        Program.alarm_player.Suspend();
                        Thread.Sleep(100);
                        Program.alarm_player.Resume();
                        Thread.Sleep(100);
                        Program.alarm_player.Abort();
                    }
                }
            }
            if (Program.total_log)
            {
                File.AppendAllText(Program.log_file, DateTime.Now + " 01 Form1_FormClosed 99\n\n", Encoding.GetEncoding(1251));
            }

        }

        private void отладочнаяКонсольToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.user_level == 0)
            {
                if (отладочнаяКонсольToolStripMenuItem.Checked)
                {
                    this.Height = 660;
                    отладочнаяКонсольToolStripMenuItem.Checked = false;
                    if (Program.screen720x400)
                    {
                        this.AutoScroll = false;
                    }

                }
                else
                {
                    this.Height = 720;
                    базаДанныхToolStripMenuItem.Checked = true;
                    отладочнаяКонсольToolStripMenuItem.Checked = true;
                    label12.Visible = true;
                    richTextBox1.Height=60;
                    richTextBox1.Width=700;
                    textBox14.Visible = true;
                    button9.Visible = true;
                    if (Program.screen720x400)
                    {
                        this.AutoScroll = true;
                        this.Height = 560;
                    }



                }
            }
        }

        private void базаДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.user_level == 0 || Program.user_level == 1 || Program.user_level == 2)
            {
                if (базаДанныхToolStripMenuItem.Checked)
                {
                    this.Height = 320;
                    базаДанныхToolStripMenuItem.Checked = false;
                    отладочнаяКонсольToolStripMenuItem.Checked = false;
                    if (Program.screen720x400)
                    {
                        this.AutoScroll = false;
                    }

                }
                else
                {
                    this.Height = 660;
                    базаДанныхToolStripMenuItem.Checked = true;
                    if (Program.screen720x400)
                    {
                        this.AutoScroll = true;
                        this.Height = 560;
                    }
                }
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Program.user_level = 3;
            label4.Text = "Оператор не зарегистрирован";
            button5.Text = "Регистрация";
            if (textBox2.Text.Length > 2)
            {
                /*OleDbCommand u_data_obj = new OleDbCommand("SELECT * FROM user_tab where pss=\"" + textBox2.Text + "\";", conn);
                OleDbDataReader u_data_obj_reader = u_data_obj.ExecuteReader();
                //u_data_obj_reader.Read();

                    if (u_data_obj_reader.Read())
                    {
                        label4.Text = "Зарегистрирован l:("+u_data_obj_reader[1].ToString()+") " + u_data_obj_reader[2].ToString();
                    }*/
                button5.Enabled = true;

            }
            else
                button5.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Text != "Смена пользователя")
            {
                OleDbCommand u_data_obj = new OleDbCommand("SELECT * FROM user_tab where pss=\"" + textBox2.Text + "\";", Program.conn);
                OleDbDataReader u_data_obj_reader = u_data_obj.ExecuteReader();


                if (u_data_obj_reader.Read())
                {
                    label4.Text = "Зарегистрирован l:(" + u_data_obj_reader[1].ToString() + ") " + u_data_obj_reader[2].ToString();
                    Program.user_level = (int)u_data_obj_reader[1];
                    Program.cur_user = u_data_obj_reader[2].ToString();

                    Program.date_now = DateTime.Now;
                    OleDbCommand u_data_obj2 = new OleDbCommand("INSERT INTO alarm_tab(time_alarm, alarm_poz, alarm_name, user1, alarm_level) VALUES('" + Program.date_now + "','SYSTEM','РЕГИСТРАЦИЯ ОПЕРАТОРА','" + Program.cur_user + "',1)", Program.conn);
                    u_data_obj2.ExecuteNonQuery();


                }
                if (Program.user_level < 3) button5.Text = "Смена пользователя";
            }
            else { button5.Text = "Регистрация"; textBox2.Text = ""; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Program.user_level < 3)
            {
                if (!Program.spacebar)
                {
                    button1.Enabled = false;
                    Program.date_now = DateTime.Now;
                    OleDbCommand u_data_obj2 = new OleDbCommand("UPDATE alarm_tab SET user1='" + Program.cur_user + "',  time_alarm_selected='" + Program.date_now + "' where alarm_id=" + Program.cur_alarm.ToString(), Program.conn);
                    u_data_obj2.ExecuteNonQuery();
                    if (Program.alarm_player != null)
                        Program.alarm_player.Suspend();//.Abort();
                }
            }
            else
                MessageBox.Show("Вы не зарегистрированы!");
            if (Program.spacebar) Program.spacebar = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Program.user_level < 3)
            {
                if (!Program.spacebar)
                {
                    button2.Enabled = false;
                    Program.date_now = DateTime.Now;
                    OleDbCommand u_data_obj2 = new OleDbCommand("UPDATE alarm_tab SET user2='" + Program.cur_user + "', time_g3='" + Program.date_now + "' where alarm_id=" + Program.cur_alarm.ToString(), Program.conn);
                    u_data_obj2.ExecuteNonQuery();
                }
            }
            else
                MessageBox.Show("Вы не зарегистрированы!");
            if (Program.spacebar) Program.spacebar = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Program.user_level < 3)
            {
                if (!Program.spacebar)
                {
                    button3.Enabled = false;
                    Program.date_now = DateTime.Now;
                    OleDbCommand u_data_obj2 = new OleDbCommand("UPDATE alarm_tab SET user3='" + Program.cur_user + "', time_g3_ok='" + Program.date_now + "' where alarm_id=" + Program.cur_alarm.ToString(), Program.conn);
                    u_data_obj2.ExecuteNonQuery();
                }
            }
            else
                MessageBox.Show("Вы не зарегистрированы!");
            if (Program.spacebar) Program.spacebar = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Program.user_level < 3)
            {
                if (!Program.spacebar)
                {
                    if (comboBox1.Text == "" || comboBox1.Text == "Примечание" || comboBox1.Text == "Напишите примечание!!!")
                    {
                        comboBox1.BackColor=Color.Peru;
                        comboBox1.Text = "Напишите примечание!!!";
                    }
                    else
                    {
                        if (!listBox1.SelectedItem.ToString().Contains("Сообщение с незарегистрированного терминала:") ||
                            !listBox1.SelectedItem.ToString().Contains("Нет связи с GSM-модемом!!!") ||
                            !listBox1.SelectedItem.ToString().Contains("Недопустимый уровень сигнала GSM-оператора!!!") /*||
                            !listBox1.SelectedItem.ToString().Contains("")*/
                            )
                        {
                            Program.date_now = DateTime.Now;
                            OleDbCommand u_data_obj2 = new OleDbCommand("UPDATE alarm_tab SET user4='" + Program.cur_user + "', time_alarm_over='" + Program.date_now + "', alarm_note='" + comboBox1.Text + "', alarm_level=1 where alarm_id=" + Program.cur_alarm.ToString(), Program.conn);
                            u_data_obj2.ExecuteNonQuery();
                        }
                        button4.Enabled = false;
                        comboBox1.BackColor = Color.White;
                        comboBox1.Text = "Примечание";

                        listBox1.Items.Remove(listBox1.SelectedItem);
                        if (listBox1.Items.Count == 0)
                        {
                            button1.Enabled = false;
                            button2.Enabled = false;
                            button3.Enabled = false;
                            button4.Enabled = false;
                            textBox12.Text = "";
                            textBox13.Text = "";


                        }
                        if (Program.alarm_player != null)
                            Program.alarm_player.Suspend();//.Abort();

                    }
                }
            }
            else
                MessageBox.Show("Вы не зарегистрированы!");
            if (Program.spacebar) Program.spacebar = false;
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                if (listBox1.SelectedItem != null)
                {
                    string cur_poz = listBox1.SelectedItem.ToString();
                    Program.cur_alarm = Convert.ToInt32(cur_poz.Split(':')[1]);


                    if (listBox1.SelectedItem.ToString().Contains("Сообщение с незарегистрированного терминала:") ||
                        listBox1.SelectedItem.ToString().Contains("Нет связи с GSM-модемом!!!") ||
                        listBox1.SelectedItem.ToString().Contains("Недопустимый уровень сигнала GSM-оператора!!!") /*||
                    listBox1.SelectedItem.ToString().Contains("")*/
                        )
                    {
                        button1.Enabled = false;
                        button2.Enabled = false;
                        button3.Enabled = false;
                        button4.Enabled = true;
                    }
                    else
                    {
                        OleDbCommand alarm_list = new OleDbCommand("SELECT * FROM alarm_tab where alarm_id=" + Program.cur_alarm, Program.conn);
                        OleDbDataReader alarm_list_reader = alarm_list.ExecuteReader();

                        string alarm_poz = "";

                        if (alarm_list_reader.Read())
                        {
                            richTextBox1.AppendText(alarm_list_reader[4].ToString());
                            richTextBox1.AppendText(alarm_list_reader[6].ToString());
                            richTextBox1.AppendText(alarm_list_reader[8].ToString());
                            richTextBox1.AppendText(alarm_list_reader[10].ToString());

                            if (alarm_list_reader[4].ToString() == "") { button1.Enabled = true; } else { button1.Enabled = false; }
                            if (alarm_list_reader[6].ToString() == "") { button2.Enabled = true; } else { button2.Enabled = false; }
                            if (alarm_list_reader[8].ToString() == "") { button3.Enabled = true; } else { button3.Enabled = false; }
                            if (alarm_list_reader[10].ToString() == "") { button4.Enabled = true; } else { button4.Enabled = false; }

                            //listBox1.Items.Add(alarm_list_reader[0].ToString() + " НЕОТРАБОТАННАЯ ТРЕВОГА: Позывной:" + alarm_list_reader[1].ToString() + " || Объект: " + alarm_list_reader[2].ToString());

                            //
                            alarm_poz = alarm_list_reader[2].ToString();
                            alarm_list_reader.Close();
                            alarm_list.Cancel();
#if (!DEMO)
                            OleDbCommand alarm_add_info = new OleDbCommand("SELECT poz, ho, addi FROM tab_obj where poz='" + alarm_poz + "';", Program.conn);
                            OleDbDataReader alarm_add_info_reader = alarm_add_info.ExecuteReader();

                            if (alarm_add_info_reader.Read())
                            {
                                textBox12.Text = alarm_add_info_reader[1].ToString();
                                textBox13.Text = alarm_add_info_reader[2].ToString();

                            }
#else
                            textBox12.Text = textBox10.Text;
                            textBox13.Text = textBox11.Text;
#endif

                        }

                    }
                }
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Form2 frm2 = new Form2();
            frm2.FormBorderStyle = FormBorderStyle.FixedSingle;
            frm2.ControlBox = true;

            frm2.Show();
            //frm2.Refresh();
            //frm2.Refresh();
        }


        private void button6_Click(object sender, EventArgs e)
        {
#if (!DEMO)
            if (Program.user_level < 2)
            {
                try
                {
                    string a_confirm="";
                    if (!checkBox1.Checked && !checkBox2.Checked) a_confirm = "";
                    if (checkBox1.Checked && !checkBox2.Checked) a_confirm = "1";
                    if (!checkBox1.Checked && checkBox2.Checked) a_confirm = "2";
                    if (checkBox1.Checked && checkBox2.Checked) a_confirm = "3";

                    OleDbCommand u_data_obj2 = new OleDbCommand("UPDATE tab_obj SET poz='" + textBox9.Text + "',  obj_name='" + textBox3.Text + "', obj_address='" + textBox4.Text + "', g3='" + textBox6.Text + "',  time_start='" + textBox5.Text + "', time_end='" + textBox7.Text + "', tel='" + textBox8.Text + "', ho='" + textBox10.Text + "', addi='" + textBox11.Text + "', a_confirm='" + a_confirm + "' WHERE poz='" + listBox2.SelectedItem.ToString() + "'", Program.conn);
                    u_data_obj2.ExecuteNonQuery();

                    listBox2.Items.Clear();

                    OleDbCommand u_conf = new OleDbCommand("SELECT * FROM tab_obj where poz<>NULL ORDER BY poz", Program.conn);
                    OleDbDataReader conf1 = u_conf.ExecuteReader();

                    while (conf1.Read())
                    {
                        listBox2.Items.Add(conf1[0].ToString());
                    }
                    textBox3.Text = "*";
                    textBox4.Text = "*";
                    textBox5.Text = "*";
                    textBox6.Text = "*";
                    textBox7.Text = "*";
                    textBox8.Text = "*";
                    textBox9.Text = "*";
                    textBox10.Text = "*";
                    textBox11.Text = "*";
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                }
                catch (Exception )
                {
                    MessageBox.Show("ERROR (неправильные действия)", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Вы не зарегистрированы!");


#else
            //MessageBox.Show("","",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (Program.user_level < 2)
            {
                try
                {
                    if (File.Exists("DEMO.txt")) File.Delete("DEMO.txt");

                    File.WriteAllText("DEMO.txt",

                        textBox3.Text + "\n" +
                        textBox4.Text + "\n" +
                        textBox5.Text + "\n" +
                        textBox6.Text + "\n" +
                        textBox7.Text + "\n" +
                        textBox8.Text + "\n" +
                        textBox9.Text + "\n" +
                        "{{{mtext}}}\n" +
                        textBox10.Text + "\n" +
                        "{{{mtext}}}\n" +
                        textBox11.Text + "\n" +
                        "{{{EOD}}}\n"
                        , Encoding.GetEncoding(1251));

                    /*textBox3.Text = "*";
                    textBox4.Text = "*";
                    textBox5.Text = "*";
                    textBox6.Text = "*";
                    textBox7.Text = "*";
                    textBox8.Text = "*";
                    textBox9.Text = "*";
                    textBox10.Text = "*";
                    textBox11.Text = "*";
                    */
                }
                catch (Exception)
                {
                    MessageBox.Show("ERROR (неправильные действия)", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Вы не зарегистрированы!");

#endif
        }

        private void button8_Click(object sender, EventArgs e)
        {
#if (!DEMO)
            if (MessageBox.Show("ОК? Удалить?","Уверены???",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                if (Program.user_level < 2)
                {
                    try
                    {
                        OleDbCommand u_data_obj2 = new OleDbCommand("DELETE FROM tab_obj WHERE poz='" + listBox2.SelectedItem.ToString() + "'", Program.conn);
                        u_data_obj2.ExecuteNonQuery();

                        listBox2.Items.Clear();

                        OleDbCommand u_conf = new OleDbCommand("SELECT * FROM tab_obj where poz<>NULL ORDER BY poz", Program.conn);
                        OleDbDataReader conf1 = u_conf.ExecuteReader();

                        while (conf1.Read())
                        {
                            listBox2.Items.Add(conf1[0].ToString());
                        }
                        textBox3.Text = "*";
                        textBox4.Text = "*";
                        textBox5.Text = "*";
                        textBox6.Text = "*";
                        textBox7.Text = "*";
                        textBox8.Text = "*";
                        textBox9.Text = "*";
                        checkBox1.Checked = false;
                        checkBox2.Checked = false;

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("ERROR (неправильные действия)", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                    MessageBox.Show("Вы не зарегистрированы!");

                
            }
#endif
        }

        private void button7_Click(object sender, EventArgs e)
        {
#if (!DEMO)
            if (Program.user_level < 2)
            {
                try
                {
                    string a_confirm = "";
                    if (!checkBox1.Checked && !checkBox2.Checked) a_confirm = "";
                    if (checkBox1.Checked && !checkBox2.Checked) a_confirm = "1";
                    if (!checkBox1.Checked && checkBox2.Checked) a_confirm = "2";
                    if (checkBox1.Checked && checkBox2.Checked) a_confirm = "3";

                    OleDbCommand u_data_obj2 = new OleDbCommand("INSERT INTO tab_obj(poz, obj_name, obj_address, g3, time_start, time_end, tel, ho, addi, a_confirm) VALUES('" + textBox9.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + textBox6.Text + "',  '" + textBox5.Text + "', '" + textBox7.Text + "', '" + textBox8.Text + "', '" + textBox10.Text + "', '" + textBox11.Text + "', '" + a_confirm + "')", Program.conn);
                    u_data_obj2.ExecuteNonQuery();

                    listBox2.Items.Clear();

                    OleDbCommand u_conf = new OleDbCommand("SELECT * FROM tab_obj where poz<>NULL ORDER BY poz", Program.conn);
                    OleDbDataReader conf1 = u_conf.ExecuteReader();

                    while (conf1.Read())
                    {
                        listBox2.Items.Add(conf1[0].ToString());
                    }
                    textBox3.Text = "*";
                    textBox4.Text = "*";
                    textBox5.Text = "*";
                    textBox6.Text = "*";
                    textBox7.Text = "*";
                    textBox8.Text = "*";
                    textBox9.Text = "*";
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("ERROR (неправильные действия)", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Вы не зарегистрированы!");
#endif
        }

        private void историяСистемыToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form3 frm3 = new Form3();
            if (Program.screen720x400)
            {
                frm3.Width = 720;
            }

            frm3.Show();

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.alarm_player != null)
                Program.alarm_player.Suspend();//.Abort();
            this.Close();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
            if (listBox2.SelectedItem != null)
            {
                string cur_poz = listBox2.SelectedItem.ToString();
                OleDbCommand u_data_obj = new OleDbCommand("SELECT * FROM tab_obj where poz=\"" + cur_poz + "\";", Program.conn);
                OleDbDataReader u_data_obj_reader33 = u_data_obj.ExecuteReader();
                u_data_obj_reader33.Read();
                textBox3.Text = u_data_obj_reader33[1].ToString();
                textBox4.Text = u_data_obj_reader33[2].ToString();
                textBox5.Text = u_data_obj_reader33[4].ToString();
                textBox6.Text = u_data_obj_reader33[3].ToString();
                textBox7.Text = u_data_obj_reader33[5].ToString();
                textBox8.Text = u_data_obj_reader33[6].ToString();
                textBox9.Text = u_data_obj_reader33[0].ToString();
                textBox10.Text = u_data_obj_reader33[7].ToString();
                textBox11.Text = u_data_obj_reader33[8].ToString();
                if (u_data_obj_reader33[9].ToString() == "")
                {
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                }
                if (u_data_obj_reader33[9].ToString() == "1")
                {
                    checkBox1.Checked = true;
                    checkBox2.Checked = false;
                }
                if (u_data_obj_reader33[9].ToString() == "2")
                {
                    checkBox1.Checked = false;
                    checkBox2.Checked = true;
                }
                if (u_data_obj_reader33[9].ToString() == "3")
                {
                    checkBox1.Checked = true;
                    checkBox2.Checked = true;
                }
 
            }
        }


        private void пользователиToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form4 frm4 = new Form4();
            frm4.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_MinimumSizeChanged(object sender, EventArgs e)
        {

        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            
        }

        private void Form1_MouseCaptureChanged(object sender, EventArgs e)
        {

        }

        private void Form1_StyleChanged(object sender, EventArgs e)
        {

        }

        private void Form1_ImeModeChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Leave(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (listBox1.Items.Count > 0)
                    this.WindowState = FormWindowState.Normal;

            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            //
            if (e.KeyCode == Keys.Enter)
            {
                OleDbCommand u_data_obj = new OleDbCommand("SELECT * FROM user_tab where pss=\"" + textBox2.Text + "\";", Program.conn);
                OleDbDataReader u_data_obj_reader = u_data_obj.ExecuteReader();


                if (u_data_obj_reader.Read())
                {
                    label4.Text = "Зарегистрирован l:(" + u_data_obj_reader[1].ToString() + ") " + u_data_obj_reader[2].ToString();
                    Program.user_level = (int)u_data_obj_reader[1];
                    Program.cur_user = u_data_obj_reader[2].ToString();

                    Program.date_now = DateTime.Now;
                    OleDbCommand u_data_obj2 = new OleDbCommand("INSERT INTO alarm_tab(time_alarm, alarm_poz, alarm_name, user1, alarm_level) VALUES('" + Program.date_now + "','SYSTEM','РЕГИСТРАЦИЯ ОПЕРАТОРА','" + Program.cur_user + "',1)", Program.conn);
                    u_data_obj2.ExecuteNonQuery();


                    button5.Text = "Смена пользователя";

                }
            }
        }

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ') { Program.spacebar = true; }
        }

        private void button2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ') { Program.spacebar = true; }
        }

        private void button3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ') { Program.spacebar = true; }
        }

        private void button4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ') { Program.spacebar = true; }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

            if (Program.user_level < 1)
            {
                serialPort1.Write(textBox14.Text + (char)13 + (char)10);
                //Thread.Sleep(100);
            }
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
        }

        private void отключитьВосстановлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.user_level == 0 || Program.user_level == 1 || Program.user_level == 2 && Program.app_close_admin_only == false)
            {

                Process[] all_prc = Process.GetProcesses();
                for (int i = 0; i < all_prc.Count(); i++)
                {
                    if (all_prc[i].ProcessName=="VenusCtrl1")
                    {
                        all_prc[i].Kill();
                        break;
                    }
                }
                отключитьВосстановлениеToolStripMenuItem.Enabled = false;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                listBox2.Items.Clear();
                listBox2.BackColor = Color.YellowGreen;

                OleDbCommand u_conf = new OleDbCommand("SELECT * FROM tab_obj where poz<>NULL ORDER BY poz", Program.conn);
                OleDbDataReader conf1 = u_conf.ExecuteReader();

                while (conf1.Read())
                {
                    if (conf1[0].ToString().ToLower().Contains(textBox1.Text.ToLower()) ||
                        conf1[1].ToString().ToLower().Contains(textBox1.Text.ToLower()) ||
                        conf1[2].ToString().ToLower().Contains(textBox1.Text.ToLower()) ||
                        conf1[3].ToString().ToLower().Contains(textBox1.Text.ToLower()) ||
                        conf1[6].ToString().ToLower().Contains(textBox1.Text.ToLower()) ||
                        conf1[7].ToString().ToLower().Contains(textBox1.Text.ToLower()) ||
                        conf1[8].ToString().ToLower().Contains(textBox1.Text.ToLower()) 
                        )
                    listBox2.Items.Add(conf1[0].ToString());
                }
                textBox3.Text = "*";
                textBox4.Text = "*";
                textBox5.Text = "*";
                textBox6.Text = "*";
                textBox7.Text = "*";
                textBox8.Text = "*";
                textBox9.Text = "*";
                textBox10.Text = "*";
                textBox11.Text = "*";
                checkBox1.Checked = false;
                checkBox2.Checked = false;
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR (неправильные действия)", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            try
            {
                listBox2.Items.Clear();
                listBox2.BackColor = Color.White;


                OleDbCommand u_conf = new OleDbCommand("SELECT * FROM tab_obj where poz<>NULL ORDER BY poz", Program.conn);
                OleDbDataReader conf1 = u_conf.ExecuteReader();

                while (conf1.Read())
                {
                    listBox2.Items.Add(conf1[0].ToString());
                }
                textBox3.Text = "*";
                textBox4.Text = "*";
                textBox5.Text = "*";
                textBox6.Text = "*";
                textBox7.Text = "*";
                textBox8.Text = "*";
                textBox9.Text = "*";
                textBox10.Text = "*";
                textBox11.Text = "*";
                checkBox1.Checked = false;
                checkBox2.Checked = false;
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR (неправильные действия)", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void резервноеКопированиеБазыДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.user_level < 2)
            {
                try
                {
                    if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                        File.Copy(Program._main_db_path, folderBrowserDialog1.SelectedPath + "\\mobilo4ka" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + ".bak");

                }
                catch { MessageBox.Show("Ошибка резервного копировния!"); }
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text.Contains("+38") && textBox8.Text.Length > 13)
                MessageBox.Show("Осторожно!!! Для номеров (+38) 13 цифр");
            if (textBox8.Text.Contains("+7") && textBox8.Text.Length > 12)
                MessageBox.Show("Осторожно!!! Для номеров (+7) 12 цифр"); 

        }
    }
}