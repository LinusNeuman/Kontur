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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace NeonShooter
{
    static class PowerUpSpawner
    {
        #region Textures

        public static Texture2D Player_Shield { get; private set; }
        public static Texture2D Good_VCred { get; private set; }
        public static Texture2D Bad_2xEnemy { get; private set; }
        public static Texture2D Bad_EnemySpd { get; private set; }
        public static Texture2D Bad_Overheat { get; private set; }
        public static Texture2D Good_2Bullets { get; private set; }
        public static Texture2D Good_2xMultiplier { get; private set; }
        public static Texture2D Good_CircleShoot { get; private set; }
        public static Texture2D Good_Shield { get; private set; }
        public static Texture2D Bad_HalfSpeed { get; private set; }
        public static Texture2D Good_Speed { get; private set; }
        public static Texture2D Good_ExtraLife { get; private set; }
        public static Texture2D Ring_Good { get; private set; }
        public static Texture2D Ring_Bad { get; private set; }

        #endregion

        static Random rand = new Random();
        static float inverseSpawnChance = 300;



        public static void Update()
        {
            if (!PlayerShip.Instance.IsDead && PowerUpManager.Count < 3)
            {
                if (rand.Next((int)inverseSpawnChance) == 0 && PowerUpManager.Count < 1)
                    Spawn();

            }

            // increase spawn rate
            if (inverseSpawnChance > 300)
                inverseSpawnChance -= 0.002f;


        }

        public static void Spawn()
        {
            int chooser = 0;
            Random r = new Random();
            chooser = r.Next(10);
            if (chooser == 0)
                PowerUpManager.Add(PowerUp.Create_1_OverHeat(GetSpawnPosition()));
            if (chooser == 1)
                PowerUpManager.Add(PowerUp.Create_2_2Bullets(GetSpawnPosition()));
            if (chooser == 2)
                PowerUpManager.Add(PowerUp.Create_3_2xMultiplier(GetSpawnPosition()));
            if (chooser == 3)
                PowerUpManager.Add(PowerUp.Create_4_CircleShoot(GetSpawnPosition()));
            if (chooser == 4)
                PowerUpManager.Add(PowerUp.Create_5_Shield(GetSpawnPosition()));
            if (chooser == 5)
                PowerUpManager.Add(PowerUp.Create_6_HalfSpeed(GetSpawnPosition()));
            if (chooser == 6)
                PowerUpManager.Add(PowerUp.Create_7_Speed(GetSpawnPosition()));
            if (chooser == 7)
                PowerUpManager.Add(PowerUp.Create_8_ExtraLife(GetSpawnPosition()));
            if (chooser == 8)
                PowerUpManager.Add(PowerUp.Create_9_2xEnemy(GetSpawnPosition()));
            if (chooser == 9)
                PowerUpManager.Add(PowerUp.Create_10_EnemySpd(GetSpawnPosition()));
            if (chooser == 10)
                PowerUpManager.Add(PowerUp.Create_11_VCred(GetSpawnPosition()));
            
        }

        public static void Load(ContentManager content)
        {
            Player_Shield = content.Load<Texture2D>("PowerUps/Outlines/Player_Shield");
            Good_VCred = content.Load<Texture2D>("PowerUps/Icons/Good_VCred");
            Bad_EnemySpd = content.Load<Texture2D>("PowerUps/Icons/Bad_EnemySpd");
            Bad_2xEnemy = content.Load<Texture2D>("PowerUps/Icons/Bad_2xEnemy");
            Bad_Overheat = content.Load<Texture2D>("PowerUps/Icons/Bad_Overheat");
            Good_2Bullets = content.Load<Texture2D>("PowerUps/Icons/Good_2Bullets");
            Good_2xMultiplier = content.Load<Texture2D>("PowerUps/Icons/Good_2xMultiplier");
            Good_CircleShoot = content.Load<Texture2D>("PowerUps/Icons/Good_CircleShoot");
            Good_Shield = content.Load<Texture2D>("PowerUps/Icons/Good_Shield");
            Bad_HalfSpeed = content.Load<Texture2D>("PowerUps/Icons/Bad_HalfSpeed");
            Good_Speed = content.Load<Texture2D>("PowerUps/Icons/Good_Speed");
            Good_ExtraLife = content.Load<Texture2D>("PowerUps/Icons/Good_ExtraLife");
            Ring_Bad = content.Load<Texture2D>("PowerUps/Outlines/Bad_Ring");
            Ring_Good = content.Load<Texture2D>("PowerUps/Outlines/Good_Ring");
        }

        private static Vector2 GetSpawnPosition()
        {
            Vector2 pos;
            pos.X = rand.Next(100, (int)GameRoot.VirtualScreenSize.X - 100);
            pos.Y = rand.Next(100, (int)GameRoot.VirtualScreenSize.Y - 100);

            return pos;
        }

        public static void Reset()
        {
            inverseSpawnChance = 300;
        }
    }
}