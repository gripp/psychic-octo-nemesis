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

        public override void init()
        {
            this.placeCharacter(9, 3, new Riedl());
            this.placeCharacter(2, 20, new SIMA());

            this.placeCharacter(MapScreen.HEIGHT - 2, 5, new Food(Food.FoodType.CHICKEN));
            this.placeCharacter(MapScreen.HEIGHT - 2, 10, new Food(Food.FoodType.LOBSTER));
            this.placeCharacter(MapScreen.HEIGHT - 2, 15, new Food(Food.FoodType.PIZZA));
            this.placeCharacter(MapScreen.HEIGHT - 2, 20, new Food(Food.FoodType.STEAK));
            this.placeCharacter(MapScreen.HEIGHT - 2, 25, new Food(Food.FoodType.CAKE));
            this.placeCharacter(2, 15, new Microwave());
        }
    }
}
