using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS8803AGA.story.map
{
    class MapScreen
    {
        public const int WIDTH = 32;
        public const int HEIGHT = 16;

        public enum TileType { GRASS, ROCK, WALL };
        private TileType[,] floor = new TileType[HEIGHT,WIDTH];

        public MapScreen()
        {
            buildFloor();
            placeCharacters();
        }

        private void placeCharacters()
        {
            // Do nothing for now.
        }

        private void buildFloor()
        {
            for (int r = 0; r < floor.GetLength(0); r++)
            {
                for (int c = 0; c < floor.GetLength(1); c++)
                {
                    if (c == 0 || c == WIDTH - 1 || r == 0 || r == HEIGHT - 1)
                    {
                        floor[r, c] = TileType.WALL;
                    }
                    else
                    {
                        floor[r, c] = TileType.ROCK;
                    }
                }
            }
        }

        protected void setFloorTile(int r, int c, TileType tile)
        {
            floor[r, c] = tile;
        }
        public TileType getFloorTile(int r, int c)
        {
            return floor[r, c];
        }
    }
}