using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.PsychSim.State;

namespace CS8803AGA.PsychSim.Probability
{
    class PolicyEvaluation
    {
        // # iterations to use to produce the next utility estimate
        private int k;
        // discount &gamma; to be used.
        private double gamma;

        public PolicyEvaluation(int k, double gamma)
        {
            if (gamma > 1.0 || gamma <= 0.0)
            {
                throw new ArgumentOutOfRangeException("Gamma must be > 0 and <= 1.0");
            }
            this.k = k;
            this.gamma = gamma;
        }
        public Dictionary<Cell<Double>, Double> evaluate(Dictionary<Cell<Double>, CS8803AGA.PsychSim.State.Action> pi_i, Dictionary<Cell<Double>, Double> U,
            MDP mdp)
        {
            Dictionary<Cell<Double>, Double> U_i = new Dictionary<Cell<Double>, Double>(U);
            Dictionary<Cell<Double>, Double> U_ip1 = new Dictionary<Cell<Double>, Double>(U);

            for (int i = 0; i < k; i++)
            {

                foreach (var s in U.Keys)
                {
                    
                    double aSum = 0;
                    if (pi_i.ContainsKey(s))
                    {
                        CS8803AGA.PsychSim.State.Action ap_i = pi_i[s];
                        foreach (var sDelta in U.Keys)
                        {
                            aSum += mdp.transitionProbability(sDelta, s, ap_i)
                                    * U_i[sDelta];
                        }
                    }
                    U_ip1[s] = (mdp.reward(s) + gamma * aSum);
                }

                Util.Merge(U_i, U_ip1);
            }
            return U_ip1;
        }
    }
}
