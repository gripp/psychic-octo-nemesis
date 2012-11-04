﻿using System;
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
using CS8803AGA.story.behaviors;

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

        private LinkedList<Behavior> example;
        private Behavior startPoint = null;

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
                string dialogue = "SIMA: Alright. I got it:\n\n";
                for(LinkedList<Behavior>.Enumerator e = example.GetEnumerator(); e.MoveNext();)
                {
                    dialogue += e.Current.getDescription();
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
                if (example.Count > 0)
                {
                    if (startPoint == null)
                    {
                        Behavior last = null;
                        foreach (Behavior b in example)
                        {
                            if (last == null)
                            {
                                startPoint = b;
                                last = startPoint;
                            }
                            else
                            {
                                last.setNext(b);
                            }
                        }
                    }
                    else
                    {
                        List<LinkedList<Behavior>> paths = getAllPaths(startPoint);
                        int[,] lcsGrid = getLCSGrid(example, paths[0]);
                        int bestPath = 0;
                        int[,] hold;
                        for (int i = 1; i < paths.Count; i++)
                        {
                            hold = getLCSGrid(example, paths[i]);
                            if (hold[hold.GetLength(0) - 1, hold.GetLength(1) - 1] > lcsGrid[lcsGrid.GetLength(0) - 1, lcsGrid.GetLength(1) - 1])
                            {
                                lcsGrid = hold;
                                bestPath = i;
                            }
                        }

                        LinkedList<Behavior> lcs = getLCS(lcsGrid, example, paths[bestPath], example.Count, paths[bestPath].Count);

                        // Now I've got everything I need to form the new task structure. Look at the window and synthesize!
                    }
                }
                GameplayManager.Game.Keys[GameState.GameFlag.SIMA_WATCHING] = false;
            }
            else if (openedPuzzle && !completedPuzzle)
            {
                example = new LinkedList<Behavior>();
                GameplayManager.Game.Keys[GameState.GameFlag.SIMA_WAITING] = true;
                GameplayManager.Game.Keys[GameState.GameFlag.PARALYZED] = true;
            }
            GameplayManager.say(getDialogue(shouting));

            //Thread t = new Thread(new ThreadStart(this.actHelper));
            //t.Start();
            //while (!t.IsAlive) ;
        }

        private List<LinkedList<Behavior>> getAllPaths(Behavior start)
        {
            List<LinkedList<Behavior>> allPaths = new List<LinkedList<Behavior>>();
            LinkedList<Behavior> list = new LinkedList<Behavior>();

            for (Behavior b = start; b.getNext() != null;)
            {
                if (b is ComboBehavior)
                {
                    LinkedList<Behavior> firstHalf;
                    List<LinkedList<Behavior>> secondHalves;
                    foreach (Behavior h in ((ComboBehavior)b).getAllBranches())
                    {
                        secondHalves = getAllPaths(h);
                        foreach (LinkedList<Behavior> secondHalf in secondHalves)
                        {
                            firstHalf = copyList(list);
                            foreach (Behavior r in secondHalf)
                            {
                                firstHalf.AddLast(r);
                            }
                            allPaths.Add(firstHalf);
                        }
                    }
                    list = null;
                    break;
                }
                else
                {
                    list.AddLast(b);
                }
            }

            if (list != null)
            {
                allPaths.Add(list);
            }
            return allPaths;
        }

        private LinkedList<Behavior> copyList(LinkedList<Behavior> list)
        {
            LinkedList<Behavior> copy = new LinkedList<Behavior>();
            foreach (Behavior b in list)
            {
                copy.AddLast(b.makeCopy());
            }

            return copy;
        }

        public static LinkedList<Behavior> getLCS(int[,] grid, LinkedList<Behavior> sequenceA, LinkedList<Behavior> sequenceB, int i, int j)
        {
            if (i == 0 || j == 0)
            {
                return new LinkedList<Behavior>();
            }
            else if (sequenceA.ElementAt(i).compareTo(sequenceB.ElementAt(j)) == 0)
            {
                LinkedList<Behavior> hold = getLCS(grid, sequenceA, sequenceB, i - 1, j - 1);
                hold.AddLast(sequenceA.ElementAt(i));
                return hold;
            }
            else
            {
                if (grid[i, j - 1] > grid[i - 1, j])
                {
                    return getLCS(grid, sequenceA, sequenceB, i, j - 1);
                }
                else
                {
                    return getLCS(grid, sequenceA, sequenceB, i - 1, j);
                }
            }
        }

        public static int[,] getLCSGrid(LinkedList<Behavior> sequenceA, LinkedList<Behavior> sequenceB)
        {
            int[,] grid = new int[sequenceA.Count + 1, sequenceB.Count + 1];
            int i;
            int j;

            for (i = 0; i < grid.GetLength(0); i++)
            {
                grid[i, 0] = 0;
            }
            for (i = 0; i < grid.GetLength(1); i++)
            {
                grid[0, i] = 0;
            }

            i = 1;
            j = 1;
            for (LinkedList<Behavior>.Enumerator a = sequenceA.GetEnumerator(); a.MoveNext(); i++)
            {
                for (LinkedList<Behavior>.Enumerator b = sequenceB.GetEnumerator(); b.MoveNext(); j++)
                {
                    if (a.Current.compareTo(b.Current) == 0)
                    {
                        grid[i, j] = grid[i-1, j-1] + 1;
                    }
                    else
                    {
                        grid[i, j] = Math.Max(grid[i, j - 1], grid[i - 1, j]);
                    }
                }
            }

            return grid;
        }

        // This might later be used to spawn extra threads. But right now, we don't need it.
        //private void actHelper()
        //{
        //    while (EngineManager.peekAtState().getStateType().CompareTo("EngineStateDialogue") == 0)
        //    {
        //        // Wait.
        //    }
        //    if (completedPuzzle3)
        //    {
        //        // Do nothing.
        //    }
        //    else if (hasForm)
        //    {
        //        // Run puzzle three and get results. Did the player succeed?
        //        GameplayManager.runPuzzle(3);
        //    }
        //    else if (openedPuzzle2 && !completedPuzzle2)
        //    {
        //        // Run puzzle three and get results. Did the player succeed?
        //        GameplayManager.runPuzzle(2);
        //        bool result = true;
        //        GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_2] = result;
        //    }
        //    else if (openedPuzzle1 && !completedPuzzle1)
        //    {
        //        // Run puzzle three and get results. Did the player succeed?
        //        GameplayManager.runPuzzle(1);
        //        bool result = true;
        //        GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_1] = result;
        //    }
        //}

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

        public void show(Behavior action)
        {
            example.AddLast(action);
        }

        public void resetTask() { example = new LinkedList<Behavior>(); }
    }
}
