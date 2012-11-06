using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MRO_Cursach
{
    class CritheriaFinder
    {
        public int[] TerminalPixelsCount(Image letterImage)
        {
            int[] result = new int[5];
            int[,] brightnessMatrix = FillBrightnessMatrix(letterImage);
            int imageWidth = letterImage.Width;
            int imageHeight = letterImage.Height;

            for (int i = 1; i < imageHeight + 1; i++)
            {
                for (int j = 1; j < imageWidth + 1; j++)
                {
                    if (brightnessMatrix[j, i] == 1)
                    {
                        Point pixelPoint = new Point(j, i);
                        int[] pixelArray = CreatePixelArray(pixelPoint, brightnessMatrix);
                        if (IsTerminalPixel(pixelArray))
                        {
                            int quarter = Quarter(pixelPoint, imageWidth, imageHeight);
                            result[quarter]++;
                            result[0]++;
                        }
                    }
                }
            }
            return result;
        }

        private int Quarter(Point pixel, int width, int height)
        {
            int x = pixel.X;
            int y = pixel.Y;
            int halfWidth = width / 2;
            int halfHeigth = height / 2;
            if ((x < halfWidth) && (y < halfHeigth))
            {
                return 1;
            }
            else if ((x >= halfWidth) && (y < halfHeigth))
            {
                return 2;
            }
            else if ((x < halfWidth) && (y >= halfHeigth))
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }

        public int[] NodalPixelsCount(Image letterImage)
        {
            int[] result = new int[5];
            int[,] brightnessMatrix = FillBrightnessMatrix(letterImage);
            int imageWidth = letterImage.Width;
            int imageHeight = letterImage.Height;

            for (int i = 1; i < imageHeight + 1; i++)
            {
                for (int j = 1; j < imageWidth + 1; j++)
                {
                    if (brightnessMatrix[j, i] == 1)
                    {
                        Point pixelPoint = new Point(j, i);
                        int[] pixelArray = CreatePixelArray(pixelPoint, brightnessMatrix);
                        if (IsNodalPixel(pixelArray))
                        {
                            int quarter = Quarter(pixelPoint, imageWidth, imageHeight);
                            result[quarter]++;
                            result[0]++;
                        }
                    }
                }
            }
            return result;
        }

        private bool IsNodalPixel(int[] neighbors)
        {
            return (Cn(neighbors) >= 3);
        }

        private int A8(int[] neighbors)
        {
            int a8Count = 0;
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] == 1)
                {
                    a8Count++;
                }
            }
            return a8Count;
        }

        private int B8(int[] neighbors)
        {
            int b8Count = 0;
            for (int i = 0; i < neighbors.Length - 1; i++)
            {
                if (neighbors[i] * neighbors[i + 1] == 1)
                {
                    b8Count++;
                }
            }
            if (neighbors[0] * neighbors[7] == 1)
            {
                b8Count++;
            }
            return b8Count;
        }

        private int Cn(int[] neighbors)
        {
            return A8(neighbors) - B8(neighbors);
        }

        private bool IsTerminalPixel(int[] neighbors)
        {
            return (Cn(neighbors) == 1);
        }

        private int[] CreatePixelArray(Point pixel, int[,] brightnessMatrix)
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

        private int[,] FillBrightnessMatrix(Image image)
        {
            Bitmap bitmapImage = (Bitmap)image;
            int imageWidth = bitmapImage.Width;
            int imageHeight = bitmapImage.Height;
            int[,] bm = new int[imageWidth + 2, imageHeight + 2];
            Array.Clear(bm, 0, (imageWidth + 2) * (imageHeight + 2));

            for (int i = 1; i < imageHeight + 1; i++)
            {
                for (int j = 1; j < imageWidth + 1; j++)
                {
                    if (bitmapImage.GetPixel(j - 1, i - 1).GetBrightness() == 0)
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

        public double Ratio(Image image)
        {
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            return (double)imageHeight / (double)imageWidth;
        }

        public double[] Magnitude(Image image)
        {
            double[] result = new double[5];
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            int pixelsCount = imageWidth * imageHeight;
            int[,] brightnessMatrix = FillBrightnessMatrix(image);
            for (int i = 1; i < imageHeight + 1; i++)
            {
                for (int j = 1; j < imageWidth + 1; j++)
                {
                    if (brightnessMatrix[j, i] == 1)
                    {
                        Point pixelPoint = new Point(j, i);
                        int quarter = Quarter(pixelPoint, imageWidth, imageHeight);
                        result[quarter]++;
                        result[0]++;
                    }
                }
            }
            for (int i = 0; i < 5; i++)
            {
                result[i] /= (double)pixelsCount;
            }
            return result;
        }
    }
}
