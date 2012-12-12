using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.State;

namespace CS8803AGA.PsychSim.Probability
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
