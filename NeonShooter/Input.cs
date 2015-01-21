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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace NeonShooter
{
    static class Input
    {

        public static void Update()
        {
            
        }

        public static Vector2 GetMovementDirection()
        {

            Vector2 direction = JoystickManager.instance.joystick.direction;
            // for movement left joystick (joystick)
            //direction.Y *= -1;




            if (direction.LengthSquared() > 1)
                direction.Normalize();

            return direction;
        }

        public static Vector2 GetAimDirection()
        {
            // for aim right joystick

            return Vector2.Zero;
        }
    }
}