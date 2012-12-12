using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CS8803AGA.PsychSim.State
{
    class StateFactory
    {
        public static State<Double> build()
        {
            State<Double> state = new State<Double>(3, 3, 0);
              return state;
        }
    }
}
