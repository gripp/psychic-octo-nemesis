using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.State;

namespace CS8803AGA.PsychSim.Probability
{
    class PolicyIteration
    {
        private PolicyEvaluation policyEvaluation = null;

        public PolicyIteration(PolicyEvaluation policyEvaluation) 
        {
		    this.policyEvaluation = policyEvaluation;
	    }

        public LookupPolicy policyIteration(MDP mdp) {

		 Dictionary<Cell<Double>, Double> U = Util.create(mdp.GetStates(), new Double());
	
		Dictionary<Cell<Double>, CS8803AGA.PsychSim.State.Action> pi = initialPolicyVector(mdp);
		bool unchanged;

		do {

			U = policyEvaluation.evaluate(pi, U, mdp);
	
			unchanged = true;

            foreach (var s in mdp.GetStates())
            {
                if (pi.ContainsKey(s))
                {
                    double aMax = Double.MinValue, piVal = 0;

                    CS8803AGA.PsychSim.State.Action aArgmax = pi[s];
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

        public static Dictionary<Cell<Double>, CS8803AGA.PsychSim.State.Action> initialPolicyVector( MDP mdp) {
		Dictionary<Cell<Double>, CS8803AGA.PsychSim.State.Action> pi = new Dictionary<Cell<Double>, CS8803AGA.PsychSim.State.Action>();
		List<CS8803AGA.PsychSim.State.Action> actions = new List<CS8803AGA.PsychSim.State.Action>();
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
