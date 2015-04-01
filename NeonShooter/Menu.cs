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

namespace NeonShooter
{
    public class Menu
    {
        #region Textures

        public static Texture2D ButtonTxt { get; private set; }

        #endregion

        Vector2 tempScale;

        static Random rand = new Random();

        public enum GameState
        {
            menu,
            ingame,
            gameover,
            pause
        }

        public static GameState gameState = GameState.menu;

        List<Button> buttonList = new List<Button>();

        public Menu()
        {
            buttonList.Add(new Button()
            {
               texture = ButtonTxt,
               Position = new Vector2(GameRoot.VirtualScreenSize.X - ButtonTxt.Width - 100, GameRoot.VirtualScreenSize.Y - ButtonTxt.Height - 200),
               bgameState = NeonShooter.Button.bGameState.ingame,
            });
        }

        public static void Load(ContentManager content)
        {
            ButtonTxt = content.Load<Texture2D>("Button");
        }

        public  void Update()
        {
            HandleTouchInput();

            FingerParticles();
        }

        public void FingerParticles()
        {
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();



                Color yellow = new Color(0.8f, 0.8f, 0.4f);
                for (int i = 0; i < 100; i++)
                {
                    float speed = 18f * (1f - 1 / rand.NextFloat(1f, 10f));

                    Color color = Color.Lerp(Color.White, yellow, rand.NextFloat(0, 1));
                    var state = new ParticleState()
                    {
                        Velocity = rand.NextVector2(speed, speed),
                        Type = ParticleType.None,
                        LengthMultiplier = 1
                    };

                    GameRoot.ParticleManager.CreateParticle(Art.LineParticle, gesture.Position, color, 190, new Vector2(1.5f, 1.5f), state);
                }

            }
        }

        public void HandleTouchInput()
        {
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();

                if (GameRoot.ScreenSize.X < GameRoot.VirtualScreenSize.X)
                {
                    tempScale.X = GameRoot.VirtualScreenSize.X / GameRoot.ScreenSize.X;
                    tempScale.Y = GameRoot.VirtualScreenSize.Y / GameRoot.ScreenSize.Y;
                }
                if (GameRoot.ScreenSize.Y < GameRoot.VirtualScreenSize.Y)
                {
                    tempScale.X = GameRoot.VirtualScreenSize.X / GameRoot.ScreenSize.X;
                    tempScale.Y = GameRoot.VirtualScreenSize.Y / GameRoot.ScreenSize.Y;
                }

                if (GameRoot.ScreenSize.X > GameRoot.VirtualScreenSize.X)
                {
                    tempScale.X = GameRoot.ScreenSize.X / GameRoot.VirtualScreenSize.X;
                    tempScale.Y = GameRoot.ScreenSize.Y / GameRoot.VirtualScreenSize.Y;
                }

                if (GameRoot.ScreenSize.Y > GameRoot.VirtualScreenSize.Y)
                {
                    tempScale.X = GameRoot.ScreenSize.X / GameRoot.VirtualScreenSize.X;
                    tempScale.Y = GameRoot.ScreenSize.Y / GameRoot.VirtualScreenSize.Y;
                }

                for (int i = 0; i < buttonList.Count; i++)
			    {		
                    if ((gesture.Position.X * tempScale.X > buttonList[i].Position.X && gesture.Position.X * tempScale.X < buttonList[i].Position.X + buttonList[i].texture.Width &&
                        gesture.Position.Y *tempScale.Y > buttonList[i].Position.Y && gesture.Position.Y * tempScale.Y < buttonList[i].Position.Y + buttonList[i].texture.Height))
                    {
                        if (buttonList[i].bgameState == Button.bGameState.ingame)
                        {
                            gameState = GameState.ingame;
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