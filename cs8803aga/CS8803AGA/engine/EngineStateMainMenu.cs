using Microsoft.Xna.Framework;
using System.Collections.Generic;
using CS8803AGA.devices;
using CS8803AGA.ui;

namespace CS8803AGA.engine
{
    public class EngineStateMainMenu : AEngineState
    {
        private const string c_StartGame = "Start Game";

        private MenuList m_menuList;

        public EngineStateMainMenu(Engine engine) : base(engine)
        {
            List<string> menuOptions = new List<string>();
            menuOptions.Add(c_StartGame);
            menuOptions.Add("Dummy 1");
            menuOptions.Add("Dummy 2");
            menuOptions.Add("Dummy 3");
            menuOptions.Add("Dummy 4");
            menuOptions.Add("Dummy 5");
            menuOptions.Add("Dummy 6");
            menuOptions.Add("Dummy 7");

            Point temp = m_engine.GraphicsDevice.Viewport.TitleSafeArea.Center;
            m_menuList = new MenuList(menuOptions, new Vector2(temp.X, temp.Y - 200));
            m_menuList.Font = FontEnum.Kootenay48;
            m_menuList.ItemSpacing = 100;
            m_menuList.SpaceAvailable = 400;
        }

        public override void update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (InputSet.getInstance().getButton(InputsEnum.CONFIRM_BUTTON))
            {
                switch (m_menuList.SelectedString)
                {
                    case c_StartGame:
                        EngineManager.replaceCurrentState(new EngineStateGameplay(m_engine));
                        return;
                    default:
                        break;
                }
            }

            if (InputSet.getInstance().getLeftDirectionalY() < 0)
            {
                m_menuList.selectNextItem();
                InputSet.getInstance().setStick(InputsEnum.LEFT_DIRECTIONAL, 5);
            }

            if (InputSet.getInstance().getLeftDirectionalY() > 0)
            {
                m_menuList.selectPreviousItem();
                InputSet.getInstance().setStick(InputsEnum.LEFT_DIRECTIONAL, 5);
            }
        }

        public override void draw()
        {
            m_menuList.draw();
        }
    }
}
