using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS8803AGA.story.characters
{
    class Receptionist : Character
    {
        public override string getDialogue()
        {
            return "Do you have your graduation form?\nNo?\nWell then you can't see the registrar yet.\nGo finish your research.";
        }

        public override string getName()
        {
            return "RECEPTIONIST";
        }
    }
}
