using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGAI.Probability;
using AGAI.State;

namespace AGAI
{
    class POMDP
    {
        public State<Double> belief;
        public Sensor sensor;
        public MDP mdp;
        public POMDP(MDP mdp)
        {
            this.mdp = mdp;
            belief = new State<Double>(3, 3, 0);
            sensor = new Sensor();

            belief.setContent(new double[] { 0.311, 0.111, 0.111, 0.111, 0.111, 0.111, 0.111, 0.111, 0 });
        }


        public State<double> updateBelief(AGAI.State.Action a, double evidance)
        {
            setBelief();

            State<Double> temp = new State<Double>(3, 3, 0);

            foreach (var s in mdp.GetStates())
            {
                foreach (var sDelta in mdp.GetStates())
                {
                    var t = temp.getCellAt(sDelta.getX(), sDelta.getY());
                    Double x = t.getContent();
                    x += mdp.transitionProbability(sDelta, s, a) * (belief.getCellAt(sDelta.getX(), sDelta.getY()).getContent());
                    t.setContent(x);
                }
            }

            double alpha = 1.56;

            foreach (var s in mdp.GetStates())
            {
                var b = belief.getCellAt(s.getX(), s.getY());
                Double x = alpha * (sensor.senses(evidance, s)) * (temp.getCellAt(s.getX(), s.getY()).getContent());
                b.setContent(x);
            }

            return belief;
        }

        public State<Double> getBelief()
        {

            foreach (var s in belief.getCells())
            {
                Console.WriteLine(s.getX() + " " + s.getY() + ": " + s.getContent() + " ");

            }
            setBelief();
            return belief;
        }

        public void setBelief()
        {
            belief.setContent(new double[] { 0.311, 0.111, 0.111, 0.111, 0.111, 0.111, 0.111, 0.111, 0 });
        }

        public Cell<Double> getState()
        {
            Cell<Double> c = new Cell<double>(1, 1, 0);
            foreach (var s in belief.getCells())
            {
                var prob = s.getContent();
                if (prob > c.getContent())
                    c = s;
            }
            return c;
        }
    }
}
