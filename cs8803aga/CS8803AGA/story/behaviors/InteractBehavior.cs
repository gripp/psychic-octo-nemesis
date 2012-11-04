using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS8803AGA.story.behaviors
{
    class InteractBehavior : Behavior
    {
        private Behavior next = null;

        #region Behavior Members

        public string getDescription()
        {
            return "Interact.";
        }

        public int compareTo(Behavior b)
        {
            return (b is InteractBehavior) ? 0 : -1;
        }

        public Behavior getNext()
        {
            return next;
        }

        public void setNext(Behavior n)
        {
            next = n;
        }

        public Behavior makeCopy()
        {
            return new InteractBehavior();
        }

        #endregion
    }
}
