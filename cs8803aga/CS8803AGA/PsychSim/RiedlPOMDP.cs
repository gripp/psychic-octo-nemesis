using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.State;
using CS8803AGA.story.characters;
namespace CS8803AGA.PsychSim
{
    public class RiedlPOMDP : Model
    {
        public bool gaveFunding = false;
        public bool askedFunding = false;
        public bool SIMProject1 = false; // will be set when SIMA project A is given student
        public bool SIMProject2 = false;// will be set when SIMA project B is given to student

        List<Riedl.ThingToDoToRiedl> l; // list will be empty if no interaction is available

        public RiedlPOMDP(): base()
        {
            this.l = new List<Riedl.ThingToDoToRiedl>();
            l.Add(Riedl.ThingToDoToRiedl.SHAKE_HAND);
            l.Add(Riedl.ThingToDoToRiedl.TELL_JOKE);
            l.Add(Riedl.ThingToDoToRiedl.ACE_TEST);
            l.Add(Riedl.ThingToDoToRiedl.DISCUSS_THEORY);
        }

        public override void updatePOMDP()
        {
            if (l.Contains(Riedl.ThingToDoToRiedl.SHAKE_HAND))
                l.Remove(Riedl.ThingToDoToRiedl.SHAKE_HAND);

            c = p.getState();
            a = mdp.getAction(c);


            if ((c.getX() == 3 && c.getY() >= 2) || (c.getX() >= 2 && c.getY() == 3))
            {
                if (!askedFunding && !l.Contains(Riedl.ThingToDoToRiedl.REQUEST_FUNDING))
                {
                    l.Add(Riedl.ThingToDoToRiedl.REQUEST_FUNDING);
                }
                SIMProject2 = true;
                if (!l.Contains(Riedl.ThingToDoToRiedl.PRESENT_THESIS) && !l.Contains(Riedl.ThingToDoToRiedl.DO_PROJECT))
                {
                    l.Add(Riedl.ThingToDoToRiedl.PRESENT_THESIS);
                    l.Add(Riedl.ThingToDoToRiedl.DO_PROJECT);
                }
            }
            c = getNextState(a,c);

            if (c.getX() == 3 && c.getY() == 3)
            {
                SIMProject1 = true;
                SIMProject2 = false;
            }

        }

        public void message(Message m)
        {
            switch (m)
            {
                //case Message.askFunding: 
                case Message.askFunding:
                    l.Remove(Riedl.ThingToDoToRiedl.REQUEST_FUNDING);
                    askedFunding = true;
                    gaveFunding = true;
                    break;

                case Message.askNoFunding:
                    l.Remove(Riedl.ThingToDoToRiedl.REQUEST_FUNDING);
                    this.mdp.changeBelief(new double[] { 0, 1, 0, 0, 3, 0, 0, 5, 0 });
                    askedFunding = true;
                    gaveFunding = false;
                    break;
                //new double[] { 1, 1, -1, 2, 2, -2, 3, 3, -3 }
                case Message.notify:
                    this.mdp.changeBelief(new double[] { -1, 0, 3, -2, 0, 5, -4, 0, 7 });
                    break;              //new double[] { -1, 1, 2, 0, 2, 3, -1, 3, 4 });
                // case Message.askProject:

                default: break;
            }
            updatePOMDP();

        }


        public List<Riedl.ThingToDoToRiedl> options()
        {
            count++;
            if (count > MAX_COUNT) l.Clear();

            return l;
        }
    }
}
