using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.story.map;

namespace CS8803AGA.story.behaviors
{
    class GoToBehavior : Behavior
    {
        private LabScreen.LabLocation location;
        private Behavior next = null;

        public GoToBehavior(LabScreen.LabLocation l)
        {
            location = l;
        }

        #region Behavior Members

        public Behavior getNext()
        {
            return next;
        }

        public void setNext(Behavior n)
        {
            next = n;
        }

        public string getDescription()
        {
            switch (location)
            {
                case LabScreen.LabLocation.SIMA:
                    return "Return to base.";
                case LabScreen.LabLocation.CAKE:
                    return "Go to cake.";
                case LabScreen.LabLocation.CHICKEN:
                    return "Go to chicken.";
                case LabScreen.LabLocation.LOBSTER:
                    return "Go to lobster.";
                case LabScreen.LabLocation.MICROWAVE:
                    return "Go to microwave.";
                case LabScreen.LabLocation.PIZZA:
                    return "Go to pizza.";
                case LabScreen.LabLocation.RIEDL:
                    return "Go to Riedl.";
                case LabScreen.LabLocation.STEAK:
                    return "Go to steak.";
                default:
                    return "";
            }
        }

        public int compareTo(Behavior b)
        {
            return (b is GoToBehavior) && (((GoToBehavior)b).getLocation() == location) ? 0 : -1;
        }

        public Behavior makeCopy()
        {
            return new GoToBehavior(location);
        }

        #endregion

        public LabScreen.LabLocation getLocation()
        {
            return location;
        }
    }
}
