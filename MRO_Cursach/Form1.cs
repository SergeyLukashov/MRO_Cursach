using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MRO_Cursach
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            pictureBox1.Image = new Bitmap(@"e:\Programming\CSharp\MRO_Cursach\MRO_Cursach\bin\Debug\6.jpg");
            //pictureBox1.Image = new Bitmap(@"e:\Programming\CSharp\MRO4_Skeletonization\MRO4_Skeletonization\bin\Debug\2.jpg");
        }

        Bitmap binImg;
        Bitmap skelImg;

        private void button1_Click(object sender, EventArgs e)
        {
            Blur b = new Blur();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //pictureBox4.Image = binImg = b.GaussianBlur5((Bitmap)pictureBox1.Image);
            pictureBox2.Image = binImg = b.GaussianBlur7((Bitmap)pictureBox1.Image);
            sw.Stop();
            this.Text = sw.ElapsedMilliseconds.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Binaryzator b = new Binaryzator();
            int s = pictureBox1.Image.Width / 12;
            pictureBox3.Image = skelImg = b.AdaptiveTreshold(binImg, s, trackBar1.Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ImageWorker iw=new ImageWorker();
            skelImg = (Bitmap)iw.DeleteSinglePixels(skelImg);
            skelImg = (Bitmap)iw.FillSinglePixels(skelImg);
            for (int i = 0; i < 1; i++)
            {
                skelImg = (Bitmap)iw.Extension(skelImg);
                skelImg = (Bitmap)iw.Erosion(skelImg);
            }
            pictureBox3.Image = skelImg;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Skeletonizator s = new Skeletonizator();
            //pictureBox3.Image =skelImg= s.TemplateMethod((Bitmap)skelImg);
            pictureBox4.Image = skelImg = s.ZongaSunja((Bitmap)skelImg);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            skelImg.Save("temp.jpg");
        }


        
    }
}
