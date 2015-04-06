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
using Microsoft.Xna.Framework.Content;

namespace NeonShooter
{
    public class JoystickManager
    {
        #region Textures

        public static Texture2D Joystick { get; private set; }
        public static Texture2D Knob { get; private set; }

        #endregion

        public static Joystick moveJoystick;
        public static Joystick aimJoystick;

        private bool anyFingerDown; // USE THIS

        public static JoystickManager instance;

        public JoystickManager()
        {
            instance = this;
        }

        public static void Initialize()
        {
            moveJoystick = new Joystick(60, 60);
            moveJoystick.init();

            aimJoystick = new Joystick((int)(GameRoot.VirtualScreenSize.X - Joystick.Width - 60), 60);
            aimJoystick.init();
        }

        public void Reset()
        {
            moveJoystick.Reset();
            aimJoystick.Reset();
        }

        public static void Load(ContentManager content)
        {
            Joystick = content.Load<Texture2D>("Joystick/Joystick");
            Knob = content.Load<Texture2D>("Joystick/Knob");
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

                if ((gesture.Position * GameRoot.tempScale - new Vector2(moveJoystick.x, moveJoystick.y)).Length() < Joystick.Width / 2) // 160
                {

                    moveJoystick.fingerIsDown = true;

                    moveJoystick.knob.x = (int)(gesture.Position.X * GameRoot.tempScale.X);
                    moveJoystick.knob.y = (int)(gesture.Position.Y * GameRoot.tempScale.Y);

                    moveJoystick.direction.X = moveJoystick.knob.x - moveJoystick.x; // To calculate we need the position _relative_ to the centre of the joystick. 
                    moveJoystick.direction.Y = moveJoystick.knob.y - moveJoystick.y;
                }
                if ((gesture.Position * GameRoot.tempScale - new Vector2(moveJoystick.x, moveJoystick.y)).Length() > Joystick.Width / 2)
                {
                    moveJoystick.fingerIsDown = false;
                }

                #endregion

                #region AimJoy

                if ((gesture.Position * GameRoot.tempScale - new Vector2(aimJoystick.x, aimJoystick.y)).Length() < Joystick.Width / 2) // 160
                {

                    aimJoystick.fingerIsDown = true;

                    aimJoystick.knob.x = (int)(gesture.Position.X * GameRoot.tempScale.X);
                    aimJoystick.knob.y = (int)(gesture.Position.Y * GameRoot.tempScale.Y);

                    aimJoystick.direction.X = aimJoystick.knob.x - aimJoystick.x; // To calculate we need the position _relative_ to the centre of the joystick. 
                    aimJoystick.direction.Y = aimJoystick.knob.y - aimJoystick.y;
                }
                if ((gesture.Position * GameRoot.tempScale - new Vector2(aimJoystick.x, aimJoystick.y)).Length() > Joystick.Width / 2) // 160
                {
                    aimJoystick.fingerIsDown = false;
                }

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