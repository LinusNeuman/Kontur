using Android.App;
using Android.Content.PM;
using Android.OS;
using System;
using Android.Widget;

namespace NeonShooter
{
    [Activity(Label = "NeonShooter"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            GameRoot.Activity = this;
            var g = new GameRoot();
            SetContentView(g.Window);
            g.Run();
        }
    }
}

