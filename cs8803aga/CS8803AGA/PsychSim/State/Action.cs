using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CS8803AGA.PsychSim.State
{
    public class Action
    {
        public int i;
        public static readonly Action Up = new Action(1);
        public static readonly Action Down = new Action(2);
        public static readonly Action Left = new Action(3);
        public static readonly Action Right = new Action(4);
        public static readonly Action None = new Action(0);
        public static HashSet<Action> _actions = new HashSet<Action>();
        static Action()
        {
            _actions.Add(Up);
            _actions.Add(Down);
            _actions.Add(Left);
            _actions.Add(Right);
            _actions.Add(None);
        }
        public Action(int i)
        {
            this.i = i;
        }

        public static HashSet<Action> actions()
        {
            return _actions;
        }

        public bool isNoOp()
        {
            if (None == this)
            {
                return true;
            }
            return false;
        }

        public int getXResult(int curX)
        {
            int newX = curX;

            switch (this.i)
            {
                case 3:
                    newX--;
                    break;
                case 4:
                    newX++;
                    break;
            }

            return newX;
        }

        public int getYResult(int curY)
        {
            int newY = curY;

            switch (this.i)
            {
                case 1:
                    newY++;
                    break;
                case 2:
                    newY--;
                    break;
            }

            return newY;
        }

        public Action getFirstRightAngledAction()
        {
            Action a = null;

            switch (this.i)
            {
                case 1:
                case 2:
                    a = Left;
                    break;
                case 3:
                case 4:
                    a = Down;
                    break;
                case 0:
                    a = None;
                    break;
            }

            return a;
        }

        public Action getSecondRightAngledAction()
        {
            Action a = null;

            switch (this.i)
            {
                case 1:
                case 2:
                    a = Right;
                    break;
                case 3:
                case 4:
                    a = Up;
                    break;
                case 0:
                    a = None;
                    break;
            }

            return a;
        }
    }
}
