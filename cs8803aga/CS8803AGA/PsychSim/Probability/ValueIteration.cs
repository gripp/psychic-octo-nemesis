using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.State;

namespace CS8803AGA.PsychSim.Probability
{
    class ValueIteration
    {
        private double gamma = 0;

        public ValueIteration(double gamma)
        {
            if (gamma > 1.0 || gamma <= 0.0)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.gamma = gamma;
        }

        public Dictionary<Cell<Double>, Double> valueIteration(MDP mdp, double epsilon)
        {

            Dictionary<Cell<Double>, Double> U = Util.create(mdp.GetStates(), 0.0);
            Dictionary<Cell<Double>, Double> Udelta = Util.create(mdp.GetStates(), 0.0);

            double delta = 0;

            double minDelta = epsilon * (1 - gamma) / gamma;

            do
            {

                Util.Merge(U, Udelta);

                delta = 0;

                foreach (Cell<Double> s in mdp.GetStates())
                {

                    HashSet<CS8803AGA.PsychSim.State.Action> actions = mdp.actions(s);

                    double aMax = 0;
                    if (actions.Count > 0)
                    {
                        aMax = Double.MinValue;
                    }
                    foreach (CS8803AGA.PsychSim.State.Action a in actions)
                    {

                        double aSum = 0;
                        foreach (Cell<Double> sDelta in mdp.GetStates())
                        {
                            aSum += mdp.transitionProbability(sDelta, s, a)
                                    * U[sDelta];
                        }
                        if (aSum > aMax)
                        {
                            aMax = aSum;
                        }
                    }
                    var val = mdp.reward(s) + gamma * aMax;
                    Udelta[s] = val;

                    double aDiff = Math.Abs(Udelta[s] - U[s]);
                    if (aDiff > delta)
                    {
                        delta = aDiff;
                    }
                }

            } while (delta > minDelta);

            return U;
        }
    }
}
