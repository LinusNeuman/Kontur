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

namespace NeonShooter
{
    public class PlayerMessage
    {
        private int timeUntilStart = 5;

        public string contentMessage;
        public float percentLife;
        public bool isExpired;
        public SpriteFont contentfont;
        public Color contentColor;
        public int duration;

        public Vector2 position;

        public PlayerMessage(int Duration, string Message)
        {
            contentfont = PlayerMessageHandler.mediumFont;
            contentColor = Color.Transparent;
            timeUntilStart = 5;

            contentMessage = Message;
            percentLife = 1f;

            duration = Duration;

            position = new Vector2(1920 / 2 - contentfont.MeasureString(contentMessage).X / 2,
                   1080 - 50 - contentfont.MeasureString(contentMessage).Y / 2);
        }

        public static PlayerMessage Died_Message()
        {
            var pmessage = new PlayerMessage(60, "You died!");

            return pmessage;
        }

        public void Kill()
        {
            
            contentColor.A -= 2;
            if (contentColor.A <= 5)
                isExpired = true;
        }

        public void Update()
        {
            position.Y -= 0.5f;

            if (timeUntilStart <= 0)
            {
                

                percentLife -= 1f / duration;

                if (percentLife < 0)
                    Kill();
            }
            else
            {
                timeUntilStart--;
                contentColor = Color.White * (1 - timeUntilStart / 60f);
            }
        }
    }
}