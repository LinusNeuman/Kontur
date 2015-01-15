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
    public class Joystick
    {
        private int my_x;
        private int my_y;

        private JoystickKnob knob; // import and represent the joystickKnob.

        private Texture2D texture;
        private int x;
        private int y;

        public Joystick(int margin_left, int margin_bottom)
        {
            my_x = margin_left;
            my_y = margin_bottom;

            texture = Art.Joystick;
        }

        public void init()
        {
            this.x = my_x + this.texture.Width / 2;
            this.y = (int)GameRoot.ScreenSize.Y - my_y - this.texture.Height / 2;

            knob = new JoystickKnob();

            knob.x = this.x - this.texture.Width/2;
            knob.y = this.y - this.texture.Height/2;

            knob.originX = this.x - this.texture.Width / 2;
            knob.originY = this.y - this.texture.Height / 2;
        }

        public void Update()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(this.x, this.y), Color.White);
            spriteBatch.Draw(knob.texture, new Vector2(knob.x, knob.y), Color.White);
        }
    }
}