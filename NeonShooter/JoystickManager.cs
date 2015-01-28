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
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;

namespace NeonShooter
{
    public class JoystickManager
    {
        public Joystick moveJoystick;
        public Joystick aimJoystick;

        public static JoystickManager instance;

        public JoystickManager()
        {
            moveJoystick = new Joystick(60, 60);
            moveJoystick.init();

            aimJoystick = new Joystick((int)(GameRoot.ScreenSize.X - Art.Joystick.Width - 60), 60);
            aimJoystick.init();

            instance = this;
        }

        public void Update()
        {
            HandleTouchInput();

            moveJoystick.Update();
            aimJoystick.Update();
        }

        public void HandleTouchInput()
        {

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();

                #region MoveJoy

                if((gesture.Position - new Vector2(moveJoystick.x, moveJoystick.y)).Length() < 170) // 160
                {
                

                //if (gesture.Position.X - moveJoystick.x >= -160 && gesture.Position.X - (int)moveJoystick.texture.Width <= 160
                //    && gesture.Position.Y - moveJoystick.y >= -160 && gesture.Position.Y - (int)moveJoystick.texture.Height >= 160)
                //{
                    moveJoystick.fingerIsDown = true;

                    moveJoystick.knob.x = (int)gesture.Position.X;
                    moveJoystick.knob.y = (int)gesture.Position.Y;

                    moveJoystick.direction.X = moveJoystick.knob.x - moveJoystick.x; // To calculate we need the position _relative_ to the centre of the joystick. 
                    moveJoystick.direction.Y = moveJoystick.knob.y - moveJoystick.y;
                }
                else
                    moveJoystick.fingerIsDown = false;

                //double final = Convert.ToDouble((gesture.Position.X - this.x) + (gesture.Position.Y - this.y));
                //double distance = Math.Sqrt(final);
                //if (distance <= 16)
                //{
                //}
                #endregion

                #region AimJoy
                //if (gesture.Position.X - aimJoystick.x + GameRoot.ScreenSize.X - Art.Joystick.Width >= -160 && gesture.Position.X - GameRoot.ScreenSize.X - (int)aimJoystick.texture.Width <= 160
                //    && gesture.Position.Y - aimJoystick.y >= -160 && gesture.Position.Y - (int)aimJoystick.texture.Height >= 160)
                if ((gesture.Position - new Vector2(aimJoystick.x, aimJoystick.y)).Length() < 170)
                {
                    aimJoystick.fingerIsDown = true;

                    aimJoystick.knob.x = (int)gesture.Position.X;
                    aimJoystick.knob.y = (int)gesture.Position.Y;

                    aimJoystick.direction.X = aimJoystick.knob.x - aimJoystick.x;// + GameRoot.ScreenSize.X - Art.Joystick.Width; // To calculate we need the position _relative_ to the centre of the joystick. 
                    aimJoystick.direction.Y = aimJoystick.knob.y - aimJoystick.y;
                }
                else
                    aimJoystick.fingerIsDown = false;

                #endregion
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            moveJoystick.Draw(spriteBatch);
            aimJoystick.Draw(spriteBatch);
        }
    }
}