using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

            iw = new ImageWorker(new Bitmap("e:\\Programming\\CSharp\\MRO_Cursach\\MRO_Cursach\\bin\\Debug\\t22.jpg"));
            pictureBox1.Image = iw.Image;
            //pictureBox1.Image = new Bitmap("e:\\Programming\\CSharp\\MRO_Cursach\\MRO_Cursach\\bin\\Debug\\t4.jpg");
            //pictureBox1.Image = iw.Haf();
            iw.SplitToLetters();
            if (iw.letters.Count > 0)
            {
                pictureBox2.Image = iw.letters[0];
            }
            pictureBox3.Image = iw.FirstLetter();
            pictureBox4.Image = iw.LastLetter();
        }

        ImageWorker iw;

        int i = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (i > 0)
                i--;
            pictureBox2.Image = iw.letters[i];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (i < iw.letters.Count - 1)
                i++;
            pictureBox2.Image = iw.letters[i];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            iw.Binaryzation1();
            pictureBox1.Image = iw.Image;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            iw.DeleteSinglePixels();
            pictureBox1.Image = iw.Image;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            iw.FillSinglePixels();
            pictureBox1.Image = iw.Image;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            iw.Erosion();
            pictureBox1.Image = iw.Image;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            iw.Extension();
            pictureBox1.Image = iw.Image;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //iw.Binaryzation();
            //iw.Binaryzation3();```
            int s = iw.Image.Width / 12;
            iw.AdaptiveTreshold(s, 13);
            pictureBox5.Image = iw.Image;
        }
    }
}
