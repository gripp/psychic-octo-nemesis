using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.Probability;
using CS8803AGA.PsychSim.State;

namespace CS8803AGA.PsychSim
{
    class MDPValueIteration
    {
        public static void values()
        {
            State<Double> S = new State<Double>(3, 3, -1.0);
            double r = -100;
            double epsilon = 0.00001;
            S.getCellAt(1, 3).setContent(r);
            S.getCellAt(3, 3).setContent(10.0);

            MDP mdp = new MDP(S.getCells(), S.getCellAt(1, 3), new ActionsFunction(S),
                    new TransitionProbabilityFunction(S),
                    new RewardFunction());

            ValueIteration vi = new ValueIteration(0.99);
            Dictionary<Cell<Double>, Double> map = vi.valueIteration(mdp, epsilon);
            foreach (var c in map)
            {
                Console.Write(c.Key.getX() + " " + c.Key.getY() + ": ");
                Console.WriteLine(c.Value);
                Console.WriteLine();
            }
        }
    }
}

