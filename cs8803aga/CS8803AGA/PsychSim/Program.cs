using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.State;
using CS8803AGA.PsychSim.Probability;
using CS8803AGA.story.characters;

namespace CS8803AGA.PsychSim
{
    class Control
    {
        bool funding = true;
        bool application = false;
        List<Model> l = new List<Model>();
        Model prof = new Model();
        Model dean = new Model();
        CS8803AGA.PsychSim.State.Action a;
        Cell<Double> c;

        static void run(string[] args)
        {

            //l.Add(prof);
            //l.Add(dean);
            //a = prof.getAction();
            //c = prof.getState();
            
            //Model.addEvidenceAll(l, 9);


            //prof.message(Message.askNoFunding, dean);
            //funding = false;

            //c = prof.getNextState(a, c);

            //a = prof.getAction();
            //c = prof.getState();
            //Model.addEvidenceAll(l, 11);

            //dean.message(Message.submitApplication, prof);
            //application = true;

            //a = prof.getAction();
            //c = prof.getState();

            //c = prof.getNextState(a, c);

        }

        public static void options()
        {
            List<Riedl.ThingToDoToRiedl> l = new List<Riedl.ThingToDoToRiedl>();

        }

    }
}
