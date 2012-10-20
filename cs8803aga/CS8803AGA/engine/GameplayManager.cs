﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CS8803AGA.controllers;
using CS8803AGA.story;
using CS8803AGA.story.characters;

namespace CS8803AGA.engine
{
    /// <summary>
    /// Static class for quickly accessing active gameplay information.
    /// Probably pure evil, but very handy.
    /// </summary>
    public class GameplayManager
    {
        public static EngineStateGameplay GameplayState { get; set; }

        public static PlayerController Player { get { return playerController; } }
        private static PlayerController playerController = null;

        public static Area ActiveArea { get { return activeArea; } }
        private static Area activeArea = null;

        public static GameState Game { get { return game; } set { game = value; } }
        private static GameState game = null;

        public static void initialize(EngineStateGameplay esg, PlayerController pc, Area startArea)
        {
            GameplayState = esg;
            playerController = pc;
            activeArea = startArea;

            activeArea.add(pc);
        }

        /// <summary>
        /// Moves the player from one area to another.
        /// </summary>
        /// <param name="arrivingArea">New area.</param>
        /// <param name="targetTile">Tile in that area the player should appear on.</param>
        public static void changeActiveArea(Area arrivingArea, Point targetTile)
        {
            Area departingArea = GameplayManager.ActiveArea;

            if (arrivingArea == departingArea)
                return;

            departingArea.remove(GameplayManager.Player);

            Rectangle targetRectangle = arrivingArea.getTileRectangle(targetTile.X, targetTile.Y);
            Vector2 newPos = new Vector2(
                targetRectangle.X + arrivingArea.TileSet.tileWidth / 2,
                targetRectangle.Y + arrivingArea.TileSet.tileHeight / 2);

            Player.getCollider().move(newPos - Player.getCollider().Bounds.Center());

            arrivingArea.add(GameplayManager.Player);

            activeArea = arrivingArea;
        }

        public static void drawHUD()
        {
            // TODO
        }

        public static void say(string text)
        {
            EngineManager.pushState(new EngineStateDialogue(text));
        }

        internal static void runPuzzle(int p)
        {
            EngineManager.pushState(new EngineStatePuzzle(p));
        }

        internal static void move(int id, Point destination)
        {
            foreach (IGameObject obj in ActiveArea.GameObjects)
            {
                CharacterController cc;

                // If this is a character...
                if (obj is CharacterController)
                {
                    // and it's the right character...
                    cc = (CharacterController)obj;
                    if (cc.NpcID == id)
                    {
                        cc.moveTo(destination);
                    }
                }
            }
        }
    }
}
