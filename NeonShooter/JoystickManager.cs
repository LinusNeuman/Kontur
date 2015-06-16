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
        public static Texture2D Sight { get; private set; }

        #endregion

        public static Joystick moveJoystick;
        public static Joystick aimJoystick;

        private bool anyFingerDown; // USE THIS

        public static JoystickManager instance;

        public Vector2 SightPos;
        public float SightRot;

        public static bool noDirection;

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
            Sight = content.Load<Texture2D>("Player/Sight");
        }

        public void Update()
        {
            HandleTouchInput();

            //moveJoystick.Update();
            //aimJoystick.Update();
   

            Vector2 tempDir = aimJoystick.direction;
            tempDir.Normalize();

            SightPos = PlayerShip.Instance.Position + (tempDir * 70);
            SightRot = (float)Math.Atan2(tempDir.Y, tempDir.X);
            SightRot = MathHelper.ToDegrees(SightRot);
            SightRot = MathHelper.ToRadians(SightRot);

        }

        public void HandleTouchInput()
        {
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();

                #region MoveJoy

               

                if(gesture.Position.X * GameRoot.tempScale.X < GameRoot.VirtualScreenSize.X / 2 - 50)
                {
                    moveJoystick.fingerIsDown = true;

                    moveJoystick.knob.x = (int)(gesture.Position.X * GameRoot.tempScale.X);
                    moveJoystick.knob.y = (int)(gesture.Position.Y * GameRoot.tempScale.Y);

                    moveJoystick.knob.x = MathHelper.Clamp(moveJoystick.knob.x, moveJoystick.my_x, moveJoystick.texture.Width + moveJoystick.my_x);
                    moveJoystick.knob.y = MathHelper.Clamp(moveJoystick.knob.y, 1080 - moveJoystick.texture.Height - moveJoystick.my_y, 1080 - moveJoystick.my_y);

                  //  Vector2 distVect = new Vector2(MathHelper.Clamp(moveJoystick.knob.x, -1 * ((gesture.Position.X * GameRoot.tempScale.X) - (moveJoystick.knob.x)), (gesture.Position.X * GameRoot.tempScale.X) - (moveJoystick.knob.x)), MathHelper.Clamp(moveJoystick.knob.y, -1 * ((gesture.Position.Y * GameRoot.tempScale.Y) - (moveJoystick.knob.y)), (gesture.Position.Y * GameRoot.tempScale.Y) - (moveJoystick.knob.y)));

                   // moveJoystick.knob.x = (int)distVect.X;
                   // moveJoystick.knob.y = (int)distVect.Y;
                    

                    Vector2 maxValue;
                    maxValue.X =moveJoystick.my_x - moveJoystick.texture.Width + moveJoystick.my_x;
                    maxValue.Y = (1080 - moveJoystick.texture.Height - moveJoystick.my_y) - (1080 - moveJoystick.my_y);

                    moveJoystick.direction.X =  ((moveJoystick.knob.x - moveJoystick.x) / maxValue.X);
                    moveJoystick.direction.Y = (moveJoystick.knob.y - moveJoystick.y) / maxValue.Y;

                    //if (moveJoystick.direction != Vector2.Zero)
                    //    moveJoystick.direction.Normalize();
                    moveJoystick.direction *= -PlayerShip.playerSpeed;

                    if(noDirection == true)
                    {
                        moveJoystick.direction = Vector2.Zero;
                    }
                }

                //if ((gesture.Position * GameRoot.tempScale - new Vector2(moveJoystick.x, moveJoystick.y)).Length() < Joystick.Width) // 160
                //{

                //    moveJoystick.fingerIsDown = true;

                //    moveJoystick.knob.x = (int)(gesture.Position.X * GameRoot.tempScale.X);
                //    moveJoystick.knob.y = (int)(gesture.Position.Y * GameRoot.tempScale.Y);

                //    moveJoystick.direction.X = moveJoystick.knob.x - moveJoystick.x; // To calculate we need the position _relative_ to the centre of the joystick. 
                //    moveJoystick.direction.Y = moveJoystick.knob.y - moveJoystick.y;
                //}
                //if ((gesture.Position * GameRoot.tempScale - new Vector2(moveJoystick.x, moveJoystick.y)).Length() > Joystick.Width)
                //{
                //    moveJoystick.fingerIsDown = false;
                    
                //}
                if (gesture.Position.X * GameRoot.tempScale.X > GameRoot.VirtualScreenSize.X / 2 - 50)
                {
                    moveJoystick.fingerIsDown = false;

                }

                #endregion

                #region AimJoy



                if (gesture.Position.X * GameRoot.tempScale.X > GameRoot.VirtualScreenSize.X / 2 + 50)
                {
                    aimJoystick.fingerIsDown = true;

                    aimJoystick.knob.x = (int)(gesture.Position.X * GameRoot.tempScale.X);
                    aimJoystick.knob.y = (int)(gesture.Position.Y * GameRoot.tempScale.Y);

                    aimJoystick.knob.x = MathHelper.Clamp(aimJoystick.knob.x, 1920 - moveJoystick.my_x - aimJoystick.texture.Width, 1920 - moveJoystick.my_x);
                    aimJoystick.knob.y = MathHelper.Clamp(aimJoystick.knob.y, 1080 - aimJoystick.texture.Height - aimJoystick.my_y, 1080 - aimJoystick.my_y);

                    aimJoystick.direction.X = aimJoystick.knob.x - aimJoystick.x;
                    aimJoystick.direction.Y = aimJoystick.knob.y - aimJoystick.y;

                    if (noDirection == true)
                    {
                        aimJoystick.direction = Vector2.Zero;
                    }
                }

                if (gesture.Position.X * GameRoot.tempScale.X < GameRoot.VirtualScreenSize.X / 2 - 50)
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

        public void DrawSight(SpriteBatch spriteBatch)
        {
            if (!PlayerShip.Instance.IsDead)
            {
                spriteBatch.Draw(Sight, SightPos, null, Color.White, SightRot, new Vector2((Sight.Width / 2), (Sight.Height / 2)), 1f, SpriteEffects.None, 0);
            }
        }
    }
}