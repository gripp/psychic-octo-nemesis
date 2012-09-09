using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CSharpQuadTree;
using CS8803AGA.controllers;

namespace CS8803AGA.collision
{
    /// <summary>
    /// Handles determining the effects of various collisions and whether
    /// movement is affected.
    /// </summary>
    public class CollisionHandler
    {
        /// <summary>
        /// Determines how to handle various types of collisions.
        /// </summary>
        /// <param name="mover">Collider that is moving</param>
        /// <param name="other">Collider which was moved into</param>
        /// <param name="deltaPosition">Attempted movement by the mover</param>
        /// <param name="allowedMovement">Out param - amount of movement which is allowed</param>
        /// <returns>True if any movement is allowed, false otherwise</returns>
        public static bool handleMovement(Collider mover, Collider other, Vector2 deltaPosition, out Vector2 allowedMovement)
        {
            switch (mover.m_type)
            {
                case ColliderType.PC:
                    return handlePCMovement(mover, other, deltaPosition, out allowedMovement);
                    break;
                //case ColliderType.NPC:
                //    break;
                //case ColliderType.Effect:
                //    break;
                //case ColliderType.Movable:
                //    break;
                default:
                    throw new Exception("Something's moving that shouldn't be");
            }
            return false;
        }

        /// <summary>
        /// Scales back an attempted movement so that two colliders don't intersect.
        /// </summary>
        /// <param name="mover">Collider that is moving</param>
        /// <param name="other">Collider that the movement shouldn't be able to move into</param>
        /// <param name="deltaPosition">Amount that mover is trying to move</param>
        /// <returns>How much mover can move so as not to intersect other</returns>
        protected static Vector2 scaleBackVelocity(Collider mover, Collider other, Vector2 deltaPosition)
        {
            // whew! a lot work just to make it so that you can slide along objects when
            //  using multiple directions

            const float EPS = 0.01f;
            
            DoubleRect dr2 = other.Bounds;

            double reducedX = deltaPosition.X;
            double reducedY = deltaPosition.Y;

            // Project x only movement and fix x velocity
            Vector2 temp = new Vector2(deltaPosition.X, 0);
            DoubleRect dr1 = mover.Bounds + temp;
            if (dr1.IntersectsWith(dr2))
            if (deltaPosition.X > 0 && dr1.X + dr1.Width >= dr2.X) // moving right, rhs collision
            {
                double overlap = dr1.X + dr1.Width - dr2.X + EPS;
                reducedX = Math.Max(0.0f, deltaPosition.X - overlap);
            }
            else if (deltaPosition.X < 0 && dr1.X <= dr2.X + dr2.Width) // moving left, lhs collision
            {
                double overlap = (dr2.X + dr2.Width) - dr1.X + EPS;
                reducedX = Math.Min(0.0f, deltaPosition.X + overlap);
            }

            // Project y only movement and fix y velocity
            temp = new Vector2(0, deltaPosition.Y);
            dr1 = mover.Bounds + temp;
            if (dr1.IntersectsWith(dr2))
            if (deltaPosition.Y > 0 && dr1.Y + dr1.Height >= dr2.Y) // moving down, bottom collision
            {
                double overlap = dr1.Y + dr1.Height - dr2.Y + EPS;
                reducedY = Math.Max(0.0f, deltaPosition.Y - overlap);
            }
            else if (deltaPosition.Y < 0 && dr1.Y <= dr2.Y + dr2.Height) // moving up, top collision
            {
                double overlap = (dr2.Y + dr2.Height) - dr1.Y + EPS;
                reducedY = Math.Min(0.0f, deltaPosition.Y + overlap);
            }

            // if neither X nor Y reduced, must be a corner collision
            // for now, we just won't allow this (if we allow, you can get stuck, but not bad)
            if (reducedX == deltaPosition.X && reducedY == deltaPosition.Y)
            {
                return Vector2.Zero;
            }

            return new Vector2((float)reducedX, (float)reducedY);
            
        }

        /// <summary>
        /// Determines how to handle collisions involving the PC
        /// </summary>
        /// <returns>True if the PC can move, false otherwise</returns>
        protected static bool handlePCMovement(Collider mover, Collider other, Vector2 deltaPosition, out Vector2 allowedMovement)
        {
            switch (other.m_type)
            {
                case ColliderType.Scenery:
                    allowedMovement = scaleBackVelocity(mover, other, deltaPosition);
                    return true;
                case ColliderType.NPC:
                    allowedMovement = scaleBackVelocity(mover, other, deltaPosition);
                    return true;
                //case ColliderType.Effect:
                //    break;
                //case ColliderType.Movable:
                //    break;
                case ColliderType.Trigger:
                    allowedMovement = deltaPosition;
                    ((ITrigger)other.m_owner).handleImpact(mover);
                    return true;
                default:
                    throw new Exception("PC moved into something it shouldn't have");
            }
        }
    }
}
