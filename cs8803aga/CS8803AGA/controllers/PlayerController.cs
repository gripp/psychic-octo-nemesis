using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CSharpQuadTree;
using CS8803AGA.engine;
using CS8803AGA.devices;
using CS8803AGA.collision;

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

            if (InputSet.getInstance().getButton(InputsEnum.CONFIRM_BUTTON))
            {
                handleInteract();
            }

            if (InputSet.getInstance().getButton(InputsEnum.BUTTON_1))
            {
                string dir = angleTo4WayAnimation(m_previousAngle);
                dir = "attack" + dir;
                AnimationController.requestAnimation(dir, AnimationController.AnimationCommand.Play);
            }

            float dx = InputSet.getInstance().getLeftDirectionalX() * m_speed;
            float dy = InputSet.getInstance().getLeftDirectionalY() * m_speed;

            if (dx == 0 && dy == 0)
            {
                return;
            }

            float angle =
                CommonFunctions.getAngle(new Vector2(dx, dy));

            string animName = angleTo4WayAnimation(angle);
            AnimationController.requestAnimation(animName, AnimationController.AnimationCommand.Play);

            if (true /* TODO - checks for paralysis, etc here */)
            {
                m_collider.handleMovement(new Vector2(dx, -dy));
            }

            m_previousAngle = angle;
        }

        /// <summary>
        /// Sample of interaction - initiates dialogue.
        /// Perty hackish.
        /// </summary>
        private void handleInteract()
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
            foreach (Collider c in queries)
            {
                if (c != this.m_collider && c.m_type == ColliderType.NPC)
                {
                    EngineManager.pushState(new EngineStateDialogue());
                    InputSet.getInstance().setAllToggles();
                    return;
                }
            }
        }
    }
}
