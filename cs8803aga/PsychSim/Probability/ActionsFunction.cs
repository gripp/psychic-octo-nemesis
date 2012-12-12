using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGAI.State;

namespace AGAI.Probability
{
    class ActionsFunction
    {
        static HashSet<Cell<Double>> terminals = new HashSet<Cell<Double>>();

        public ActionsFunction(State<Double> state)
        {
            Cell<Double> c = state.getCellAt(3, 3);
            terminals.Add(c);

        }

        public HashSet<AGAI.State.Action> actions(Cell<Double> s)
        {
            if (terminals.Contains(s))
            {
                HashSet<AGAI.State.Action> t = new HashSet<AGAI.State.Action>();
                return t;
            }
            return AGAI.State.Action.actions();
        }



    }
}


