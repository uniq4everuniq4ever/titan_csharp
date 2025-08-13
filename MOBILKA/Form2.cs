using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MOBILKA
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (this.ControlBox == true)
                button1.Visible = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
