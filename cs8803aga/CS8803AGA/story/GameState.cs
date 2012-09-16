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
        private Map map = new Map();

        public GameState()
        {
        }
    }
}
