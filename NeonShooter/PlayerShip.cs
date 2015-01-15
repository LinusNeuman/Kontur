using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework.Graphics;

namespace NeonShooter
{
    class PlayerShip : Entity
    {
        private static PlayerShip instance;
        public static PlayerShip Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerShip();

                return instance;
            }
        }

        JoystickManager joystickMgr;

        private PlayerShip()
        {
            image = Art.Player;
            Position = GameRoot.ScreenSize / 2;
            Radius = 10;

            joystickMgr = new JoystickManager();
        }

        public override void Update()
        {
            // ship logic

            joystickMgr.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {


            joystickMgr.Draw(spriteBatch);
        }
    }
}