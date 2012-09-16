using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS8803AGA.story.characters
{
    public abstract class Character
    {
        static int ID_COUNTER
        {
            get
            {
                idCounter++;
                return idCounter;
            }
        }
        private static int idCounter = -1;

        public int ID
        {
            get
            {
                return id;
            }
        }
        private int id = -2;

        public Character()
        {
            id = ID_COUNTER;
        }

        public abstract string getDialogue();
    }
}
