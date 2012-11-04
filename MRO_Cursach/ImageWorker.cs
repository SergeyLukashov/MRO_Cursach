using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

public class ImageWorker
{
    private Bitmap image;//////////////modific!!

    public Bitmap Image
    {
        set
        {
            image = value;
        }
        get
        {
            return image;
        }
    }


    public ImageWorker(Bitmap img)
    {
        image = img;
        //Binaryzation1();
        //DeleteSinglePixels();
        //FillSinglePixels();
        //RotateImage(2.3f);
    }

    public List<Bitmap> letters = new List<Bitmap>();
    public void SplitToLetters()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        bool letter = false;
        int start = 0;
        for (int i = 0; i < image.Width; i++)
        {
            int counter = 0;
            for (int j = 0; j < image.Height; j++)
            {
                if (image.GetPixel(i, j).GetBrightness() >= theta)/////white
                {
                    counter++;
                }
                else
                {
                    if (letter == false)
                    {
                        letter = true;
                        start = i;
                    }
                    break;
                }
            }
            if ((counter == image.Height) && (letter == true))
            {
                Bitmap temp = image.Clone(new Rectangle(start, 0, i - start, counter), System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                letters.Add(temp);
                letter = false;
            }
        }

        sw.Stop();
        //MessageBox.Show(sw.Elapsed.ToString());
    }

    public Bitmap FirstLetter()
    {
        //find first letter
        bool letter = false;
        int start = 0;
        for (int i = 0; i < image.Width; i++)
        {
            int counter = 0;
            for (int j = 0; j < image.Height; j++)
            {
                if (image.GetPixel(i, j).GetBrightness() >= theta)/////white
                {
                    counter++;
                }
                else
                {
                    if (letter == false)
                    {
                        letter = true;
                        start = i;
                    }
                    break;
                }
            }
            if ((counter == image.Height) && (letter == true))
            {
                return image.Clone(new Rectangle(start, 0, i - start, counter), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            }
        }
        return null;
    }

    public Bitmap LastLetter()
    {
        //find first letter
        bool letter = false;
        int start = 0;
        for (int i = image.Width-1; i >= 0; i--)
        {
            int counter = 0;
            for (int j = 0; j < image.Height; j++)
            {
                if (image.GetPixel(i, j).GetBrightness() >= theta)/////white
                {
                    counter++;
                }
                else
                {
                    if (letter == false)
                    {
                        letter = true;
                        start = i;
                    }
                    break;
                }
            }
            if ((counter == image.Height) && (letter == true))
            {
                return image.Clone(new Rectangle(i + 1, 0, start - i, counter), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            }
        }
        return null;
    }

    public void SetImageHorizontally()
    {
        for (int i = 0; i < image.Height; i++)
        {
            for (int j = 0; j < image.Width; j++)
            {
                if (image.GetPixel(j, i).GetBrightness() <= theta)//black
                {

                }
            }
        }
    }

    int[,] mat = new int[1000, 180];
    public Bitmap Haf()
    {
        for (int i = 0; i < 1000; i++)
        {
            for (int j = 0; j < 180; j++)
            {
                mat[i,j] = 0;
            }
        }


        for (int i = 0; i < image.Height; i++)
        {
            for (int j = 0; j < image.Width; j++)
            {
                if (image.GetPixel(j, i).GetBrightness() <= 0.3)//black
                {
                    for (int n = 0; n < 800; n++)
                    {
                        for (int m = 0; m < 180; m++)
                        {
                            double teta = (double)m / 180 * Math.PI;
                            if (Math.Abs(((i + 1) * Math.Sin(teta) + (j + 1) * Math.Cos(teta)) - n) < 0.1)
                            {
                                mat[n, m] += 1;
                            }
                        }
                    }
                }
            }
        }

        Bitmap b = new Bitmap(900, 180);

        for (int i = 0; i < 900; i++)
        {
            for (int j = 0; j < 180; j++)
            {
                if (mat[i, j] > 255)
                {
                    b.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                }
                else
                {
                    b.SetPixel(j, i, Color.FromArgb(mat[i, j], mat[i, j], mat[i, j]));
                }
            }
        }

        return b;
    }

    private void RotateImage(float angle)
    {
        Bitmap res = new Bitmap(image.Width, image.Height);
        Graphics g = Graphics.FromImage(res);
        g.Clear(Color.White);
        float tx = (float)image.Width / 2;
        float ty = (float)image.Height / 2;
        g.TranslateTransform(tx, ty);
        g.RotateTransform(angle);
        g.TranslateTransform(-tx, -ty);
        g.DrawImage(image, 0, 0, image.Width, image.Height);

        image = res;
        //return res;
    }

    const double theta = 0.3;
    public void Binaryzation1()//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        for (int i = 0; i < image.Height; i++)
        {
            for (int j = 0; j < image.Width; j++)
            {
                if (image.GetPixel(j, i).GetBrightness() <= theta)//black
                {
                    image.SetPixel(j, i, Color.Black);
                }
                else
                {
                    image.SetPixel(j, i, Color.White);
                }
            }
        }
    }

    public void Binaryzation()//ABCDEFGHIJKLMNOPQRSTUVWXYZ Y = 0.299*R + 0.587*G + 0.114*B
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        float[,] tresholds = FindTreshold(6);
        
        for (int i = 0; i < image.Height; i++)
        {
            for (int j = 0; j < image.Width; j++)
            {
                if (image.GetPixel(j, i).GetBrightness() <= tresholds[j,i])//black
                {
                    image.SetPixel(j, i, Color.Black);
                }
                else
                {
                    image.SetPixel(j, i, Color.White);
                }
            }
        }

        sw.Stop();
        MessageBox.Show(sw.ElapsedMilliseconds.ToString());
    }

