using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.State;

namespace CS8803AGA.PsychSim
{
    class Model
    {
        MDPPolicyIteration mdp;
        POMDP p;

        public Model()
        {
            mdp = new MDPPolicyIteration();

            p = new POMDP(mdp.mdp);
            mdp.makePolicy();
        }

        public CS8803AGA.PsychSim.State.Action getAction()
        {
            Cell<Double> s = p.getState();
            return mdp.getAction(s);
        }

        public Cell<Double> getState()
        {
            Cell<Double> s = p.getState();
            return s;
        }

        public void addEvidance(double e)
        {
            p.updateBelief(CS8803AGA.PsychSim.State.Action.Up, e);
        }

        public void message(Message m, Model Npc)
        {
            switch (m)
            {
                //case Message.askFunding: 
                case Message.askFunding: break;

                case Message.askNoFunding:
                    this.mdp.changeBelief(new double[] { 0, 1, 0, 0, 3, 0, 0, 5, 0 });
                    break;
                                        //new double[] { 1, 1, -1, 2, 2, -2, 3, 3, -3 }
                case Message.acceptApplication:   
                    this.mdp.changeBelief(new double[] { -1, 0, 3, -2, 0, 5, -4, 0, 7 });
                    break;              //new double[] { -1, 1, 2, 0, 2, 3, -1, 3, 4 });
                // case Message.askProject:
                case Message.submitApplication:
                    Npc.message(Message.acceptApplication, this);
                    break;
                default: break;
            }
        }
        public static void addEvidanceAll(List<Model> l, double e)
        {
            foreach (Model m in l)
            {
                   m.addEvidance(e);
            }
            
        }
    }
}
