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

// test for google play services
using Android.Media;
using Android.Gms;
using Android.Gms.Common;
using Android.Gms.Games;
using Android.Gms.Games.LeaderBoard;
using Android.Gms.Plus;
using Android.Gms.Plus.Model.People;
using Android.Gms.Common.Apis;
using Android.Content;
using Android.Views;
// test for in app billing

namespace NeonShooter
{
    public class AchievementManager
    {

        public enum Achievements
        {
            OneHundredKScore, // Score over 100K in one game
            FiftyKScore, // Score over 50K in one game
            OneHundredKills, // Kill over 100 enemies in one game
            BlackHole, // Get killed by a black hole
            Survive, // Survive for 5 minutes
            Million,
            Count,
        }

        /// <summary>
        /// Wraps up all the data about a single Achievement
        /// </summary>
        public struct AchievementData
        {
            /// <summary>
            /// The id used for looking up this Achievement.
            /// </summary>
            public Int32 mResourceId;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="resourceId">Resource identifier.</param>
            public AchievementData(Int32 resourceId)
            {
                mResourceId = resourceId;
            }
        }

        /// <summary>
        /// Single static instance of this class.
        /// </summary>
        private static AchievementManager mInstance;

        /// <summary>
        /// Maps an Achiements enum to actual achievement data.
        /// </summary>
        private Dictionary<Int32, AchievementData> mAchievementMapping;

        /// <summary>
        /// Call this before using the singleton.
        /// </summary>
        public void Initialize()
        {
            mAchievementMapping = new Dictionary<Int32, AchievementData>
            {
                { (Int32)Achievements.OneHundredKScore,               new AchievementData(Resource.String.achievement_100k) }, 
                { (Int32)Achievements.FiftyKScore,        new AchievementData(Resource.String.achievement_50k) }, 
                { (Int32)Achievements.OneHundredKills,        new AchievementData(Resource.String.achievement_100_explosions) }, 
                { (Int32)Achievements.BlackHole,           new AchievementData(Resource.String.achievement_dangerous_void) }, 
                { (Int32)Achievements.Survive,              new AchievementData(Resource.String.achievement_survivor) }, 
                { (Int32)Achievements.Million,              new AchievementData(Resource.String.achievement_1_Million) }, 
            };
            

            System.Diagnostics.Debug.Assert(mAchievementMapping.Count == (Int32)Achievements.Count);

        }

        /// <summary>
        /// Platform agnostic way for unlocking achievements. Can be called repeatedly.
        /// </summary>
        /// <param name="ach">The achievement to unlock.</param>
        public void UnlockAchievement(Achievements ach)
        {
            NeonShooter.Activity1 activity = GameRoot.Activity as NeonShooter.Activity1;
            if (activity.pGooglePlayClient.IsConnected)
            {
                GamesClass.Achievements.Unlock(activity.pGooglePlayClient, activity.Resources.GetString(mAchievementMapping[(Int32)ach].mResourceId));
            }
            //DebugMessageDisplay.pInstance.AddConstantMessage("Unlocked: " + ach.ToString());
        }

        /// <summary>
        /// Access to the singleton.
        /// </summary>
        public static AchievementManager pInstance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new AchievementManager();
                }

                return mInstance;
            }
        }
    }
}