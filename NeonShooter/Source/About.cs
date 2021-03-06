using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

// test for google play services
// test for in app billing

namespace NeonShooter
{
    public class About
    {
        #region Textures

        public static Texture2D BgTxt { get; private set; }
        public static Texture2D GoBackTxt { get; private set; }


        #endregion

        static Random rand = new Random();

        List<Button> buttonList = new List<Button>();

        public About()
        {
            buttonList.Add(new Button()
            {
                texture = GoBackTxt,
                Position = new Vector2(0 + 40, GameRoot.VirtualScreenSize.Y - GoBackTxt.Height + 0),
                bgameState = NeonShooter.Button.bGameState.menu,
            });
        }


        public static void Load(ContentManager content)
        {
            GoBackTxt = content.Load<Texture2D>("Upgrades/GoBack");
            BgTxt = content.Load<Texture2D>("Graphics/About");
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
                            Menu.gameState = Menu.GameState.menu;
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