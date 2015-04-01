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
        public static Texture2D Wanderer_Part1 { get; private set; }
        public static Texture2D Wanderer_Part2 { get; private set; }

        #endregion

        static Random rand = new Random();
        static float inverseSpawnChance = 60;
        static float inverseBlackHoleChance = 600;

        public static void Update()
        {
            if (!PlayerShip.Instance.IsDead && EntityManager.Count < 200)
            {
                if (rand.Next((int)inverseSpawnChance) == 0 && EntityManager.EnemyCount < 10)
                    EntityManager.Add(Enemy.CreateSeeker(GetSpawnPosition()));
                if (rand.Next((int)inverseSpawnChance) == 0 && EntityManager.EnemyCount < 10)
                    EntityManager.Add(Enemy.CreateWanderer(GetSpawnPosition()));

                if (EntityManager.BlackHoleCount < 1 && rand.Next((int)inverseBlackHoleChance) == 0)
                    EntityManager.Add(new BlackHole(GetSpawnPosition()));
            }

            // increase spawn rate
            if (inverseSpawnChance > 20)
                inverseSpawnChance -= 0.005f;

            
        }

        public static void Load(ContentManager content)
        {
            Follower = content.Load<Texture2D>("Enemies/Follower");
            Wanderer_Part1 = content.Load<Texture2D>("Enemies/WandererLvl1");
            Wanderer_Part2 = content.Load<Texture2D>("Enemies/WandererLvl1Small");
        }

        private static Vector2 GetSpawnPosition()
        {
            Vector2 pos;
            do
            {
                pos = new Vector2(rand.Next((int)GameRoot.VirtualScreenSize.X - 50), rand.Next((int)GameRoot.VirtualScreenSize.Y - 50));
            }
            while (Vector2.DistanceSquared(pos, PlayerShip.Instance.Position) < 300 * 300);

            return pos;
        }

        public static void Reset()
        {
            inverseSpawnChance = 60;
        }
    }
}