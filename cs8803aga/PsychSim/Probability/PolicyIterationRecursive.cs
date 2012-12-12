using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGAI.State;

namespace AGAI.Probability
{
    class PolicyIterationRecursive
    {
        private PolicyEvaluationRecursive policyEvaluation = null;

        public PolicyIterationRecursive(PolicyEvaluationRecursive policyEvaluation) 
        {
		    this.policyEvaluation = policyEvaluation;
	    }

        public LookupPolicy policyIteration(MDP mdp, MDP mdp1, MDP mdp2)
        {

		 Dictionary<Cell<Double>, Double> U = Util.create(mdp.GetStates(), new Double());
	
		Dictionary<Cell<Double>, AGAI.State.Action> pi = initialPolicyVector(mdp);
		bool unchanged;

		do {

			U = policyEvaluation.evaluate(pi, U, mdp, mdp1, mdp2);
	
			unchanged = true;

            foreach (var s in mdp.GetStates())
            {
                if (pi.ContainsKey(s))
                {
                    double aMax = Double.MinValue, piVal = 0;

                    AGAI.State.Action aArgmax = pi[s];
                    foreach (var a in mdp.actions(s))
                    {
                        double aSum = 0;
                        foreach (var sDelta in mdp.GetStates())
                        {
                            aSum += mdp.transitionProbability(sDelta, s, a)
                                    * U[sDelta];
                        }
                        if (aSum > aMax)
                        {
                            aMax = aSum;
                            aArgmax = a;
                        }

                        if (a.Equals(pi[s]))
                        {
                            piVal = aSum;
                        }
                    }

                    if (aMax > piVal)
                    {

                        pi[s] = aArgmax;

                        unchanged = false;
                    }
                }
            }

		} while (!unchanged);

		return new LookupPolicy(pi);
	}

        public static Dictionary<Cell<Double>, AGAI.State.Action> initialPolicyVector( MDP mdp) {
		Dictionary<Cell<Double>, AGAI.State.Action> pi = new Dictionary<Cell<Double>, AGAI.State.Action>();
		List<AGAI.State.Action> actions = new List<AGAI.State.Action>();
		foreach (var s in mdp.GetStates()) {
			actions.Clear();
            foreach (var x in mdp.actions(s))
            {
                actions.Add(x);
            }


			if (actions.Count() > 0) {
				pi[s] =  Util.selectRandomlyFromList(actions);
			}
		}
		return pi;
	}

    }
}
