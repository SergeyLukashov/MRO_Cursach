using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perceptron
{
    class Sensor
    {
        public Sensor()
        {
            state = 0;
        }
        int state;
        public int State
        {
            set
            {
                if (state != value)
                {
                    state = value;
                }
            }
            get
            {
                return state;
            }
        }
    }
}
