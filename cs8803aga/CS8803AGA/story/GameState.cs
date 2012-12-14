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
            DEAN_WAITING,

            PLAYER_ACCESSED_PUZZLE,
            PLAYER_COMPLETED_PUZZLE,
            PLAYER_PARALYZED,
            PLAYER_WON,

            // These are things that the player can do
            // to influence Riedl's opinion of her. They
            // are ordered from least to most influential.
            PLAYER_SHOOK_RIEDL_HAND, // 1
            PLAYER_TOLD_RIEDL_JOKE, // 5
            PLAYER_DISCUSSED_RIEDL_THEORY, // 6
            PLAYER_ACED_TEST, // 3
            PLAYER_TURNED_IN_RIEDL_PROJECT, // 4
            PLAYER_WROTE_RIEDL_THESIS, // 2
            PLAYER_REQUESTED_FUNDING,

            // These are things she can do with the dean...
            PLAYER_SHOOK_DEAN_HAND, // 1
            PLAYER_TOLD_DEAN_JOKE, // 2
            PLAYER_DISCUSSED_EDUCATIONAL_THEORY, // 3
            PLAYER_EXPLAINED_THESIS_TO_DEAN, // 4
            PLAYER_REQUESTED_SCHOLARSHIP, // 5
            PLAYER_WON_SCHOLARSHIP,

            REGISTRAR_DOOR_IS_OPEN,
            REGISTRAR_SIGNED_FORM,

            RIEDL_WAITING,
            RIEDL_HAS_EXPLAINED,

            SIMA_ACTING,
            SIMA_WAITING,
            SIMA_WATCHING,

            SYSTEM_HANDLED_COLLISION
        };
        public List<Character> Characters
        {
            get
            {
                return characters;
            }
        }
        private List<Character> characters = new List<Character>();

        public Map Map
        {
            get
            {
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
            getSIMA().show(action);
        }

        public SIMA getSIMA()
        {
            foreach (Character sima in Characters)
            {
                if (sima is SIMA)
                {
                    return ((SIMA)sima);
                }
            }
            return null;
        }

        public Riedl getRiedl()
        {
            foreach (Character riedl in Characters)
            {
                if (riedl is Riedl)
                {
                    return ((Riedl)riedl);
                }
            }
            return null;
        }

        public GameState()
        {
            map = new Map();
        }

        internal void resetTask()
        {
            getSIMA().resetTask();
        }

        //internal void addEvidence(int strength)
        //{
        //    getRiedl().Mind.addEvidance(strength);
        //    getDean().Mind.addEvidance(strength);
        //}

        public Dean getDean()
        {
            foreach (Character dean in Characters)
            {
                if (dean is Dean)
                {
                    return ((Dean)dean);
                }
            }
            return null;
        }

        internal string getOutcome()
        {
            throw new NotImplementedException();
        }
    }
}
