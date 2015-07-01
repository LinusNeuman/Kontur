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

        public static Texture2D Bad_Overheat { get; private set; }
        public static Texture2D Good_2Bullets { get; private set; }
        public static Texture2D Good_2xMultiplier { get; private set; }
        public static Texture2D Good_CircleShoot { get; private set; }
        public static Texture2D Good_Shield { get; private set; }
        public static Texture2D Good_Slowmotion { get; private set; }
        public static Texture2D Good_Speed { get; private set; }
        public static Texture2D Good_ExtraLife { get; private set; }

        #endregion

        static Random rand = new Random();
        static float inverseSpawnChance = 300;



        public static void Update()
        {
            if (!PlayerShip.Instance.IsDead && PowerUpManager.Count < 1)
            {
                if (rand.Next((int)inverseSpawnChance) == 0 && PowerUpManager.Count < 1)
                    PowerUpManager.Add(PowerUp.Create_1_OverHeat(GetSpawnPosition()));
                if (rand.Next((int)inverseSpawnChance) == 0 && PowerUpManager.Count < 1)
                    //PowerUpManager.Add(Enemy.CreateWanderer(GetSpawnPosition()));

            }

            // increase spawn rate
            if (inverseSpawnChance > 15)
                inverseSpawnChance -= 0.002f;


        }

        public static void Load(ContentManager content)
        {
            Bad_Overheat = content.Load<Texture2D>("PowerUps/Icons/Bad_Overheat");
            Good_2Bullets = content.Load<Texture2D>("PowerUps/Icons/Good_2Bullets");
            Good_2xMultiplier = content.Load<Texture2D>("PowerUps/Icons/Good_2xMultiplier");
            Good_CircleShoot = content.Load<Texture2D>("PowerUps/Icons/Good_CircleShoot");
            Good_Shield = content.Load<Texture2D>("PowerUps/Icons/Good_Shield");
            Good_Slowmotion = content.Load<Texture2D>("PowerUps/Icons/Good_Slowmotion");
            Good_Speed = content.Load<Texture2D>("PowerUps/Icons/Good_Speed");
            Good_ExtraLife = content.Load<Texture2D>("PowerUps/Icons/Good_ExtraLife");
        }

        private static Vector2 GetSpawnPosition()
        {
            Vector2 pos;
            do
            {
                pos = new Vector2(rand.Next((int)GameRoot.VirtualScreenSize.X - 100), rand.Next((int)GameRoot.VirtualScreenSize.Y - 100));
            }
            while (Vector2.DistanceSquared(pos, PlayerShip.Instance.Position) < 100 * 300);

            return pos;
        }

        public static void Reset()
        {
            inverseSpawnChance = 300;
        }
    }
}