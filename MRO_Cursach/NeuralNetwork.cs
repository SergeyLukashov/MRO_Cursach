using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace Perceptron
{
    class NeuralNetwork
    {
        public const int sensorsCount = 225;
        public const int aElementsCount = 100;
        const int summarorsCount = 2;
        Sensor[] sensors = new Sensor[sensorsCount];
        AElement[] aElements = new AElement[aElementsCount];
        int[,] connectionsMatrix = new int[sensorsCount, aElementsCount];

        public DataTable connectionsMatrixTable;
        public DataTable aElementsTable;

        public NeuralNetwork()
        {
            for (int i = 0; i < sensorsCount; i++)
            {
                sensors[i] = new Sensor();
            }
            for (int i = 0; i < aElementsCount; i++)
            {
                aElements[i] = new AElement();
            }

            PrepareTabels();

            //connectionsMatrixTable = ConnectSensors(new FileStream("c_matrix.s", FileMode.Open, FileAccess.Read, FileShare.Read));
            ConnectSensors();
            SetLambdaDefault();
        }

        private void PrepareTabels()
        {
            aElementsTable = new DataTable();
            for (int i = 0; i < aElementsCount; i++)
            {
                aElementsTable.Columns.Add("A" + (i + 1).ToString());
            }
            aElementsTable.Rows.Add();
            aElementsTable.Rows.Add();


            connectionsMatrixTable = new DataTable();
            for (int i = 0; i < aElementsCount; i++)
            {
                connectionsMatrixTable.Columns.Add("A" + (i + 1).ToString());
                connectionsMatrixTable.Columns[i].DefaultValue = 0;
            }
            for (int i = 0; i < sensorsCount; i++)
            {
                connectionsMatrixTable.Rows.Add();
            }
        }

        public void SetSensorsValues(int[] mass)
        {
            for (int i = 0; i < sensorsCount; i++)
            {
                if (sensors[i] != null)
                {
                    sensors[i].State = mass[i];
                }
            }
        }

        public int[] Recognition()
        {
            PrepareAElements();

            return GetOutput();
        }

        private void WriteConnectionsMatrix()
        {
            FileStream fs = new FileStream("c_matrix.s", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, connectionsMatrix);
            fs.Close();
        }

        private DataTable ConnectSensors(FileStream fs)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < aElementsCount; i++)
            {
                dt.Columns.Add("A" + (i + 1).ToString());
                dt.Columns[i].DefaultValue = 0;
            }
            for (int i = 0; i < sensorsCount; i++)
            {
                dt.Rows.Add();
            }

            BinaryFormatter bf = new BinaryFormatter();
            connectionsMatrix = (int[,])bf.Deserialize(fs);

            for (int i = 0; i < sensorsCount; i++)
                for (int j = 0; j < aElementsCount; j++)
                    dt.Rows[i][j] = connectionsMatrix[i, j];

            return dt;
        }

        private void ConnectSensors()//used
        {
            Random rnd = new Random();
            bool[] b = new bool[sensorsCount];

            for (int i = 0; i < aElementsCount; i++)
            {
                b[i] = false;
            }

            for (int i = 0; i < aElementsCount; i++)
            {
                int numberSensor = rnd.Next(0, sensorsCount);
                int temp = rnd.Next(-100, 100);
                if (temp >= 0)
                {
                    connectionsMatrix[numberSensor, i] = 1;
                }
                else
                {
                    connectionsMatrix[numberSensor, i] = -1;
                }
                connectionsMatrixTable.Rows[numberSensor][i] = connectionsMatrix[numberSensor, i];
            }

            for (int i = 0; i < sensorsCount; i++)
            {
                bool iSensorConnected = false;
                for (int j = 0; j < aElementsCount; j++)
                {
                    if (connectionsMatrix[i, j] != 0)
                    {
                        iSensorConnected = true;
                        break;
                    }
                }
                if (iSensorConnected)
                {
                    continue;
                }
                int numberAElement = rnd.Next(0, aElementsCount);
                int temp = rnd.Next(-100, 100);
                if (temp >= 0)
                {
                    connectionsMatrix[i, numberAElement] = 1;
                }
                else
                {
                    connectionsMatrix[i, numberAElement] = -1;
                }
                connectionsMatrixTable.Rows[i][numberAElement] = connectionsMatrix[i, numberAElement];
            }

            //WriteConnectionsMatrix();
        }

        private void PrepareAElements()
        {
            for (int i = 0; i < aElementsCount; i++)
            {
                aElements[i].sum = 0;
                for (int j = 0; j < sensorsCount; j++)
                {
                    if (connectionsMatrix[j, i] != 0)
                    {
                        aElements[i].AddToSum(connectionsMatrix[j, i] * sensors[j].State);
                    }
                }

                aElementsOutput[i] = aElements[i].Result();//выход А-элемента
                aElementsTable.Rows[0][i] = aElements[i].sum;
            }
        }

        public int[] Summator()//or private
        {
            int[] generalSum = new int[summarorsCount];
            int aElementsPerSummator = aElementsCount / summarorsCount;
            for (int i = 0; i < aElementsCount; i++)
            {
                int j = i / aElementsPerSummator;
                generalSum[j] += aElements[i].Amplify();
            }

            return generalSum;
        }

        byte[] aElementsOutput = new byte[aElementsCount];
        public void Studing(bool nnAnswer)//без учета ошибок (для распознавания 2-ух классов)
        {
            const int delta = 1;

            TextWriter tw = new StreamWriter("lambda.txt");
            for (int i = 0; i < aElementsCount; i++)
            {
                if (aElementsOutput[i] == 1)
                {
                    if (nnAnswer == true)
                    {
                        tw.WriteLine(aElements[i].AmplifierValue + delta);
                    }
                    else
                    {
                        tw.WriteLine(aElements[i].AmplifierValue - delta);
                    }
                }
                else
                {
                    tw.WriteLine(aElements[i].AmplifierValue);
                }
            }
            tw.Flush();
            tw.Close();
        }

        int studingIteration = 1;
        public void Studing(int[] nnAnswer)//без учета ошибок
        {
            aElementsTable.Rows.Add();

            const int delta = 1;

            //TextWriter tw = new StreamWriter("lambda.txt");
            int aElementsPerSummator = aElementsCount / summarorsCount;
            for (int i = 0; i < aElementsCount; i++)
            {
                int j = i / aElementsPerSummator;
                if (aElementsOutput[i] == 1)
                {
                    if (nnAnswer[j] == 1)
                    {
                        aElements[i].AmplifierValue += delta;
                    }
                    else
                    {
                        aElements[i].AmplifierValue -= delta;
                    }
                }
                
                aElementsTable.Rows[studingIteration][i] = aElements[i].AmplifierValue;
                //tw.WriteLine(aElements[i].AmplifierValue);
            }
            //tw.Flush();
            //tw.Close();
            studingIteration++;
        }

        private void SetLambdaDefault()
        {
            TextWriter tw = new StreamWriter("lambda.txt");
            for (int i = 0; i < aElementsCount; i++)
            {
                tw.WriteLine(0);
            }
            tw.Flush();
            tw.Close();
        }

        private int[] GetOutput()
        {
            int[] summatorsOutputs = Summator();
            int[] res = new int[summarorsCount];

            for (int i = 0; i < summarorsCount; i++)
            {
                if (summatorsOutputs[i] >= 0)
                {
                    res[i] = 1;
                }
                else
                {
                    res[i] = 0;
                }
            }

            return res;
        }
    }
}
