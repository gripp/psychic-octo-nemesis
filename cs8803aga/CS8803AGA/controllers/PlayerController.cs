using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CSharpQuadTree;
using CS8803AGA.engine;
using CS8803AGA.devices;
using CS8803AGA.collision;
using CS8803AGA.story.characters;
using CS8803AGA.story;
using CS8803AGA.story.behaviors;
using CS8803AGA.story.map;

namespace CS8803AGA.controllers
{
    /// <summary>
    /// CharacterController for a player-controlled Character.
    /// Reads from inputs to update movement and animations.
    /// </summary>
    public class PlayerController : CharacterController
    {
        public PlayerController()
        {
            // nch, should only be called by CharacterController.construct
        }



        public override void update()
        {
            AnimationController.update();

            List<Collider> collisions = getCollisions();
            if (collisions.Count == 0)
            {
                GameplayManager.Game.Keys[GameState.GameFlag.SYSTEM_HANDLED_COLLISION] = false;
            }
            else if ((GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.SIMA_WATCHING) &&
                GameplayManager.Game.Keys[GameState.GameFlag.SIMA_WATCHING]) &&
                !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.SYSTEM_HANDLED_COLLISION) &&
                GameplayManager.Game.Keys[GameState.GameFlag.SYSTEM_HANDLED_COLLISION]))
            {
                foreach (Collider c in collisions)
                {
                    if (c != this.m_collider && c.m_type == ColliderType.NPC)
                    {
                        foreach (Character npc in GameplayManager.Game.Characters)
                        {
                            if (npc.ID == c.NPC_ID)
                            {
                                if (npc is Riedl)
                                {
                                    GameplayManager.Game.showSIMA(new GoToBehavior(LabScreen.LabLocation.RIEDL));
                                }
                                else if (npc is Microwave)
                                {
                                    GameplayManager.Game.showSIMA(new GoToBehavior(LabScreen.LabLocation.MICROWAVE));
                                }
                                else if (npc is Food)
                                {
                                    Food f = (Food)npc;
                                    switch (f.Type)
                                    {
                                        case Food.FoodType.CAKE:
                                            GameplayManager.Game.showSIMA(new GoToBehavior(LabScreen.LabLocation.CAKE));
                                            break;
                                        case Food.FoodType.CHICKEN:
                                            GameplayManager.Game.showSIMA(new GoToBehavior(LabScreen.LabLocation.CHICKEN));
                                            break;
                                        case Food.FoodType.LOBSTER:
                                            GameplayManager.Game.showSIMA(new GoToBehavior(LabScreen.LabLocation.LOBSTER));
                                            break;
                                        case Food.FoodType.PIZZA:
                                            GameplayManager.Game.showSIMA(new GoToBehavior(LabScreen.LabLocation.PIZZA));
                                            break;
                                        case Food.FoodType.STEAK:
                                            GameplayManager.Game.showSIMA(new GoToBehavior(LabScreen.LabLocation.STEAK));
                                            break;
                                    }
                                }
                            }
                        }
                        GameplayManager.Game.Keys[GameState.GameFlag.SYSTEM_HANDLED_COLLISION] = true;
                    }
                }
            }

            if (!(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_PARALYZED) &&
                GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_PARALYZED]))
            {
                if (InputSet.getInstance().getButton(InputsEnum.BUTTON_1))
                {
                    handleInteract();
                    if (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.SIMA_WATCHING) &&
                        GameplayManager.Game.Keys[GameState.GameFlag.SIMA_WATCHING])
                    {
                        GameplayManager.Game.showSIMA(new InteractBehavior());
                    }

                    // Commenting out these lines should prevent attacking.
                    // string dir = angleTo4WayAnimation(m_previousAngle);
                    // dir = "attack" + dir;
                    // AnimationController.requestAnimation(dir, AnimationController.AnimationCommand.Play);
                }

                float dx = InputSet.getInstance().getLeftDirectionalX() * m_speed;
                float dy = InputSet.getInstance().getLeftDirectionalY() * m_speed;

                if (dx == 0 && dy == 0)
                {
                    return;
                }

                float angle =
                    CommonFunctions.getAngle(new Vector2(dx, dy));

                // string animName = angleTo4WayAnimation(angle);
                string animName = (dx <= 0) ? "left" : "right";
                AnimationController.requestAnimation(animName, AnimationController.AnimationCommand.Play);

                if (true /* TODO - checks for paralysis, etc here */)
                {
                    m_collider.handleMovement(new Vector2(dx, -dy));
                }

                m_previousAngle = angle;
            }
            else if (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.SIMA_WAITING) &&
                GameplayManager.Game.Keys[GameState.GameFlag.SIMA_WAITING])
            {
                if (InputSet.getInstance().getButton(InputsEnum.LEFT_BUMPER))
                {
                    // Choose watch.
                    GameplayManager.Game.resetTask();
                    GameplayManager.Game.Keys[GameState.GameFlag.SIMA_WAITING] = false;
                    GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_PARALYZED] = false;
                    GameplayManager.Game.Keys[GameState.GameFlag.SIMA_WATCHING] = true;
                }
                else if (InputSet.getInstance().getButton(InputsEnum.RIGHT_BUMPER))
                {
                    // Choose try.
                    GameplayManager.invokeSIMA();
                    GameplayManager.Game.Keys[GameState.GameFlag.SIMA_WAITING] = false;
                    GameplayManager.Game.Keys[GameState.GameFlag.SIMA_ACTING] = true;
                }
            }
            else if (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.DEAN_WAITING) &&
                GameplayManager.Game.Keys[GameState.GameFlag.DEAN_WAITING])
            {
                if (InputSet.getInstance().getButton(InputsEnum.BUTTON_2)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_SHOOK_DEAN_HAND)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_SHOOK_DEAN_HAND]))
                {
                    // PLAYER_SHOOK_HAND
                    GameplayManager.Game.getDean().Mind.addEvidence(2);
                }
                else if (InputSet.getInstance().getButton(InputsEnum.BUTTON_3)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_TOLD_DEAN_JOKE)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_TOLD_DEAN_JOKE]))
                {
                    // Tell a joke...
                    GameplayManager.Game.getDean().Mind.addEvidence(1);
                }
                else if (InputSet.getInstance().getButton(InputsEnum.BUTTON_4)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_DISCUSSED_EDUCATIONAL_THEORY)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_DISCUSSED_EDUCATIONAL_THEORY]))
                {
                    // Discuss educational theory...
                    GameplayManager.Game.getDean().Mind.addEvidence(10);
                }
                else if (InputSet.getInstance().getButton(InputsEnum.BUTTON_5)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_EXPLAINED_THESIS)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_EXPLAINED_THESIS]))
                {
                    // Talk about your thesis...
                    GameplayManager.Game.getDean().Mind.addEvidence(5);
                }
                else if (InputSet.getInstance().getButton(InputsEnum.BUTTON_6)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_REQUESTED_SCHOLARSHIP)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_REQUESTED_SCHOLARSHIP]))
                {
                    // Request a scholarship...
                    GameplayManager.Game.getDean().Mind.message(CS8803AGA.PsychSim.Message.submitApplication, GameplayManager.Game.getRiedl().Mind);
                }
            }
            else if (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.RIEDL_WAITING) && GameplayManager.Game.Keys[GameState.GameFlag.RIEDL_WAITING])
            {
                if (InputSet.getInstance().getButton(InputsEnum.BUTTON_2)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_SHOOK_RIEDL_HAND)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_SHOOK_RIEDL_HAND]))
                {
                    // PLAYER_SHOOK_HAND
                    GameplayManager.Game.getRiedl().Mind.addEvidence(1);
                }
                else if (InputSet.getInstance().getButton(InputsEnum.BUTTON_3)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_WROTE_RIEDL_THESIS)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_WROTE_RIEDL_THESIS]))
                {
                    // PLAYER_WROTE_THESIS
                    GameplayManager.Game.getRiedl().Mind.addEvidence(10);
                }
                else if (InputSet.getInstance().getButton(InputsEnum.BUTTON_4)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_ACED_TEST)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_ACED_TEST]))
                {
                    // PLAYER_ACED_TEST
                    GameplayManager.Game.getRiedl().Mind.addEvidence(4);
                }
                else if (InputSet.getInstance().getButton(InputsEnum.BUTTON_5)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_TURNED_IN_RIEDL_PROJECT)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_TURNED_IN_RIEDL_PROJECT]))
                {
                    // PLAYER_TURNED_IN_PROJECT
                    GameplayManager.Game.getRiedl().Mind.addEvidence(9);
                }
                else if (InputSet.getInstance().getButton(InputsEnum.BUTTON_6) &&
                    !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_TOLD_RIEDL_JOKE)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_TOLD_RIEDL_JOKE]))
                {
                    // PLAYER_TOLD_JOKE
                    GameplayManager.Game.getRiedl().Mind.addEvidence(2);
                }
                else if (InputSet.getInstance().getButton(InputsEnum.BUTTON_7)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_DISCUSSED_RIEDL_THEORY)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_DISCUSSED_RIEDL_THEORY]))
                {
                    // PLAYER_DISCUSSED_THEORY
                    GameplayManager.Game.getRiedl().Mind.addEvidence(3);
                }
                else if (InputSet.getInstance().getButton(InputsEnum.BUTTON_8)
                    && !(GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.PLAYER_REQUESTED_FUNDING)
                    && GameplayManager.Game.Keys[GameState.GameFlag.PLAYER_REQUESTED_FUNDING]))
                {
                    // Request funding...
                    GameplayManager.Game.getRiedl().Mind.message(CS8803AGA.PsychSim.Message.askFunding, GameplayManager.Game.getDean().Mind);
                }
            }
        }

        /// <summary>
        /// Sample of interaction - initiates dialogue.
        /// Perty hackish.
        /// </summary>
        private void handleInteract()
        {
            List<Collider> queries = getCollisions();
            foreach (Collider c in queries)
            {
                if (c != this.m_collider && c.m_type == ColliderType.NPC)
                {
                    foreach (Character npc in GameplayManager.Game.Characters)
                    {
                        if (npc.ID == c.NPC_ID)
                        {
                            npc.act(this.m_collider, false);
                        }
                    }

                    InputSet.getInstance().setAllToggles();
                    return;
                }
            }
        }

        private List<Collider> getCollisions()
        {
            string facing = angleTo4WayAnimation(m_previousAngle);
            DoubleRect queryRectangle;
            if (facing == "up")
            {
                queryRectangle = new DoubleRect(
                    m_collider.Bounds.X, m_collider.Bounds.Y - 50, m_collider.Bounds.Width, 50);
            }
            else if (facing == "down")
            {
                queryRectangle = new DoubleRect(
                    m_collider.Bounds.X, m_collider.Bounds.Y + m_collider.Bounds.Height, m_collider.Bounds.Width, 50);
            }
            else if (facing == "left")
            {
                queryRectangle = new DoubleRect(
                    m_collider.Bounds.X - 50, m_collider.Bounds.Y + m_collider.Bounds.Height / 2 - 25, 50, 50);
            }
            else if (facing == "right")
            {
                queryRectangle = new DoubleRect(
                    m_collider.Bounds.X + m_collider.Bounds.Width, m_collider.Bounds.Y + m_collider.Bounds.Height / 2 - 25, 50, 50);
            }
            else
            {
                throw new Exception("Something aint right");
            }

            List<Collider> queries = m_collider.queryDetector(queryRectangle);
            return queries;
        }
    }
}
