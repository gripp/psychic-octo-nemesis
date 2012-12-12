using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGAI.State;
using System.Collections;

namespace AGAI.Probability
{
    class TransitionProbabilityFunction
    {
        State<Double> state;
        private double[] distribution = new double[] { 0.8, 0.1, 0.1 };

        public double probability(Cell<Double> sDelta, Cell<Double> s, AGAI.State.Action a)
        {
            double prob = 0;

            List<Cell<Double>> outcomes = possibleOutcomes(s, a);
            for (int i = 0; i < outcomes.Count; i++)
            {
                if (sDelta.Equals(outcomes[i]))
                {
                    prob += distribution[i];
                }
            }

            return prob;
        }
        private List<Cell<Double>> possibleOutcomes(Cell<Double> c, AGAI.State.Action a)
        {
            List<Cell<Double>> outcomes = new List<Cell<Double>>();

            outcomes.Add(state.result(c, a));
            outcomes.Add(state.result(c, a.getFirstRightAngledAction()));
            outcomes.Add(state.result(c, a.getSecondRightAngledAction()));

            return outcomes;
        }
        public TransitionProbabilityFunction(State<Double> state)
        {
            this.state = state;
        }
    }
}

