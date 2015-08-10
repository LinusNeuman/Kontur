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
        public static Random rand = new Random();

        private int timeUntilStart = 60;

        public Texture2D texture;
        public Texture2D ringTexture;
        public Vector2 scale;
        public float percentLife;
        public float lifeTime;
        public Vector2 position;
        public float radius;
        public bool isExpired;
        public Color color;
        public int duration;

        public bool IsActive { get { return timeUntilStart <= 0; } }

        public int ID;


        //public enum Ability
        //{
        //    _1_ExtraLife,
        //    _2_ShootAroundShip,
        //    _3_Overheat,
        //    _4_Shield,
        //    _5_SlowMo,
        //    _6_Speed,
        //    _7_DoubleShot,
        //    _8_DoubleScore,
        //    _9_2xEnemy,
        //    _10_EnemySpd,
        //};

         public PowerUp(Texture2D image, Vector2 Position, bool Bad, int id, int Duration)
        {
            this.texture = image;
            position = Position;
            
            color = Color.Transparent;
            timeUntilStart = 60;
            if (Bad == true)
            {
                ringTexture = PowerUpSpawner.Ring_Bad;
            }
            else
            {
                ringTexture = PowerUpSpawner.Ring_Good;
            }
            radius = ringTexture.Width / 2f;

            ID = id;

            lifeTime = 300;
            percentLife = 1f;

            duration = Duration;
        }

        public static PowerUp Create_1_OverHeat(Vector2 position) // overheat
        {
            var powerup = new PowerUp(PowerUpSpawner.Bad_Overheat, position, true, 0, 600);

            Sound.Spawn.Play(0.4f, rand.NextFloat(-0.2f, 0.2f), 0);

            return powerup;
        }

        public static PowerUp Create_2_2Bullets(Vector2 position) // overheat
        {
            var powerup = new PowerUp(PowerUpSpawner.Good_2Bullets, position, false, 1, 600);
            Sound.Spawn.Play(0.4f, rand.NextFloat(-0.2f, 0.2f), 0);

            return powerup;
        }

        public static PowerUp Create_3_2xMultiplier(Vector2 position) // overheat
        {
            var powerup = new PowerUp(PowerUpSpawner.Good_2xMultiplier, position, false, 2, 600);
            Sound.Spawn.Play(0.4f, rand.NextFloat(-0.2f, 0.2f), 0);

            return powerup;
        }

        public static PowerUp Create_4_CircleShoot(Vector2 position) // overheat
        {
            var powerup = new PowerUp(PowerUpSpawner.Good_CircleShoot, position, false, 3, 600);
            Sound.Spawn.Play(0.4f, rand.NextFloat(-0.2f, 0.2f), 0);

            return powerup;
        }

        public static PowerUp Create_5_Shield(Vector2 position) // overheat
        {
            var powerup = new PowerUp(PowerUpSpawner.Good_Shield, position, false, 4, 600);
            Sound.Spawn.Play(0.4f, rand.NextFloat(-0.2f, 0.2f), 0);

            return powerup;
        }

        public static PowerUp Create_6_Slowmotion(Vector2 position) // overheat
        {
            var powerup = new PowerUp(PowerUpSpawner.Good_Slowmotion, position, false, 5, 600);
            Sound.Spawn.Play(0.4f, rand.NextFloat(-0.2f, 0.2f), 0);

            return powerup;
        }

        public static PowerUp Create_7_Speed(Vector2 position) // overheat
        {
            var powerup = new PowerUp(PowerUpSpawner.Good_Speed, position, false, 6, 600);
            Sound.Spawn.Play(0.4f, rand.NextFloat(-0.2f, 0.2f), 0);

            return powerup;
        }

        public static PowerUp Create_8_ExtraLife(Vector2 position) // overheat
        {
            var powerup = new PowerUp(PowerUpSpawner.Good_ExtraLife, position, false, 7, 600);
            Sound.Spawn.Play(0.4f, rand.NextFloat(-0.2f, 0.2f), 0);

            return powerup;
        }

        public static PowerUp Create_9_2xEnemy(Vector2 position) // overheat
        {
            var powerup = new PowerUp(PowerUpSpawner.Bad_2xEnemy, position, true, 8, 600);
            Sound.Spawn.Play(0.4f, rand.NextFloat(-0.2f, 0.2f), 0);

            return powerup;
        }

        public static PowerUp Create_10_EnemySpd(Vector2 position) // overheat
        {
            var powerup = new PowerUp(PowerUpSpawner.Bad_EnemySpd, position, true, 9, 600);
            Sound.Spawn.Play(0.4f, rand.NextFloat(-0.2f, 0.2f), 0);

            return powerup;
        }

        public static PowerUp Create_11_VCred(Vector2 position) // overheat
        {
            var powerup = new PowerUp(PowerUpSpawner.Good_VCred, position, false, 10, 600);
            Sound.Spawn.Play(0.4f, rand.NextFloat(-0.2f, 0.2f), 0);

            return powerup;
        }

        public void WasShot()
        {
            isExpired = true;

            // powerup disappears
        }

        public void Kill()
        {
            System.Console.WriteLine("Alpha: " + color.A.ToString());
            color.A -= 1;
            if (color.A <= 5)
                isExpired = true;
        }

        public void Update()
        {
            

            if (timeUntilStart <= 0)
            {
                
                percentLife -= 1f / lifeTime;

                if (percentLife < 0)
                    Kill();
            }
            else
            {
                timeUntilStart--;
                color = Color.White * (1 - timeUntilStart / 60f);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // make the size of the black hole pulsate
            float scale = 1f + 0.1f * (float)Math.Sin(2 * GameRoot.GameTime.TotalGameTime.TotalSeconds);
            spriteBatch.Draw(texture, new Vector2(position.X,position.Y), null, null, new Vector2(texture.Width/2,texture.Height/2), 0f, new Vector2(1f,1f), color, SpriteEffects.None, 0f);
            spriteBatch.Draw(ringTexture, position, null, null, new Vector2(ringTexture.Width/2, ringTexture.Height/2), 0f, new Vector2(scale, scale), color, SpriteEffects.None, 0f);
        }
    }
}