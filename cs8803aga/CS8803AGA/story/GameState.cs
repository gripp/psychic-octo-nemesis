using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CS8803AGA.story.characters;
using CS8803AGA.story.map;

namespace CS8803AGA.story
{
    public class GameState
    {
        public enum GameFlag
        {
            ACCESSED_PUZZLE_1,
            ACCESSED_PUZZLE_2,
            COMPLETED_PUZZLE_1,
            COMPLETED_PUZZLE_2,
            COMPLETED_PUZZLE_3,
            PLAYER_HAS_GRADUATION_FORM,
            PLAYER_WON,
            REGISTRAR_DOOR_IS_OPEN, 
            REGISTRAR_SIGNED_FORM,
            RIEDL_HAS_EXPLAINED
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

        public GameState()
        {
            map = new Map();
        }
    }
}
