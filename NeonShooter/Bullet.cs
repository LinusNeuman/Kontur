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

namespace NeonShooter
{
    class Bullet : Entity
    {
        public Bullet(Vector2 position, Vector2 velocity)
        {
            image = Art.BulletLvl1;
            Position = position;
            Velocity = velocity;
            Orientation = Velocity.ToAngle();
            Radius = 8;
        }

        private static Random rand = new Random();

        public override void Update()
        {
            
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += Velocity;
            GameRoot.Grid.ApplyExplosiveForce(0.5f * Velocity.Length(), Position, 80);

            //delete the bullets that go outside the screen
            if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
            {
                IsExpired = true;

                for (int i = 0; i < 30; i++)
                {
                    GameRoot.ParticleManager.CreateParticle(Art.LineParticle, Position, Color.LightBlue, 50, new Vector2(1,1), 
                        new ParticleState() { Velocity = rand.NextVector2(0, 9), Type = ParticleType.Bullet, LengthMultiplier = 1 });
                }
            }
        }
    }
}