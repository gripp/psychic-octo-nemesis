using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.Probability;
using CS8803AGA.PsychSim.State;

namespace CS8803AGA.PsychSim
{
    public class MDPPolicyIteration
    {
        LookupPolicy policy;
        public State<Double> S;
        public MDP mdp;
        PolicyIteration pi;
        public MDPPolicyIteration()
        {
            S = new State<Double>(3, 3, 0);
            Double[] val = { 1, 0, -1, 2, -1, -2, 3, -2, -3 };

            S.setContent(val);

            mdp = new MDP(S.getCells(), S.getCellAt(1, 1), new ActionsFunction(S),
                   new TransitionProbabilityFunction(S),
                   new RewardFunction());

            double epsilon = 0.00001;

            PolicyEvaluation pev = new PolicyEvaluation(1000, epsilon);

            pi = new PolicyIteration(pev);

            policy = pi.policyIteration(mdp);
        }
        public void makePolicy()
        {
            policy = pi.policyIteration(mdp);
        }

        public void changeBelief(Double[] val)
        {
            if (val.Length == 9)
            {
                int count = 0;
                for (int i = 1; i < 4; i++)
                {
                    for (int j = 1; j < 4; j++)
                    {
                        var c = S.cellLookup[i][j];
                        c.setContent(c.getContent() + val[count++]);
                    }
                }
                //S.setContent(val);
                makePolicy();
            }
        }


        public void printPolicy()
        {
            foreach (var s in mdp.GetStates())
            {
                try
                {
                    CS8803AGA.PsychSim.State.Action a = policy.action(s);

                    Console.Write(s.getX() + " " + s.getY() + ": ");
                    Console.WriteLine(a.i);
                    Console.WriteLine();
                }
                catch (Exception e)
                {
                }
            }
        }

        public CS8803AGA.PsychSim.State.Action getAction(Cell<Double> s)
        {
            return policy.action(s);
        }
    }
}
