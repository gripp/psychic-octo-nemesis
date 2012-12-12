using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.State;
using CS8803AGA.PsychSim.Probability;
namespace CS8803AGA.PsychSim
{
    class Program
    {
        static void TempMain(string[] args)
        {
            bool funding = true;
            bool application = false;
            List<Model> l = new List<Model>();
            Model prof = new Model();
            Model dean = new Model();
            l.Add(prof);
            l.Add(dean);
            CS8803AGA.PsychSim.State.Action a = prof.getAction();
            Cell<Double> c = prof.getState();

            Model.addEvidanceAll(l, 9);


            prof.message(Message.askNoFunding, dean);
            funding = false;

            a = prof.getAction();
            c = prof.getState();
            Model.addEvidanceAll(l, 11);

            dean.message(Message.submitApplication, prof);
            application = true;

            a = prof.getAction();
            c = prof.getState();



        }

    }
}
