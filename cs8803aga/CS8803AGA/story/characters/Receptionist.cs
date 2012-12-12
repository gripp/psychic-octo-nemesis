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
    class Receptionist : Character
    {
        public override string getDialogue(bool shouting)
        {
            if (!shouting &&
                !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_ACCESSED_PUZZLE) &&
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_ACCESSED_PUZZLE]))
            {
                return "RECEPTIONIST: Do you have your GRADUATION APPLICATION?\nNo?\nWell then you can't see the REGISTRAR yet.\nGo finish your research.";
            }
            else if (!(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_ACCESSED_PUZZLE) &&
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_ACCESSED_PUZZLE]))
            {
                return "RECEPTIONIST: You can't go through there without a GRADUATION APPLICATION.\nGo finish your research.";
            }
            else if (!shouting && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.REGISTRAR_DOOR_IS_OPEN) &&
                GameplayManager.Game.Keys[GameState.GameFlag.REGISTRAR_DOOR_IS_OPEN]))
            {
                return "RECEPTIONIST: You seem to have finished your research.\nCongratulations!\nGo get your GRADUATION APPLICATION signed by the REGISTRAR.";
            }
            else if (!(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.REGISTRAR_DOOR_IS_OPEN) &&
                GameplayManager.Game.Keys[GameState.GameFlag.REGISTRAR_DOOR_IS_OPEN]))
            {
                return "RECEPTIONIST: You can't go through there without a GRADUATION APPLICATION.\nWhat?\nOh. You have the form.\nWell go on through!";
            }
            else if (!(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.REGISTRAR_SIGNED_FORM) &&
                GameplayManager.Game.Keys[GameState.GameFlag.REGISTRAR_SIGNED_FORM]))
            {
                return "RECEPTIONIST: You need to get that form signed before you can graduate.";
            }
            else if (!(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_WON) &&
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_WON]))
            {
                return "RECEPTIONIST: You got the form signed! You're almost ready to graduate.";
            }
            else if ((GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_WON) &&
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_WON]))
            {
                return "RECEPTIONIST: Congratulations on your graduation.";
            }
            else
            {
                return "RECEPTIONIST: I've nothing to say!";
            }
        }

        public override void act(Collider mover, bool shouting)
        {
            // if (the door is open)
            if (shouting && GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.REGISTRAR_DOOR_IS_OPEN) && GameplayManager.Game.Keys[GameState.GameFlag.REGISTRAR_DOOR_IS_OPEN])
            {
                // Do nothing. Just let the user pass.
            }
            else if (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.REGISTRAR_DOOR_IS_OPEN) && GameplayManager.Game.Keys[GameState.GameFlag.REGISTRAR_DOOR_IS_OPEN])
            {
                GameplayManager.say(getDialogue(shouting));
            }
            // else if (have the graduation form)
            else if (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_ACCESSED_PUZZLE) && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_ACCESSED_PUZZLE])
            {
                // Announce that the door has been opened.
                GameplayManager.say(getDialogue(shouting));
                // Open the door.
                GameplayManager.Game.Keys[GameState.GameFlag.REGISTRAR_DOOR_IS_OPEN] = true;

            }
            // else
            else
            {
                // Announce that you can't do that, homeboy.
                GameplayManager.say(getDialogue(shouting));

                // Send player back.
                if (shouting)
                {
                    ((PlayerController)mover.m_owner).AnimationController.requestAnimation("left", AnimationController.AnimationCommand.Play);
                    mover.handleMovement(new Vector2(-20, 0));
                }
            }
        }

        public override CharacterInfo getCharacterInfo()
        {
            return GlobalHelper.loadContent<CharacterInfo>(@"Characters/Receptionist");
        }
    }
}