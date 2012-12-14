using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.State;
using CS8803AGA.story.characters;
namespace CS8803AGA.PsychSim
{
    public class DeanPOMDP : Model
    {
        public bool scholarship = false; // sccholarship is set in the end with probability of 30%
        public bool application = false; // intially student do not have application

        List<Dean.ThingToDoToDean> l;

        public DeanPOMDP()
            : base()
        {
            this.l = new List<Dean.ThingToDoToDean>();
            l.Add(Dean.ThingToDoToDean.SHAKE_HAND);
            l.Add(Dean.ThingToDoToDean.TELL_JOKE);
            l.Add(Dean.ThingToDoToDean.DISCUSS_THEORY);

        }

        public override void updatePOMDP()
        {
            if (l.Contains(Dean.ThingToDoToDean.SHAKE_HAND))
                l.Remove(Dean.ThingToDoToDean.SHAKE_HAND);

            c = p.getState();
            a = mdp.getAction(c);

            if (c.getX() >= 2 || c.getY() >= 2)
            {
                if (!application && !l.Contains(Dean.ThingToDoToDean.REQUEST_SCHOLARSHIP))
                {
                    l.Add(Dean.ThingToDoToDean.REQUEST_SCHOLARSHIP);
                }
                if (!l.Contains(Dean.ThingToDoToDean.PRESENT_THESIS ))
                {
                    l.Add(Dean.ThingToDoToDean.PRESENT_THESIS);
                }
                var belief = new double[] { -1, 1, 2, 0, 2, 3, -1, 3, 4 };
                this.mdp.changeBelief(belief);
            }
            c = getNextState(a,c);

            if (c.getX() == 3 || c.getY() == 3)
            {
                if (!l.Contains(Dean.ThingToDoToDean.REQUEST_SCHOLARSHIP))
                    l.Add(Dean.ThingToDoToDean.REQUEST_SCHOLARSHIP);
            }
        }
        public void message(Message m)
        {
            switch (m)
            {
                case Message.acceptApplication:
                    this.mdp.changeBelief(new double[] { -1, 0, 3, -2, 0, 5, -4, 0, 7 });
                    l.Remove(Dean.ThingToDoToDean.REQUEST_SCHOLARSHIP);
                    application = true;
                    break;              //new double[] { -1, 1, 2, 0, 2, 3, -1, 3, 4 });
                // case Message.askProject:

                default: break;
            }

        }


        public List<Dean.ThingToDoToDean> options()
        {
            count++;
            if (count > 10) l.Clear();
            return l;

        }
    }
}
