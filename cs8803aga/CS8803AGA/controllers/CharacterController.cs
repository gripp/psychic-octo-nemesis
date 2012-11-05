using Microsoft.Xna.Framework;
using System;
using CS8803AGAGameLibrary;
using CS8803AGA.collision;
using CS8803AGA.engine;
using CS8803AGA.actions;
using CS8803AGA.story;
using CS8803AGA.story.behaviors;
using System.Collections.Generic;
using CS8803AGA.story.map;
using CS8803AGA.story.characters;

namespace CS8803AGA.controllers
{
    public class CharacterController : ICollidable
    {
        public int NpcID
        {
            get
            {
                return m_npc_id;
            }
        }
        private int m_npc_id;

        public int Health { get; set; }
        public AnimationController AnimationController { get; protected set; }

        protected Vector2 m_position;
        protected Collider m_collider;
        protected int m_speed;

        protected float m_previousAngle;
        private LinkedList<Behavior>.Enumerator task;
        private Riedl.Evaluation eval;

        /// <summary>
        /// Factory method to create CharacterControllers
        /// </summary>
        /// <param name="ci">Information about character apperance and stats</param>
        /// <param name="startpos">Where in the Area the character should be placed</param>
        /// <param name="playerControlled">True if the character should be a PC, false if NPC</param>
        /// <returns>Constructed CharacterController</returns>
        public static CharacterController construct(CharacterInfo ci, Vector2 startpos, bool playerControlled, int npcID)
        {
            CharacterController cc;
            ColliderType type;
            if (playerControlled)
            {
                cc = new PlayerController();
                type = ColliderType.PC;
            }
            else
            {
                cc = new CharacterController();
                type = ColliderType.NPC;
            }

            cc.m_npc_id = npcID;
            cc.m_position = startpos;

            cc.AnimationController = new AnimationController(ci.animationDataPath, ci.animationTexturePath);
            cc.AnimationController.ActionTriggered += new ActionTriggeredEventHandler(cc.handleAction);
            cc.AnimationController.Scale = ci.scale;

            Rectangle bounds = ci.collisionBox;
            bounds.Offset((int)cc.m_position.X, (int)cc.m_position.Y);
            cc.m_collider = new Collider(cc, bounds, type, cc.m_npc_id);

            cc.m_speed = ci.speed;

            cc.m_previousAngle = (float)Math.PI / 2;

            return cc;
        }

        /// <summary>
        /// Factory method to construct non-player character controllers
        /// See other construct() method for more details
        /// </summary>
        public static CharacterController construct(CharacterInfo ci, Vector2 startpos, int npc_id)
        {
            return construct(ci, startpos, false, npc_id);
        }

        /// <summary>
        /// Protected ctor - use the construct() method
        /// </summary>
        protected CharacterController()
        {
            // protected so we have to use the factory
            // we want the factory so that later we can store subclass info in CharacterInfo
            //  and then have the factory instantiate the subclass we want
            Health = 2;
        }

        private Point m_destination = new Point(-1, -1);

        public virtual void update()
        {
            if (GameplayManager.Game.Keys.ContainsKey(GameState.GameFlag.SIMA_ACTING) &&
                GameplayManager.Game.Keys[GameState.GameFlag.SIMA_ACTING] && this.NpcID == GameplayManager.Game.getSIMA().ID)
            {
                if (m_destination.X != -1 && m_destination.Y != -1)
                {
                    if (DrawPosition.X == m_destination.X && DrawPosition.Y == m_destination.Y)
                    {
                        m_destination = new Point(-1, -1);
                    }
                    else
                    {
                        float dx = (DrawPosition.X == m_destination.X) ? 0 : (DrawPosition.X > m_destination.X) ? -1 * m_speed : m_speed;
                        float dy = (DrawPosition.Y == m_destination.Y) ? 0 : (DrawPosition.Y > m_destination.Y) ? -1 * m_speed : m_speed;
                        m_collider.handleMovement(new Vector2(dx, dy));
                    }
                }
                else if (task.MoveNext())
                {
                    if (task.Current is GoToBehavior)
                    {
                        m_destination = LabScreen.LOCATIONS[((GoToBehavior)task.Current).getLocation()];
                    }
                }
                else if (DrawPosition.X != LabScreen.LOCATIONS[LabScreen.LabLocation.SIMA].X ||
                    DrawPosition.Y != LabScreen.LOCATIONS[LabScreen.LabLocation.SIMA].Y)
                {
                    m_destination = LabScreen.LOCATIONS[LabScreen.LabLocation.SIMA];
                }
                else
                {
                    GameplayManager.Game.getSIMA().finishAttempt(eval);
                }
            }
        }

        public virtual void draw()
        {
            float depth = GameplayManager.ActiveArea.getDrawDepth(this.m_collider.Bounds);
            AnimationController.draw(m_position, depth);
        }

        /// <summary>
        /// Converts an angle in radians to a direction left,right,up,down
        /// </summary>
        /// <param name="angle">Angle in radians, where 0 = right</param>
        /// <returns>Left, right, up, or down</returns>
        protected virtual string angleTo4WayAnimation(float angle)
        {
            angle += MathHelper.PiOver4;
            angle += MathHelper.Pi;
            if (angle > MathHelper.TwoPi) angle -= MathHelper.TwoPi;
            angle *= 4.00f / MathHelper.TwoPi;
            angle -= 0.001f;
            int angleInt = (int)angle;
            switch (angleInt)
            {
                case 0: return "left";
                case 1: return "down";
                case 2: return "right";
                case 3: return "up";
                default: throw new Exception("Math is wrong");
            }
        }

        /// <summary>
        /// Converts an angle in radians to an 8-way direction
        /// </summary>
        /// <param name="angle">Angle in radians, where 0 = right</param>
        /// <returns>Left, right, up, down, upleft, upright, downleft, or downright</returns>
        protected virtual string angleTo8WayAnimation(float angle)
        {
            // complicated.. essentially takes angle and maps to 8 directions
            angle += MathHelper.PiOver4 / 2; // adjust so ranges don't wrap around -pi
            angle += MathHelper.Pi; // shift ranges to 0-TwoPi
            if (angle > MathHelper.TwoPi) angle -= MathHelper.TwoPi; // fix edge case
            angle *= 8.00f / MathHelper.TwoPi;
            angle -= 0.001f;
            int angleInt = (int)angle;
            switch (angleInt)
            {
                case 0: return "left";
                case 1: return "downleft";
                case 2: return "down";
                case 3: return "downright";
                case 4: return "right";
                case 5: return "upright";
                case 6: return "up";
                case 7: return "upleft";
                default: throw new Exception("Math is wrong");
            }
        }

        /// <summary>
        /// Handles actions passed to it from its Animation Controller
        /// </summary>
        /// <param name="sender">Object sending the action, probably AnimationController</param>
        /// <param name="action">The action itself</param>
        protected void handleAction(object sender, IAction action)
        {
            action.execute(this);
        }

        #region Collider Members

        public Collider getCollider()
        {
            return m_collider;
        }

        public Vector2 DrawPosition
        {
            get
            {
                return m_position;
            }
            set
            {
                m_position = value;
            }
        }

        #endregion

        #region IGameObject Members

        public bool isAlive()
        {
            return Health > 0;
        }

        #endregion

        internal void moveTo(Point dest)
        {
            m_destination = dest;
        }

        internal void setTask(LinkedList<Behavior> steps)
        {
            eval = Riedl.evaluateTask(steps);
            task = steps.GetEnumerator();
        }
    }
}