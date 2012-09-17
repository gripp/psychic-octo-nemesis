using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.controllers;
using Microsoft.Xna.Framework;
using CS8803AGA.collision;
using CS8803AGA.engine;
using CS8803AGA.story.characters;

namespace CS8803AGA.story
{
    public class KeyCheck : ATrigger
    {
        private bool shouting;
        private int characterID;
        public KeyCheck(int id, bool shout, Rectangle bounds)
            : base(bounds)
        {
            shouting = shout;
            characterID = id;
        }

        public override bool isAlive() { return true; }

        public override void update() { /* Do nothing. */ }

        public override void draw() { /* Do nothing. */ }

        public override void handleImpact(Collider mover)
        {
            foreach (Character c in GameplayManager.Game.Characters)
            {
                if (c.ID == characterID)
                {
                    c.act(mover, shouting);
                }
            }
        }
    }
}