    private float[,] FindTreshold(int ambitRadius)
    {
        int imageWidth=image.Width;
        int imageHeight=image.Height;

        List<float> temp=new List<float>();
        float[,] tresholds = new float[imageWidth, imageHeight];
        //int half = ambit / 2;

        for (int i = 0; i < imageHeight; i++)
        {
            for (int j = 0; j < imageWidth; j++)
            {
                temp.Clear();
                for (int n = i - ambitRadius; n <= i + ambitRadius; n++)
                {
                    for (int m = j - ambitRadius; m <= j + ambitRadius; m++)
                    {
                        if ((n >= 0) && (m >= 0) && (n < imageHeight) && (m < imageWidth))
                        {
                            temp.Add(image.GetPixel(m,n).GetBrightness());
                        }
                    }
                }
                tresholds[j, i] = 3*(temp.Max() - temp.Min()) / 5;
            }
        }

        return tresholds;
    }

    public void Binaryzation3()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        float[,] tresholds = FindTreshold3(6);

        for (int i = 0; i < image.Height; i++)
        {
            for (int j = 0; j < image.Width; j++)
            {
                if (image.GetPixel(j, i).GetBrightness() <= tresholds[j, i])//black
                {
                    image.SetPixel(j, i, Color.Black);
                }
                else
                {
                    image.SetPixel(j, i, Color.White);
                }
            }
        }

