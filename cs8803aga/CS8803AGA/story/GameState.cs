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
            DEAN_FATIGUE,
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
            PLAYER_HAD_FUNDING_OPTION,
            PLAYER_REQUESTED_FUNDING,
            PLAYER_GOT_FUNDING,

            // These are things she can do with the dean...
            PLAYER_SHOOK_DEAN_HAND, // 1
            PLAYER_TOLD_DEAN_JOKE, // 2
            PLAYER_DISCUSSED_EDUCATIONAL_THEORY, // 3
            PLAYER_EXPLAINED_THESIS_TO_DEAN, // 4
            PLAYER_REQUESTED_SCHOLARSHIP, // 5
            PLAYER_WON_SCHOLARSHIP,

            REGISTRAR_DOOR_IS_OPEN,
            REGISTRAR_SIGNED_FORM,

            RIEDL_FATIGUE,
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
            string outcome = "";

            if (Keys[GameFlag.PLAYER_COMPLETED_PUZZLE])
            {
                outcome += "You were assigned the SIMA project.\n";
            }
            else
            {
                outcome += "You were not assigned the SIMA project.\n";
            }

            if (Keys[GameFlag.PLAYER_WON_SCHOLARSHIP])
            {
                outcome += "You received a scholarship.\n";
            }
            else
            {
                outcome += "You did not receive a scholarship.\n";
            }

            if (Keys[GameFlag.PLAYER_GOT_FUNDING])
            {
                outcome += "You got funding from RIEDL.\n";
            }
            else
            {
                outcome += "You did not get funding from RIEDL.\n";
            }

            return outcome;
        }

        internal void updateState()
        {
            Keys[GameFlag.PLAYER_ACCESSED_PUZZLE] =
                (Keys.ContainsKey(GameFlag.PLAYER_ACCESSED_PUZZLE) && Keys[GameFlag.PLAYER_ACCESSED_PUZZLE])
                || getRiedl().Mind.SIMProject1;

            Keys[GameFlag.PLAYER_GOT_FUNDING] =
                (Keys.ContainsKey(GameFlag.PLAYER_GOT_FUNDING) && Keys[GameFlag.PLAYER_GOT_FUNDING])
                || getRiedl().Mind.gaveFunding;

            Keys[GameFlag.REGISTRAR_DOOR_IS_OPEN] =
                (Keys.ContainsKey(GameFlag.REGISTRAR_DOOR_IS_OPEN) && Keys[GameFlag.REGISTRAR_DOOR_IS_OPEN])
                || getRiedl().Mind.SIMProject1
                || getRiedl().Mind.SIMProject2
                || ((Keys.ContainsKey(GameFlag.RIEDL_FATIGUE) && Keys[GameFlag.RIEDL_FATIGUE]) && (Keys.ContainsKey(GameFlag.DEAN_FATIGUE) && Keys[GameFlag.DEAN_FATIGUE]));

            Keys[GameFlag.PLAYER_WON_SCHOLARSHIP] =
                (Keys.ContainsKey(GameFlag.PLAYER_WON_SCHOLARSHIP) && Keys[GameFlag.PLAYER_WON_SCHOLARSHIP])
                || getDean().Mind.scholarship;

        }
    }
}
