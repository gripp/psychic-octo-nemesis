using CS8803AGAGameLibrary;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CS8803AGAGameLibrary.actions;
using CS8803AGA.actions;
using System.Collections.Generic;

namespace CS8803AGA.controllers
{
    public delegate void ActionTriggeredEventHandler(object sender, IAction action);

    /// <summary>
    /// Class for playing back Animations stored in an AnimationSet.
    /// </summary>
    public class AnimationController
    {
        public enum AnimationCommand
        {
            Play, Idle
        };

        #region Properties

        /// <summary>
        /// Amount to scale the animation
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Currently playing animation
        /// </summary>
        public Animation CurrentAnimation { get { return m_animSet.anims[m_currentAnimIdx]; } }

        /// <summary>
        /// Current sprite in the currently playing animation
        /// </summary>
        public Sprite CurrentSprite { get { return CurrentAnimation.sprites[m_currentSpriteIdx]; } }

        #endregion

        #region Fields

        /// <summary>
        /// Animation set used by this controller
        /// </summary>
        private AnimationSet m_animSet;

        /// <summary>
        /// Idx of currently running animation
        /// </summary>
        private int m_currentAnimIdx = 0;

        /// <summary>
        /// Idx of currently drawn sprite in the current animation
        /// </summary>
        private int m_currentSpriteIdx = 0;

        /// <summary>
        /// Number of frames the current sprite has been playing
        /// </summary>
        private int m_frameCounter = 0;

        /// <summary>
        /// Whether the animation is idle
        /// </summary>
        private bool m_isIdle = false;

        /// <summary>
        /// Sprite sheet containing the animations
        /// </summary>
        private GameTexture m_texture;

        /// <summary>
        /// Maps m_currentAnimIdx,m_currentSpriteIdx to the appropriate idx
        /// for the ImageDimensions in the GameTexture
        /// </summary>
        private int[][] m_sourceRectangleIdxs;

        /// <summary>
        /// Animation command which a caller has requested this frame.
        /// </summary>
        protected AnimationCommand m_requestedCommand;

        #endregion

        #region Constructors and Public Methods

        /// <summary>
        /// Loads an AnimationSet into the AnimationController.
        /// </summary>
        /// <param name="animSetPath">Asset path to the AnimationSet.</param>
        /// <param name="texturePath">Asset path to the image file to use.</param>
        public AnimationController(string animSetPath, string texturePath)
        {
            m_animSet =
                GlobalHelper.loadContent<AnimationSet>(animSetPath);

            // Build up a mapping of animation & sprite indices to image dimension idxs
            List<Rectangle> imageDimensionsList = new List<Rectangle>();
            int counter = 0;
            m_sourceRectangleIdxs = new int[m_animSet.anims.Length][];
            for (int i = 0; i < m_animSet.anims.Length; ++i)
            {
                m_sourceRectangleIdxs[i] = new int[m_animSet.anims[i].sprites.Length];
                for (int j = 0; j < m_animSet.anims[i].sprites.Length; ++j)
                {
                    imageDimensionsList.Add(m_animSet.anims[i].sprites[j].box);
                    m_sourceRectangleIdxs[i][j] = counter;
                    counter++;
                }
            }

            m_texture = new GameTexture(texturePath, imageDimensionsList.ToArray());
        }

        /// <summary>
        /// Callers use this to suggest an animation to play. Priority checks
        /// are made to determine whether to accept the suggestion or keep
        /// playing the old animation.
        /// </summary>
        /// <param name="animName">Name of animation to play.</param>
        /// <param name="comm">What to do with that animation.</param>
        public void requestAnimation(string animName, AnimationCommand command)
        {
            int requestedAnimIdx = findAnimationIdx(animName);
            Animation requestedAnim = m_animSet.anims[requestedAnimIdx];

            if (requestedAnim == CurrentAnimation)
            {
                m_requestedCommand = command;
                return;
            }

            if (CurrentAnimation.priority == 0 ||
                requestedAnim.priority > CurrentAnimation.priority ||
                m_isIdle)
            {
                m_currentAnimIdx = requestedAnimIdx;
                m_currentSpriteIdx = 0;
                m_requestedCommand = command;
                return;
            }
        }

