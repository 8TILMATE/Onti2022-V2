using Onti2022_V2.Models;
using Onti2022_V2.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Onti2022_V2
{
    public partial class Form1 : Form
    {
        private List<string> Images = new List<string>();
        private string selectedImg;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DatabaseHelper.DatabaseHelper.GetUseri();
            Images.Clear();
            foreach(string x in Directory.GetFiles(Resources.backgroundImages))
            {
                Images.Add(x);
            }
            foreach(Control c in this.Controls)
            {
                if(c is PictureBox)
                {
                    PictureBox pictureBox = (PictureBox)c;
                    pictureBox.Click+=ImageClick;
                    Random rand = new Random();
                    int index = rand.Next(0, Images.Count-1);
                    pictureBox.ImageLocation= Images[index];
                    Images.RemoveAt(index);
                }
            }
            comboBox1.Items.Clear();
            foreach(UserModel model in DatabaseHelper.DatabaseHelper.utilizatori)
            {
                comboBox1.Items.Add(model.username);
            }

        }
        private void ImageClick(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            selectedImg = pictureBox.ImageLocation;
            foreach(UserModel model in DatabaseHelper.DatabaseHelper.utilizatori)
            {
                if (model.username == comboBox1.SelectedItem.ToString())
                {
                    if(model.password.Trim()==textBox1.Text.Trim())
                    {
                        this.Hide();
                        InterferenteEco eco = new InterferenteEco(selectedImg);
                        eco.ShowDialog();
                        this.Close();
                    }
                }
            }
        }
    }
}
