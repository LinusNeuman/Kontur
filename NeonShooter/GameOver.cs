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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BloomPostprocess;

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
    public class GameOver
    {
        #region Textures

        public static Texture2D RestartTxt { get; private set; }
        public static Texture2D MainmenuTxt { get; private set; }
        public static Texture2D BgTxt { get; private set; }


        #endregion

        static Random rand = new Random();

        List<Button> buttonList = new List<Button>();

        public GameOver()
        {
            buttonList.Add(new Button()
            {
                texture = RestartTxt,
                Position = new Vector2(0 + RestartTxt.Width + 200, GameRoot.VirtualScreenSize.Y - RestartTxt.Height - 300),
                bgameState = NeonShooter.Button.bGameState.ingame,
            });
            buttonList.Add(new Button()
            {
                texture = MainmenuTxt,
                Position = new Vector2(GameRoot.VirtualScreenSize.X - MainmenuTxt.Width - 500, GameRoot.VirtualScreenSize.Y - MainmenuTxt.Height - 300),
                bgameState = NeonShooter.Button.bGameState.menu,
            });
        }


        public static void Load(ContentManager content)
        {
            RestartTxt = content.Load<Texture2D>("Buttons/Restart");
            MainmenuTxt = content.Load<Texture2D>("Buttons/Mainmenu");
            BgTxt = content.Load<Texture2D>("Graphics/GameoverBg");
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
                        if (buttonList[i].bgameState == Button.bGameState.menu)
                        {
                            PlayerShip.Instance.Kill();
                            PlayerShip.Instance.ResetGame();

                            EntityManager.ResetGame();

                            Menu.Load(Content);
                            Sound.LoadTheme(Content);

                            Sound.Music.Dispose();
                            Sound.Explosion.Dispose();
                            Sound.Shot.Dispose();
                            Sound.Spawn.Dispose();
                            Bullet.BulletDamage.Dispose();
                            Bullet.BulletSpeed.Dispose();
                            Bullet.BulletStandard.Dispose();
                            Bullet.BulletTank.Dispose();


                            PlayerShip.Pixel.Dispose();
                            PlayerShip.PlayerDmgShip.Dispose();
                            PlayerShip.PlayerSpdShip.Dispose();
                            PlayerShip.PlayerStndShip.Dispose();
                            PlayerShip.PlayerTnkShip.Dispose();

                            EntityManager.ResetGame();

                            EnemySpawner.Follower.Dispose();
                            EnemySpawner.Wanderer.Dispose();

                            Menu.gameState = Menu.GameState.menu;


                        }

                        if (buttonList[i].bgameState == Button.bGameState.ingame)
                        {

                            for (int g = 0; g < buttonList.Count; g++)
                            {
                                buttonList[g].texture.Dispose();
                            }


                            Sound.MainTheme.Dispose();
                            Sound.Load(Content);
                            Bullet.Load(Content);
                      
                            PlayerShip.Load(Content);
                            EntityManager.ResetGame();
                            EnemySpawner.Load(Content);

                            MediaPlayer.Volume = 0.8f;
                            MediaPlayer.Play(Sound.Music);

                            MediaPlayer.IsRepeating = true;

                            int f = Array.FindIndex(BloomSettings.PresetSettings, row => row.Name == "Default2");
                            GameRoot.Instance.bloom.Settings = BloomSettings.PresetSettings[f];



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