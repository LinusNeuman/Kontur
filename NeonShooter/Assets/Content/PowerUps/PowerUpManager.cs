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
        static List<PowerUp> powerups = new List<PowerUp>();



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
                if (powerups[i].IsActive && IsColliding(PlayerShip.Instance, powerups[i]))
                {
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

            foreach (var entity in entities)
                entity.Update();

            isUpdating = false;

            foreach (var entity in addedEntities)
                AddEntity(entity);

            addedEntities.Clear();

            // remove all expired entities.
            entities = entities.Where(x => !x.IsExpired).ToList();

            bullets = bullets.Where(x => !x.IsExpired).ToList();
            enemies = enemies.Where(x => !x.IsExpired).ToList();



        }

        private static bool IsColliding(Entity a, Entity b)
        {
            float radius = a.Radius + b.Radius;
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
                entity.Draw(spriteBatch);
        }
    }
}