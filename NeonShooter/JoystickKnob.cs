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
    public class JoystickKnob
    {
        private int _originX;
        private int _originY;

        public Texture2D texture;
        public int x;
        public int y;

        public JoystickKnob()
        {
            // code for constructor
            texture = JoystickManager.Knob;
        }

        public int originX
        {
            get { return _originX; }
            set { _originX = value; }
        }
        public int originY
        {
            get { return _originY; }
            set { _originY = value; }
        }
    }
}