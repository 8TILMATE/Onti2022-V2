using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Onti2022_V2
{
    public partial class WhatDirection : Form
    {
        public WhatDirection()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.ToLower() == "sus")
            {
                DatabaseHelper.DatabaseHelper.hedy = -1;
                DatabaseHelper.DatabaseHelper.hedx = 0;
                this.Close();

            }
            else if (textBox1.Text.ToLower() == "jos")
            {
                DatabaseHelper.DatabaseHelper.hedy = 1;
                DatabaseHelper.DatabaseHelper.hedx = 0;
                this.Close();

            }
            else if (textBox1.Text.ToLower() == "stanga")
            {
                DatabaseHelper.DatabaseHelper.hedy = 0;
                DatabaseHelper.DatabaseHelper.hedx = -1;
                this.Close();

            }
            else if (textBox1.Text.ToLower() == "dreapta")
            {
                DatabaseHelper.DatabaseHelper.hedy = 0;
                DatabaseHelper.DatabaseHelper.hedx = 1;
                this.Close();
            }
            else
            {
                MessageBox.Show("NOT GOOD");
            }
        }
    }
}
