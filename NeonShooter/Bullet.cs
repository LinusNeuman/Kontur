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

        public static Texture2D BulletLvl1 { get; private set; }
        public static Texture2D BulletLvl2 { get; private set; }
        public static Texture2D BulletLvl3 { get; private set; }

        #endregion

        public Bullet(Vector2 position, Vector2 velocity)
        {
            if (PlayerStatus.selectedShip == 0)
            {
                image = BulletLvl1;
            }
            if (PlayerStatus.selectedShip == 1)
            {
                image = BulletLvl2;
            }
            if (PlayerStatus.selectedShip == 2)
            {
                image = BulletLvl1;
            }
            if (PlayerStatus.selectedShip == 3)
            {
                image = BulletLvl3;
            }
            Position = position;
            Velocity = velocity;
            Orientation = Velocity.ToAngle();
            Radius = 8;
        }

        private static Random rand = new Random();

        public static void Load(ContentManager content)
        {
            BulletLvl1 = content.Load<Texture2D>("Bullets/BulletLvl1");
            BulletLvl2 = content.Load<Texture2D>("Bullets/BulletLvl2");
            BulletLvl3 = content.Load<Texture2D>("Bullets/BulletLvl3");
        }

        public override void Update()
        {
            
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += Velocity;

            //delete the bullets that go outside the screen

            if(Position.X > GameRoot.VirtualScreenSize.X || Position.X < 0 || Position.Y > GameRoot.VirtualScreenSize.Y || Position.Y < 0)
            {
                IsExpired = true;

                for (int i = 0; i < 15; i++)
                {
                    GameRoot.ParticleManager.CreateParticle(Art.LineParticle, Position, Color.LightBlue, 50, new Vector2(1, 1),
                        new ParticleState() { Velocity = rand.NextVector2(0, 9), Type = ParticleType.Bullet, LengthMultiplier = 1 });
                }
            }
        }
    }
}