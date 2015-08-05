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
    static class PowerUpManager
    {
        public static List<PowerUp> powerups = new List<PowerUp>();



        static bool isUpdating;
        static List<PowerUp> addedEntities = new List<PowerUp>();

        public static int Count { get { return powerups.Count; } }

        public static void Add(PowerUp powerup)
        {
            if (!isUpdating)
                AddEntity(powerup);
            else
                addedEntities.Add(powerup);
        }

        

        private static void AddEntity(PowerUp powerup)
        {
            powerups.Add(powerup);

        }

        public static void ResetGame()
        {
            addedEntities.Clear();
            powerups.Clear();


            PowerUpSpawner.Reset();
        }

        

        static void HandleCollisions()
        {
       


       

            //handle collision between the player and enemies
            for (int i = 0; i < powerups.Count; i++)
            {
                if (powerups[i].IsActive && IsColliding2(powerups[i], PlayerShip.Instance))
                {
                    PlayerStatus.GiveEffect(powerups[i].ID);
                    // give player power up
                    powerups.ForEach(x => x.WasShot());
                    break;
                }
            }


        }

        public static void Update()
        {
            isUpdating = true;

            HandleCollisions();

            foreach (var powerup in powerups)
                powerup.Update();

            isUpdating = false;

            foreach (var entity in addedEntities)
                AddEntity(entity);

            addedEntities.Clear();

            // remove all expired entities.
            powerups = powerups.Where(x => !x.isExpired).ToList();



        }

        private static bool IsColliding(PowerUp a, PowerUp b)
        {
            float radius = a.radius + b.radius;
            return !a.isExpired && !b.isExpired && Vector2.DistanceSquared(a.position, b.position) < radius * radius;
        }

        private static bool IsColliding2(PowerUp a, Entity b)
        {
            float radius = a.radius + b.Radius;
            return !a.isExpired && !b.IsExpired && Vector2.DistanceSquared(a.position, b.Position) < radius * radius;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var powerup in powerups)
                powerup.Draw(spriteBatch);
        }
    }
}