using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGAI.Probability;

namespace AGAI.State
{
    class Sensor
    {
        public Sensor()
        {
        
        }

        internal double senses(double evidance, Cell<double> s)
        {
            double exp = s.getX() * s.getY();
            exp = evidance / exp;
            if (exp > 1) exp = 1 / exp;
            return exp;
        }
    }

}
