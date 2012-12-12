using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.engine;
using CS8803AGA.collision;
using CS8803AGAGameLibrary;
using CS8803AGA.story.behaviors;
using CS8803AGA.story.map;
using CS8803AGA.PsychSim;

namespace CS8803AGA.story.characters
{
    public class Riedl : Character
    {
        public static LabScreen.LabLocation TASTE = acquireTaste();

        public enum ThingToDoToRiedl
        {
            SHAKE_HAND = 1,
            TELL_JOKE = 2,
            DISCUSS_THEORY = 3,
            ACE_TEST = 4,
            DO_PROJECT = 9,
            PRESENT_THESIS = 10,
            REQUEST_FUNDING
        };

        public Model Mind
        {
            get { return mnd; }
        }
        private Model mnd = new Model();

        bool explained;
        bool openedPuzzle;
        //bool openedPuzzle2;
        bool completedPuzzle;
        //bool completedPuzzle2;
        //bool completedPuzzle3;
        //bool hasForm;
        bool hasSignature;
        bool playerWon;
        bool simaWatching;

        public static LabScreen.LabLocation acquireTaste()
        {
            switch (RandomManager.get().Next(4))
            {
                case 0:
                    return LabScreen.LabLocation.CHICKEN;
                case 1:
                    return LabScreen.LabLocation.LOBSTER;
                case 2:
                    return LabScreen.LabLocation.PIZZA;
                default:
                    return LabScreen.LabLocation.STEAK;
            }
        }

        public struct Evaluation
        {
            public string description;
            public string explanation;
            public bool successful;
        }

        public static Evaluation evaluateTask(LinkedList<Behavior> attempt)
        {
            bool rightFood = false;
            bool hasFood = false;
            bool hasHotFood = false;
            bool deliveredFood = false;
            bool hasCake = false;
            bool delieveredCake = false;
            string description = "";

            Behavior last = null;
            for (LinkedList<Behavior>.Enumerator e = attempt.GetEnumerator(); e.MoveNext(); )
            {
                description += e.Current.getDescription() + "\n";
                if (e.Current is InteractBehavior && last != null)
                {
                    if (last is GoToBehavior)
                    {
                        GoToBehavior gtb = (GoToBehavior)last;
                        if (gtb.getLocation() == LabScreen.LabLocation.CHICKEN ||
                            gtb.getLocation() == LabScreen.LabLocation.LOBSTER ||
                            gtb.getLocation() == LabScreen.LabLocation.PIZZA ||
                            gtb.getLocation() == LabScreen.LabLocation.STEAK)
                        {
                            hasFood = true;
                            rightFood = (gtb.getLocation() == TASTE);
                        }
                        else if (gtb.getLocation() == LabScreen.LabLocation.CAKE)
                        {
                            hasCake = true;
                        }
                        else if (gtb.getLocation() == LabScreen.LabLocation.MICROWAVE && hasFood)
                        {
                            hasHotFood = true;
                        }
                        else if (gtb.getLocation() == LabScreen.LabLocation.RIEDL)
                        {
                            deliveredFood = deliveredFood || hasFood;
                            delieveredCake = delieveredCake || hasCake;
                        }
                    }
                }

                last = e.Current;
            }

            Evaluation response = new Evaluation();
            response.description = description;
            response.successful = (hasHotFood && rightFood && deliveredFood && delieveredCake);

            if (response.successful)
            {
                response.explanation = "Well done! SIMA's got it!";
            }
            else if (!deliveredFood)
            {
                response.explanation = "Where is my food?";
            }
            else if (!hasHotFood)
            {
                response.explanation = "This food is cold.";
            }
            else if (!rightFood)
            {
                response.explanation = "That's not the food I wanted.";
            }
            else if (!delieveredCake)
            {
                response.explanation = "Where is my cake?";
            }

            return response;
        }

