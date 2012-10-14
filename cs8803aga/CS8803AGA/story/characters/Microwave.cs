using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.engine;
using CS8803AGA.controllers;
using Microsoft.Xna.Framework;
using CS8803AGA.collision;
using CS8803AGAGameLibrary;

namespace CS8803AGA.story.characters
{
    class Microwave : Character
    {
        public override string getDialogue(bool shouting)
        {
            return "Microwaves can't talk!";
        }

        public override void act(Collider mover, bool shouting)
        {
        }

        public override CharacterInfo getCharacterInfo()
        {
            return GlobalHelper.loadContent<CharacterInfo>(@"Characters/Microwave");
        }
    }
}