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

        private static Random rand = new Random();

        public ParticleState(Vector2 velocity, ParticleType type, float lengthMultiplier = 1f)
        {
            Velocity = velocity;
            Type = type;
            LengthMultiplier = lengthMultiplier;
        }

        public static ParticleState GetRandom(float minVel, float maxVel)
        {
            var state = new ParticleState();
            state.Velocity = rand.NextVector2(minVel, maxVel);
            state.Type = ParticleType.None;
            state.LengthMultiplier = 1;

            return state;
        }

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


            if(particle.State.Type != ParticleType.IgnoreGravity)
            {
                for (int i = 0; i < EntityManager.BlackHoles.Count; i++)
                {
                    var dPos = EntityManager.BlackHoles[i].Position - pos;
                    float distance = dPos.Length();
                    var n = dPos / distance;
                    vel += 10000 * n / (distance * distance + 10000);

                    // tangential acceleration for nearby particles
                    if (distance < 400)
                        vel += 45 * new Vector2(n.Y, -n.X) / (distance + 100);
                }
            }

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