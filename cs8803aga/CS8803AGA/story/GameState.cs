using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CS8803AGA.story.characters;
using CS8803AGA.story.map;
using CS8803AGA.story.behaviors;

namespace CS8803AGA.story
{
    public class GameState
    {
        public enum GameFlag
        {
            ACCESSED_PUZZLE,
            //ACCESSED_PUZZLE_2,
            COLLISION_HANDLED,
            COMPLETED_PUZZLE,
            //COMPLETED_PUZZLE_2,
            //COMPLETED_PUZZLE_3,
            //PLAYER_HAS_GRADUATION_FORM,
            PARALYZED,
            PLAYER_WON,
            REGISTRAR_DOOR_IS_OPEN, 
            REGISTRAR_SIGNED_FORM,
            //RIEDL_HAS_EXPLAINED
            SIMA_ACTING,
            SIMA_WAITING,
            SIMA_WATCHING
        };
        public List<Character> Characters
        {
            get
            { 
                return characters;
            }
        }
        private List<Character> characters = new List<Character>();

        public Map Map {
            get {
                return map;
            }
        }
        private Map map;

        public Dictionary<GameFlag, bool> Keys
        {
            get
            {
                return keys;
            }
        }
        private Dictionary<GameFlag, bool> keys = new Dictionary<GameFlag, bool>();

        public void showSIMA(Behavior action)
        {
            foreach (Character sima in Characters)
            {
                if (sima is SIMA)
                {
                    ((SIMA)sima).show(action);
                }
            }
        }

        public GameState()
        {
            map = new Map();
        }

        internal void resetTask()
        {
            foreach (Character sima in Characters)
            {
                if (sima is SIMA)
                {
                    ((SIMA)sima).resetTask();
                }
            }
        }
    }
}
