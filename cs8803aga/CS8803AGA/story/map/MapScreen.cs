using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.story.characters;

namespace CS8803AGA.story.map
{
    abstract class MapScreen
    {
        public const int WIDTH = 32;
        public const int HEIGHT = 16;

        public enum TileType { GRASS, ROCK, WALL };
        private TileType[,] floor = new TileType[HEIGHT,WIDTH];

        public List<CharacterRecord> Characters
        {
            get
            {
                return characters;
            }
        }
        private List<CharacterRecord> characters = new List<CharacterRecord>();

        public struct CharacterRecord
        {
            public CharacterRecord(int row, int col, Character chrctr)
            {
                r = row;
                c = col;
                character = chrctr;
            }

            public int r;
            public int c;
            public Character character;
        }

        public MapScreen()
        {
            buildFloor();
            placeCharacters();
        }

        public abstract void placeCharacters();

        protected void placeCharacter(int r, int c, Character chrctr)
        {
            characters.Add(new CharacterRecord(r, c, chrctr));
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