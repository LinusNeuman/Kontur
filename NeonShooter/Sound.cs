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
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace NeonShooter
{
    static class Sound
    {
        public static Song Music { get; private set; }
        public static Song MainTheme { get; private set; }

        private static readonly Random rand = new Random();

        private static SoundEffect explosions;
        public static SoundEffect Explosion { get { return explosions; } }

        private static SoundEffect shots;
        public static SoundEffect Shot { get { return shots; } }

        private static SoundEffect spawns;
        public static SoundEffect Spawn { get { return spawns; } }

        public static void Load(ContentManager content)
        {
            Music = content.Load<Song>("Sound/LBS_LGA_ANTHEM");
            MainTheme = content.Load<Song>("Sound/8bit");

            explosions = content.Load<SoundEffect>("Sound/explosion");
            shots = content.Load<SoundEffect>("Sound/shoot");
            spawns = content.Load<SoundEffect>("Sound/spawn"); 
        }
    }
}