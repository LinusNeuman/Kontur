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
    public class PowerUp
    {
        public Texture2D texture;
        public Vector2 scale;
        public float lifeTime;
        public Vector2 position;
        public float radius;
        public bool isExpired;

        public bool IsActive { get { return timeUntilStart <= 0; } }

        public enum Ability
        {
            _1_ExtraLife,
            _2_ShootAroundShip,
            _3_Overheat,
            _4_Shield,
            _5_SlowMo,
            _6_Speed,
            _7_DoubleShot,
            _8_DoubleScore,
        };

        public PowerUp()
        {

        }

        public static PowerUp Create_1_OverHeat(Vector2 position)
        {
            var powerup = new PowerUp(PowerUpSpawner.Bad_Overheat, position);
            enemy.AddBehaviour(enemy.FollowPlayer());
            enemy.PointValue = 100;

            Sound.Spawn.Play(0.4f, rand.NextFloat(-0.2f, 0.2f), 0);

            return enemy;
        }

        public void WasShot()
        {
            isExpired = true;

            // powerup disappears
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // make the size of the black hole pulsate
            float scale = 1 + 0.1f * (float)Math.Sin(10 * GameRoot.GameTime.TotalGameTime.TotalSeconds);
            spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
        }
    }
}