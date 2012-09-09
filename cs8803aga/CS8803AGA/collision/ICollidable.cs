using CS8803AGA.controllers;
using Microsoft.Xna.Framework;

namespace CS8803AGA.collision
{
    /// <summary>
    /// Interface for all objects which should register themselves with the
    /// CollisionDetector.
    /// </summary>
    public interface ICollidable : IGameObject
    {
        /// <summary>
        /// Get the object's collider.
        /// </summary>
        /// <returns></returns>
        Collider getCollider();

        /// <summary>
        /// Where the object is drawing to the screen (usu. center).
        /// </summary>
        Vector2 DrawPosition
        {
            get;
            set;
        }
    }
}
