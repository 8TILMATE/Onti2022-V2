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
    public partial class InterferenteEco : Form
    {
        private string adresaImagine;
        private int rotangle = 0;
        private static List<PictureBox> obstacoleishan = new List<PictureBox>();
        PictureBox pictureToBeAdded = null;
        private static List<DeflectorModel> deflectoare = new List<DeflectorModel>();
        PictureBox Robot;
        private static int plastic, sticla, hartie;
        private static int itemstobecollected = 0;
        private static Point initHome = new Point();
        public InterferenteEco(string FileName)
        {
            InitializeComponent();
            adresaImagine = FileName;
        }

        private void InterferenteEco_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(adresaImagine);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();

            if (checkBox1.Checked)
            {
                Pen pen = new Pen(Color.White);
                for(int i = 1; i <= 20; i++)
                {
                    g.DrawLine(pen, 40 * i, 0, 40 * i, 600);
                }
                for (int j = 1; j <= 10; j++)
                {
                    g.DrawLine(pen, 0, 60*j, 800, 60*j);

                }
                g.Dispose();
            }
            else
            {
                pictureBox1.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (DialogResult.OK == dialog.ShowDialog())
            {
                DatabaseHelper.DatabaseHelper.LoadObjects(dialog.FileName);

                AddObjectsToMap();
            }

        }
        private void AddObjectsToMap()
        {
            pictureBox1.Controls.Clear();
            foreach(HartaModel x in DatabaseHelper.DatabaseHelper.obiecte)
            {
                PictureBox pictureBox = new PictureBox();
                if (x.type == "Robot")
                {
                    pictureBox.ImageLocation =Resources.robotString;
                    

                }
               else if (x.type.Contains("Meduza"))
                {
                    foreach(string file in Directory.GetFiles(Resources.meduzaString))
                    {
                        if (file.Contains(x.type))
                        {
                            pictureBox.ImageLocation = file;
                            break;

                        }
                    }
                }
                else
                {
                    foreach (string file in Directory.GetFiles(Resources.materialeReciclabile))
                    {
                        if (file.Contains(x.type))
                        {
                            pictureBox.ImageLocation = file;
                            itemstobecollected++;
                            break;

                        }
                    }
                }
                pictureBox.Location= new Point(40*x.casx,60*x.casy); pictureBox.Size = new Size(40, 60);

                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                obstacoleishan.Add(pictureBox);
                pictureBox1.Controls.Add(pictureBox);
                if(x.type == "Robot")
                {
                    Robot = pictureBox;
                    initHome = pictureBox.Location;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Image img = pictureBox3.Image;
            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox3.Image = img;
            if (rotangle < 270)
            {
                rotangle += 90;
            }
            else
            {
                rotangle = 0;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            PictureBox picture = new PictureBox();
            picture.Image = pictureBox3.Image;
            picture.Size = new Size(40, 60);
            pictureToBeAdded = picture;

            pictureToBeAdded.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Controls.Add(pictureToBeAdded);
            pictureToBeAdded.Click += pictureBox1_Click;
            timer1.Start();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureToBeAdded.Location = pictureBox1.PointToClient(Cursor.Position);
            pictureToBeAdded.Location = new Point(pictureToBeAdded.Location.X-pictureToBeAdded.Location.X%40, pictureToBeAdded.Location.Y- pictureToBeAdded.Location.Y % 60);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
                deflectoare.Add(new DeflectorModel
                {
                    imagine = pictureToBeAdded,
                    rotangle = rotangle,
                });
                pictureToBeAdded = null;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Controls.Clear();
            
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (Robot.Location.X < initHome.X)
            {
                Robot.Location = new Point(Robot.Location.X + 40, Robot.Location.Y);

            }
            else if (Robot.Location.X > initHome.X)
            {
                Robot.Location = new Point(Robot.Location.X - 40, Robot.Location.Y);
            }
            else
            {
                if (Robot.Location.Y < initHome.Y)
                {
                    Robot.Location = new Point(Robot.Location.X, Robot.Location.Y + 60);

                }
                else if (Robot.Location.Y > initHome.Y)
                {
                    Robot.Location = new Point(Robot.Location.X, Robot.Location.Y - 60);
                }
                else
                {
                    MessageBox.Show("Parcat!");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var directieselect = new WhatDirection();
            directieselect.ShowDialog();
            timer2.Start();

            

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Robot.Location = new Point(Robot.Location.X + 40 * DatabaseHelper.DatabaseHelper.hedx, Robot.Location.Y + 60 * DatabaseHelper.DatabaseHelper.hedy);
            /*
            Graphics g = pictureBox1.CreateGraphics();
            Brush brush = new SolidBrush(Color.Purple);
            Rectangle rect = new Rectangle(Robot.Location, new Size(40, 60));
            g.FillRectangle(brush, rect);
            g.Dispose();
            */
            label1.Text = "Plastic: " + plastic.ToString();
            label2.Text = "Sticla: " + sticla.ToString();
            label3.Text = "Hartie: " + hartie.ToString();

            if (itemstobecollected == sticla + plastic + hartie)
            {
                timer2.Stop();
                timer3.Start();
            }
            foreach(var item in deflectoare)
            {
                if (item.imagine.Bounds.IntersectsWith(Robot.Bounds))
                {
                    if (item.rotangle == 0)
                    {
                        if(DatabaseHelper.DatabaseHelper.hedx==1 && DatabaseHelper.DatabaseHelper.hedy == 0)
                        {
                            DatabaseHelper.DatabaseHelper.hedx = 0;
                            DatabaseHelper.DatabaseHelper.hedy = 1;
                            Robot.Location= new Point(Robot.Location.X+40*DatabaseHelper.DatabaseHelper.hedx,Robot.Location.Y+60*DatabaseHelper.DatabaseHelper.hedy);
                        }
                        else if(DatabaseHelper.DatabaseHelper.hedx==0 && DatabaseHelper.DatabaseHelper.hedy == -1)
                        {
                            DatabaseHelper.DatabaseHelper.hedx = -1;
                            DatabaseHelper.DatabaseHelper.hedy = 0;
                            Robot.Location = new Point(Robot.Location.X + 40 * DatabaseHelper.DatabaseHelper.hedx, Robot.Location.Y + 60 * DatabaseHelper.DatabaseHelper.hedy);

                        }
                        else
                        {
                            DatabaseHelper.DatabaseHelper.hedy = -DatabaseHelper.DatabaseHelper.hedy;
                            DatabaseHelper.DatabaseHelper.hedx = -DatabaseHelper.DatabaseHelper.hedx;
                            Robot.Location = new Point(Robot.Location.X + 40 * DatabaseHelper.DatabaseHelper.hedx, Robot.Location.Y + 60 * DatabaseHelper.DatabaseHelper.hedy);

                        }
                    }
                    if (item.rotangle == 90)
                    {
                        if (DatabaseHelper.DatabaseHelper.hedx == 1 && DatabaseHelper.DatabaseHelper.hedy == 0)
                        {
                            DatabaseHelper.DatabaseHelper.hedx = 0;
                            DatabaseHelper.DatabaseHelper.hedy = -1;
                            Robot.Location = new Point(Robot.Location.X + 40 * DatabaseHelper.DatabaseHelper.hedx, Robot.Location.Y + 60 * DatabaseHelper.DatabaseHelper.hedy);

                        }
                        else if (DatabaseHelper.DatabaseHelper.hedx == 0 && DatabaseHelper.DatabaseHelper.hedy == 1)
                        {
                            DatabaseHelper.DatabaseHelper.hedx = -1;
                            DatabaseHelper.DatabaseHelper.hedy = 0;
                            Robot.Location = new Point(Robot.Location.X + 40 * DatabaseHelper.DatabaseHelper.hedx, Robot.Location.Y + 60 * DatabaseHelper.DatabaseHelper.hedy);

                        }

                        else
                        {
                            DatabaseHelper.DatabaseHelper.hedy = -DatabaseHelper.DatabaseHelper.hedy;

                            DatabaseHelper.DatabaseHelper.hedx = -DatabaseHelper.DatabaseHelper.hedx;
                            Robot.Location = new Point(Robot.Location.X + 40 * DatabaseHelper.DatabaseHelper.hedx, Robot.Location.Y + 60 * DatabaseHelper.DatabaseHelper.hedy);

                        }
                    }
                    if (item.rotangle == 180)
                    {
                        if (DatabaseHelper.DatabaseHelper.hedx == -1 && DatabaseHelper.DatabaseHelper.hedy == 0)
                        {
                            DatabaseHelper.DatabaseHelper.hedx = 0;
                            DatabaseHelper.DatabaseHelper.hedy = 1;
                            Robot.Location = new Point(Robot.Location.X + 40 * DatabaseHelper.DatabaseHelper.hedx, Robot.Location.Y + 60 * DatabaseHelper.DatabaseHelper.hedy);
                        }
                        else if (DatabaseHelper.DatabaseHelper.hedx == 0 && DatabaseHelper.DatabaseHelper.hedy == -1)
                        {
                            DatabaseHelper.DatabaseHelper.hedx = 1;
                            DatabaseHelper.DatabaseHelper.hedy = 0;
                            Robot.Location = new Point(Robot.Location.X + 40 * DatabaseHelper.DatabaseHelper.hedx, Robot.Location.Y + 60 * DatabaseHelper.DatabaseHelper.hedy);
                        }
                        else
                        {
                            DatabaseHelper.DatabaseHelper.hedy = -DatabaseHelper.DatabaseHelper.hedy;
                            DatabaseHelper.DatabaseHelper.hedx = -DatabaseHelper.DatabaseHelper.hedx;
                            Robot.Location = new Point(Robot.Location.X + 40 * DatabaseHelper.DatabaseHelper.hedx, Robot.Location.Y + 60 * DatabaseHelper.DatabaseHelper.hedy);
                        }
                    }
                    if (item.rotangle == 270)
                    {
                        if (DatabaseHelper.DatabaseHelper.hedx == -1 && DatabaseHelper.DatabaseHelper.hedy == 0)
                        {
                            DatabaseHelper.DatabaseHelper.hedx = 0;
                            DatabaseHelper.DatabaseHelper.hedy = 1;
                            Robot.Location = new Point(Robot.Location.X + 40 * DatabaseHelper.DatabaseHelper.hedx, Robot.Location.Y + 60 * DatabaseHelper.DatabaseHelper.hedy);
                        }
                        else if (DatabaseHelper.DatabaseHelper.hedx == 0 && DatabaseHelper.DatabaseHelper.hedy == -1)
                        {
                            DatabaseHelper.DatabaseHelper.hedx = 1;
                            DatabaseHelper.DatabaseHelper.hedy = 0;
                            Robot.Location = new Point(Robot.Location.X + 40 * DatabaseHelper.DatabaseHelper.hedx, Robot.Location.Y + 60 * DatabaseHelper.DatabaseHelper.hedy);
                        }
                        else
                        {
                            DatabaseHelper.DatabaseHelper.hedy = -DatabaseHelper.DatabaseHelper.hedy;
                            DatabaseHelper.DatabaseHelper.hedx = -DatabaseHelper.DatabaseHelper.hedx;
                            Robot.Location = new Point(Robot.Location.X + 40 * DatabaseHelper.DatabaseHelper.hedx, Robot.Location.Y + 60 * DatabaseHelper.DatabaseHelper.hedy);

                        }
                    }
                }
            }
            foreach(var item in obstacoleishan)
            {
                if (Robot.Bounds.IntersectsWith(item.Bounds))
                {
                    if (item.ImageLocation.Contains("Hartie"))
                    {
                        hartie++;
                        obstacoleishan.Remove(item);
                        pictureBox1.Controls.Remove(item);
                        break;
                    }
                    if (item.ImageLocation.Contains("Plastic"))
                    {
                        plastic++;
                        obstacoleishan.Remove(item);
                        pictureBox1.Controls.Remove(item);

                        break;
                    }
                    if (item.ImageLocation.Contains("Sticla"))
                    {
                        sticla++;
                        obstacoleishan.Remove(item);
                        pictureBox1.Controls.Remove(item);
                        break;
                    }
                    if (item.ImageLocation.Contains("Meduza"))
                    {
                        MessageBox.Show("Ai pierdut!");
                        timer1.Stop();
                        this.Close();
                        break;
                        
                    }
                }
            }
        }
    }
}
