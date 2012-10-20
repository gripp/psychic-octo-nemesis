using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.collision;
using CS8803AGA.engine;
using CS8803AGAGameLibrary;
using System.Threading;
using Microsoft.Xna.Framework;
using CS8803AGA.story.map;

namespace CS8803AGA.story.characters
{
    class SIMA : Character
    {
        bool openedPuzzle1;
        bool openedPuzzle2;
        bool completedPuzzle1;
        bool completedPuzzle2;
        bool completedPuzzle3;
        bool hasForm;

        public override string getDialogue(bool shouting)
        {
            if (completedPuzzle3)
            {
                return "SIMA: Boop beep boop.\nI am SIMA: the SUPER INTELLIGENT MIND AGENT.\nBoop beep boop.\nI can feel the knowledge seething through my circuits.\nI have achieved sentience.";
            }
            else if (hasForm)
            {
                return "SIMA: Boop beep boop.\nI am SIMA: the SUPER INTELLIGENT MIND AGENT.\nBoop beep boop.\nI am ready to be trained in the ways of your intelligence.";
            }
            else if (completedPuzzle2)
            {
                return "SIMA: Boop beep boop.\nI am SIMA: the SUPER INTELLIGENT MIND AGENT.\nBoop beep boop.\nI believe we are coming to an understanding.";
            }
            else if (openedPuzzle2)
            {
                return "SIMA: Boop beep boop.\nI am SIMA: the SUPER INTELLIGENT MIND AGENT.\nBoop beep boop.\nI am ready to be trained in the ways of your intelligence.";
            }
            else if (completedPuzzle1)
            {
                return "SIMA: Boop beep boop.\nI am SIMA: the SUPER INTELLIGENT MIND AGENT.\nBoop beep boop.\nMy knowledge grows daily.";
            }
            else if (openedPuzzle1)
            {
                return "SIMA: Boop beep boop.\nI am SIMA: the SUPER INTELLIGENT MIND AGENT.\nBoop beep boop.\nI am ready to be trained in the ways of your intelligence.";
            }
            else
            {
                return "SIMA: Boop beep boop.\nI am SIMA: the SUPER INTELLIGENT MIND AGENT.\nBoop beep boop.\nAt this particular moment, that seems to me a misnomer.";
            }
        }

        public override void act(Collider mover, bool shouting)
        {
            setFlags();
            if (!openedPuzzle1 || completedPuzzle1)
            {
                GameplayManager.say(getDialogue(shouting));
            }
            else
            {
                GameplayManager.move(this.ID, LabScreen.LOCATIONS[LabScreen.LabLocation.PIZZA]);
            }


            //Thread t = new Thread(new ThreadStart(this.actHelper));
            //t.Start();
            //while (!t.IsAlive) ;
        }
        private void actHelper()
        {
            while (EngineManager.peekAtState().getStateType().CompareTo("EngineStateDialogue") == 0)
            {
                // Wait.
            }
            if (completedPuzzle3)
            {
                // Do nothing.
            }
            else if (hasForm)
            {
                // Run puzzle three and get results. Did the player succeed?
                GameplayManager.runPuzzle(3);
            }
            else if (openedPuzzle2 && !completedPuzzle2)
            {
                // Run puzzle three and get results. Did the player succeed?
                GameplayManager.runPuzzle(2);
                bool result = true;
                GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_2] = result;
            }
            else if (openedPuzzle1 && !completedPuzzle1)
            {
                // Run puzzle three and get results. Did the player succeed?
                GameplayManager.runPuzzle(1);
                bool result = true;
                GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_1] = result;
            }
        }


        private void setFlags()
        {
            openedPuzzle1 = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.ACCESSED_PUZZLE_1) &&
                GameplayManager.Game.Keys[GameState.GameFlag.ACCESSED_PUZZLE_1]);
            openedPuzzle2 = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.ACCESSED_PUZZLE_2) &&
                GameplayManager.Game.Keys[GameState.GameFlag.ACCESSED_PUZZLE_2]);
            completedPuzzle1 = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.COMPLETED_PUZZLE_1) &&
                GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_1]);
            completedPuzzle2 = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.COMPLETED_PUZZLE_2) &&
                GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_2]);
            completedPuzzle3 = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.COMPLETED_PUZZLE_3) &&
                GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_3]);
            hasForm = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_HAS_GRADUATION_FORM) &&
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_HAS_GRADUATION_FORM]);
        }

        public override CharacterInfo getCharacterInfo()
        {
            return GlobalHelper.loadContent<CharacterInfo>(@"Characters/SIMA");
        }
    }
}
