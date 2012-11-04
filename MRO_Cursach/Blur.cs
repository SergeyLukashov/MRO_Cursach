using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MRO_Cursach
{
    class Blur
    {
        public Bitmap GaussianBlur5(Bitmap image)//1221 3.jpg
        {
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            Bitmap firstStepRezult = new Bitmap(imageWidth, imageHeight);
            double[] gaussArray = new double[] { 0.028087, 0.23431, 0.475207, 0.23431, 0.028087 };
            int[,] rMatrix = FillRedMatrix5(image);
            int[,] gMatrix = FillGreenMatrix5(image);
            int[,] bMatrix = FillBlueMatrix5(image);
            for (int i = 2; i < imageHeight + 2; i++)
            {
                for (int j = 2; j < imageWidth + 2; j++)
                {
                    double r = rMatrix[j - 2, i] * gaussArray[0] + rMatrix[j - 1, i] * gaussArray[1] +
                        rMatrix[j, i] * gaussArray[2] + rMatrix[j + 1, i] * gaussArray[3] +
                        rMatrix[j + 2, i] * gaussArray[4];
                    double g = gMatrix[j - 2, i] * gaussArray[0] + gMatrix[j - 1, i] * gaussArray[1] +
                        gMatrix[j, i] * gaussArray[2] + gMatrix[j + 1, i] * gaussArray[3] +
                        gMatrix[j + 2, i] * gaussArray[4];
                    double b = bMatrix[j - 2, i] * gaussArray[0] + bMatrix[j - 1, i] * gaussArray[1] +
                        bMatrix[j, i] * gaussArray[2] + bMatrix[j + 1, i] * gaussArray[3] +
                        bMatrix[j + 2, i] * gaussArray[4];
                    firstStepRezult.SetPixel(j - 2, i - 2, Color.FromArgb((int)r, (int)g, (int)b));
                }
            }

            rMatrix = FillRedMatrix5(firstStepRezult);
            gMatrix = FillGreenMatrix5(firstStepRezult);
            bMatrix = FillBlueMatrix5(firstStepRezult);
            Bitmap secondStepRezult = new Bitmap(imageWidth, imageHeight);
            for (int i = 2; i < imageHeight + 2; i++)
            {
                for (int j = 2; j < imageWidth + 2; j++)
                {
                    double r = rMatrix[j, i - 2] * gaussArray[0] + rMatrix[j, i - 1] * gaussArray[1] +
                        rMatrix[j, i] * gaussArray[2] + rMatrix[j, i + 1] * gaussArray[3] +
                        rMatrix[j, i + 2] * gaussArray[4];
                    double g = gMatrix[j, i - 2] * gaussArray[0] + gMatrix[j, i - 2] * gaussArray[1] +
                        gMatrix[j, i] * gaussArray[2] + gMatrix[j, i + 1] * gaussArray[3] +
                        gMatrix[j, i + 2] * gaussArray[4];
                    double b = bMatrix[j, i - 2] * gaussArray[0] + bMatrix[j, i - 1] * gaussArray[1] +
                        bMatrix[j, i] * gaussArray[2] + bMatrix[j, i + 1] * gaussArray[3] +
                        bMatrix[j, i + 2] * gaussArray[4];
                    secondStepRezult.SetPixel(j - 2, i - 2, Color.FromArgb((int)r, (int)g, (int)b));
                }
            }
            return secondStepRezult;
        }

        private int[,] FillRedMatrix5(Bitmap image)
        {
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            int[,] bm = new int[imageWidth + 4, imageHeight + 4];
            bm = ClearMatrix(bm);
            for (int i = 2; i < imageHeight + 2; i++)
            {
                for (int j = 2; j < imageWidth + 2; j++)
                {
                    bm[j, i] = (int)image.GetPixel(j - 2, i - 2).R;
                }
            }

            return bm;
        }

        private int[,] FillGreenMatrix5(Bitmap image)
        {
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            int[,] bm = new int[imageWidth + 4, imageHeight + 4];
            bm = ClearMatrix(bm);
            for (int i = 2; i < imageHeight + 2; i++)
            {
                for (int j = 2; j < imageWidth + 2; j++)
                {
                    bm[j, i] = (int)image.GetPixel(j - 2, i - 2).G;
                }
            }

            return bm;
        }

        private int[,] FillBlueMatrix5(Bitmap image)
        {
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            int[,] bm = new int[imageWidth + 4, imageHeight + 4];
            bm = ClearMatrix(bm);
            for (int i = 2; i < imageHeight + 2; i++)
            {
                for (int j = 2; j < imageWidth + 2; j++)
                {
                    bm[j, i] = (int)image.GetPixel(j - 2, i - 2).B;
                }
            }

            return bm;
        }


        public Bitmap GaussianBlur7(Bitmap image)//1092 3.jpg
        {
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            double[] gaussArray = new double[] { 0.00081723, 0.02804153, 0.23392642, 0.47443, 0.23392642, 0.02804153, 0.00081723 };
            int[,] rMatrix = FillRedMatrix7(image);
            int[,] gMatrix = FillGreenMatrix7(image);
            int[,] bMatrix = FillBlueMatrix7(image);
            for (int i = 3; i < imageHeight + 3; i++)
            {
                for (int j = 3; j < imageWidth + 3; j++)
                {
                    rMatrix[j, i] = (int)(rMatrix[j - 3, i] * gaussArray[0] + rMatrix[j - 2, i] * gaussArray[1] + rMatrix[j - 1, i] * gaussArray[2] +
                        rMatrix[j, i] * gaussArray[3] + rMatrix[j + 1, i] * gaussArray[4] +
                        rMatrix[j + 2, i] * gaussArray[5] + rMatrix[j + 3, i] * gaussArray[6]);
                    gMatrix[j, i] = (int)(gMatrix[j - 3, i] * gaussArray[0] + gMatrix[j - 2, i] * gaussArray[1] + gMatrix[j - 1, i] * gaussArray[2] +
                        gMatrix[j, i] * gaussArray[3] + gMatrix[j + 1, i] * gaussArray[4] +
                        gMatrix[j + 2, i] * gaussArray[5] + gMatrix[j + 3, i] * gaussArray[6]);
                    bMatrix[j, i] = (int)(bMatrix[j - 3, i] * gaussArray[0] + bMatrix[j - 2, i] * gaussArray[1] + bMatrix[j - 1, i] * gaussArray[2] +
                        bMatrix[j, i] * gaussArray[3] + bMatrix[j + 1, i] * gaussArray[4] +
                        bMatrix[j + 2, i] * gaussArray[5] + bMatrix[j + 3, i] * gaussArray[6]);
                }
            }

            Bitmap rezult = new Bitmap(imageWidth, imageHeight);
            double r;
            double g;
            double b;
            for (int i = 3; i < imageHeight + 3; i++)
            {
                for (int j = 3; j < imageWidth + 3; j++)
                {
                    r = rMatrix[j, i - 3] * gaussArray[0] + rMatrix[j, i - 2] * gaussArray[1] + rMatrix[j, i - 1] * gaussArray[2] +
                       rMatrix[j, i] * gaussArray[3] + rMatrix[j, i + 1] * gaussArray[4] +
                       rMatrix[j, i + 2] * gaussArray[5] + rMatrix[j, i + 3] * gaussArray[6];
                    g = gMatrix[j, i - 3] * gaussArray[0] + gMatrix[j, i - 2] * gaussArray[1] + gMatrix[j, i - 1] * gaussArray[2] +
                       gMatrix[j, i] * gaussArray[3] + gMatrix[j, i + 1] * gaussArray[4] +
                       gMatrix[j, i + 2] * gaussArray[5] + gMatrix[j, i + 3] * gaussArray[6];
                    b = bMatrix[j, i - 3] * gaussArray[0] + bMatrix[j, i - 2] * gaussArray[1] + bMatrix[j, i - 1] * gaussArray[2] +
                       bMatrix[j, i] * gaussArray[3] + bMatrix[j, i + 1] * gaussArray[4] +
                       bMatrix[j, i + 2] * gaussArray[5] + bMatrix[j, i + 3] * gaussArray[6];
                    rezult.SetPixel(j - 3, i - 3, Color.FromArgb((int)r, (int)g, (int)b));
                }
            }
            return rezult;
        }

        private int[,] FillRedMatrix7(Bitmap image)
        {
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            int[,] bm = new int[imageWidth + 6, imageHeight + 6];
            bm = ClearMatrix(bm);
            for (int i = 3; i < imageHeight + 3; i++)
            {
                for (int j = 3; j < imageWidth + 3; j++)
                {
                    bm[j, i] = (int)image.GetPixel(j - 3, i - 3).R;
                }
            }

            return bm;
        }

        private int[,] FillGreenMatrix7(Bitmap image)
        {
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            int[,] bm = new int[imageWidth + 6, imageHeight + 6];
            bm = ClearMatrix(bm);
            for (int i = 3; i < imageHeight + 3; i++)
            {
                for (int j = 3; j < imageWidth + 3; j++)
                {
                    bm[j, i] = (int)image.GetPixel(j - 3, i - 3).G;
                }
            }

            return bm;
        }

        private int[,] FillBlueMatrix7(Bitmap image)
        {
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            int[,] bm = new int[imageWidth + 6, imageHeight + 6];
            bm = ClearMatrix(bm);
            for (int i = 3; i < imageHeight + 3; i++)
            {
                for (int j = 3; j < imageWidth + 3; j++)
                {
                    bm[j, i] = (int)image.GetPixel(j - 3, i - 3).B;
                }
            }

            return bm;
        }

        private int[,] ClearMatrix(int[,] matrix)
        {
            int[,] result = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = 255;
                }
            }
            return result;
        }
    }
}
