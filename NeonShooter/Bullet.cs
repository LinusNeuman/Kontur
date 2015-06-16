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
using Microsoft.Xna.Framework.Content;

namespace NeonShooter
{
    class Bullet : Entity
    {
        #region Textures

        public static Texture2D BulletSpeed { get; private set; }
        public static Texture2D BulletTank { get; private set; }
        public static Texture2D BulletStandard { get; private set; }
        public static Texture2D BulletDamage { get; private set; }

        #endregion

        public Bullet(Vector2 position, Vector2 velocity)
        {
            if (PlayerStatus.selectedShip == 0)
            {
                image = BulletSpeed;
            }
            if (PlayerStatus.selectedShip == 1)
            {
                image = BulletStandard;
            }
            if (PlayerStatus.selectedShip == 2)
            {
                image = BulletTank;
            }
            if (PlayerStatus.selectedShip == 3)
            {
                image = BulletDamage;
            }
            Position = position;
            Velocity = velocity;
            Orientation = Velocity.ToAngle();
            Radius = 8;
        }

        private static Random rand = new Random();

        public static void Load(ContentManager content)
        {
            BulletSpeed = content.Load<Texture2D>("Bullets/BulletSpeed");
            BulletTank = content.Load<Texture2D>("Bullets/BulletTank");
            BulletStandard = content.Load<Texture2D>("Bullets/BulletStandard");
            BulletDamage = content.Load<Texture2D>("Bullets/BulletDamage");
        }

        public override void Update()
        {
            
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += Velocity;

            //delete the bullets that go outside the screen
            if(Position.X + image.Width < 0)
            {
                IsExpired = true;
            }
            if(Position.X - image.Width > 1920)
            {
                IsExpired = true;
            }

            if (Position.Y + image.Height < 0)
            {
                IsExpired = true;
            }
            if (Position.Y - image.Height > 1080)
            {
                IsExpired = true;
            }
           
        }
    }
}