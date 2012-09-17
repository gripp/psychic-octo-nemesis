using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.engine;
using CS8803AGA.collision;

namespace CS8803AGA.story.characters
{
    class Riedl : Character
    {
        bool explained;
        bool openedPuzzle1;
        bool openedPuzzle2;
        bool completedPuzzle1;
        bool completedPuzzle2;
        bool completedPuzzle3;
        bool hasForm;
        bool hasSignature;
        bool playerWon;

        public override string getDialogue(bool shouting)
        {
            if (playerWon)
            {
                return "DR. RIEDL: Now that you have graduated, I guess you should find a job.\nI can't help you with that.\nGood luck.";
            }
            else if (hasSignature && completedPuzzle3)
            {
                return "DR. RIEDL: Excellent work!\nWith your research complete and approval from the REGISTRAR,\nI feel comfortable signing off on your graduation.\nYour parents will be so proud.";
            }
            else if (hasSignature && !completedPuzzle3)
            {
                return "DR. RIEDL: Good.\nYou got your GRADUATION APPLICATION signed.\nNow finalize your research so you graduate.";
            }
            else if (!hasSignature && completedPuzzle3)
            {
                return "DR. RIEDL: Congratulations!\nYour research is complete.\nGo get your GRADUATION APPLICATION signed by the REGISTRAR\nso that you can graduate.";
            }
            else if (hasForm)
            {
                return "DR. RIEDL: Go get your GRADUATION APPLICATION signed by the REGISTRAR\nand finalize your results.";
            }
            else if (completedPuzzle2)
            {
                return "DR. RIEDL: Wow!\nYour latest results are even more exciting.\nYour graduation is approaching quickly.\nGo get this GRADUATION APPLICATION signed by the REGISTRAR,\nthen finalize your research with SIMA.\n\n\n\n(You got a GRADUATION APPLICATION.)";
            }
            else if (openedPuzzle2)
            {
                return "DR. RIEDL: Shouldn't you be working on your research?";
            }
            else if (completedPuzzle1)
            {
                return "DR. RIEDL: Aha!\nI see that you have completed the first phase of your research.\nCongratulations.\nYou still have a long way to go, though.\nWhy don't you head back over to SIMA and do some more work.";
            }
            else if (openedPuzzle1)
            {
                return "DR. RIEDL: Go power up SIMA and begin your research.";
            }
            else if (!explained)
            {
                return "DR. RIEDL: Welcome to the lab, young Computer Scientist.\nYou are here to complete your Master's project under my tutilege.\nI am Dr. Riedl.\nHead over to the computer system and start working.";
            }
            else
            {
                return "DR. RIEDL: This is a curious situation. I've nothing to say.";
            }
        }

        public override void act(Collider mover, bool shouting)
        {
            setFlags();
            GameplayManager.say(getDialogue(shouting));

            if (completedPuzzle3 && hasSignature)
            {
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_WON] = true;
            }
            if (completedPuzzle2 && !hasForm)
            {
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_HAS_GRADUATION_FORM] = true;
            }
            if (completedPuzzle1)
            {
                GameplayManager.Game.Keys[GameState.GameFlag.ACCESSED_PUZZLE_2] = true;
            }
            if (!explained)
            {
                GameplayManager.Game.Keys[GameState.GameFlag.RIEDL_HAS_EXPLAINED] = true;
                GameplayManager.Game.Keys[GameState.GameFlag.ACCESSED_PUZZLE_1] = true;
            }
        }

        private void setFlags()
        {
            explained = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.RIEDL_HAS_EXPLAINED) &&
                GameplayManager.Game.Keys[GameState.GameFlag.RIEDL_HAS_EXPLAINED]);
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
            hasSignature = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.REGISTRAR_SIGNED_FORM) &&
                GameplayManager.Game.Keys[GameState.GameFlag.REGISTRAR_SIGNED_FORM]);
            playerWon= (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_WON) &&
                            GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_WON]);
        }
    }
}
