using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CS8803AGA.devices;
using CS8803AGA.story.characters;

namespace CS8803AGA.engine
{
    public class EngineStateDialogue : AEngineState
    {

        #region Graphics

        private GameTexture m_baseImage;

        #endregion

        private string m_text;

        public EngineStateDialogue(string text)
            : base(EngineManager.Engine)
        {
            m_baseImage = new GameTexture(@"Sprites/RPG/PopupScreen");
            m_text = text;
        }

        public override void update(GameTime gameTime)
        {
            if (InputSet.getInstance().getButton(InputsEnum.BUTTON_1) ||
                InputSet.getInstance().getButton(InputsEnum.BUTTON_2) ||
                InputSet.getInstance().getButton(InputsEnum.CONFIRM_BUTTON) ||
                InputSet.getInstance().getButton(InputsEnum.CANCEL_BUTTON))
            {
                EngineManager.popState();
                InputSet.getInstance().setAllToggles();
                return;
            }
        }

        public override void draw()
        {
             EngineManager.peekBelowState(this).draw();

            DrawCommand td = DrawBuffer.getInstance().DrawCommands.pushGet();
            Point p = m_engine.GraphicsDevice.Viewport.TitleSafeArea.Center;
            Vector2 v = new Vector2(p.X, p.Y);
            td.set(m_baseImage, 0, v, CoordinateTypeEnum.ABSOLUTE, Constants.DepthDialoguePage,
                true, Color.White, 0f, 1f);

            // WorldManager.DrawMap(new Vector2(300, 100), 600, 500, Constants.DepthDialogueText);
            FontMap.getInstance().getFont(FontEnum.Kootenay14).drawString(this.m_text, new Vector2(300, 100), Color.Black);
        }

        public override string getStateType()
        {
            return "EngineStateDialogue";
        }
    }
}
