using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.State;
using CS8803AGA.story.characters;

namespace CS8803AGA.PsychSim
{
    public class Model
    {
        public MDPPolicyIteration mdp;
        public POMDP p;
        public CS8803AGA.PsychSim.State.Action a;
        public Cell<Double> c;
        public int count = 0;
        public static int MAX_COUNT = 4;

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

        public void addEvidence(double e)
        {
            p.updateBelief(CS8803AGA.PsychSim.State.Action.Up, e);
            this.updatePOMDP();
        }

        public virtual void updatePOMDP()
        {
            //throw new NotImplementedException();
        }


        public Cell<Double> getNextState(CS8803AGA.PsychSim.State.Action a, Cell<Double> c)
        {
            int x = a.getXResult(c.getX());
            int y = a.getYResult(c.getY());
            return this.mdp.S.cellLookup[x][y];
        }


    }
}



//public static void addEvidenceAll(List<Model> l, double e)
//{
//    foreach (Model m in l)
//    {
//           m.addEvidence(e);
//    }

//}