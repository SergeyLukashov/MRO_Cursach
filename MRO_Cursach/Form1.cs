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
            sw.Start();//1308

            Blur blur = new Blur();
            //binImg = blur.GaussianBlur5((Bitmap)pictureBox1.Image);
            binImg = blur.GaussianBlur7((Bitmap)pictureBox1.Image);

            Binaryzator bin = new Binaryzator();
            int s = pictureBox1.Image.Width / 8;
            skelImg = bin.AdaptiveTreshold(binImg, s, trackBar1.Value);

            ImageWorker iw = new ImageWorker();
            skelImg = (Bitmap)iw.DeleteSinglePixels(skelImg);
            skelImg = (Bitmap)iw.FillSinglePixels(skelImg);
            for (int i = 0; i < 1; i++)
            {
                skelImg = (Bitmap)iw.Extension(skelImg);
                //skelImg = (Bitmap)iw.Erosion(skelImg);
            }
            pictureBox2.Image = skelImg;
            
            sw.Stop();
            this.Text = sw.ElapsedMilliseconds.ToString();
        }

        int currentLetterIndex = 0;
        List<Image> letters;
        private void button2_Click(object sender, EventArgs e)//<<
        {
            ImageWorker iw = new ImageWorker();//!!!!!!!!!!!!!
            letters = iw.SplitToLetters(skelImg);
            imageList1.Images.Clear();
            imageList1.Images.AddRange(letters.ToArray());
            int i = 0;
            foreach(var letter in letters)
            {
                listView1.Items.Add("", i++);
            }

            if (currentLetterIndex > 0)
            {
                currentLetterIndex--;
                //pictureBox4.Image = letters[currentLetter];
            }
            pictureBox4.Image = letters[currentLetterIndex];
            CritheriaFinder cf=new CritheriaFinder();
            label1.Text = cf.TerminalPixelsCount(letters[currentLetterIndex]).ToString();
            label2.Text = cf.NodalPixelsCount(letters[currentLetterIndex]).ToString();
        }

        private void button3_Click(object sender, EventArgs e)//>>
        {
            ImageWorker iw = new ImageWorker();//!!!!!!!!!!!!!
            letters = iw.SplitToLetters(skelImg);
            

            if (currentLetterIndex < letters.Count-1)
            {
                currentLetterIndex++;
            }
            pictureBox4.Image = letters[currentLetterIndex];
            CritheriaFinder cf = new CritheriaFinder();
            label1.Text = cf.TerminalPixelsCount(letters[currentLetterIndex]).ToString();
            label2.Text = cf.NodalPixelsCount(letters[currentLetterIndex]).ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();//5542 4804

            Skeletonizator s = new Skeletonizator();
            for (int i = 0; i < 18; i++)
            {
                skelImg = s.ZongaSunja((Bitmap)skelImg);
            }
            pictureBox3.Image = skelImg;

            sw.Stop();
            this.Text = sw.ElapsedMilliseconds.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox4.Image.Save("temp.jpg");
        }
    }
}
