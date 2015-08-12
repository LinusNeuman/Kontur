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
    public class AppliedEffects
    {
        public int id;
        public float duration;
        public float lifePercent;
        public bool isExpired;

        public AppliedEffects(int ID, float Duration)
        {
            duration = Duration;
            id = ID;

            lifePercent = 1f;


        }

        
    }

    static class PlayerStatus
    {
        // time it takes for multiplier to go away
        private const float multiplierExpiryTime = 2f;
        private const int maxMultiplier = 5;

        public static int Lives { get; private set; }
        public static int Score { get; private set; }
        public static int Multiplier { get; private set; }
        public static int HighScore { get; private set; }
        public static bool IsGameOver { get { return Lives == 0; } }

        private static float multiplierTimeLeft;
        private static int scoreForExtraLife;

        public static List<AppliedEffects> appliedEffects = new List<AppliedEffects>();

        public static bool checked1Million;
        public static bool checked50K;
        public static bool checked100K;
        public static bool checked100Kills;

        public static int selectedShip;

        private const string highScoreFilename = "highscores.txt";

        public static int achkills;
        public static bool achkilledbyblackhole;
        public static int achscore;
        public static int achlifetime;

        public static int credits;

        public static bool FindAE(AppliedEffects ae, int id)
        {
            return ae.id == id;
        }

        public static void UpdateAppliedEffects()
        {
            for (int i = 0; i < appliedEffects.Count; i++)
            {
                if(appliedEffects[i].id == 0)
                {
                    // apply the bad: overheat
                }

                if (appliedEffects[i].id == 1)
                {
                    // double bullets
                }

                if (appliedEffects[i].id == 2)
                {
                    // apply the good: 2xmultiplier
                }

                if (appliedEffects[i].id == 3)
                {
                    // apply the good: circleshoot
                }

                if (appliedEffects[i].id == 4)
                {
                    // apply the good: Shield
                }

                if (appliedEffects[i].id == 5)
                {
                    // apply the good: slowmotion
                }

                if (appliedEffects[i].id == 6)
                {
                    // apply the good: speed
                }

                if (appliedEffects[i].id == 7)
                {
                    // apply the good: extra life
                }

                if (appliedEffects[i].id == 8)
                {
                    // apply the bad: 2x enemy
                }

                if (appliedEffects[i].id == 9)
                {
                    // apply the bad: enemy speed
                }

                if (appliedEffects[i].id == 10)
                {
                    // apply the good: v credit
                }

                appliedEffects[i].lifePercent -= 1f / appliedEffects[i].duration;

                if(appliedEffects[i].lifePercent <= 0)
                {
                    appliedEffects[i].isExpired = true;
                }

                appliedEffects = appliedEffects.Where(x => !x.isExpired).ToList();
            }
        }

        private static int LoadHighScore()
        {
            int score = 0;
            //return File.Exists(highScoreFilename) && int.TryParse(File.ReadAllText(highScoreFilename), out score) ? score : 0;

            NeonShooter.Activity1 activity = GameRoot.Activity as NeonShooter.Activity1;
            if(activity.pGooglePlayClient.IsConnected)
            {
                IPendingResult result;
                result = GamesClass.Leaderboards.LoadCurrentPlayerLeaderboardScore(activity.pGooglePlayClient, "CgkI3bWJ_OoVEAIQCQ", LeaderboardVariant.TimeSpanAllTime, LeaderboardVariant.CollectionPublic);
                //result.SetResultCallback(new IResultCallback<IPlayersLoadPlayersResult>()
                System.Console.WriteLine(result.ToString());
            }

            return score;
        }

       

        private static void SaveHighScore(int score)
        {
            
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

        public static void GiveEffect(int id, int duration)
        {
            

            for (int i = 0; i < appliedEffects.Count; i++)
            {
                if (appliedEffects[i].id == id)
                {
                    appliedEffects[i].lifePercent = 1f;
                }

                if (appliedEffects[i].id != id && appliedEffects[i].id != 10 && appliedEffects[i].id != 7 && appliedEffects.Count > 0)
                {
                    appliedEffects.Add(new AppliedEffects(id, duration));
                }
            }

            if (id == 10)
            {
                // increase v-credits by one
            }

            if (id == 7)
            {
                Lives += 1;
            }

            if (appliedEffects.Count == 0 && id != 10 && id != 7)
                appliedEffects.Add(new AppliedEffects(id, duration));

        }

        public static void ResetAchievementData()
        {
            achkills = 0;
            achlifetime = 0;
            achscore = 0;
            achkilledbyblackhole = false;

            checked100K = false;
            checked100Kills = false;
            checked1Million = false;
            checked50K = false;
        }

        public static void Reset()
        {
            if (Score > HighScore)
                SaveHighScore(HighScore = Score);

            Score = 0;
            Multiplier = 1;
            Lives = 2;
            scoreForExtraLife = 2000;
            multiplierTimeLeft = 2;

            ResetAchievementData();
        }

        public static void Update()
        {
            UpdateAppliedEffects();

            if (Multiplier > 1)
            {
                //if((multiplierTimeLeft -= (float)GameRoot.GameTime.ElapsedGameTime.TotalSeconds) <= 3)
                //{
                //    multiplierTimeLeft = multiplierExpiryTime;
                //    ResetMultiplier();
                //}

                multiplierTimeLeft -= (float)GameRoot.GameTime.ElapsedGameTime.TotalSeconds;

                if(multiplierTimeLeft <= 0)
                {
                    multiplierTimeLeft = multiplierExpiryTime;
                    ResetMultiplier();
                }
                
            }


            if (PlayerStatus.IsGameOver)
            {
                Menu.gameState = Menu.GameState.gameover;
                GameOver.TransmitScore();
            }

            CheckAchievementProgress();
        }

        public static void CheckAchievementProgress()
        {
            if(Score >= 1000000 && checked1Million == false)
            {
                AchievementManager.pInstance.UnlockAchievement(AchievementManager.Achievements.Million);
                checked1Million = true;
            }


            if (Score >= 50000 && checked50K == false)
            {
                AchievementManager.pInstance.UnlockAchievement(AchievementManager.Achievements.FiftyKScore);
                checked50K = true;
            }

            if (Score >= 100000 && checked100K == false)
            {
                AchievementManager.pInstance.UnlockAchievement(AchievementManager.Achievements.OneHundredKScore);
                checked100K = true;
            }

            if (achkills >= 100 && checked100Kills == false)
            {
                AchievementManager.pInstance.UnlockAchievement(AchievementManager.Achievements.OneHundredKills);
                checked100Kills = true;
            }
        }

        public static void AddPoints(int basePoints)
        {
            if (PlayerShip.Instance.IsDead)
                return;

            if (!PlayerStatus.appliedEffects.Exists(ae => PlayerStatus.FindAE(ae, 2)))
                Score += (basePoints * Multiplier);
            if (PlayerStatus.appliedEffects.Exists(ae => PlayerStatus.FindAE(ae, 2)))
                Score += (basePoints * Multiplier) * 2;

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