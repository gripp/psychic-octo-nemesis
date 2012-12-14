using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CS8803AGA.PsychSim.State;
using CS8803AGA.story.characters;

namespace CS8803AGA.PsychSim
{
    public class testProgram
    {
        public static void test()
        {
            RiedlPOMDP r = new RiedlPOMDP();
            DeanPOMDP d = new DeanPOMDP();

            r.addEvidence(1);
            d.addEvidence(1);

            r.addEvidence(2);
            d.addEvidence(2);

            r.addEvidence(6);
            d.addEvidence(6);

            r.addEvidence(9);
            d.addEvidence(9);

            r.message(Message.askNoFunding);
            d.message(Message.acceptApplication);
            r.message(Message.notify);


        }
    }
}
