using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS8803AGA.story.behaviors
{
    public interface Behavior
    {
        string getDescription();

        int compareTo(Behavior b);

        Behavior getNext();
        void setNext(Behavior n);

        Behavior makeCopy();
    }
}
