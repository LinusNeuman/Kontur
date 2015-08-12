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
    static class EnemySpawner
    {
        #region Textures

        public static Texture2D Follower { get; private set; } // change names
        public static Texture2D Wanderer { get; private set; }

        #endregion

        static Random rand = new Random();
        static float inverseSpawnChance = 60;
        static float inverseBlackHoleChance = 600;

        public static void Update()
        {
            if (!PlayerShip.Instance.IsDead && EntityManager.Count < 20)
            {
                if (rand.Next((int)inverseSpawnChance) == 0 && EntityManager.EnemyCount < 15)
                    EntityManager.Add(Enemy.CreateSeeker(GetSpawnPosition()));
                if (rand.Next((int)inverseSpawnChance) == 0 && EntityManager.EnemyCount < 5)
                    EntityManager.Add(Enemy.CreateWanderer(GetSpawnPosition()));

            }

            // increase spawn rate
            if (inverseSpawnChance > 15)
                inverseSpawnChance -= 0.02f;

            
        }

        public static void Load(ContentManager content)
        {
            Follower = content.Load<Texture2D>("Enemies/Follower");
            Wanderer = content.Load<Texture2D>("Enemies/Wanderer");
        }

        private static Vector2 GetSpawnPosition()
        {
            Vector2 pos;
            do
            {
                pos = new Vector2(rand.Next((int)GameRoot.VirtualScreenSize.X - 100), rand.Next((int)GameRoot.VirtualScreenSize.Y - 100));
            }
            while (Vector2.DistanceSquared(pos, PlayerShip.Instance.Position) < 800);

            return pos;
        }

        public static void Reset()
        {
            inverseSpawnChance = 60;
        }
    }
}