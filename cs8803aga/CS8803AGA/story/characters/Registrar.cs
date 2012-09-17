using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.engine;
using CS8803AGAGameLibrary;

namespace CS8803AGA.story.characters
{
    class Registrar : Character
    {
        bool hasForm;
        bool hasSignature;

        public override string getDialogue(bool shouting)
        {
            if (hasSignature)
            {
                return "REGISTRAR: I've already signed your form.\nThere's nothing more I can do for you.\nGood luck!";
            }
            else if (hasForm)
            {
                return "REGISTRAR: What's that?\nOf course I'll sign your GRADUATION APPLICATION for you.\nWhy would I make this difficult?";
            }
            else
            {
                return "REGISTRAR: How did you get in here without a graduation application?";
            }
        }

        public override void act(CS8803AGA.collision.Collider mover, bool shouting)
        {
            setFlags();
            GameplayManager.say(getDialogue(shouting));

            if (hasForm && !hasSignature)
            {
                GameplayManager.Game.Keys[GameState.GameFlag.REGISTRAR_SIGNED_FORM] = true;
            }
        }

        private void setFlags()
        {
            hasForm = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_HAS_GRADUATION_FORM) &&
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_HAS_GRADUATION_FORM]);
            hasSignature = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.REGISTRAR_SIGNED_FORM) &&
                GameplayManager.Game.Keys[GameState.GameFlag.REGISTRAR_SIGNED_FORM]);
        }

        public override CharacterInfo getCharacterInfo()
        {
            return GlobalHelper.loadContent<CharacterInfo>(@"Characters/Registrar");
        }
    }
}
