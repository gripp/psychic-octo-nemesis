using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.engine;
using CS8803AGA.controllers;
using Microsoft.Xna.Framework;
using CS8803AGA.collision;
using CS8803AGAGameLibrary;
using CS8803AGA.PsychSim;

namespace CS8803AGA.story.characters
{
    public class Dean : Character
    {
        public enum ThingToDoToDean
        {
            SHAKE_HAND = 2,
            TELL_JOKE = 1,
            DISCUSS_THEORY = 10,
            PRESENT_THESIS = 5,
            REQUEST_SCHOLARSHIP
        };

        public DeanPOMDP Mind
        {
            get { return mnd; }
        }
        private DeanPOMDP mnd = new DeanPOMDP();

        bool f_deanFatigue;
        bool f_playerAccessedPuzzle;
        bool f_playerRequestedScholarship;
        bool f_playerWon;
        bool f_playerWonScholarship;
        bool f_registrarDoorIsOpen;

        public override string getDialogue(bool shouting)
        {
            if (f_playerWon)
            {
                return "I heard you graduated. Congratulations!";
            }
            else if (!f_registrarDoorIsOpen && !f_deanFatigue)
            {
                return getOptions();
            }
            else if (!f_registrarDoorIsOpen)
            {
                return "I am very busy. Go dicuss your issues with your professor.";
            }
            else if (f_playerAccessedPuzzle && f_playerWonScholarship)
            {
                return "How is your research going? Congratulations on winning a scholarship!";
            }
            else if (!f_playerAccessedPuzzle && f_registrarDoorIsOpen && f_playerWonScholarship)
            {
                return "How is your degree program going? Congratulations on winning a scholarship!";
            }
            else if (!f_playerAccessedPuzzle && f_playerRequestedScholarship && f_registrarDoorIsOpen && !f_playerWonScholarship)
            {
                return "How is your degree program going? I'm sorry that you weren't accepted for the scholarship.";
            }
            else if (!f_playerAccessedPuzzle && f_playerRequestedScholarship && !f_registrarDoorIsOpen && !f_playerWonScholarship)
            {
                return "I'm sorry that you weren't accepted for the scholarship.";
            }
            else if (!f_playerAccessedPuzzle && f_playerRequestedScholarship && !f_registrarDoorIsOpen)
            {
                return "How is your degree program going?";
            }
            else
            {
                return "I've nothing to say, apparently.";
            }
        }

        private string getOptions()
        {
            List<ThingToDoToDean> options = GameplayManager.Game.getDean().Mind.options();

            if (options.Count == 0)
            {
                GameplayManager.Game.Keys[GameState.GameFlag.DEAN_FATIGUE] = true;
                GameplayManager.Game.Keys[GameState.GameFlag.DEAN_WAITING] = false;
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_PARALYZED] = false;
                return "SYSTEM: You can't interact with the DEAN anymore!";
            }
            else
            {
                string optionsString = "SYSTEM: What would you like to do?\n";

                if (options.Contains(ThingToDoToDean.SHAKE_HAND)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_SHOOK_DEAN_HAND)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_SHOOK_DEAN_HAND]))
                {
                    optionsString += "   1: Shake the DEAN's hand.";
                }

                if (options.Contains(ThingToDoToDean.TELL_JOKE)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_TOLD_DEAN_JOKE)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_TOLD_DEAN_JOKE]))
                {
                    optionsString += "   2: Tell the DEAN a joke.";
                }

                if (options.Contains(ThingToDoToDean.DISCUSS_THEORY)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_DISCUSSED_EDUCATIONAL_THEORY)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_DISCUSSED_EDUCATIONAL_THEORY]))
                {
                    optionsString += "   3: Discuss educational theory with the DEAN.";
                }

                if (options.Contains(ThingToDoToDean.PRESENT_THESIS)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_EXPLAINED_THESIS_TO_DEAN)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_EXPLAINED_THESIS_TO_DEAN]))
                {
                    optionsString += "   4: Tell the DEAN about a paper you wrote.";
                }

                if (options.Contains(ThingToDoToDean.REQUEST_SCHOLARSHIP)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_REQUESTED_SCHOLARSHIP)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_REQUESTED_SCHOLARSHIP]))
                {
                    optionsString += "   5: Apply for a scholarship.";
                }

                return optionsString;
            }
        }

        public override void act(Collider mover, bool shouting)
        {
            setFlags();

            if (!f_registrarDoorIsOpen && !f_deanFatigue)
            {
                GameplayManager.Game.Keys[GameState.GameFlag.DEAN_WAITING] = true;
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_PARALYZED] = true;
            }

            GameplayManager.say(getDialogue(shouting));
        }

        private void setFlags()
        {
            f_deanFatigue = GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.DEAN_FATIGUE)
                && GameplayManager.Game.Keys[GameState.GameFlag.DEAN_FATIGUE];
            f_playerAccessedPuzzle = GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_ACCESSED_PUZZLE)
                && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_ACCESSED_PUZZLE];
            f_playerRequestedScholarship = GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_REQUESTED_SCHOLARSHIP)
                && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_REQUESTED_SCHOLARSHIP];
            f_playerWon = GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_WON)
                && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_WON];
            f_playerWonScholarship = GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_WON_SCHOLARSHIP)
                && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_WON_SCHOLARSHIP];
            f_registrarDoorIsOpen = GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.REGISTRAR_DOOR_IS_OPEN)
                && GameplayManager.Game.Keys[GameState.GameFlag.REGISTRAR_DOOR_IS_OPEN];
        }

        public override CharacterInfo getCharacterInfo()
        {
            return GlobalHelper.loadContent<CharacterInfo>(@"Characters/Dean");
        }

        internal void simulate(ThingToDoToDean thingToDoToDean)
        {
            switch (thingToDoToDean)
            {
                case ThingToDoToDean.DISCUSS_THEORY:
                    GameplayManager.say("DEAN: Your stances are so interesting!\n"
                        + "I love it when students take an interest in the direction of their education!");
                    GameplayManager.Game.getDean().Mind.addEvidence(10);
                    break;
                case ThingToDoToDean.PRESENT_THESIS:
                    GameplayManager.say("DEAN: I think I understand.\n"
                        + "Computer Science isn't really my field, but you're clearly very intelligent.");
                    GameplayManager.Game.getDean().Mind.addEvidence(5);
                    break;
                case ThingToDoToDean.REQUEST_SCHOLARSHIP:
                    GameplayManager.Game.getDean().Mind.message(CS8803AGA.PsychSim.Message.submitApplication);
                    GameplayManager.Game.getRiedl().Mind.message(Message.notify);
                    break;
                case ThingToDoToDean.SHAKE_HAND:
                    GameplayManager.say("DEAN: Nice to meet you, too.\n"
                        + "I'm a bit busy, but I do enjoy talking to students now and again.");
                    GameplayManager.Game.getDean().Mind.addEvidence(2);
                    break;
                case ThingToDoToDean.TELL_JOKE:
                    GameplayManager.say("DEAN: I don't get it.\n"
                        + "If you don't mind, I have some work to do...");
                    GameplayManager.Game.getDean().Mind.addEvidence(1);
                    break;
            }
            GameplayManager.Game.updateState();
            GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_PARALYZED] = false;
        }
    }
}