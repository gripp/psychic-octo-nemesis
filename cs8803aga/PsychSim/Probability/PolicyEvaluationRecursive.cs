using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGAI.State;

namespace AGAI.Probability
{
    class PolicyEvaluationRecursive
    {
        // # iterations to use to produce the next utility estimate
        private int k;
        // discount &gamma; to be used.
        private double gamma;

        public PolicyEvaluationRecursive(int k, double gamma)
        {
            if (gamma > 1.0 || gamma <= 0.0)
            {
                throw new ArgumentOutOfRangeException("Gamma must be > 0 and <= 1.0");
            }
            this.k = k;
            this.gamma = gamma;
        }
        public Dictionary<Cell<Double>, Double> evaluate(Dictionary<Cell<Double>, AGAI.State.Action> pi_i, Dictionary<Cell<Double>, Double> U,
            MDP mdp, MDP mdp1, MDP mdp2)
        {
            Dictionary<Cell<Double>, Double> U_i = new Dictionary<Cell<Double>, Double>(U);
            Dictionary<Cell<Double>, Double> U_ip1 = new Dictionary<Cell<Double>, Double>(U);

            Dictionary<Cell<Double>, Double> U_i1 = new Dictionary<Cell<Double>, Double>(U);
            Dictionary<Cell<Double>, Double> U_i2 = new Dictionary<Cell<Double>, Double>(U);

            Dictionary<Cell<Double>, Double> U_ip3 = new Dictionary<Cell<Double>, Double>(U);
            Dictionary<Cell<Double>, Double> U_ip2 = new Dictionary<Cell<Double>, Double>(U);
            for (int i = 0; i < k; i++)
            {
                foreach (var s in U.Keys)
                {
                    double aSum = 0;
                    if (pi_i.ContainsKey(s))
                    {
                        AGAI.State.Action ap_i = pi_i[s];
                        foreach (var sDelta in U.Keys)
                        {
                            aSum += mdp.transitionProbability(sDelta, s, ap_i) * U_i[sDelta];
                            aSum += mdp1.transitionProbability(sDelta, s, ap_i) * U_i1[sDelta];
                            aSum += mdp2.transitionProbability(sDelta, s, ap_i) * U_i2[sDelta];
                        }
                    }
                    U_ip2[s] = (mdp1.reward(s) + gamma * aSum);
                    U_ip3[s] = (mdp2.reward(s) + gamma * aSum);
                    U_ip1[s] = (mdp.reward(s) + gamma * aSum) + U_ip2[s] + U_ip3[s];

                }

                Util.Merge(U_i, U_ip1);
                Util.Merge(U_i1, U_ip2);
                Util.Merge(U_i2, U_ip3);

            }
            return U_ip1;
        }
    }
}
