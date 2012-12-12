using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGAI.Probability;
using AGAI.State;

namespace AGAI
{
    class MDPPolicyIterationRecursive
    {
        public static void values()
        {
            LookupPolicy policy = null;
            State<Double> S = new State<Double>(3, 3, 0);

            MDP mdp1 = new MDP(S.getCells(), S.getCellAt(1, 1), new ActionsFunction(S),
                               new TransitionProbabilityFunction(S),
                               new RewardFunction());

            State<Double> S1 = new State<Double>(3, 3, 0);
            S1.getCellAt(1, 1).setContent(-1);
            S1.getCellAt(1, 2).setContent(0);
            S1.getCellAt(1, 3).setContent(-1);

            S1.getCellAt(2, 1).setContent(-2);
            S1.getCellAt(2, 2).setContent(1);
            S1.getCellAt(2, 3).setContent(-2);

            S1.getCellAt(3, 1).setContent(-3);
            S1.getCellAt(3, 2).setContent(2);
            S1.getCellAt(3, 3).setContent(-3);

       

  
            MDP mdp2 = new MDP(S1.getCells(), S1.getCellAt(1, 1), new ActionsFunction(S1),
                                    new TransitionProbabilityFunction(S1),
                                    new RewardFunction());
            State<Double> S2 = new State<Double>(3, 3, 0);


            // double r = -100;
            double epsilon = 0.00001;
            S2.getCellAt(1, 1).setContent(1);
            S2.getCellAt(1, 2).setContent(0);
            S2.getCellAt(1, 3).setContent(-1);

            S2.getCellAt(2, 1).setContent(2);
            S2.getCellAt(2, 2).setContent(-1);
            S2.getCellAt(2, 3).setContent(-2);

            S2.getCellAt(3, 1).setContent(3);
            S2.getCellAt(3, 2).setContent(-2);
            S2.getCellAt(3, 3).setContent(-3);

            MDP mdp = new MDP(S2.getCells(), S2.getCellAt(1, 1), new ActionsFunction(S2),
                   new TransitionProbabilityFunction(S2),
                   new RewardFunction());



            PolicyEvaluationRecursive pev = new PolicyEvaluationRecursive(1000, epsilon);

            PolicyIterationRecursive pi = new PolicyIterationRecursive(pev);

            policy = pi.policyIteration(mdp, mdp1, mdp2);

            foreach (var s in S.getCells())
            {

                try
                {
                    AGAI.State.Action a = policy.action(s);

                    Console.Write(s.getX() + " " + s.getY() + ": ");
                    Console.WriteLine(a.i);
                    Console.WriteLine();
                }
                catch (Exception e)
                {
                }
            }
        }
    }
}
