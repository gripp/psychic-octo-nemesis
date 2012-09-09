using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CS8803AGA.engine;

namespace CS8803AGA
{
    /// <summary>
    /// Static class for storing the Areas and Regions of the world
    /// TODO - make a singleton or store in a GameplayState
    /// </summary>
    public static class WorldManager
    {
        private static Dictionary<Point,Area> WorldMap;

        static WorldManager()
        {
            WorldMap = new Dictionary<Point, Area>();
        }

        public static Area GetArea(Point p)
        {
            if (WorldMap.ContainsKey(p))
                return WorldMap[p];

            return null;
        }

        public static void RegisterArea(Point p, Area area)
        {
            if (WorldMap.ContainsKey(p))
                throw new Exception("Area at p already registered");

            WorldMap[p] = area;
        }

        /// <summary>
        /// Draws a map to the screen.
        /// </summary>
        /// <param name="corner">Top left corner the map should be placed, in pixels.</param>
        /// <param name="mapWidth">Size available for map width, in pixels.</param>
        /// <param name="mapHeight">Size available for map height, in pixels.</param>
        /// <param name="depth">Drawing depth.</param>
        public static void DrawMap(Vector2 corner, float mapWidth, float mapHeight, float depth)
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            foreach (Point p in WorldMap.Keys)
            {
                minX = Math.Min(p.X, minX);
                minY = Math.Min(p.Y, minY);
                maxX = Math.Max(p.X, maxX);
                maxY = Math.Max(p.Y, maxY);
            }

            float scale = Math.Min(mapWidth / (Area.WIDTH_IN_TILES * Area.TILE_WIDTH * (maxX - minX + 1)),
                                    mapHeight / (Area.HEIGHT_IN_TILES * Area.HEIGHT_IN_TILES * (maxY - maxY + 1)));

            float mapAreaSizeX = Area.WIDTH_IN_TILES * Area.TILE_WIDTH * scale;
            float mapAreaSizeY = Area.HEIGHT_IN_TILES * Area.TILE_HEIGHT * scale;
            for (int i = minX; i <= maxX; ++i)
            {
                for (int j = minY; j <= maxY; ++j)
                {
                    Area a = WorldManager.GetArea(new Point(i, j));
                    if (a == null) continue;
                    Vector2 areaTopLeftCorner =
                        new Vector2(mapAreaSizeX * (i - minX) + corner.X,
                                    mapAreaSizeY * (j - minY) + corner.Y);

                    a.drawMap(areaTopLeftCorner, scale, depth);

                    // draw a box around the active area on the map
                    if (a == GameplayManager.ActiveArea)
                    {
                        Vector2 areaTopRightCorner =
                            new Vector2(mapAreaSizeX * (i - minX + 1) + corner.X,
                                        mapAreaSizeY * (j - minY) + corner.Y);
                        Vector2 areaBottomLeftCorner =
                            new Vector2(mapAreaSizeX * (i - minX) + corner.X,
                                        mapAreaSizeY * (j - minY + 1) + corner.Y);
                        Vector2 areaBottomRightCorner =
                            new Vector2(mapAreaSizeX * (i - minX + 1) + corner.X,
                                        mapAreaSizeY * (j - minY + 1) + corner.Y);
                        LineDrawer.drawLine(areaTopLeftCorner, areaTopRightCorner);
                        LineDrawer.drawLine(areaTopLeftCorner, areaBottomLeftCorner);
                        LineDrawer.drawLine(areaTopRightCorner, areaBottomRightCorner);
                        LineDrawer.drawLine(areaBottomLeftCorner, areaBottomRightCorner);
                    }
                }
            }
        }

        public static void reset()
        {
            WorldMap.Clear();
        }
    }
}
