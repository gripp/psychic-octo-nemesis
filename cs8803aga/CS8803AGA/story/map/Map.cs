using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS8803AGA.story.map
{
    public class Map
    {
        public int MapRows
        {
            get
            {
                return map.GetLength(0);
            }
        }

        public int MapCols
        {
            get
            {
                return map.GetLength(1);
            }
        }
        private MapScreen[,] map = new MapScreen[1, 3];

        public Map()
        {
            map[0, 0] = new LabScreen();
            map[0, 1] = new ReceptionScreen();
            map[0, 2] = new RegistrarOfficeScreen();
        }

        internal MapScreen getMapScreen(int r, int c)
        {
            return map[r, c];
        }
    }
}
