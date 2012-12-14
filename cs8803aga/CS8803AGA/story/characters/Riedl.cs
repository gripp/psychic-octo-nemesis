using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.engine;
using CS8803AGA.collision;
using CS8803AGAGameLibrary;
using CS8803AGA.story.behaviors;
using CS8803AGA.story.map;
using CS8803AGA.PsychSim;

namespace CS8803AGA.story.characters
{
    public class Riedl : Character
    {
        public static LabScreen.LabLocation TASTE = acquireTaste();

        public enum ThingToDoToRiedl
        {
            SHAKE_HAND = 1,
            TELL_JOKE = 2,
            DISCUSS_THEORY = 3,
            ACE_TEST = 4,
            DO_PROJECT = 9,
            PRESENT_THESIS = 10,
            REQUEST_FUNDING
        };

        public Model Mind
        {
            get { return mnd; }
        }
        private Model mnd = new Model();

        bool f_explained;
        bool f_playerAccessedPuzzle;
        bool f_playerCompletedPuzzle;
        bool f_playerHasSignature;
        bool f_playerWon;
        bool f_registrarDoorIsOpen;

        public static LabScreen.LabLocation acquireTaste()
        {
            switch (RandomManager.get().Next(4))
            {
                case 0:
                    return LabScreen.LabLocation.CHICKEN;
                case 1:
                    return LabScreen.LabLocation.LOBSTER;
                case 2:
                    return LabScreen.LabLocation.PIZZA;
                default:
                    return LabScreen.LabLocation.STEAK;
            }
        }

        public struct Evaluation
        {
            public string description;
            public string explanation;
            public bool successful;
        }

        public static Evaluation evaluateTask(LinkedList<Behavior> attempt)
        {
            bool rightFood = false;
            bool hasFood = false;
            bool hasHotFood = false;
            bool deliveredFood = false;
            bool hasCake = false;
            bool delieveredCake = false;
            string description = "";

            Behavior last = null;
            for (LinkedList<Behavior>.Enumerator e = attempt.GetEnumerator(); e.MoveNext(); )
            {
                description += e.Current.getDescription() + "\n";
                if (e.Current is InteractBehavior && last != null)
                {
                    if (last is GoToBehavior)
                    {
                        GoToBehavior gtb = (GoToBehavior)last;
                        if (gtb.getLocation() == LabScreen.LabLocation.CHICKEN ||
                            gtb.getLocation() == LabScreen.LabLocation.LOBSTER ||
                            gtb.getLocation() == LabScreen.LabLocation.PIZZA ||
                            gtb.getLocation() == LabScreen.LabLocation.STEAK)
                        {
                            hasFood = true;
                            rightFood = (gtb.getLocation() == TASTE);
                        }
                        else if (gtb.getLocation() == LabScreen.LabLocation.CAKE)
                        {
                            hasCake = true;
                        }
                        else if (gtb.getLocation() == LabScreen.LabLocation.MICROWAVE && hasFood)
                        {
                            hasHotFood = true;
                        }
                        else if (gtb.getLocation() == LabScreen.LabLocation.RIEDL)
                        {
                            deliveredFood = deliveredFood || hasFood;
                            delieveredCake = delieveredCake || hasCake;
                        }
                    }
                }

                last = e.Current;
            }

            Evaluation response = new Evaluation();
            response.description = description;
            response.successful = (hasHotFood && rightFood && deliveredFood && delieveredCake);

            if (response.successful)
            {
                response.explanation = "Well done! SIMA's got it!";
            }
            else if (!deliveredFood)
            {
                response.explanation = "Where is my food?";
            }
            else if (!hasHotFood)
            {
                response.explanation = "This food is cold.";
            }
            else if (!rightFood)
            {
                response.explanation = "That's not the food I wanted.";
            }
            else if (!delieveredCake)
            {
                response.explanation = "Where is my cake?";
            }

            return response;
        }

        public override string getDialogue(bool shouting)
        {
            if (f_playerWon)
            {
                return "DR. RIEDL: Now that you have graduated, I guess you should find a job.\n"
                    + "I can't help you with that.\nGood luck.";
            }
            else if (f_playerHasSignature && f_playerCompletedPuzzle)
            {
                return "DR. RIEDL: Excellent work!\n"
                    + "With your research complete and approval from the REGISTRAR,\n"
                    + "I feel comfortable signing off on your graduation.\nYour parents will be so proud.\n\n"
                    + GameplayManager.Game.getOutcome();
            }
            else if (f_playerHasSignature && !f_playerCompletedPuzzle && f_playerAccessedPuzzle)
            {
                return "DR. RIEDL: Good.\nYou got your GRADUATION APPLICATION signed.\n"
                    + "Now finalize your research with SIMA so you graduate.\n"
                    + "I trust you to do good work. That's why I assigned you this project.";
            }
            else if (f_playerHasSignature && !f_playerAccessedPuzzle)
            {
                return "DR. RIEDL: With the work you've done and approval from the REGISTRAR,\n"
                    + "I feel comfortable signing off on your graduation.\nYour parents will be so proud.\n\n"
                    + GameplayManager.Game.getOutcome();
            }
            else if (f_registrarDoorIsOpen && !f_playerHasSignature && f_playerCompletedPuzzle)
            {
                return "DR. RIEDL: Congratulations!\nYour research is complete.\n"
                    + "Go get your GRADUATION APPLICATION signed by the REGISTRAR\nso that you can graduate.";
            }
            else if (!f_playerHasSignature && !f_playerCompletedPuzzle && f_playerAccessedPuzzle)
            {
                return "DR. RIEDL: Go finalize your research with SIMA so you graduate.\n"
                    + "I trust you to do good work. That's why I assigned you this project.\n"
                    + "And dont' forget to get your GRADUATION APPLICATION signed.";
            }
            else if (!f_explained)
            {
                return "DR. RIEDL: Welcome to the lab, young Computer Scientist.\n"
                    + "You are here to complete your Master's degree under my tutilege.\nI am Dr. Riedl.\n\n"
                    + getOptions();
            }
            else if (!f_registrarDoorIsOpen && f_explained)
            {
                return getOptions();
            }
            else if (f_registrarDoorIsOpen && !f_playerAccessedPuzzle)
            {
                return "DR. RIEDL: The work you have done seems like it should be sufficient.\n"
                    + "Go get your GRADUATION APPLICATION signed by the REGISTRAR\nso that you can graduate.";
            }
            else
            {
                return "DR. RIEDL: This is a curious situation. I've nothing to say.";
            }
        }

