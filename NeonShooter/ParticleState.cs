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
using Microsoft.Xna.Framework;

namespace NeonShooter
{
    public enum ParticleType { None, Enemy, Bullet, IgnoreGravity }

    public struct ParticleState
    {
        public Vector2 Velocity;
        public ParticleType Type;
        public float LengthMultiplier;

        public static void UpdateParticle(ParticleManager<ParticleState>.Particle particle)
        {
            var vel = particle.State.Velocity;

            var pos = particle.Position;
            int width = (int)GameRoot.ScreenSize.X;
            int height = (int)GameRoot.ScreenSize.Y;

            // collision for bullets with edges
            if (pos.X < 0) vel.X = Math.Abs(vel.X);
            else if (pos.X > width)
                vel.X = -Math.Abs(vel.X);
            if (pos.Y < 0) vel.Y = Math.Abs(vel.Y);
            else if (pos.Y > height)
                vel.Y = -Math.Abs(vel.Y);

            //collision for player with edges


            particle.Position += vel;
            particle.Orientation = vel.ToAngle();

            float speed = vel.Length();
            float alpha = Math.Min(1, Math.Min(particle.PercentLife * 2, speed * 1f));
            alpha *= alpha;

            particle.Color.A = (byte)(255 * alpha);

            particle.Scale.X = particle.State.LengthMultiplier * Math.Min(Math.Min(1f, 0.2f * speed + 0.1f), alpha);


            // denormalied floats cause significant performance issues
            if (Math.Abs(vel.X) + Math.Abs(vel.Y) < 0.00000000001f)
                vel = Vector2.Zero;

            vel *= 0.97f;
            particle.State.Velocity = vel;
        }
    }
}