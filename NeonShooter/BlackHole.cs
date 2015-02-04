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
    class BlackHole : Entity
    {
        private static Random rand = new Random();

        private int hitpoints = 10;

        public BlackHole(Vector2 position)
        {
            image = Art.BlackHole;
            Position = position;
            Radius = image.Width / 2f;
        }

            public void WasShot()
            {
                hitpoints--;
                if (hitpoints <= 0)
                    IsExpired = true;
            }

            public void Kill()
            {
                hitpoints = 0;
                WasShot();
            }

        public override void Update()        {            var entities = EntityManager.GetNerbyEnteties(Position, 250);                        foreach (var entity in entities)            {                if (entity is Enemy && !(entity as Enemy).IsActive)                    continue;                //bullets are repelled by black holes and everything else is pulled towards it, into it.                if (entity is Bullet)                    entity.Velocity += (entity.Position - Position).ScaleTo(0.3f);                else                 {                    var dPos = Position - entity.Position;                    var length = dPos.Length();                    entity.Velocity += dPos.ScaleTo(MathHelper.Lerp(2, 0, length / 205f));                }            }        }

            public override void  Draw(SpriteBatch spriteBatch)
            {
 	            //make the size of black holes pulsate
                float scale = 1 + 0.1f *(float) Math.Sin(10 * GameRoot.GameTime.TotalGameTime.TotalSeconds);
                spriteBatch.Draw(image, Position, null, color, Orientation, Size / 2f, scale, 0, 0);
            }

        }
    }
