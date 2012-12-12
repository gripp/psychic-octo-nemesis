using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGAI.State;

namespace AGAI.Probability
{
    class LookupPolicy
    {
        private Dictionary<Cell<Double>, AGAI.State.Action> policy = new Dictionary<Cell<Double>, AGAI.State.Action>();
        public LookupPolicy(Dictionary<Cell<Double>, AGAI.State.Action> aPolicy)
        {
            foreach (var k in aPolicy)
            {
                policy.Add(k.Key,k.Value);
            }
           
        }

        public AGAI.State.Action action(Cell<Double> s)
        {
            return policy[s];
        }
    }
}
