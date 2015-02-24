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

namespace NeonShooter
{
    static class Art
    {
        public static Texture2D PlayerDmgShip { get; private set; }
        public static Texture2D PlayerSpdShip { get; private set; }
        public static Texture2D PlayerStndShip { get; private set; }
        public static Texture2D PlayerTnkShip { get; private set; }

        public static Texture2D Seeker { get; private set; } // change names
        public static Texture2D Wanderer_Part1 { get; private set; }
        public static Texture2D Wanderer_Part2 { get; private set; }

        public static Texture2D BlackHole { get; private set; }

        public static Texture2D BulletLvl1 { get; private set; }
        public static Texture2D BulletLvl2 { get; private set; }
        public static Texture2D BulletLvl3 { get; private set; }

        public static Texture2D TitleScreenBg { get; private set; }

        public static Texture2D Joystick { get; private set; }
        public static Texture2D Knob { get; private set; }

        public static SpriteFont Font { get; private set; }

        public static Texture2D LineParticle { get; private set; }
        public static Texture2D Glow { get; private set; }

        public static void Load(ContentManager content)
        {
            PlayerDmgShip = content.Load<Texture2D>("Player/DamageShip");
            PlayerSpdShip = content.Load<Texture2D>("Player/SpeedShip");
            PlayerStndShip = content.Load<Texture2D>("Player/StandardShip");
            PlayerTnkShip = content.Load<Texture2D>("Player/TankShip");

            Seeker = content.Load<Texture2D>("Enemies/Seeker");
            Wanderer_Part1 = content.Load<Texture2D>("Enemies/WandererLvl1");
            Wanderer_Part2 = content.Load<Texture2D>("Enemies/WandererLvl1Small");

            BlackHole = content.Load<Texture2D>("Enemies/BlackHole");

            BulletLvl1 = content.Load<Texture2D>("Bullets/BulletLvl1");
            BulletLvl2 = content.Load<Texture2D>("Bullets/BulletLvl2");
            BulletLvl3 = content.Load<Texture2D>("Bullets/BulletLvl3");

            TitleScreenBg = content.Load<Texture2D>("Graphics/TitleScreenBg");
            
            Joystick = content.Load<Texture2D>("Joystick/Joystick");
            Knob = content.Load<Texture2D>("Joystick/Knob");
            
            Font = content.Load<SpriteFont>("Fonts/Font");

            LineParticle = content.Load<Texture2D>("Particles/Laser");
            Glow = content.Load<Texture2D>("Particles/Glow");
        }
    }
}