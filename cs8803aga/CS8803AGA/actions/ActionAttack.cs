using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGAGameLibrary.actions;
using CS8803AGA.controllers;
using Microsoft.Xna.Framework;
using CS8803AGA.engine;

namespace CS8803AGA.actions
{
    class ActionAttack : AAction<ActionInfoAttack>
    {
        public ActionAttack(ActionInfoAttack info) : base(info)
        {
            // nch
        }

        #region Action Members

        public override void execute(object sendingCharacterController)
        {
            CharacterController cc = (CharacterController)sendingCharacterController;
            Vector2 actionPos = cc.DrawPosition +
                new Vector2(m_info.location.X * cc.AnimationController.Scale,
                            m_info.location.Y * cc.AnimationController.Scale);
            Rectangle bounds =
                new Rectangle((int)(actionPos.X),
                              (int)(actionPos.Y),
                              (int)(m_info.location.Width * cc.AnimationController.Scale), 
                              (int)(m_info.location.Height * cc.AnimationController.Scale));

            int damageAmt = 1;
            GameplayManager.ActiveArea.add(
                new DamageTrigger(bounds, cc, damageAmt));
        }

        #endregion
    }
}
