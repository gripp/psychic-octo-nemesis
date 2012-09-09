using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CS8803AGAGameLibrary;
using System.Collections.Generic;
using CSharpQuadTree;
using System;
using CS8803AGA.collision;
using CS8803AGA.devices;
using CS8803AGA.controllers;

namespace CS8803AGA.engine
{
    /// <summary>
    /// Engine state for the main gameplay processes of the game.
    /// </summary>
    public class EngineStateGameplay : AEngineState
    {
        /// <summary>
        /// Creates an EngineStateGameplay and registers it with the GameplayManager.
        /// Also creates and initializes a PlayerController and starting Area.
        /// </summary>
        /// <param name="engine">Engine instance for the game.</param>
        public EngineStateGameplay(Engine engine) : base(engine)
        {
            if (GameplayManager.GameplayState != null)
                throw new Exception("Only one EngineStateGameplay allowed at once!");
             

            CharacterInfo ci = GlobalHelper.loadContent<CharacterInfo>(@"Characters/Jason");

            PlayerController player =
                (PlayerController)CharacterController.construct(ci, new Vector2(600, 400), true);

            Point startPoint = new Point(0, 0);
            Area.makeTestArea(startPoint);
            GameplayManager.initialize(this, player, WorldManager.GetArea(startPoint));
        }

        /// <summary>
        /// Main game loop, checks for UI-related inputs and tells game objects to update.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public override void update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (InputSet.getInstance().getButton(InputsEnum.BUTTON_3))
            {
                EngineManager.pushState(new EngineStateMap());
                return;
            }

            Area area = GameplayManager.ActiveArea;
            area.GameObjects.ForEach(i => i.update());
            area.GameObjects.ForEach(i => { if (!i.isAlive() && i is ICollidable) ((ICollidable)i).getCollider().unregister(); });
            area.GameObjects.RemoveAll(i => !i.isAlive());

            if (InputSet.getInstance().getButton(InputsEnum.LEFT_TRIGGER))
            {
                InputSet.getInstance().setToggle(InputsEnum.LEFT_TRIGGER);

                Vector2 rclickspot = new Vector2(InputSet.getInstance().getRightDirectionalX(), InputSet.getInstance().getRightDirectionalY());
                DecorationSet ds = DecorationSet.construct("World/town");
                Decoration d = ds.makeDecoration("house1", rclickspot);

                GameplayManager.ActiveArea.add(d);
            }
        }

        public override void draw()
        {
            GameplayManager.ActiveArea.draw();
            GameplayManager.drawHUD();

            drawCollisionDetector(false);
        }

        /// <summary>
        /// Draws all of the ActiveArea's colliders.
        /// </summary>
        /// <param name="drawQuadTree">True if QuadTree partitions should also be drawn.</param>
        protected void drawCollisionDetector(bool drawQuadTree)
        {
            Area area = GameplayManager.ActiveArea;

            List<QuadTree<Collider>.QuadNode> nodes = area.CollisionDetector.getAllNodes();
            foreach (QuadTree<Collider>.QuadNode node in nodes)
            {
                if (drawQuadTree)
                {
                    DoubleRect dr = node.Bounds;
                    LineDrawer.drawLine(new Vector2((float)dr.X, (float)dr.Y),
                                        new Vector2((float)dr.X + (float)dr.Width, (float)dr.Y),
                                        Color.AliceBlue);
                    LineDrawer.drawLine(new Vector2((float)dr.X, (float)dr.Y),
                                        new Vector2((float)dr.X, (float)dr.Y + (float)dr.Height),
                                        Color.AliceBlue);
                    LineDrawer.drawLine(new Vector2((float)dr.X + (float)dr.Width, (float)dr.Y),
                                        new Vector2((float)dr.X + (float)dr.Width, (float)dr.Y + (float)dr.Height),
                                        Color.AliceBlue);
                    LineDrawer.drawLine(new Vector2((float)dr.X, (float)dr.Y + (float)dr.Height),
                                        new Vector2((float)dr.X + (float)dr.Width, (float)dr.Y + (float)dr.Height),
                                        Color.AliceBlue);
                }
                
                foreach (Collider collider in node.quadObjects)
                {
                    DoubleRect dr2 = collider.Bounds;
                    LineDrawer.drawLine(new Vector2((float)dr2.X, (float)dr2.Y),
                                    new Vector2((float)dr2.X + (float)dr2.Width, (float)dr2.Y),
                                    Color.LimeGreen);
                    LineDrawer.drawLine(new Vector2((float)dr2.X, (float)dr2.Y),
                                        new Vector2((float)dr2.X, (float)dr2.Y + (float)dr2.Height),
                                        Color.LimeGreen);
                    LineDrawer.drawLine(new Vector2((float)dr2.X + (float)dr2.Width, (float)dr2.Y),
                                        new Vector2((float)dr2.X + (float)dr2.Width, (float)dr2.Y + (float)dr2.Height),
                                        Color.LimeGreen);
                    LineDrawer.drawLine(new Vector2((float)dr2.X, (float)dr2.Y + (float)dr2.Height),
                                        new Vector2((float)dr2.X + (float)dr2.Width, (float)dr2.Y + (float)dr2.Height),
                                        Color.LimeGreen);
                }
            }
        }

        public void cleanup()
        {
            GameplayManager.GameplayState = null;
            WorldManager.reset();
        }
    }
}