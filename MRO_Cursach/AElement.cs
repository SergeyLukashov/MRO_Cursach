using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Perceptron
{
    class AElement
    {
        public AElement()
        {
            //TextReader tr = File.OpenText("lambda.txt");
        }
        public int sum = 0;
        const int theta = 0;//porog

        public void AddToSum(int reflection)
        {
            sum += reflection;
        }

        byte result;
        public byte Result()//result of A-element
        {
            if (sum > theta)
            {
                result = 1;
                return result;
            }
            else
            {
                result = 0;
                return result;
            }
        }

        int amplifierValue;//lambda
        public int AmplifierValue
        {
            set
            {
                amplifierValue = value;
            }
            get
            {
                return amplifierValue;
            }
        }

        public int Amplify()
        {
            return amplifierValue * result;
        }

    }
}
