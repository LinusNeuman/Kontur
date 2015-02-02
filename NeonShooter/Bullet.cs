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

        public override void Update()
        {
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += Velocity;

            //delete the bullets that go outside the screen
            if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }
    }
}