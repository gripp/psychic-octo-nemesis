using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.story.characters;

namespace CS8803AGA.story.map
{
    class RegistrarOfficeScreen : MapScreen
    {
        public RegistrarOfficeScreen()
            : base()
        {
            setFloorTile((MapScreen.HEIGHT / 2) - 1, 0, TileType.ROCK);
            setFloorTile(MapScreen.HEIGHT / 2, 0, TileType.ROCK);
        }

        public override void init()
        {
            this.placeCharacter(10, 10, new Registrar());
        }
    }
}
