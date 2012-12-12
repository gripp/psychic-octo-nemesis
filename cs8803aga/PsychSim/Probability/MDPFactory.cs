using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGAI.State;

namespace AGAI.Probability
{
    class MDPFactory
    {
        public static MDP createMDP(State<Double> state)
        {

            return new MDP(state.getCells(),
                    state.getCellAt(1, 1), new ActionsFunction(state),
                    new TransitionProbabilityFunction(state),
                     new RewardFunction());
        }


    }
}
