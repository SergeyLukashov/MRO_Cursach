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

            //pictureBox1.Image = new Bitmap(@"e:\Programming\CSharp\MRO_Cursach\MRO_Cursach\bin\Debug\7.jpg");
            pictureBox1.Image = new Bitmap(@"e:\Programming\CSharp\MRO_Cursach\MRO_Cursach\bin\Debug\12.jpg");
        }

        Bitmap binImg;
        Bitmap skelImg;

        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();//1950 1665

            Blur blur = new Blur();
            //binImg = blur.GaussianBlur5((Bitmap)pictureBox1.Image);
            binImg = blur.GaussianBlur7((Bitmap)pictureBox1.Image);

            Binaryzator bin = new Binaryzator();
            int s = pictureBox1.Image.Width / 8;
            skelImg = bin.AdaptiveTreshold(binImg, s, trackBar1.Value);

            ImageWorker iw = new ImageWorker();
            //skelImg = (Bitmap)iw.DeleteSinglePixels(skelImg);
            //skelImg = (Bitmap)iw.FillSinglePixels(skelImg);
            for (int i = 0; i < 1; i++)
            {
                skelImg = (Bitmap)iw.Extension(skelImg);
                //skelImg = (Bitmap)iw.Erosion(skelImg);
            }
            pictureBox2.Image = skelImg;
            
            sw.Stop();
            this.Text = sw.ElapsedMilliseconds.ToString();
        }

        
        private void button2_Click(object sender, EventArgs e)//<<
        {
            if (currentLetterIndex > 0)
            {
                currentLetterIndex--;
            }
            DrawCritheria(letters[currentLetterIndex]);
        }

        private void button3_Click(object sender, EventArgs e)//>>
        {
            if (currentLetterIndex < letters.Count-1)
            {
                currentLetterIndex++;
            }
            DrawCritheria(letters[currentLetterIndex]);
        }

        int currentLetterIndex = 0;
        List<Image> letters;
        private void button4_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();//5542 4804 2892 2667 / 396

            Skeletonizator s = new Skeletonizator();
            pictureBox3.Image = skelImg = s.ZongaSunja((Bitmap)skelImg);

            sw.Stop();
            this.Text = sw.ElapsedMilliseconds.ToString();

            ImageWorker iw = new ImageWorker();
            letters = iw.SplitToLetters(skelImg);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox4.Image.Save("temp.jpg");
        }

        private void DrawCritheria(Image letterImage)
        {
            pictureBox4.Image = letterImage;

            CritheriaFinder cf = new CritheriaFinder();
            label1.Text = cf.TerminalPixelsCount(letterImage)[0].ToString();
            label2.Text = cf.NodalPixelsCount(letterImage)[0].ToString();

            label3.Text = cf.TerminalPixelsCount(letterImage)[1].ToString();
            label4.Text = cf.TerminalPixelsCount(letterImage)[2].ToString();
            label5.Text = cf.TerminalPixelsCount(letterImage)[3].ToString();
            label6.Text = cf.TerminalPixelsCount(letterImage)[4].ToString();

            label7.Text = cf.NodalPixelsCount(letterImage)[1].ToString();
            label8.Text = cf.NodalPixelsCount(letterImage)[2].ToString();
            label9.Text = cf.NodalPixelsCount(letterImage)[3].ToString();
            label10.Text = cf.NodalPixelsCount(letterImage)[4].ToString();

            label11.Text = cf.Ratio(letterImage).ToString();

            label16.Text = cf.Magnitude(letterImage)[0].ToString();
            label12.Text = cf.Magnitude(letterImage)[1].ToString();
            label13.Text = cf.Magnitude(letterImage)[2].ToString();
            label14.Text = cf.Magnitude(letterImage)[3].ToString();
            label15.Text = cf.Magnitude(letterImage)[4].ToString();
        }
        //Perceptron.AElement d;
        
    }
}
