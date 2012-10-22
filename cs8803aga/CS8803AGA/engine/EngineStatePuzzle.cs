using Microsoft.Xna.Framework;
using PicturePuzzle;
using CS8803AGA.story;
using System.Threading;

namespace CS8803AGA.engine
{
    public class EngineStatePuzzle : AEngineState
    {
        int PUZZLE_NUMBER;
        PuzzleForm pf = null;
        public EngineStatePuzzle(int puzzleNum)
            : base(EngineManager.Engine)
        {
            PUZZLE_NUMBER = puzzleNum;
            pf = new PuzzleForm(puzzleNum);
            pf.ShowDialog();
        }

        public override void update(GameTime gameTime)
        {
            if (pf != null && !pf.Visible)
            {
                switch (PUZZLE_NUMBER)
                {
                    case 3:
                        // GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_3] = pf.Solved;
                        break;
                    case 2:
                        // GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_2] = pf.Solved;
                        break;
                    default:
                        // GameplayManager.Game.Keys[GameState.GameFlag.COMPLETED_PUZZLE_1] = pf.Solved;
                        break;
                }
                EngineManager.popState();
            }
        }

        public override void draw()
        {
        }

        public override string getStateType()
        {
            return "EngineStatePuzzle";
        }
    }
}