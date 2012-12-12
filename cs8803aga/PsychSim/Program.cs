using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGAI.State;
using AGAI.Probability;
namespace AGAI
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Model> l = new List<Model>();
            Model prof = new Model();
            Model dean = new Model();
            l.Add(prof);
            l.Add(dean);
            AGAI.State.Action a = prof.getAction();
            Cell<Double> c = prof.getState();

            Model.addEvidanceAll(l, 9);


            prof.message(Message.askNoFunding, dean);


            a = prof.getAction();
            c = prof.getState();
            Model.addEvidanceAll(l, 11);

            dean.message(Message.submitApplication, prof);

            a = prof.getAction();
            c = prof.getState();



        }

    }
}
