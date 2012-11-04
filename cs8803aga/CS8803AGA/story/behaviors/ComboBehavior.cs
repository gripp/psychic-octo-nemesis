using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS8803AGA.story.behaviors
{
    class ComboBehavior : Behavior
    {
        private List<Behavior> next = new List<Behavior>();

        public List<Behavior> getAllBranches()
        {
            return next;
        }

        #region Behavior Members

        public Behavior getNext()
        {
            if (next.Count == 0)
            {
                return null;
            }
            else
            {
                return next[(new Random()).Next(next.Count - 1)];
            }
        }

        public void setNext(Behavior n)
        {
            next.Add(n);
        }

        public string getDescription()
        {
            throw new NotImplementedException();
        }

        public int compareTo(Behavior behavior)
        {
            throw new NotImplementedException();
        }

        public Behavior makeCopy()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
