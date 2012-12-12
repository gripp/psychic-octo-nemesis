using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CS8803AGA.PsychSim.State
{
    class State<C>
    {
        HashSet<Cell<C>> cells = new HashSet<Cell<C>>();
        public Dictionary<int, Dictionary<int, Cell<C>>> cellLookup = new Dictionary<int, Dictionary<int, Cell<C>>>();


        public State(int xDimension, int yDimension, C defaultCellContent)
        {
            for (int x = 1; x <= xDimension; x++)
            {
                Dictionary<int, Cell<C>> xCol = new Dictionary<int, Cell<C>>();
                for (int y = 1; y <= yDimension; y++)
                {
                    Cell<C> c = new Cell<C>(x, y, defaultCellContent);
                    cells.Add(c);
                    xCol.Add(y, c);
                }
                cellLookup.Add(x, xCol);
            }
        }

        public HashSet<Cell<C>> getCells()
        {
            return cells;
        }

        public void removeCell(int x, int y)
        {

            if (cellLookup.ContainsKey(x))
            {
                Dictionary<int, Cell<C>> xCol = cellLookup[x];
                if (xCol.ContainsKey(y))
                {
                    Cell<C> temp = xCol[y];
                    cells.Remove(temp);
                    xCol.Remove(y);
                }
            }
        }

        public Cell<C> getCellAt(int x, int y)
        {
            Cell<C> c = null;

            if (cellLookup.ContainsKey(x))
            {
                Dictionary<int, Cell<C>> xCol = cellLookup[x];
                if (xCol.ContainsKey(y))
                {
                    c = xCol[y];
                }
            }

            return c;
        }

        public Cell<C> result(Cell<C> s, Action a)
        {
            Cell<C> sDelta = getCellAt(a.getXResult(s.getX()), a.getYResult(s.getY()));
            if (null == sDelta)
            {
                sDelta = s;
            }

            return sDelta;
        }

        public void setContent(C[] val)
        {
            getCellAt(1, 1).setContent(val[0]);
            getCellAt(1, 2).setContent(val[1]);
            getCellAt(1, 3).setContent(val[2]);

            getCellAt(2, 1).setContent(val[3]);
            getCellAt(2, 2).setContent(val[4]);
            getCellAt(2, 3).setContent(val[5]);

            getCellAt(3, 1).setContent(val[6]);
            getCellAt(3, 2).setContent(val[7]);
            getCellAt(3, 3).setContent(val[8]);
        }

    }
}
