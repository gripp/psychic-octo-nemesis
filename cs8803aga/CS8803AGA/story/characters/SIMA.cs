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

        public LinkedList<Behavior> getTaskAttempt()
        {
            LinkedList<Behavior> taskAttempt = new LinkedList<Behavior>();
            for (Behavior node = startPoint; node != null; node = node.getNext())
            {
                if (!(node is ComboBehavior))
                {
                    taskAttempt.AddLast(node);
                }
            }
            return taskAttempt;
        }

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
                            }
                            else
                            {
                                last.setNext(b);
                            }

                            last = b;
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

                        startPoint = null;
                        LinkedList<Behavior> lcs = getLCS(lcsGrid, example, paths[bestPath], example.Count, paths[bestPath].Count);
                        
                        startPoint = mergePaths(example, paths[bestPath], lcs);
                        paths.RemoveAt(bestPath);

                        foreach (LinkedList<Behavior> llb in paths)
                        {
                            foldIn(llb, lcs);
                        }
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

        private void foldIn(LinkedList<Behavior> llb, LinkedList<Behavior> lcs)
        {
            bool go;
            bool start = true;
            bool pause = false;
            Behavior holdCurrent;
            Behavior holdLast;
            Behavior node = startPoint;
            Behavior lineStart;
            LinkedList<Behavior>.Enumerator e = llb.GetEnumerator();

            for (LinkedList<Behavior>.Enumerator lcsEnumerator = lcs.GetEnumerator(); pause || lcsEnumerator.MoveNext(); )
            {
                if (!start)
                {
                    node = lcsEnumerator.Current;
                }

                lineStart = null;

                holdLast = null;
                go = e.MoveNext();
                while (go && e.Current.compareTo(lcsEnumerator.Current) != 0)
                {
                    holdCurrent = e.Current.makeCopy();
                    if (lineStart == null)
                    {
                        lineStart = holdCurrent;
                    }
                    else
                    {
                        holdLast.setNext(holdCurrent);
                    }
                    holdLast = holdCurrent;
                    go = e.MoveNext();
                }
                if (lineStart != null && go)
                {
                    holdLast.setNext(node);
                }

                if (node is ComboBehavior)
                {
                    bool moveOn = false;
                    foreach (Behavior b in ((ComboBehavior)node).getAllBranches())
                    {
                        if (b.compareTo(lcsEnumerator.Current) == 0 && lineStart == null)
                        {
                            moveOn = true;
                            break;
                        }
                    }
                    if (!moveOn)
                    {
                        node.setNext(lineStart == null ? lcsEnumerator.Current : lineStart);
                    }
                }
                // Make a combo behavior if this isn't just the same LCS node.
                else if (!(lineStart == null && node.compareTo(lcsEnumerator.Current) == 0))
                {
                    ComboBehavior cb = new ComboBehavior();
                    cb.setNext(node);
                    cb.setNext(lcsEnumerator.Current);
                    startPoint = cb;
                }
                else
                {
                    lineStart = lcsEnumerator.Current;
                    pause = start;
                }

                start = false;
            }
        }

        private Behavior mergePaths(LinkedList<Behavior> list1, LinkedList<Behavior> list2, LinkedList<Behavior> lcs)
        {
            Behavior start = null;
            Behavior last = null;
            Behavior line1Start = null;
            Behavior line2Start = null;
            Behavior holdLast;
            Behavior holdCurrent;
            LinkedList<Behavior>.Enumerator enumerator1 = list1.GetEnumerator();
            LinkedList<Behavior>.Enumerator enumerator2 = list2.GetEnumerator();

            for (LinkedList<Behavior>.Enumerator lcsEnumerator = lcs.GetEnumerator(); lcsEnumerator.MoveNext(); )
            {
                line1Start = null;
                line2Start = null;
                
                holdLast = null;
                while (enumerator1.MoveNext() && enumerator1.Current.compareTo(lcsEnumerator.Current) != 0)
                {
                    holdCurrent = enumerator1.Current.makeCopy();
                    if (line1Start == null)
                    {
                        line1Start = holdCurrent;
                    }
                    else
                    {
                        holdLast.setNext(holdCurrent);
                    }
                    holdLast = holdCurrent;
                }
                if (line1Start != null)
                {
                    holdLast.setNext(lcsEnumerator.Current);
                }

                holdLast = null;
                while (enumerator2.MoveNext() && enumerator2.Current.compareTo(lcsEnumerator.Current) != 0)
                {
                    holdCurrent = enumerator2.Current.makeCopy();
                    if (line2Start == null)
                    {
                        line2Start = holdCurrent;
                    }
                    else
                    {
                        holdLast.setNext(holdCurrent);
                    }
                    holdLast = holdCurrent;
                }
                if (line2Start != null)
                {
                    holdLast.setNext(lcsEnumerator.Current);
                }

                if (line1Start == null && line2Start == null)
                {
                    holdCurrent = lcsEnumerator.Current;
                }
                else
                {
                    holdCurrent = new ComboBehavior();

                    holdCurrent.setNext(line1Start == null ? lcsEnumerator.Current : line1Start);
                    holdCurrent.setNext(line2Start == null ? lcsEnumerator.Current : line2Start);
                }

                if (start == null)
                {
                    start = holdCurrent;
                }
                else
                {
                    last.setNext(holdCurrent);
                }

                last = lcsEnumerator.Current;
            }

            return start;
        }

        private List<LinkedList<Behavior>> getAllPaths(Behavior start)
        {
            List<LinkedList<Behavior>> allPaths = new List<LinkedList<Behavior>>();
            LinkedList<Behavior> list = new LinkedList<Behavior>();

            for (Behavior b = start; b != null; b = b.getNext())
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
            else if (sequenceA.ElementAt(i - 1).compareTo(sequenceB.ElementAt(j - 1)) == 0)
            {
                LinkedList<Behavior> hold = getLCS(grid, sequenceA, sequenceB, i - 1, j - 1);
                hold.AddLast(sequenceA.ElementAt(i - 1).makeCopy());
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
            for (LinkedList<Behavior>.Enumerator a = sequenceA.GetEnumerator(); a.MoveNext(); i++)
            {
                j = 1;
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

        internal void finishAttempt(Riedl.Evaluation eval)
        {
            GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE] =
                GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.COMPLETED_PUZZLE) ?
                GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE] || eval.successful :
                eval.successful;
            GameplayManager.say("SIMA: Here's what I did:\n" + eval.description + "\n\nRIEDL: " + eval.explanation);
            GameplayManager.Game.Keys[GameState.GameFlag.PARALYZED] = false;
            GameplayManager.Game.Keys[GameState.GameFlag.SIMA_ACTING] = false;
            GameplayManager.Game.Keys[GameState.GameFlag.SIMA_WAITING] = false;
            GameplayManager.Game.Keys[GameState.GameFlag.SIMA_WATCHING] = false;
        }
    }
}
