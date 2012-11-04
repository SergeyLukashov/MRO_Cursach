using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;


public class Binaryzator
{
    public Binaryzator()
    {
    }

    public Bitmap Binaryzation(Bitmap image, int ambitRadius)//8216 567 480
    {
        int imageHeight = image.Height;
        int imageWidth = image.Width;
        Bitmap rez = new Bitmap(imageWidth, imageHeight);
        brightnessMatrix = FillBrightnessMatrix1(image);
        float[] tresholds = FindTreshold(image, ambitRadius);

        for (int i = 0; i < imageHeight; i++)
        {
            for (int j = 0; j < imageWidth; j++)
            {
                if (brightnessMatrix[j, i] <= tresholds[j + i * imageHeight])//black
                {
                    rez.SetPixel(j, i, Color.Black);
                }
                else
                {
                    rez.SetPixel(j, i, Color.White);
                }
            }
        }

        return rez;
    }

    float[,] brightnessMatrix;

    private float[,] FillBrightnessMatrix1(Bitmap image)
    {
        int imageWidth = image.Width;
        int imageHeight = image.Height;
        float[,] bm = new float[imageWidth, imageHeight];

        for (int i = 0; i < imageHeight; i++)
        {
            for (int j = 0; j < imageWidth; j++)
            {
                bm[j, i] = image.GetPixel(j, i).GetBrightness();
            }
        }

        return bm;
    }

    private int[,] FillBrightnessMatrix2(Bitmap image)
    {
        int imageWidth = image.Width;
        int imageHeight = image.Height;
        int[,] bm = new int[imageWidth, imageHeight];

        for (int i = 0; i < imageHeight; i++)
        {
            for (int j = 0; j < imageWidth; j++)
            {
                bm[j, i] = Brightless(image, j, i);
            }
        }

        return bm;
    }

    private float[] FindTreshold(Bitmap image, int ambitRadius)
    {
        int imageWidth = image.Width;
        int imageHeight = image.Height;

        List<float> temp = new List<float>();
        float[] tresholds = new float[imageWidth * imageHeight];


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
                            temp.Add(brightnessMatrix[m, n]);
                        }
                    }
                }
                tresholds[j + i * imageHeight] = 3 * (temp.Max() - temp.Min()) / 5;
            }
        }

        return tresholds;
    }

    private int Brightless(Bitmap image, int x, int y)
    {
        //int i = (int)(0.299 * image.GetPixel(x, y).R + 0.587 * image.GetPixel(x, y).G + 0.114 * image.GetPixel(x, y).B) / 3;
        int i = (int)(image.GetPixel(x, y).R + image.GetPixel(x, y).G + image.GetPixel(x, y).B) / 3;
        return i;
    }

    public Bitmap AdaptiveTreshold(Bitmap image, int s, int t)//3260,3126 673 383
    {
        int[,] brightnessM = FillBrightnessMatrix2(image);

        int half = s / 2;
        float p = (float)(100 - t) / (float)100;

        int imageWidth = image.Width;
        int imageHeight = image.Height;
        Bitmap rez = new Bitmap(imageWidth, imageHeight);
        int[,] intImage = new int[imageWidth, imageHeight];
        int sum;
        for (int i = 0; i < imageHeight; i++)
        {
            sum = 0;
            for (int j = 0; j < imageWidth; j++)
            {
                sum += brightnessM[j, i];
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
        int count;
        int summa;
        for (int i = 0; i < imageHeight; i++)
        {
            for (int j = 0; j < imageWidth; j++)
            {
                int x1 = j - half;
                int x2 = j + half;
                int y1 = i - half;
                int y2 = i + half;

                if (x1 < 0) x1 = 0;

                if (y1 < 0) y1 = 0;

                if (x2 >= imageWidth) x2 = imageWidth - 1;

                if (y2 >= imageHeight) y2 = imageHeight - 1;

                count = (x2 - x1) * (y2 - y1);

                if ((x1 - 1 < 0) && (y1 - 1 < 0))
                {
                    summa = intImage[x2, y2] - intImage[x2, 0] - intImage[0, y2] + intImage[0, 0];
                }
                else if (x1 - 1 < 0)
                {
                    summa = intImage[x2, y2] - intImage[x2, y1 - 1] - intImage[0, y2] + intImage[0, y1 - 1];
                }
                else if (y1 - 1 < 0)
                {
                    summa = intImage[x2, y2] - intImage[x2, 0] - intImage[x1 - 1, y2] + intImage[x1 - 1, 0];
                }
                else
                {
                    summa = intImage[x2, y2] - intImage[x2, y1 - 1] - intImage[x1 - 1, y2] + intImage[x1 - 1, y1 - 1];
                }

                if (brightnessM[j, i] * count <= summa * p)
                {
                    rez.SetPixel(j, i, Color.Black);
                }
                else
                {
                    rez.SetPixel(j, i, Color.White);
                }
            }
        }

        return rez;
    }
}
