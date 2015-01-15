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
        Joystick joystick;

        public JoystickManager()
        {
            joystick = new Joystick(30, 30);
            joystick.init();
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