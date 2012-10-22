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
using System.ComponentModel;

namespace CS8803AGA.story.characters
{
    public class SIMA : Character
    {
        bool openedPuzzle;
        //bool openedPuzzle2;
        bool completedPuzzle;
        //bool completedPuzzle2;
        //bool completedPuzzle3;
        // bool hasForm;
        bool watching;

        public enum Behavior
        {
            GOTO_BASE,
            GOTO_CAKE,
            GOTO_CHICKEN,
            GOTO_LOBSTER,
            GOTO_MICROWAVE,
            GOTO_PIZZA,
            GOTO_RIEDL,
            GOTO_STEAK,
            INTERACT
        };

        public static string getBehaviorDescription(Behavior b)
        {
            switch (b)
            {
                case Behavior.GOTO_BASE:
                    return "Return to base.";
                case Behavior.GOTO_CAKE:
                    return "Go to cake.";
                case Behavior.GOTO_CHICKEN:
                    return "Go to chicken.";
                case Behavior.GOTO_LOBSTER:
                    return "Go to lobster.";
                case Behavior.GOTO_MICROWAVE:
                    return "Go to microwave.";
                case Behavior.GOTO_PIZZA:
                    return "Go to pizza.";
                case Behavior.GOTO_RIEDL:
                    return "Go to Riedl.";
                case Behavior.GOTO_STEAK:
                    return "Go to steak.";
                case Behavior.INTERACT:
                    return "Interact.";
                default:
                    return "";
            }
        }

        private Queue<Behavior> task;

        public override string getDialogue(bool shouting)
        {
            if (completedPuzzle)
            {
                return "SIMA: Boop beep boop.\nI am SIMA: the SUPER INTELLIGENT MIND AGENT.\nBoop beep boop.\nI can feel the knowledge seething through my circuits.\nI have achieved sentience.";
            }
            //else if (hasForm)
            //{
            //    return "SIMA: Boop beep boop.\nI am SIMA: the SUPER INTELLIGENT MIND AGENT.\nBoop beep boop.\nI am ready to be trained in the ways of your intelligence.";
            //}
            //else if (completedPuzzle2)
            //{
            //    return "SIMA: Boop beep boop.\nI am SIMA: the SUPER INTELLIGENT MIND AGENT.\nBoop beep boop.\nI believe we are coming to an understanding.";
            //}
            //else if (openedPuzzle2)
            //{
            //    return "SIMA: Boop beep boop.\nI am SIMA: the SUPER INTELLIGENT MIND AGENT.\nBoop beep boop.\nI am ready to be trained in the ways of your intelligence.";
            //}
            //else if (completedPuzzle1)
            //{
            //    return "SIMA: Boop beep boop.\nI am SIMA: the SUPER INTELLIGENT MIND AGENT.\nBoop beep boop.\nMy knowledge grows daily.";
            //}
            else if (watching)
            {
                string dialogue = "SIMA: Alright. I got it:\n";
                while (task.Count > 0)
                {
                    dialogue += getBehaviorDescription(task.Dequeue());
                    dialogue += "\n";
                }
                return dialogue;
            }
            else if (openedPuzzle)
            {
                return "SIMA: Boop beep boop.\nI am SIMA: the SUPER INTELLIGENT MIND AGENT.\nBoop beep boop.\nI am ready to be trained in the ways of your intelligence.\nTeach me to make lunch.\nPress W if you want me to watch.\nPress T if you want me to try to make lunch.";
            }
            else
            {
                return "SIMA: Boop beep boop.\nI am SIMA: the SUPER INTELLIGENT MIND AGENT.\nBoop beep boop.\nAt this particular moment, that seems to me a misnomer.";
            }
        }

        public override void act(Collider mover, bool shouting)
        {
            setFlags();
            if (watching)
            {
                GameplayManager.Game.Keys[GameState.GameFlag.SIMA_WATCHING] = false;
            }
            else if (openedPuzzle && !completedPuzzle)
            {
                task = new Queue<Behavior>();
                GameplayManager.Game.Keys[GameState.GameFlag.SIMA_WAITING] = true;
                GameplayManager.Game.Keys[GameState.GameFlag.PARALYZED] = true;
            }
            GameplayManager.say(getDialogue(shouting));

            //Thread t = new Thread(new ThreadStart(this.actHelper));
            //t.Start();
            //while (!t.IsAlive) ;
        }

        private void actHelper()
        {
            //while (EngineManager.peekAtState().getStateType().CompareTo("EngineStateDialogue") == 0)
            //{
            //    // Wait.
            //}
            //if (completedPuzzle3)
            //{
            //    // Do nothing.
            //}
            //else if (hasForm)
            //{
            //    // Run puzzle three and get results. Did the player succeed?
            //    GameplayManager.runPuzzle(3);
            //}
            //else if (openedPuzzle2 && !completedPuzzle2)
            //{
            //    // Run puzzle three and get results. Did the player succeed?
            //    GameplayManager.runPuzzle(2);
            //    bool result = true;
            //    GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_2] = result;
            //}
            //else if (openedPuzzle1 && !completedPuzzle1)
            //{
            //    // Run puzzle three and get results. Did the player succeed?
            //    GameplayManager.runPuzzle(1);
            //    bool result = true;
            //    GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_1] = result;
            //}
        }

        private void setFlags()
        {
            openedPuzzle = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.ACCESSED_PUZZLE) &&
                GameplayManager.Game.Keys[GameState.GameFlag.ACCESSED_PUZZLE]);
            //openedPuzzle2 = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.ACCESSED_PUZZLE_2) &&
            //    GameplayManager.Game.Keys[GameState.GameFlag.ACCESSED_PUZZLE_2]);
            completedPuzzle = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.COMPLETED_PUZZLE) &&
                GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE]);
            //completedPuzzle2 = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.COMPLETED_PUZZLE_2) &&
            //    GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_2]);
            //completedPuzzle3 = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.COMPLETED_PUZZLE_3) &&
            //    GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_3]);
            //hasForm = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_HAS_GRADUATION_FORM) &&
            //    GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_HAS_GRADUATION_FORM]);
            watching = (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.SIMA_WATCHING) &&
                GameplayManager.Game.Keys[GameState.GameFlag.SIMA_WATCHING]);
        }

        public override CharacterInfo getCharacterInfo()
        {
            return GlobalHelper.loadContent<CharacterInfo>(@"Characters/SIMA");
        }

        public void show(SIMA.Behavior action)
        {
            task.Enqueue(action);
        }

        public void resetTask() { task = new Queue<Behavior>(); }
    }
}
