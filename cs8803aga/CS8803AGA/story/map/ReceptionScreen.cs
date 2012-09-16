using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS8803AGA.story.map
{
    class ReceptionScreen : MapScreen
    {
        public ReceptionScreen() : base()
        {
            for (int r = 1; r < MapScreen.HEIGHT-1; r++)
            {
                setFloorTile(r, 0, TileType.ROCK);
            }
        }
    }
}