        /// <summary>
        /// Update the actual animation (should be after all requests).
        /// </summary>
        public void update()
        {
            switch (m_requestedCommand)
            {
                case AnimationCommand.Play:
                    play();
                    break;
                case AnimationCommand.Idle:
                    idle();
                    break;
                default:
                    throw new Exception("Shouldn't reach here");
            }

            m_requestedCommand = AnimationCommand.Idle;
        }

        /// <summary>
        /// Draw the animation.
        /// </summary>
        /// <param name="pos">Center point where animation should be drawn.</param>
        /// <param name="depth">Render depth of the animation.</param>
        public void draw(Vector2 pos, float depth)
        {
            pos += new Vector2(CurrentSprite.offset.X * Scale,
                               CurrentSprite.offset.Y * Scale);

            int imageDimensionsIdx = m_sourceRectangleIdxs[m_currentAnimIdx][m_currentSpriteIdx];

            DrawCommand td = DrawBuffer.getInstance().DrawCommands.pushGet();
            td.set(m_texture, imageDimensionsIdx, pos, CoordinateTypeEnum.ABSOLUTE, depth, true, Color.White, 0, Scale);
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Find an animation from the set via its name.
        /// </summary>
        /// <param name="animName">Name of animation to find.</param>
        /// <returns>Index in array of requested animation.</returns>
        private int findAnimationIdx(string animName)
        {
            int i = 0;
            while (i < m_animSet.anims.Length)
            {
                if (animName == m_animSet.anims[i].name)
                {
                    return i;
                }
                ++i;
            }
            throw new Exception(String.Format("Animation '{0}' not found", animName));
        }

        /// <summary>
        /// Move current animation forward in time.
        /// </summary>
        private void play()
        {
            m_frameCounter++;
            if (m_frameCounter >= CurrentAnimation.sprites[m_currentSpriteIdx].duration)
            {
                m_currentSpriteIdx++;
                m_frameCounter = 0;
            }
            m_isIdle = false;

            // handle end of animation
            if (m_currentSpriteIdx >= CurrentAnimation.sprites.Length)
            {
                if (CurrentAnimation.loops) // loop
                {
                    m_currentSpriteIdx = 0;
                }
                else // handle end of animation
                {
                    if (!String.IsNullOrEmpty(CurrentAnimation.whenDone) && CurrentAnimation.whenDone != "idle")
                    {
                        throw new NotImplementedException("Animations can only go idle currently");
                    }

                    m_currentSpriteIdx = CurrentAnimation.idleFrame;
                    m_isIdle = true;
                }
            }

            // handle actions at start of display of sprite
            if (m_frameCounter == 0 && CurrentAnimation.sprites[m_currentSpriteIdx].action != null)
            {
                foreach (AActionInfo info in CurrentAnimation.sprites[m_currentSpriteIdx].action)
                {
                    IAction action = ActionFactory.construct(info);
                    ActionTriggered(this, action);
                }
            }
        }

        /// <summary>
        /// Idle the current animation unless it has priority > 0, meaning
        /// that it must continue playing.
        /// </summary>
        private void idle()
        {
            if (CurrentAnimation.priority != 0 && !m_isIdle)
            {
                play();
                return;
            }
            m_currentSpriteIdx = CurrentAnimation.idleFrame;
            m_frameCounter = 0;
            m_isIdle = true;
        }

#endregion

        #region Events

        /// <summary>
        /// Fires whenever an IAction is activated.
        /// </summary>
        public event ActionTriggeredEventHandler ActionTriggered;

        #endregion

    }
}