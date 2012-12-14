using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.Probability;

namespace CS8803AGA.PsychSim.State
{
    public class Sensor
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
