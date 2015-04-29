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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

// test for google play services
using Android.Gms;
using Android.Gms.Common;
using Android.Gms.Games;
using Android.Gms.Games.LeaderBoard;
using Android.Gms.Plus;
using Android.Gms.Plus.Model.People;
using Android.Gms.Common.Apis;
using Android.Views;
using BloomPostprocess;
// test for in app billing

namespace NeonShooter
{
    public class Menu
    {
        #region Textures

        public static Texture2D PlayTxt { get; private set; }
        public static Texture2D HighscoresTxt { get; private set; }
        public static Texture2D AboutTxt { get; private set; }
        public static Texture2D SettingsTxt { get; private set; }
        public static Texture2D BgTxt { get; private set; }

        #endregion

        static Random rand = new Random();

        public enum GameState
        {
            menu,
            ingame,
            gameover,
            pause,
            upgrades,
            play,
            about,
            settings,
            leaderboards
        }

        public static GameState gameState = GameState.menu;

        List<Button> buttonList = new List<Button>();

        public Menu()
        {

            buttonList.Add(new Button()
            {
               texture = PlayTxt,
               Position = new Vector2(GameRoot.VirtualScreenSize.X - PlayTxt.Width - 140, GameRoot.VirtualScreenSize.Y - PlayTxt.Height - 680),
               bgameState = NeonShooter.Button.bGameState.upgrades,
            });
            buttonList.Add(new Button()
            {
                texture = HighscoresTxt,
                Position = new Vector2(GameRoot.VirtualScreenSize.X - HighscoresTxt.Width - 140, GameRoot.VirtualScreenSize.Y - HighscoresTxt.Height - 430),
                bgameState = NeonShooter.Button.bGameState.leaderboards,
            });
            buttonList.Add(new Button()
            {
                texture = SettingsTxt,
                Position = new Vector2(GameRoot.VirtualScreenSize.X - AboutTxt.Width - 140, GameRoot.VirtualScreenSize.Y - AboutTxt.Height - 200),
                bgameState = NeonShooter.Button.bGameState.settings,
            });
            buttonList.Add(new Button()
            {
                texture = AboutTxt,
                Position = new Vector2(GameRoot.VirtualScreenSize.X - AboutTxt.Width - 400, GameRoot.VirtualScreenSize.Y - AboutTxt.Height - 200),
                bgameState = NeonShooter.Button.bGameState.about,
            });

            MediaPlayer.Play(Sound.MainTheme);
            MediaPlayer.IsRepeating = true;
        }

        public static void Reload()
        {
            
        }

        public static void Load(ContentManager Content)
        {
            PlayTxt = Content.Load<Texture2D>("Buttons/Play");
            SettingsTxt = Content.Load<Texture2D>("Buttons/Settings");
            AboutTxt = Content.Load<Texture2D>("Buttons/About");
            HighscoresTxt = Content.Load<Texture2D>("Buttons/Highscores");
            BgTxt = Content.Load<Texture2D>("Graphics/MainMenuBG");
        }

        public void Update(ContentManager Content)
        {
            HandleTouchInput(Content);

        }

        public void HandleTouchInput(ContentManager Content)
        {
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();
                

                for (int i = 0; i < buttonList.Count; i++)
			    {
                    if ((gesture.Position.X * GameRoot.tempScale.X > buttonList[i].Position.X && gesture.Position.X * GameRoot.tempScale.X < buttonList[i].Position.X + buttonList[i].texture.Width &&
                        gesture.Position.Y * GameRoot.tempScale.Y > buttonList[i].Position.Y && gesture.Position.Y * GameRoot.tempScale.Y < buttonList[i].Position.Y + buttonList[i].texture.Height))
                    {
                        if (buttonList[i].bgameState == Button.bGameState.ingame)
                        {
                            for (int f = 0; f < buttonList.Count; f++)
                            {
                                buttonList[f].texture.Dispose();
                            }
                            Bullet.Load(Content);
                      
                            PlayerShip.Load(Content);
                            EntityManager.Add(PlayerShip.Instance);
                            EnemySpawner.Load(Content);
                            Upgrades.LoadButtons(Content);

                            MediaPlayer.Play(Sound.Music);
                            MediaPlayer.IsRepeating = true;
                            gameState = GameState.ingame;
                        }

                        if (buttonList[i].bgameState == Button.bGameState.play)
                        {
                            gameState = GameState.play;
                        }

                        if (buttonList[i].bgameState == Button.bGameState.settings)
                        {
                            gameState = GameState.settings;
                        }

                        if (buttonList[i].bgameState == Button.bGameState.upgrades)
                        {
                            for (int g = 0; g < buttonList.Count; g++)
                            {
                                buttonList[g].texture.Dispose();
                            }
                            BgTxt.Dispose();

                            int f = Array.FindIndex(BloomSettings.PresetSettings, row => row.Name == "Preview");
                            GameRoot.Instance.bloom.Settings = BloomSettings.PresetSettings[f];
                            Upgrades.Load(Content);
                            Upgrades.LoadButtons(Content);
                            Upgrades.ReloadButtons();
                            gameState = GameState.upgrades;
                        }

                        if (buttonList[i].bgameState == Button.bGameState.about)
                        {
                            gameState = GameState.about;
                        }

                        if (buttonList[i].bgameState == Button.bGameState.leaderboards)
                        {
                            NeonShooter.Activity1 activity = GameRoot.Activity as NeonShooter.Activity1;
                            if (activity.pGooglePlayClient.IsConnected)
                                activity.StartActivityForResult(GamesClass.Leaderboards.GetAllLeaderboardsIntent(activity.pGooglePlayClient), Activity1.REQUEST_LEADERBOARD);
                        }
                    }
                    else
                    {
                        if (gesture.GestureType == GestureType.Tap)
                        {
                            Color yellow = new Color(47, 206, 251);
                            //Color color2 = ColorUtil.HSVToColor(5, 0.5f, 0.8f);

                            for (int o = 0; o < 4; o++)
                            {
                                float speed = 6f * (1f - 1 / rand.NextFloat(1f, 6f));

                                Color color = Color.Lerp(Color.White, yellow, rand.NextFloat(0, 1));

                                var state = new ParticleState()
                                {
                                    Velocity = rand.NextVector2(speed, speed),
                                    Type = ParticleType.None,
                                    LengthMultiplier = 0.5f
                                };

                                GameRoot.ParticleManager2.CreateParticle(Art.LineParticle, gesture.Position * GameRoot.tempScale, color, 190, new Vector2(1f, 1f), state);
                            }
                        }
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BgTxt, Vector2.Zero, Color.White);



            for (int i = 0; i < buttonList.Count; i++)
            {
                spriteBatch.Draw(buttonList[i].texture, buttonList[i].Position, Color.White);
            }
        }
    }
}