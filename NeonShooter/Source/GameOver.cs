using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

// test for google play services
using Android.Gms.Games;
// test for in app billing

namespace NeonShooter
{
    public class GameOver
    {
        #region Textures

        public static Texture2D RestartTxt { get; private set; }
        public static Texture2D MainmenuTxt { get; private set; }
        public static Texture2D BgTxt { get; private set; }


        #endregion

        static Random rand = new Random();

        bool readyForInput = false;

        List<Button> buttonList = new List<Button>();

        public GameOver()
        {
            buttonList.Add(new Button()
            {
                texture = RestartTxt,
                Position = new Vector2(211 - 66, GameRoot.VirtualScreenSize.Y - RestartTxt.Height - 300),
                bgameState = NeonShooter.Button.bGameState.ingame,
            });
            buttonList.Add(new Button()
            {
                texture = MainmenuTxt,
                Position = new Vector2(GameRoot.VirtualScreenSize.X - MainmenuTxt.Width - 211 + 66, GameRoot.VirtualScreenSize.Y - MainmenuTxt.Height - 300),
                bgameState = NeonShooter.Button.bGameState.menu,
            });
        }


        public static void Load(ContentManager content)
        {
            RestartTxt = content.Load<Texture2D>("GameOver/Restart");
            MainmenuTxt = content.Load<Texture2D>("GameOver/MainMenu");
            BgTxt = content.Load<Texture2D>("GameOver/GameOver");
        }

        public void Update(ContentManager Content)
        {
            if (readyForInput == true)
            {
                HandleTouchInput(Content);
            }
            else
            {
                if(!TouchPanel.IsGestureAvailable)
                {
                    readyForInput = true;
                }
            }
        }

        public static void TransmitScore()
        {
            NeonShooter.Activity1 activity = GameRoot.Activity as NeonShooter.Activity1;
            if (activity.pGooglePlayClient.IsConnected)
                GamesClass.Leaderboards.SubmitScore(activity.pGooglePlayClient, "CgkI3bWJ_OoVEAIQCQ", PlayerStatus.Score);
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
                        if (buttonList[i].bgameState == Button.bGameState.menu)
                        {
                            EntityManager.ResetGame();
                            PlayerShip.Instance.ResetGame();
                            PlayerShip.SetStatsAndSpec();

                            PlayerShip.Instance.framesUntilRespawn = 0;
                            JoystickManager.noDirection = false;
                            GameRoot.ParticleManager.Reset();

                            PowerUpManager.ResetGame();

                            MediaPlayer.Volume = 0.8f;
                            MediaPlayer.Play(Sound.MainTheme);

                            Menu.gameState = Menu.GameState.menu;

                            readyForInput = false;
                        }

                        if (buttonList[i].bgameState == Button.bGameState.ingame)
                        {

                            EntityManager.ResetGame();
                            PlayerShip.Instance.ResetGame();

                            PlayerStatus.selectedShip = Upgrades.selectedShip;
                            PlayerShip.SetStatsAndSpec();
                            EntityManager.Add(PlayerShip.Instance);

                            PlayerShip.Instance.framesUntilRespawn = 0;
                            JoystickManager.noDirection = false;
                            GameRoot.ParticleManager.Reset();

                            PowerUpManager.ResetGame();

                            MediaPlayer.Volume = 0.8f;
                            MediaPlayer.Play(Sound.Music);

                            

                            MediaPlayer.IsRepeating = true;


                            readyForInput = false;
                            Menu.gameState = Menu.GameState.ingame;
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