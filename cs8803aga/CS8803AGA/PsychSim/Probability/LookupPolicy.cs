using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.State;

namespace CS8803AGA.PsychSim.Probability
{
    class LookupPolicy
    {
        private Dictionary<Cell<Double>, CS8803AGA.PsychSim.State.Action> policy = new Dictionary<Cell<Double>, CS8803AGA.PsychSim.State.Action>();
        public LookupPolicy(Dictionary<Cell<Double>, CS8803AGA.PsychSim.State.Action> aPolicy)
        {
            foreach (var k in aPolicy)
            {
                policy.Add(k.Key,k.Value);
            }
           
        }

        public CS8803AGA.PsychSim.State.Action action(Cell<Double> s)
        {
            return policy[s];
        }
    }
}