        public override string getDialogue(bool shouting)
        {
            if (playerWon)
            {
                return "DR. RIEDL: Now that you have graduated, I guess you should find a job.\nI can't help you with that.\nGood luck.";
            }
            else if (hasSignature && completedPuzzle)
            {
                return "DR. RIEDL: Excellent work!\nWith your research complete and approval from the REGISTRAR,\nI feel comfortable signing off on your graduation.\nYour parents will be so proud.";
            }
            else if (hasSignature && !completedPuzzle)
            {
                return "DR. RIEDL: Good.\nYou got your GRADUATION APPLICATION signed.\nNow finalize your research so you graduate.";
            }
            else if (!hasSignature && completedPuzzle)
            {
                return "DR. RIEDL: Congratulations!\nYour research is complete.\nGo get your GRADUATION APPLICATION signed by the REGISTRAR\nso that you can graduate.";
            }
            //else if (hasForm)
            //{
            //    return "DR. RIEDL: Go get your GRADUATION APPLICATION signed by the REGISTRAR\nand finalize your results.";
            //}
            //else if (completedPuzzle2)
            //{
            //    return "DR. RIEDL: Wow!\nYour latest results are even more exciting.\nYour graduation is approaching quickly.\nGo get this GRADUATION APPLICATION signed by the REGISTRAR,\nthen finalize your research with SIMA.\n\n\n\n(You got a GRADUATION APPLICATION.)";
            //}
            //else if (openedPuzzle2)
            //{
            //    return "DR. RIEDL: Shouldn't you be working on your research?";
            //}
            //else if (completedPuzzle1)
            //{
            //    return "DR. RIEDL: Aha!\nI see that you have completed the first phase of your research.\nCongratulations.\nYou still have a long way to go, though.\nWhy don't you head back over to SIMA and do some more work.";
            //}
            //else if (openedPuzzle1)
            //{
            //    return "DR. RIEDL: Go power up SIMA and begin your research.";
            //}
            else if (!explained)
            {
                return "DR. RIEDL: Welcome to the lab, young Computer Scientist.\nYou are here to complete your Master's project under my tutilege.\nI am Dr. Riedl.\nHead over to the computer system and start working.\nYou will need to show it how to make me lunch.\nPlus, go get your GRADUATION APPLICATION signed by the REGISTRAR.\n\n\n\n(You got a GRADUATION APPLICATION.)";
            }
            else
            {
                return "DR. RIEDL: This is a curious situation. I've nothing to say.";
            }
        }

        public override void act(Collider mover, bool shouting)
        {
            setFlags();

            if (!simaWatching)
            {
                GameplayManager.say(getDialogue(shouting));
            }

            if (completedPuzzle && hasSignature)
            {
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_WON] = true;
                GameplayManager.say(getDialogue(shouting));
            }
            else if (openedPuzzle && !completedPuzzle)
            {
                GameplayManager.Game.Keys[GameState.GameFlag.RIEDL_WAITING] = true;
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_PARALYZED] = true;
            }
            //if (completedPuzzle2 && !hasForm)
            //{
            //    GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_HAS_GRADUATION_FORM] = true;
            //}
            //if (completedPuzzle1)
            //{
            //    GameplayManager.Game.Keys[GameState.GameFlag.ACCESSED_PUZZLE_2] = true;
            //}
            if (!explained)
            {
                // GameplayManager.Game.Keys[GameState.GameFlag.RIEDL_HAS_EXPLAINED] = true;
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_ACCESSED_PUZZLE] = true;
                // GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_HAS_GRADUATION_FORM] = true;
            }
        }

        private void setFlags()
        {
            explained = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_ACCESSED_PUZZLE) &&
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_ACCESSED_PUZZLE]);
            //explained = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.RIEDL_HAS_EXPLAINED) &&
            //    GameplayManager.Game.Keys[GameState.GameFlag.RIEDL_HAS_EXPLAINED]);
            //openedPuzzle1 = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.ACCESSED_PUZZLE_1) &&
            //    GameplayManager.Game.Keys[GameState.GameFlag.ACCESSED_PUZZLE_1]);
            //openedPuzzle2 = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.ACCESSED_PUZZLE_2) &&
            //    GameplayManager.Game.Keys[GameState.GameFlag.ACCESSED_PUZZLE_2]);
            completedPuzzle = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_COMPLETED_PUZZLE) &&
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_COMPLETED_PUZZLE]);
            //completedPuzzle2 = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.COMPLETED_PUZZLE_2) &&
            //    GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_2]);
            //completedPuzzle3 = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.COMPLETED_PUZZLE_3) &&
            //    GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_3]);
            //hasForm = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_HAS_GRADUATION_FORM) &&
            //    GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_HAS_GRADUATION_FORM]);
            hasSignature = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.REGISTRAR_SIGNED_FORM) &&
                GameplayManager.Game.Keys[GameState.GameFlag.REGISTRAR_SIGNED_FORM]);
            playerWon = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_WON) &&
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_WON]);
            simaWatching = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.SIMA_WATCHING) &&
                GameplayManager.Game.Keys[GameState.GameFlag.SIMA_WATCHING]);
        }

        public override CharacterInfo getCharacterInfo()
        {
            return GlobalHelper.loadContent<CharacterInfo>(@"Characters/Riedl");
        }
    }
}
