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
using System.IO;

// test for google play services
using Android.Gms;
using Android.Gms.Common;
using Android.Gms.Games;
using Android.Gms.Games.LeaderBoard;
using Android.Gms.Plus;
using Android.Gms.Plus.Model.People;
using Android.Gms.Common.Apis;
using Android.Views;
// test for in app billing

namespace NeonShooter
{
    static class PlayerStatus
    {
        // time it takes for multiplier to go away
        private const float multiplierExpiryTime = 0.8f;
        private const int maxMultiplier = 20;

        public static int Lives { get; private set; }
        public static int Score { get; private set; }
        public static int Multiplier { get; private set; }
        public static int HighScore { get; private set; }
        public static bool IsGameOver { get { return Lives == 0; } }

        private static float multiplierTimeLeft;
        private static int scoreForExtraLife;

        private const string highScoreFilename = "highscores.txt";

        public static int achkills;
        public static bool achkilledbyblackhole;
        public static int achscore;
        public static int achlifetime;
        public static int achtimer;

        public static int credits;

        private static int LoadHighScore()
        {
            int score;
            return File.Exists(highScoreFilename) && int.TryParse(File.ReadAllText(highScoreFilename), out score) ? score : 0;
        }

        private static void SaveHighScore(int score)
        {
            NeonShooter.Activity1 activity = GameRoot.Activity as NeonShooter.Activity1;
            if (activity.pGooglePlayClient.IsConnected)
                GamesClass.Leaderboards.SubmitScore(activity.pGooglePlayClient, "CgkI3bWJ_OoVEAIQCQ", score);
            
            try 
            {
                File.WriteAllText(highScoreFilename, score.ToString());
            }     
            catch(Exception e) 
            {
                System.Console.WriteLine("Error: " + e.ToString());
            }
        }

        static PlayerStatus()
        {
            HighScore = LoadHighScore();
            Reset();
        }

        public static void ResetAchievementData()
        {
            achkills = 0;
            achlifetime = 0;
            achscore = 0;
            achkilledbyblackhole = false;
        }

        public static void Reset()
        {
            if (Score > HighScore)
                SaveHighScore(HighScore = Score);

            Score = 0;
            Multiplier = 1;
            Lives = 4;
            scoreForExtraLife = 2000;
            multiplierTimeLeft = 0;

            ResetAchievementData();
        }

        public static void Update()
        {
            if (Multiplier > 1)
            {
                if((multiplierTimeLeft -= (float)GameRoot.GameTime.ElapsedGameTime.TotalSeconds) <= 3)
                {
                    multiplierTimeLeft = multiplierExpiryTime;
                    ResetMultiplier();
                }
                
            }


            if (PlayerStatus.IsGameOver)
            {
                Menu.gameState = Menu.GameState.gameover;
            }

            CheckAchievementProgress();
        }

        public static void CheckAchievementProgress()
        {
            achtimer += (int)GameRoot.GameTime.ElapsedGameTime.TotalMinutes;

            if (achtimer >= 5)
            {
                AchievementManager.pInstance.UnlockAchievement(AchievementManager.Achievements.Survive);
            }

            if(Score >= 50000)
            {
                AchievementManager.pInstance.UnlockAchievement(AchievementManager.Achievements.FiftyKScore);
            }

            if (Score >= 100000)
            {
                AchievementManager.pInstance.UnlockAchievement(AchievementManager.Achievements.OneHundredKScore);
            }

            if (achkills >= 100)
            {
                AchievementManager.pInstance.UnlockAchievement(AchievementManager.Achievements.OneHundredKills);
            }
        }

        public static void AddPoints(int basePoints)
        {
            if (PlayerShip.Instance.IsDead)
                return;

            Score += basePoints * Multiplier;
            while (Score >= scoreForExtraLife)
            {
                scoreForExtraLife += 2000;
                Lives++;
            }
        }

        public static void IncreaseMultiplier()
        {
            if (PlayerShip.Instance.IsDead)
                return;

            multiplierTimeLeft = multiplierExpiryTime;
            if (Multiplier < maxMultiplier)
                Multiplier++;
        }

        public static void ResetMultiplier()
        {
            Multiplier = 1;
        }

        public static void RemoveLife()
        {
            Lives--;
        }
    }
}