using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Threading;

namespace MOBILKA
{
    public partial class Form3 : Form
    {
        static public string html_out = "";
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            if (Program.screen720x400)
            {
                listView1.Width = 550;

            }

            OleDbCommand u_conf = new OleDbCommand("SELECT DISTINCT alarm_poz FROM alarm_tab WHERE alarm_poz<>NULL ORDER BY alarm_poz", Program.conn);
            OleDbDataReader conf1 = u_conf.ExecuteReader();

            while (conf1.Read())
            {
                comboBox1.Items.Add(conf1[0].ToString());
            }
            u_conf = new OleDbCommand("SELECT TOP 500 * FROM alarm_tab WHERE alarm_poz<>'SYSTEM' ORDER BY alarm_id DESC", Program.conn);
            conf1 = u_conf.ExecuteReader();

            listView1.Items.Clear();

            while (conf1.Read())
            {
                string[] history_row = new string[] { conf1[0].ToString(), conf1[1].ToString(), conf1[3].ToString(), conf1[4].ToString(), conf1[5].ToString(), conf1[6].ToString(), conf1[7].ToString(), conf1[8].ToString(), conf1[9].ToString(), conf1[10].ToString(), conf1[11].ToString(), conf1[12].ToString() };

                ListViewItem lv = new ListViewItem(history_row);
                listView1.Items.Add(lv);
            }

            if (File.Exists("label11_text.txt")) { label1.Text = "Пультовый номер"; }


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OleDbCommand u_conf = new OleDbCommand("SELECT * FROM alarm_tab WHERE alarm_poz='" + comboBox1.SelectedItem.ToString() + "' ORDER BY alarm_id", Program.conn);
            OleDbDataReader conf1 = u_conf.ExecuteReader();

            listView1.Items.Clear();

            while (conf1.Read())
            {
                string[] history_row = new string[] { conf1[0].ToString(), conf1[1].ToString(), conf1[3].ToString(), conf1[4].ToString(), conf1[5].ToString(), conf1[6].ToString(), conf1[7].ToString(), conf1[8].ToString(), conf1[9].ToString(), conf1[10].ToString(), conf1[11].ToString(), conf1[12].ToString() };

                ListViewItem lv = new ListViewItem(history_row);
                listView1.Items.Add(lv);
            }

            label2.Text = "История " + comboBox1.SelectedItem.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ThreadStart thr31 = new ThreadStart(html_writer_func);
            Thread html_writer = new Thread(thr31);
            html_writer.Start();
            progressBar1.Value = 0;
            progressBar1.Minimum=0;
            progressBar1.Maximum = listView1.Items.Count;
            button1.Enabled = false;

        }
        public void html_writer_func()
        {
            saveFileDialog1.DefaultExt = "htm";
            saveFileDialog1.ShowDialog(this);
            if (saveFileDialog1.FileName != "")
            {
                if (File.Exists(saveFileDialog1.FileName)) File.Delete(saveFileDialog1.FileName);
                OleDbCommand u_conf;
                if (comboBox1.Text!="")
                    u_conf = new OleDbCommand("SELECT * FROM alarm_tab WHERE alarm_poz='" + comboBox1.SelectedItem.ToString() + "' ORDER BY alarm_id", Program.conn);
                else
                    u_conf = new OleDbCommand("SELECT * FROM alarm_tab WHERE alarm_poz<>'SYSTEM' ORDER BY alarm_id", Program.conn);
                OleDbDataReader conf1 = u_conf.ExecuteReader();

                File.AppendAllText(saveFileDialog1.FileName, "<html><head><title>Выписка из журнала событий СПТС \"Венера\"</title></head><body><table border=1 cellpadding=0 cellspacing=0>\n", Encoding.GetEncoding(1251));
                File.AppendAllText(saveFileDialog1.FileName, "<tr>" + "<td colspan=12>" + "Выписка из журнала событий СПТС \"Венера\""+"</td>" + "</tr>", Encoding.GetEncoding(1251));
                File.AppendAllText(saveFileDialog1.FileName, "<tr bgcolor=#CCC>" + "<td>" + listView1.Columns[0].Text + "</td>" + "<td>" + listView1.Columns[1].Text + "</td>" + "<td>" + listView1.Columns[2].Text + "</td>" + "<td>" + listView1.Columns[3].Text + "</td>" + "<td>" + listView1.Columns[4].Text + "</td>" + "<td>" + listView1.Columns[5].Text + "</td>" + "<td>" + listView1.Columns[6].Text + "</td>" + "<td>" + listView1.Columns[7].Text + "</td>" + "<td>" + listView1.Columns[8].Text + "</td>" + "<td>" + listView1.Columns[9].Text + "</td>" + "<td>" + listView1.Columns[10].Text + "</td>" + "<td>" + listView1.Columns[11].Text + "</td>" + "</tr>\n", Encoding.GetEncoding(1251));
                while (conf1.Read())
                {
                    File.AppendAllText(saveFileDialog1.FileName, "<tr>" + "<td>" + conf1[0].ToString() + "</td>" + "<td>" + conf1[1].ToString() + "</td>" + "<td>" + conf1[3].ToString() + "</td>" + "<td>" + conf1[4].ToString() + "</td>" + "<td>" + conf1[5].ToString() + "</td>" + "<td>" + conf1[6].ToString() + "</td>" + "<td>" + conf1[7].ToString() + "</td>" + "<td>" + conf1[8].ToString() + "</td>" + "<td>" + conf1[9].ToString() + "</td>" + "<td>" + conf1[10].ToString() + "</td>" + "<td>" + conf1[11].ToString() + "</td>" + "<td>" + conf1[12].ToString() + "</td>" + "</tr>\n", Encoding.GetEncoding(1251));
                    progressBar1.Value++;
                }
                File.AppendAllText(saveFileDialog1.FileName, "</table>\n </body></html>", Encoding.GetEncoding(1251));

            }
            saveFileDialog1.FileName = "";
            button1.Enabled = true;
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (progressBar1.Value>progressBar1.Minimum && progressBar1.Value<progressBar1.Maximum)
                e.Cancel = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            OleDbCommand u_conf = new OleDbCommand("SELECT * FROM alarm_tab WHERE alarm_poz<>'SYSTEM' ORDER BY alarm_id", Program.conn);
            OleDbDataReader conf1 = u_conf.ExecuteReader();

            listView1.Items.Clear();

            while (conf1.Read())
            {
                string[] history_row = new string[] { conf1[0].ToString(), conf1[1].ToString(), conf1[3].ToString(), conf1[4].ToString(), conf1[5].ToString(), conf1[6].ToString(), conf1[7].ToString(), conf1[8].ToString(), conf1[9].ToString(), conf1[10].ToString(), conf1[11].ToString(), conf1[12].ToString() };

                ListViewItem lv = new ListViewItem(history_row);
                listView1.Items.Add(lv);
            }
            label2.Text = "Вся история!";

        }

    }
}
/*
<html>
<head>
    <title>Untitled Page</title>
</head>
<body>

</body>
</html>
*/