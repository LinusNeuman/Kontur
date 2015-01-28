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

namespace NeonShooter
{
    static class EnemySpawner
    {
        static Random rand = new Random();
        static float inverseSpawnChance = 60;

        public static void Update()
        {
            if (!PlayerShip.Instance.IsDead && EntityManager.Count < 200)
            {
                if (rand.Next((int)inverseSpawnChance) == 0)
                    EntityManager.Add(Enemy.CreateSeeker(GetSpawnPosition()));
                if (rand.Next((int)inverseSpawnChance) == 0)
                    EntityManager.Add(Enemy.CreateWanderer(GetSpawnPosition()));
            }

            // increase spawn rate
            if (inverseSpawnChance > 20)
                inverseSpawnChance -= 0.005f;
        }

        private 
    }
}