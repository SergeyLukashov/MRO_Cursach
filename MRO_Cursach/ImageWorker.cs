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
    public ImageWorker()
    {
    }

    private int[,] FillBrightnessMatrix(Image image)
    {
        Bitmap bitmapImage = (Bitmap)image;
        int imageWidth = bitmapImage.Width;
        int imageHeight = bitmapImage.Height;
        int[,] bm = new int[imageWidth, imageHeight];
        Array.Clear(bm, 0, imageWidth * imageHeight);

        for (int i = 0; i < imageHeight; i++)
        {
            for (int j = 0; j < imageWidth; j++)
            {
                if (bitmapImage.GetPixel(j, i).GetBrightness() == 0)
                {
                    bm[j, i] = 1;
                }
                else
                {
                    bm[j, i] = 0;
                }
            }
        }
        return bm;
    }

    public List<Image> SplitToLetters(Image image)
    {
        List<Image> letters = new List<Image>();
        int[,] brigthnessMatrix = FillBrightnessMatrix(image);
        int imageWidth = image.Width;
        int imageHeight = image.Height;
        Bitmap bitmapImage = (Bitmap)image;
        bool isLetter = false;
        int start = 0;
        for (int i = 0; i < imageWidth; i++)
        {
            int counter = 0;
            for (int j = 0; j < imageHeight; j++)
            {
                if (brigthnessMatrix[i, j] == 0)/////white
                {
                    counter++;
                }
                else
                {
                    if (isLetter == false)
                    {
                        isLetter = true;
                        start = i;
                    }
                    break;
                }
            }
            if ((counter == imageHeight) && (isLetter == true))
            {
                Bitmap temp = bitmapImage.Clone(new Rectangle(start, 0, i - start, counter), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                letters.Add(temp);
                isLetter = false;
            }
        }
        return letters;
    }

    public Image FirstLetter(Image image)
    {
        int[,] brigthnessMatrix = FillBrightnessMatrix(image);
        int imageWidth = image.Width;
        int imageHeight = image.Height;
        Bitmap bitmapImage = (Bitmap)image;
        bool isLetter = false;
        int start = 0;
        for (int i = 0; i < imageWidth; i++)
        {
            int counter = 0;
            for (int j = 0; j < imageHeight; j++)
            {
                if (brigthnessMatrix[i, j] == 0)
                {
                    counter++;
                }
                else
                {
                    if (isLetter == false)
                    {
                        isLetter = true;
                        start = i;
                    }
                    break;
                }
            }
            if ((counter == imageHeight) && (isLetter == true))
            {
                return bitmapImage.Clone(new Rectangle(start, 0, i - start, counter), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            }
        }
        return null;
    }

    public Image LastLetter(Image image)
    {
        int[,] brigthnessMatrix = FillBrightnessMatrix(image);
        int imageWidth = image.Width;
        int imageHeight = image.Height;
        Bitmap bitmapImage = (Bitmap)image;
        bool isLetter = false;
        int start = 0;
        for (int i = imageWidth - 1; i >= 0; i--)
        {
            int counter = 0;
            for (int j = 0; j < imageHeight; j++)
            {
                if (brigthnessMatrix[i, j] == 0)//white
                {
                    counter++;
                }
                else
                {
                    if (isLetter == false)
                    {
                        isLetter = true;
                        start = i;
                    }
                    break;
                }
            }
            if ((counter == image.Height) && (isLetter == true))
            {
                return bitmapImage.Clone(new Rectangle(i + 1, 0, start - i, counter), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            }
        }
        return null;
    }

    public void SetImageHorizontally()
    {
        
    }

    private Image RotateImage(Image image, float angle)
    {
        Bitmap result = new Bitmap(image.Width, image.Height);
        Graphics g = Graphics.FromImage(result);
        g.Clear(Color.White);
        float tx = (float)image.Width / 2;
        float ty = (float)image.Height / 2;
        g.TranslateTransform(tx, ty);
        g.RotateTransform(angle);
        g.TranslateTransform(-tx, -ty);
        g.DrawImage(image, 0, 0, image.Width, image.Height);

        return result;
    }

    public Image DeleteSinglePixels(Image image)
    {
        int[,] brigthnessMatrix = FillBrightnessMatrix(image);
        int imageWidth = image.Width;
        int imageHeight = image.Height;
        Bitmap bitmapImage = (Bitmap)image;
        Bitmap result = new Bitmap(image);
        for (int i = 1; i < imageHeight - 1; i++)
        {
            for (int j = 1; j < imageWidth - 1; j++)
            {
                if ((brigthnessMatrix[j, i] == 1) &&//black
                    (brigthnessMatrix[j + 1, i] == 0) &&
                    (brigthnessMatrix[j - 1, i] == 0) &&
                    (brigthnessMatrix[j, i + 1] == 0) &&
                    (brigthnessMatrix[j, i - 1] == 0) &&
                    (brigthnessMatrix[j + 1, i + 1] == 0) &&
                    (brigthnessMatrix[j - 1, i - 1] == 0) &&
                    (brigthnessMatrix[j + 1, i - 1] == 0) &&
                    (brigthnessMatrix[j - 1, i + 1] == 0))
                {
                    result.SetPixel(j, i, Color.White);
                }
            }
        }
        return result;
    }

    public Image FillSinglePixels(Image image)
    {
        int[,] brigthnessMatrix = FillBrightnessMatrix(image);
        int imageWidth = image.Width;
        int imageHeight = image.Height;
        Bitmap bitmapImage = (Bitmap)image;
        Bitmap result = new Bitmap(image);
        for (int i = 1; i < imageHeight - 1; i++)
        {
            for (int j = 1; j < imageWidth - 1; j++)
            {
                if ((brigthnessMatrix[j, i] == 0) &&//white
                    (brigthnessMatrix[j + 1, i] == 1) &&
                    (brigthnessMatrix[j - 1, i] == 1) &&
                    (brigthnessMatrix[j, i + 1] == 1) &&
                    (brigthnessMatrix[j, i - 1] == 1) &&
                    (brigthnessMatrix[j + 1, i + 1] == 1) &&
                    (brigthnessMatrix[j - 1, i - 1] == 1) &&
                    (brigthnessMatrix[j + 1, i - 1] == 1) &&
                    (brigthnessMatrix[j - 1, i + 1] == 1))
                {
                    result.SetPixel(j, i, Color.Black);
                }
            }
        }
        return result;
    }

    public Image Erosion(Image image)
    {
        int[,] brigthnessMatrix = FillBrightnessMatrix(image);
        int imageWidth = image.Width;
        int imageHeight = image.Height;
        Bitmap bitmapImage = (Bitmap)image;
        Bitmap result = new Bitmap(image);
        for (int i = 1; i < imageHeight - 1; i++)
        {
            for (int j = 1; j < imageWidth - 1; j++)
            {
                if (((brigthnessMatrix[j, i] == 1)) &&
                    ((brigthnessMatrix[j, i + 1] == 0) ||
                    //(brigthnessMatrix[j, i - 1] == 0) ||
                    (brigthnessMatrix[j + 1, i] == 0)))
                {
                    result.SetPixel(j, i, Color.White);
                }
            }
        }
        return result;
    }

    public Image Extension(Image image)
    {
        int[,] brigthnessMatrix = FillBrightnessMatrix(image);
        int imageWidth = image.Width;
        int imageHeight = image.Height;
        Bitmap bitmapImage = (Bitmap)image;
        Bitmap result = new Bitmap(image);
        for (int i = 1; i < imageHeight - 1; i++)
        {
            for (int j = 1; j < imageWidth - 1; j++)
            {
                if (((brigthnessMatrix[j, i] == 0)) &&
                    ((brigthnessMatrix[j, i + 1] == 1) ||
                    //(brigthnessMatrix[j, i - 1] ==1) ||
                    //(brigthnessMatrix[j - 1, i] == 1) ||
                    (brigthnessMatrix[j + 1, i] == 1)))
                {
                    result.SetPixel(j, i, Color.Black);
                }
            }
        }
        return result;
    }
}
