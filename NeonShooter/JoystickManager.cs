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
    public class JoystickManager
    {
        public Joystick joystick;

        public static JoystickManager instance;

        public JoystickManager()
        {
            joystick = new Joystick(60, 60);
            joystick.init();

            instance = this;
        }

        public void Update()
        {
            joystick.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            joystick.Draw(spriteBatch);
        }
    }
}