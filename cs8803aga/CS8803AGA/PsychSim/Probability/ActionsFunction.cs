using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.State;

namespace CS8803AGA.PsychSim.Probability
{
    class ActionsFunction
    {
        static HashSet<Cell<Double>> terminals = new HashSet<Cell<Double>>();

        public ActionsFunction(State<Double> state)
        {
            Cell<Double> c = state.getCellAt(3, 3);
            terminals.Add(c);

        }

        public HashSet<CS8803AGA.PsychSim.State.Action> actions(Cell<Double> s)
        {
            if (terminals.Contains(s))
            {
                HashSet<CS8803AGA.PsychSim.State.Action> t = new HashSet<CS8803AGA.PsychSim.State.Action>();
                return t;
            }
            return CS8803AGA.PsychSim.State.Action.actions();
        }



    }
}


