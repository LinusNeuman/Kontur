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

namespace NeonShooter
{
    public class Pause
    {

        static Random rand = new Random();

        List<Button> buttonList = new List<Button>();

        public Pause()
        {

        }


        public static void Load(ContentManager content)
        {

        }

        public void Update(GameTime gameTime)
        {

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
                            Menu.gameState = Menu.GameState.menu;
                        }

                        if (buttonList[i].bgameState == Button.bGameState.ingame)
                        {
                            Bullet.Load(Content);
                            BlackHole.Load(Content);
                            PlayerShip.Load(Content);
                            EntityManager.Add(PlayerShip.Instance);
                            EnemySpawner.Load(Content);


                            MediaPlayer.Resume();

                            MediaPlayer.IsRepeating = true;

                            int f = Array.FindIndex(BloomSettings.PresetSettings, row => row.Name == "Default");
                            GameRoot.Instance.bloom.Settings = BloomSettings.PresetSettings[f];



                            Menu.gameState = Menu.GameState.ingame;
                        }

                        if (buttonList[i].texture == null)
                        {
                        }

                        if (buttonList[i].texture == null)
                        {
                            
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

                                GameRoot.ParticleManager.CreateParticle(Art.LineParticle, gesture.Position, color, 190, new Vector2(1f, 1f), state);
                            }
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < buttonList.Count; i++)
            {
                spriteBatch.Draw(buttonList[i].texture, buttonList[i].Position, Color.White);
            }
        }
    }
}