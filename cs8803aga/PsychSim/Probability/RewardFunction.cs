using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGAI.State;

namespace AGAI.Probability
{
    public class RewardFunction
    {
        public double reward(Cell<Double> s)
        {
            return s.getContent();
        }


    }
}
