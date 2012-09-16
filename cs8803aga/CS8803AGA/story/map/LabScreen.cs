using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.story.characters;

namespace CS8803AGA.story.map
{
    class LabScreen : MapScreen
    {
        public LabScreen() : base()
        {
            for (int r = 1; r < MapScreen.HEIGHT-1; r++)
            {
                setFloorTile(r, MapScreen.WIDTH - 1, TileType.ROCK);
            }
        }

        public override void placeCharacters()
        {
            this.placeCharacter(5, 10, new Riedl());
        }
    }
}
