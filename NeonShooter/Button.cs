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

namespace NeonShooter
{
    public class Button
    {
        public Texture2D texture;
        public Vector2 Position;
        public string Name;


        public enum bGameState
        {
            menu,
            ingame,
            gameover,
            pause,
            upgrades,
            play,
            about,
            settings,
            leaderboards,
            none
        }

        public bGameState bgameState;
    }
}