        private string getOptions()
        {
            List<ThingToDoToRiedl> options = new List<ThingToDoToRiedl>();

            if (options.Count == 0)
            {
                return "";
            }
            else
            {
                string optionsString = "SYSTEM: What would you like to do?\n";

                if (options.Contains(ThingToDoToRiedl.SHAKE_HAND)) { optionsString += "   1: Shake RIEDL's hand."; }
                if (options.Contains(ThingToDoToRiedl.PRESENT_THESIS)) { optionsString += "   2: Tell RIEDL about a paper you wrote."; }
                if (options.Contains(ThingToDoToRiedl.ACE_TEST)) { optionsString += "   3: Tell RIEDL about a test you aced."; }
                if (options.Contains(ThingToDoToRiedl.DO_PROJECT)) { optionsString += "   4: Tell RIEDL about a project you did."; }
                if (options.Contains(ThingToDoToRiedl.TELL_JOKE)) { optionsString += "   5: Tell RIEDL a joke."; }
                if (options.Contains(ThingToDoToRiedl.DISCUSS_THEORY)) { optionsString += "   6: Discuss computational theory with RIEDL."; }
                if (options.Contains(ThingToDoToRiedl.REQUEST_FUNDING)) { optionsString += "   7: Request funding."; }

                return optionsString;
            }
        }

        public override void act(Collider mover, bool shouting)
        {
            setFlags();
        }

        private void setFlags()
        {
            f_explained = GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.RIEDL_HAS_EXPLAINED)
                && GameplayManager.Game.Keys[GameState.GameFlag.RIEDL_HAS_EXPLAINED];
            f_playerAccessedPuzzle = GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_ACCESSED_PUZZLE)
                && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_ACCESSED_PUZZLE];
            f_playerCompletedPuzzle = GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_COMPLETED_PUZZLE)
                && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_COMPLETED_PUZZLE];
            f_playerHasSignature = GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.REGISTRAR_SIGNED_FORM)
                && GameplayManager.Game.Keys[GameState.GameFlag.REGISTRAR_SIGNED_FORM];
            f_playerWon = GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_WON)
                && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_WON];
            f_registrarDoorIsOpen = GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.REGISTRAR_DOOR_IS_OPEN)
                && GameplayManager.Game.Keys[GameState.GameFlag.REGISTRAR_DOOR_IS_OPEN];
        }

        public override CharacterInfo getCharacterInfo()
        {
            return GlobalHelper.loadContent<CharacterInfo>(@"Characters/Riedl");
        }

        internal void simulate(ThingToDoToRiedl thingToDoToRiedl)
        {
            switch (thingToDoToRiedl)
            {
                case ThingToDoToRiedl.ACE_TEST:
                    GameplayManager.say("RIEDL: Wow. You got an A on that test?\n"
                        + "A lot of students were complaining about how difficult it was.");
                    GameplayManager.Game.getRiedl().Mind.addEvidence(4);
                    break;
                case ThingToDoToRiedl.DISCUSS_THEORY:
                    GameplayManager.say("RIEDL: I'm happy that you take the time to actually read papers.");
                    GameplayManager.Game.getRiedl().Mind.addEvidence(3);
                    break;
                case ThingToDoToRiedl.DO_PROJECT:
                    GameplayManager.say("RIEDL: This project looked like it took a whole lot of work.\n"
                        + "You did this in your spare time? Very impressive!");
                    GameplayManager.Game.getRiedl().Mind.addEvidence(9);
                    break;
                case ThingToDoToRiedl.PRESENT_THESIS:
                    GameplayManager.say("RIEDL: Your conclusions in this paper are fascinating.\n"
                        + "This may be publishable work.");
                    GameplayManager.Game.getRiedl().Mind.addEvidence(10);
                    break;
                case ThingToDoToRiedl.REQUEST_FUNDING:
                    GameplayManager.Game.getRiedl().Mind.message(CS8803AGA.PsychSim.Message.askFunding, GameplayManager.Game.getDean().Mind);
                    break;
                case ThingToDoToRiedl.SHAKE_HAND:
                    GameplayManager.say("RIEDL: Thank you for introducing yourself.");
                    GameplayManager.Game.getRiedl().Mind.addEvidence(1);
                    break;
                case ThingToDoToRiedl.TELL_JOKE:
                    GameplayManager.say("RIEDL: Ha! That's a good one. I do enjoy a joke now and again.");
                    GameplayManager.Game.getRiedl().Mind.addEvidence(2);
                    break;
            }
            GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_PARALYZED] = false;
        }
    }
}
