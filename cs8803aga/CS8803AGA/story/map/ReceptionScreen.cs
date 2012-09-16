using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.story.characters;

namespace CS8803AGA.story.map
{
    class ReceptionScreen : MapScreen
    {
        public ReceptionScreen() : base()
        {
            // Open up the left wall.
            for (int r = 1; r < MapScreen.HEIGHT-1; r++)
            {
                setFloorTile(r, 0, TileType.ROCK);
            }

            // Open up the door.
            setFloorTile((MapScreen.HEIGHT / 2) - 1, MapScreen.WIDTH - 1, TileType.ROCK);
            setFloorTile(MapScreen.HEIGHT / 2, MapScreen.WIDTH - 1, TileType.ROCK);

            // Open 
            for (int c = MapScreen.WIDTH - 1; c > MapScreen.WIDTH - 7; c--)
            {
                if (c == MapScreen.WIDTH - 6)
                {
                    setFloorTile(1, c, TileType.DESK);
                    setFloorTile(2, c, TileType.DESK);
                    setFloorTile(3, c, TileType.DESK);
                }
                setFloorTile(4, c, TileType.DESK);
            }
        }

        public override void placeCharacters()
        {
            // This should be a receptionist.
            this.placeCharacter(3, MapScreen.WIDTH - 3, new Receptionist());
        }
    }
}
