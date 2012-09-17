using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.story.characters;
using CS8803AGA.maze;

namespace CS8803AGA.story.map
{
    class RegistrarOfficeScreen : MapScreen
    {
        public RegistrarOfficeScreen()
            : base()
        {
            setFloorTile((MapScreen.HEIGHT / 2) - 1, 0, TileType.ROCK);
            setFloorTile(MapScreen.HEIGHT / 2, 0, TileType.ROCK);

            Maze maze = new Maze(12, 7);
            maze.Generate(12, 7, 0);
            Cell[,] mazeGrid = maze.Cells;

            for (int r = 0; r < mazeGrid.GetLength(0) * 2; r++)
            {
                for (int c = 0; c < mazeGrid.GetLength(1) * 2; c++)
                {
                    setFloorTile(r + 1, c + 4, TileType.WALL);
                }
            }

            for (int r = 0; r < mazeGrid.GetLength(0); r++)
            {
                int row = (r * 2) + 1;
                for (int c = 0; c < mazeGrid.GetLength(1); c++)
                {
                    int col = (c * 2) + 4;
                    setFloorTile(row, col, TileType.ROCK);

                    if (!mazeGrid[r, c].DownWall)
                    {
                        setFloorTile(row + 1, col, TileType.ROCK);
                    }
                    if (!mazeGrid[r, c].LeftWall)
                    {
                        setFloorTile(row, col - 1, TileType.ROCK);
                    }
                    if (!mazeGrid[r, c].RightWall)
                    {
                        setFloorTile(row, col + 1, TileType.ROCK);
                    }
                    if (!mazeGrid[r, c].UpWall)
                    {
                        setFloorTile(row - 1, col, TileType.ROCK);
                    }
                }
            }
        }

        public override void init()
        {
            this.placeCharacter(MapScreen.HEIGHT / 2, MapScreen.WIDTH - 2, new Registrar());
        }
    }
}
