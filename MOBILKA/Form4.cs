using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace MOBILKA
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string cur_poz = listBox1.SelectedItem.ToString();
                OleDbCommand u_data_obj = new OleDbCommand("SELECT * FROM user_tab where name='" + listBox1.SelectedItem.ToString() + "';", Program.conn);
                OleDbDataReader u_data_obj_reader = u_data_obj.ExecuteReader();
                u_data_obj_reader.Read();
                textBox1.Text = u_data_obj_reader[2].ToString();
                if (u_data_obj_reader[1].ToString() == "1")
                    comboBox1.SelectedItem = "Администратор";
                if (u_data_obj_reader[1].ToString() == "2")
                    comboBox1.SelectedItem = "Оператор";
                textBox2.Text = u_data_obj_reader[3].ToString();
                textBox3.Text = u_data_obj_reader[4].ToString();
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            OleDbCommand u_conf = new OleDbCommand("SELECT name FROM user_tab WHERE levelu>0 ORDER BY name", Program.conn);
            OleDbDataReader conf1 = u_conf.ExecuteReader();

            while (conf1.Read())
            {
                listBox1.Items.Add(conf1[0].ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Program.user_level < 2)
            {
                if (textBox3.Text == textBox4.Text && textBox4.Text.Length>2)
                {
                    try
                    {
                        int temp_user_level=5;
                        if (comboBox1.SelectedItem.ToString() == "Администратор")
                            temp_user_level = 1;
                        if (comboBox1.SelectedItem.ToString() == "Оператор")
                            temp_user_level = 2;



                        OleDbCommand u_data_obj2 = new OleDbCommand("UPDATE user_tab SET name='" + textBox1.Text + "',  tel='" + textBox2.Text + "', levelu=" + temp_user_level.ToString() + ",  pss='" + textBox3.Text + "' WHERE name='" + listBox1.SelectedItem.ToString() + "'", Program.conn);
                        u_data_obj2.ExecuteNonQuery();

                        listBox1.Items.Clear();

                        OleDbCommand u_conf = new OleDbCommand("SELECT name FROM user_tab WHERE levelu>0 ORDER BY name", Program.conn);
                        OleDbDataReader conf1 = u_conf.ExecuteReader();

                        while (conf1.Read())
                        {
                            listBox1.Items.Add(conf1[0].ToString());
                        }
                        textBox1.Text = "*";
                        textBox2.Text = "*";
                        textBox3.Text = "";
                        textBox4.Text = "";
                          

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("ERROR (неправильные действия)", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                    MessageBox.Show("Пароль не прошел!");
            }
            else
                MessageBox.Show("Вы не зарегистрированы!");





        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Program.user_level < 2)
            {
                if (textBox3.Text == textBox4.Text && textBox4.Text.Length > 2)
                {
                    //try
                    //{
                        int temp_user_level = 5;
                        if (comboBox1.SelectedItem.ToString() == "Администратор")
                            temp_user_level = 1;
                        if (comboBox1.SelectedItem.ToString() == "Оператор")
                            temp_user_level = 2;



                        OleDbCommand u_data_obj2 = new OleDbCommand("INSERT INTO user_tab (name,tel,levelu,pss) VALUES ('" + textBox1.Text + "',  '" + textBox2.Text + "', " + temp_user_level.ToString() + ",  '" + textBox3.Text + "')", Program.conn);
                        u_data_obj2.ExecuteNonQuery();

                        listBox1.Items.Clear();

                        OleDbCommand u_conf = new OleDbCommand("SELECT name FROM user_tab WHERE levelu>0 ORDER BY name", Program.conn);
                        OleDbDataReader conf1 = u_conf.ExecuteReader();

                        while (conf1.Read())
                        {
                            listBox1.Items.Add(conf1[0].ToString());
                        }
                        textBox1.Text = "*";
                        textBox2.Text = "*";
                        textBox3.Text = "";
                        textBox4.Text = "";


                    /*}
                    catch (Exception)
                    {
                        MessageBox.Show("ERROR (неправильные действия)", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }*/
                }
                else
                    MessageBox.Show("Пароль не прошел!");
            }
            else
                MessageBox.Show("Вы не зарегистрированы!");

        }
    }
}
