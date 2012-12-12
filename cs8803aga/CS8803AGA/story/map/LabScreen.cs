using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.story.characters;
using Microsoft.Xna.Framework;

namespace CS8803AGA.story.map
{
    public class LabScreen : MapScreen
    {
        public enum LabLocation { CAKE, CHICKEN, LOBSTER, MICROWAVE, PIZZA, RIEDL, SIMA, STEAK };

        public static readonly Dictionary<LabLocation, Point> LOCATIONS = new Dictionary<LabLocation, Point>() {
            { LabLocation.CAKE, new Point(25*40, (MapScreen.HEIGHT-2)*40) },
            { LabLocation.CHICKEN, new Point(5*40, (MapScreen.HEIGHT-2)*40) },
            { LabLocation.LOBSTER, new Point(10*40, (MapScreen.HEIGHT-2)*40) },
            { LabLocation.MICROWAVE, new Point(15*40, 2*40) },
            { LabLocation.PIZZA, new Point(15*40, (MapScreen.HEIGHT-2)*40) },
            { LabLocation.RIEDL, new Point(3*40, 9*40) },
            { LabLocation.SIMA, new Point(20*40, 2*40) },
            { LabLocation.STEAK, new Point(20*40, (MapScreen.HEIGHT-2)*40) },
        };

        public LabScreen()
            : base()
        {
            for (int r = 1; r < MapScreen.HEIGHT - 1; r++)
            {
                setFloorTile(r, MapScreen.WIDTH - 1, TileType.ROCK);
            }
        }

        public override void init()
        {
            this.placeCharacter(LOCATIONS[LabLocation.RIEDL].Y / 40, LOCATIONS[LabLocation.RIEDL].X / 40, new Riedl());
            this.placeCharacter(LOCATIONS[LabLocation.SIMA].Y / 40, LOCATIONS[LabLocation.SIMA].X / 40, new SIMA());

            this.placeCharacter(LOCATIONS[LabLocation.CHICKEN].Y / 40, LOCATIONS[LabLocation.CHICKEN].X / 40, new Food(Food.FoodType.CHICKEN));
            this.placeCharacter(LOCATIONS[LabLocation.LOBSTER].Y / 40, LOCATIONS[LabLocation.LOBSTER].X / 40, new Food(Food.FoodType.LOBSTER));
            this.placeCharacter(LOCATIONS[LabLocation.PIZZA].Y / 40, LOCATIONS[LabLocation.PIZZA].X / 40, new Food(Food.FoodType.PIZZA));
            this.placeCharacter(LOCATIONS[LabLocation.STEAK].Y / 40, LOCATIONS[LabLocation.STEAK].X / 40, new Food(Food.FoodType.STEAK));
            this.placeCharacter(LOCATIONS[LabLocation.CAKE].Y / 40, LOCATIONS[LabLocation.CAKE].X / 40, new Food(Food.FoodType.CAKE));
            this.placeCharacter(LOCATIONS[LabLocation.MICROWAVE].Y / 40, LOCATIONS[LabLocation.MICROWAVE].X / 40, new Microwave());

            this.placeCharacter(3, MapScreen.WIDTH - 2, new Dean());
        }
    }
}
