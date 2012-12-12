using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.PsychSim.State;

namespace CS8803AGA.PsychSim.Probability
{
    public class RewardFunction
    {
        public double reward(Cell<Double> s)
        {
            return s.getContent();
        }


    }
}
