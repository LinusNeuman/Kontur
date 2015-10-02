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
using Microsoft.Xna.Framework.Content;

namespace NeonShooter
{
    public class PlayerMessageHandler
    {
        public static SpriteFont mediumFont;

        public static List<PlayerMessage> pMessages = new List<PlayerMessage>();



        static bool isUpdating;
        static List<PlayerMessage> addedEntities = new List<PlayerMessage>();

        public static int Count { get { return pMessages.Count; } }

        public static void Add(PlayerMessage pmessage)
        {
            if (!isUpdating)
                AddEntity(pmessage);
            else
                addedEntities.Add(pmessage);
        }



        private static void AddEntity(PlayerMessage pmessage)
        {
            pMessages.Add(pmessage);

        }

        public static void ResetGame()
        {
            addedEntities.Clear();
            pMessages.Clear();
        }

        public static void Load(ContentManager content)
        {
            mediumFont = content.Load<SpriteFont>("Fonts/MessageFont");
        }

        

        public static void Update()
        {
            isUpdating = true;

            foreach (var pmessage in pMessages)
                pmessage.Update();

            isUpdating = false;

            foreach (var entity in addedEntities)
                AddEntity(entity);

            addedEntities.Clear();

            // remove all expired entities.
            pMessages = pMessages.Where(x => !x.isExpired).ToList();



        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < pMessages.Count; i++)
            {
                spriteBatch.DrawString(pMessages[i].contentfont, pMessages[i].contentMessage, new Vector2(pMessages[i].position.X, pMessages[i].position.Y + (i*50)), pMessages[i].contentColor);
            }
        }
    }
}