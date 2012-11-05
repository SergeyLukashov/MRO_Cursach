using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MRO_Cursach
{
    class Skeletonizator
    {
        public Skeletonizator()
        {

        }

        int[,] brightnessMatrix;

        public Bitmap TemplateMethod(Bitmap image)
        {
            int imageHeight = image.Height;
            int imageWidth = image.Width;
            Bitmap rez = new Bitmap(image);

            brightnessMatrix = FillBrightnessMatrix(image);

            for (int i = 1; i < imageHeight + 1; i++)
            {
                for (int j = 1; j < imageWidth + 1; j++)
                {
                    if ((brightnessMatrix[j, i] == 0) && (IsDeletable1(j, i)))
                    {
                        rez.SetPixel(j - 1, i - 1, Color.White);
                        brightnessMatrix[j, i] = 1;
                    }
                }
            }

            for (int i = 1; i < imageHeight + 1; i++)
            {
                for (int j = 1; j < imageWidth + 1; j++)
                {
                    if ((brightnessMatrix[j, i] == 0) && (IsDeletable2(j, i)))
                    {
                        rez.SetPixel(j - 1, i - 1, Color.White);
                        brightnessMatrix[j, i] = 1;
                    }
                }
            }

            return rez;
        }

        private bool IsDeletable1(int x, int y)
        {
            if ((brightnessMatrix[x - 1, y] == 0) && (brightnessMatrix[x, y + 1] == 0) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x + 1, y] == 1) && (brightnessMatrix[x + 1, y - 1] == 1)) return true;
            if ((brightnessMatrix[x - 1, y] == 0) && (brightnessMatrix[x, y - 1] == 0) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x + 1, y] == 1) && (brightnessMatrix[x + 1, y + 1] == 1)) return true;
            if ((brightnessMatrix[x + 1, y] == 0) && (brightnessMatrix[x, y - 1] == 0) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x - 1, y + 1] == 1)) return true;
            if ((brightnessMatrix[x + 1, y] == 0) && (brightnessMatrix[x, y + 1] == 0) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x - 1, y - 1] == 1)) return true;

            if ((brightnessMatrix[x + 1, y] == 0) && (brightnessMatrix[x - 1, y] == 0) && (brightnessMatrix[x, y + 1] == 0) && (brightnessMatrix[x - 1, y - 1] == 1) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x + 1, y - 1] == 1)) return true;
            if ((brightnessMatrix[x, y + 1] == 0) && (brightnessMatrix[x, y - 1] == 0) && (brightnessMatrix[x - 1, y] == 0) && (brightnessMatrix[x + 1, y - 1] == 1) && (brightnessMatrix[x + 1, y] == 1) && (brightnessMatrix[x + 1, y + 1] == 1)) return true;
            if ((brightnessMatrix[x, y + 1] == 0) && (brightnessMatrix[x, y - 1] == 0) && (brightnessMatrix[x + 1, y] == 0) && (brightnessMatrix[x - 1, y - 1] == 1) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x - 1, y + 1] == 1)) return true;
            if ((brightnessMatrix[x + 1, y] == 0) && (brightnessMatrix[x - 1, y] == 0) && (brightnessMatrix[x, y - 1] == 0) && (brightnessMatrix[x - 1, y + 1] == 1) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x + 1, y + 1] == 1)) return true;


            return false;
        }

        private bool IsDeletable2(int x, int y)
        {
            if ((brightnessMatrix[x + 1, y] == 1) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x - 1, y - 1] == 1) && (brightnessMatrix[x + 1, y - 1] == 1) && (brightnessMatrix[x + 1, y + 1] == 1) && (brightnessMatrix[x - 1, y + 1] == 1)) return true;

            if ((brightnessMatrix[x + 1, y + 1] == 0) && (brightnessMatrix[x, y + 1] == 0) && (brightnessMatrix[x - 1, y + 1] == 0) && (brightnessMatrix[x + 1, y] == 1) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x - 1, y - 1] == 1) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x + 1, y - 1] == 1)) return true;
            if ((brightnessMatrix[x + 1, y] == 0) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x - 1, y - 1] == 1) && (brightnessMatrix[x + 1, y - 1] == 0) && (brightnessMatrix[x + 1, y + 1] == 0) && (brightnessMatrix[x - 1, y + 1] == 1)) return true;
            if ((brightnessMatrix[x + 1, y] == 1) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x, y - 1] == 0) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x - 1, y - 1] == 0) && (brightnessMatrix[x + 1, y - 1] == 0) && (brightnessMatrix[x + 1, y + 1] == 1) && (brightnessMatrix[x - 1, y + 1] == 1)) return true;
            if ((brightnessMatrix[x + 1, y] == 1) && (brightnessMatrix[x - 1, y] == 0) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x - 1, y - 1] == 0) && (brightnessMatrix[x + 1, y - 1] == 1) && (brightnessMatrix[x + 1, y + 1] == 1) && (brightnessMatrix[x - 1, y + 1] == 0)) return true;

            if ((brightnessMatrix[x + 1, y + 1] == 1) && (brightnessMatrix[x, y + 1] == 0) && (brightnessMatrix[x - 1, y + 1] == 0) && (brightnessMatrix[x + 1, y] == 1) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x - 1, y - 1] == 1) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x + 1, y - 1] == 1)) return true;
            if ((brightnessMatrix[x + 1, y] == 0) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x - 1, y - 1] == 1) && (brightnessMatrix[x + 1, y - 1] == 1) && (brightnessMatrix[x + 1, y + 1] == 0) && (brightnessMatrix[x - 1, y + 1] == 1)) return true;
            if ((brightnessMatrix[x + 1, y] == 1) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x, y - 1] == 0) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x - 1, y - 1] == 1) && (brightnessMatrix[x + 1, y - 1] == 0) && (brightnessMatrix[x + 1, y + 1] == 1) && (brightnessMatrix[x - 1, y + 1] == 1)) return true;
            if ((brightnessMatrix[x + 1, y] == 1) && (brightnessMatrix[x - 1, y] == 0) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x - 1, y - 1] == 0) && (brightnessMatrix[x + 1, y - 1] == 1) && (brightnessMatrix[x + 1, y + 1] == 1) && (brightnessMatrix[x - 1, y + 1] == 1)) return true;

            if ((brightnessMatrix[x + 1, y + 1] == 0) && (brightnessMatrix[x, y + 1] == 0) && (brightnessMatrix[x - 1, y + 1] == 1) && (brightnessMatrix[x + 1, y] == 1) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x - 1, y - 1] == 1) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x + 1, y - 1] == 1)) return true;
            if ((brightnessMatrix[x + 1, y] == 0) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x - 1, y - 1] == 1) && (brightnessMatrix[x + 1, y - 1] == 0) && (brightnessMatrix[x + 1, y + 1] == 1) && (brightnessMatrix[x - 1, y + 1] == 1)) return true;
            if ((brightnessMatrix[x + 1, y] == 1) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x, y - 1] == 0) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x - 1, y - 1] == 0) && (brightnessMatrix[x + 1, y - 1] == 1) && (brightnessMatrix[x + 1, y + 1] == 1) && (brightnessMatrix[x - 1, y + 1] == 1)) return true;
            if ((brightnessMatrix[x + 1, y] == 1) && (brightnessMatrix[x - 1, y] == 0) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x - 1, y - 1] == 1) && (brightnessMatrix[x + 1, y - 1] == 1) && (brightnessMatrix[x + 1, y + 1] == 1) && (brightnessMatrix[x - 1, y + 1] == 0)) return true;

            //my masks
            //if ((brightnessMatrix[x + 1, y] == 0) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x - 1, y - 1] == 1) && (brightnessMatrix[x + 1, y - 1] == 1) && (brightnessMatrix[x + 1, y + 1] == 1) && (brightnessMatrix[x - 1, y + 1] == 1)) return true;
            if ((brightnessMatrix[x + 1, y] == 1) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x - 1, y - 1] == 1) && (brightnessMatrix[x + 1, y - 1] == 1) && (brightnessMatrix[x + 1, y + 1] == 0) && (brightnessMatrix[x - 1, y + 1] == 1)) return true;
            if ((brightnessMatrix[x + 1, y] == 1) && (brightnessMatrix[x - 1, y] == 1) && (brightnessMatrix[x, y - 1] == 1) && (brightnessMatrix[x, y + 1] == 1) && (brightnessMatrix[x - 1, y - 1] == 1) && (brightnessMatrix[x + 1, y - 1] == 0) && (brightnessMatrix[x + 1, y + 1] == 1) && (brightnessMatrix[x - 1, y + 1] == 1)) return true;

            return false;
        }

        private int[,] FillBrightnessMatrix(Bitmap image)
        {
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            int[,] bm = new int[imageWidth + 2, imageHeight + 2];
            Array.Clear(bm, 0, (imageWidth + 2) * (imageHeight + 2));

            for (int i = 1; i < imageHeight + 1; i++)
            {
                for (int j = 1; j < imageWidth + 1; j++)
                {
                    if (image.GetPixel(j - 1, i - 1).GetBrightness() == 0)
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

        public Bitmap ZongaSunja(Bitmap image)
        {
            brightnessMatrix = FillBrightnessMatrix(image);
            Bitmap firstStepRezult = new Bitmap(image);
            int imageWidth = image.Width;
            int imageHeight = image.Height;

            for (int i = 1; i < imageHeight + 1; i++)
            {
                for (int j = 1; j < imageWidth + 1; j++)
                {
                    Point currentPoint = new Point(j, i);
                    int[] pixelArray = CreatePixelArray(currentPoint);
                    if (DeletableOnFirstStep(currentPoint, pixelArray))
                    {
                        firstStepRezult.SetPixel(j - 1, i - 1, Color.White);
                    }
                }
            }

            Bitmap secondStepRezult = new Bitmap(firstStepRezult);
            brightnessMatrix = FillBrightnessMatrix(firstStepRezult);
            for (int i = 1; i < imageHeight + 1; i++)
            {
                for (int j = 1; j < imageWidth + 1; j++)
                {
                    Point currentPoint = new Point(j, i);
                    int[] pixelArray = CreatePixelArray(currentPoint);
                    if (DeletableOnSecondStep(currentPoint, pixelArray))
                    {
                        secondStepRezult.SetPixel(j - 1, i - 1, Color.White);
                    }
                }
            }
            return secondStepRezult;
        }

        private bool DeletableOnFirstStep(Point pixel, int[] pixelArray)
        {
            int critheria1 = pixelArray[0] * pixelArray[2] * pixelArray[4];
            int critheria2 = pixelArray[2] * pixelArray[4] * pixelArray[6];
            int b = B(pixel, pixelArray);
            return ((b >= 2) && ((b <= 6)) && (A(pixel, pixelArray) == 1) && (critheria1 == 0) && (critheria2 == 0));
        }

        private bool DeletableOnSecondStep(Point pixel, int[] pixelArray)
        {
            int critheria1 = pixelArray[0] * pixelArray[2] * pixelArray[6];
            int critheria2 = pixelArray[0] * pixelArray[4] * pixelArray[6];
            int b = B(pixel,pixelArray);
            return ((b >= 2) && ((b <= 6)) && (A(pixel,pixelArray) == 1) && (critheria1 == 0) && (critheria2 == 0));
        }

        private int A(Point pixel, int[] pixelArray)
        {
            int transitionCount = 0;
            for (int i = 0; i < 7; i++)
            {
                if ((pixelArray[i] == 0) && (pixelArray[i + 1] == 1))
                {
                    transitionCount++;
                }
            }
            if ((pixelArray[7] == 0) && (pixelArray[0] == 1))
            {
                transitionCount++;
            }
            return transitionCount;
        }

        private int B(Point pixel, int[] pixelArray)
        {
            int transitionCount = 0;
            for (int i = 0; i < 8; i++)
            {
                if (pixelArray[i] == 1)
                {
                    transitionCount++;
                }
            }
            return transitionCount;
        }

        private int[] CreatePixelArray(Point pixel)
        {
            int x = pixel.X;
            int y = pixel.Y;
            return new int[] { brightnessMatrix[x, y-1],
                brightnessMatrix[x+1, y-1],
                brightnessMatrix[x+1, y],
                brightnessMatrix[x+1, y+1],
                brightnessMatrix[x, y+1],
                brightnessMatrix[x-1, y+1],
                brightnessMatrix[x-1, y],
                brightnessMatrix[x-1, y-1]};
        }
    }
}

