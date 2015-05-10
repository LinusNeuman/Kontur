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

            Vector2 direction = JoystickManager.moveJoystick.direction;
            //direction.Y *= -1;





            return direction;
        }

        public static Vector2 GetAimDirection()
        {
            Vector2 direction = JoystickManager.aimJoystick.direction;
            //direction.Y *= -1;

            // if there is no input return zero, else normalize the direction to have a length of 1
            if (direction == Vector2.Zero)
                return Vector2.Zero;
            else
                return Vector2.Normalize(direction);
        }
    }
}