        sw.Stop();
        MessageBox.Show(sw.ElapsedMilliseconds.ToString());
    }

    private float[,] FindTreshold3(int ambitRadius)
    {
        int imageWidth = image.Width;
        int imageHeight = image.Height;

        List<float> temp = new List<float>();
        List<float> temp2 = new List<float>();
        float[,] tresholds = new float[imageWidth, imageHeight];
        //int half = ambit / 2;

        for (int i = 0; i < imageHeight; i++)
        {
            for (int j = 0; j < imageWidth; j++)
            {
                temp.Clear();
                for (int n = i - ambitRadius; n <= i + ambitRadius; n++)
                {
                    for (int m = j - ambitRadius; m <= j + ambitRadius; m++)
                    {
                        if ((n >= 0) && (m >= 0) && (n < imageHeight) && (m < imageWidth))
                        {
                            float brightless = image.GetPixel(m, n).GetBrightness();
                            temp.Add(brightless);
                            temp2.Add(brightless * brightless);
                        }
                    }
                }
                float avge=temp.Average();
                float d = temp2.Average() - avge * avge;
                tresholds[j, i] = avge + 0.3f * d;
            }
        }

        return tresholds;
    }

    private int Brightless(int x, int y)
    {
        int i = (int)(0.299 * image.GetPixel(x, y).R + 0.587 * image.GetPixel(x, y).G + 0.114 * image.GetPixel(x, y).B) / 3;
        //int i = (int)(image.GetPixel(x, y).R + image.GetPixel(x, y).G + image.GetPixel(x, y).B) / 3;
        return i;
    }

    public void AdaptiveTreshold(int s, int t)
    {
        int imageWidth = image.Width;
        int imageHeight = image.Height;
        int[,] intImage = new int[imageWidth,imageHeight];
        int sum;
        for (int i = 0; i < imageHeight; i++)
        {
            sum = 0;
            for (int j = 0; j < imageWidth; j++)
            {
                sum += Brightless(j, i);
                if (i == 0)
                {
                    intImage[j, i] = sum;
                }
                else
                {
                    intImage[j, i] = intImage[j, i - 1] + sum;
                }
            }
        }

        for (int i = 0; i < imageHeight; i++)
        {
            for (int j = 0; j < imageWidth; j++)
            {
                int x1 = j - s / 2;
                int x2 = j + s / 2;
                int y1 = i - s / 2;
                int y2 = i + s / 2;

                if (x1 < 0)
                    x1 = 0;

                if (y1 < 0)
                    y1 = 0;

                if (x2 >= imageWidth)
                    x2 = imageWidth - 1;

                if (y2 >= imageHeight)
                    y2 = imageHeight - 1;

                int count = (x2 - x1) * (y2 - y1);
                int summa;
                if ((x1 - 1 < 0) && (y1 - 1 < 0))
                {
                    //summa = intImage[y2, x2] - intImage[0,x2] - intImage[y2,0] + intImage[0, 0];
                    summa = intImage[x2, y2] - intImage[x2, 0] - intImage[0, y2] + intImage[0, 0];
                }
                else if (x1 - 1 < 0)
                {
                    //summa = intImage[y2, x2] - intImage[ y1 - 1,x2] - intImage[ y2,0] + intImage[ y1 - 1,0];
                    summa = intImage[x2, y2] - intImage[x2, y1 - 1] - intImage[0, y2] + intImage[0, y1 - 1];
                }
                else if (y1 - 1 < 0)
                {
                    //summa = intImage[y2, x2] - intImage[0,x2] - intImage[y2,x1 - 1] + intImage[0,x1 - 1];
                    summa = intImage[x2, y2] - intImage[x2, 0] - intImage[x1 - 1, y2] + intImage[x1 - 1, 0];
                }
                else
                {
                    //summa = intImage[y2, x2] - intImage[y1 - 1, x2] - intImage[y2, x1 - 1] + intImage[y1 - 1, x1 - 1];
                    summa = intImage[x2, y2] - intImage[x2, y1 - 1] - intImage[x1 - 1, y2] + intImage[x1 - 1, y1 - 1];
                }

                float f = summa * (float)(100 - t) / (float)100;
                if (Brightless(j, i) * count <= f)
                {
                    image.SetPixel(j, i, Color.Black);
                }
                else
                {
                    image.SetPixel(j, i, Color.White);
                }
            }
        }
    }

    

    public void DeleteSinglePixels()
    {
        Bitmap temp = new Bitmap(image);
        for (int i = 1; i < image.Height-1; i++)
        {
            for (int j = 1; j < image.Width-1; j++)
            {
                if ((image.GetPixel(j, i).GetBrightness() <= theta) &&//black
                    (image.GetPixel(j + 1, i).GetBrightness() > theta) &&
                    (image.GetPixel(j - 1, i).GetBrightness() > theta) &&
                    (image.GetPixel(j, i + 1).GetBrightness() > theta) &&
                    (image.GetPixel(j, i - 1).GetBrightness() > theta) &&
                    (image.GetPixel(j + 1, i + 1).GetBrightness() > theta) &&
                    (image.GetPixel(j - 1, i - 1).GetBrightness() > theta) &&
                    (image.GetPixel(j + 1, i - 1).GetBrightness() > theta) &&
                    (image.GetPixel(j - 1, i + 1).GetBrightness() > theta))
                {
                    temp.SetPixel(j, i, Color.White);
                }
            }
        }
        image = temp;
    }

    public void FillSinglePixels()
    {
        Bitmap temp = new Bitmap(image);
        for (int i = 1; i < image.Height - 1; i++)
        {
            for (int j = 1; j < image.Width - 1; j++)
            {
                if ((image.GetPixel(j, i).GetBrightness() > theta) &&
                    (image.GetPixel(j + 1, i).GetBrightness()<= theta) &&
                    (image.GetPixel(j - 1, i).GetBrightness() <= theta) &&
                    (image.GetPixel(j, i + 1).GetBrightness() <= theta) &&
                    (image.GetPixel(j, i - 1).GetBrightness() <= theta) &&
                    (image.GetPixel(j + 1, i + 1).GetBrightness() <= theta) &&
                    (image.GetPixel(j - 1, i - 1).GetBrightness() <= theta) &&
                    (image.GetPixel(j + 1, i - 1).GetBrightness() <= theta) &&
                    (image.GetPixel(j - 1, i + 1).GetBrightness() <= theta))
                {
                    temp.SetPixel(j, i, Color.Black);
                }
            }
        }
        image = temp;
    }

    public void Erosion()
    {
        Bitmap temp = new Bitmap(image);
        for (int i = 1; i < image.Height - 1; i++)
        {
            for (int j = 1; j < image.Width - 1; j++)
            {
                if (((image.GetPixel(j, i).GetBrightness() <= theta)) &&
                    ((image.GetPixel(j, i+1).GetBrightness() > theta) ||
                    //(image.GetPixel(j , i-1).GetBrightness() > theta) ||
                    (image.GetPixel(j + 1, i ).GetBrightness() > theta)))
                {
                    temp.SetPixel(j, i, Color.White);
                }
            }
        }
        image = temp;
    }

    public void Extension()
    {
        Bitmap temp = new Bitmap(image);
        for (int i = 1; i < image.Height - 1; i++)
        {
            for (int j = 1; j < image.Width - 1; j++)
            {
                if (((image.GetPixel(j, i).GetBrightness() > theta)) &&
                    ((image.GetPixel(j , i+1).GetBrightness() <= theta) ||
                    //(image.GetPixel(j - 1, i).GetBrightness() <= theta) ||
                    (image.GetPixel(j + 1, i).GetBrightness() <= theta)))
                {
                    temp.SetPixel(j, i, Color.Black);
                }
            }
        }
        image = temp;
    }
}
