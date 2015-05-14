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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace NeonShooter
{
    static class Art
    {








        public static Texture2D TitleScreenBg { get; private set; }



        public static SpriteFont Font { get; private set; }

        public static Texture2D Glow { get; private set; }
        public static Texture2D LineParticle { get; private set; }
        public static Texture2D LineParticle2 { get; private set; }
        public static Texture2D ScoreBar { get; private set; }


        public static void Load(ContentManager content)
        {
            TitleScreenBg = content.Load<Texture2D>("Graphics/gridBackground");
            Font = content.Load<SpriteFont>("Fonts/Font");
            LineParticle = content.Load<Texture2D>("Particles/Laser");
            LineParticle2 = content.Load<Texture2D>("Particles/LaserBig");
            Glow = content.Load<Texture2D>("Particles/Glow");
            ScoreBar = content.Load<Texture2D>("Graphics/ScoreBar");
        }
    }
}