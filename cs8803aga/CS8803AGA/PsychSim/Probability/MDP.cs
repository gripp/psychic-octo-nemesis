using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.State;

namespace CS8803AGA.PsychSim.Probability
{
    class MDP
    {
        private HashSet<Cell<Double>> states = null;
        private Cell<Double> initialState;
        private ActionsFunction actionsFunction = null;
        private TransitionProbabilityFunction transitionProbabilityFunction = null;
        private RewardFunction rewardFunction = null;

        public MDP(HashSet<Cell<Double>> states, Cell<Double> initialState,
                ActionsFunction actionsFunction,
                TransitionProbabilityFunction transitionProbabilityFunction,
                RewardFunction rewardFunction)
        {
            this.states = states;
            this.initialState = initialState;
            this.actionsFunction = actionsFunction;
            this.transitionProbabilityFunction = transitionProbabilityFunction;
            this.rewardFunction = rewardFunction;
        }
        public HashSet<Cell<Double>> GetStates()
        {
            return states;
        }
        public Cell<Double> getInitialState()
        {
            return initialState;
        }

        public HashSet<CS8803AGA.PsychSim.State.Action> actions(Cell<Double> s)
        {
            return actionsFunction.actions(s);
        }

        public double transitionProbability(Cell<Double> sDelta, Cell<Double> s, CS8803AGA.PsychSim.State.Action a)
        {
            return transitionProbabilityFunction.probability(sDelta, s, a);
        }

        public double reward(Cell<Double> s)
        {
            return rewardFunction.reward(s);
        }
    }
}
