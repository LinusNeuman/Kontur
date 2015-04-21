using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

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
// test for in app billing

namespace NeonShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameRoot : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        SpriteFont fpsFont;


        Matrix SpriteScale;

        public BloomComponent bloom;

        public static ParticleManager<ParticleState> ParticleManager { get; private set; }
        public static ParticleManager<ParticleState> ParticleManager2 { get; private set; }

        public static GameRoot Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static Vector2 VirtualScreenSize { get { return new Vector2(1920, 1080); } }
        public static GameTime GameTime { get; private set; }
        public static Vector2 tempScale;
        public static float aspectRatio;

        Menu menu;
        Upgrades upgrades;
        Pause pause;

        private FrameCounter _frameCounter = new FrameCounter();
        private bool showFPS = true;

        public GameRoot()
        {

            graphics = new GraphicsDeviceManager(this);



            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft; // | DisplayOrientation.LandscapeRight;

            Instance = this;

            this.IsFixedTimeStep = true;

            bloom = new BloomComponent(this);
            Components.Add(bloom);
            //bloom.Settings = new BloomSettings(null, 0.25f, 4, 2, 1, 1.5f, 1);
            int i = Array.FindIndex(BloomSettings.PresetSettings, row => row.Name == "Default");
            bloom.Settings = BloomSettings.PresetSettings[i];
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            AchievementManager.pInstance.Initialize();

            float screenscaleX =
                (((float)ScreenSize.X / VirtualScreenSize.X));
            float screenscaleY =
                (((float)ScreenSize.Y / VirtualScreenSize.Y));
            SpriteScale = Matrix.CreateScale(screenscaleX, screenscaleY, 1);

            ParticleManager = new ParticleManager<ParticleState>(1024 * 20, ParticleState.UpdateParticle);
            ParticleManager2 = new ParticleManager<ParticleState>(1024 * 20, ParticleState.UpdateParticle);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("Fonts/spriteFont1");
            fpsFont = Content.Load<SpriteFont>("Fonts/FPSFont");

            Art.Load(Content);
            Sound.LoadTheme(Content);
            Menu.Load(Content);
            Pause.Load(Content);
            JoystickManager.Load(Content);
            JoystickManager.Initialize();
            
            menu = new Menu();
            upgrades = new Upgrades();
            pause = new Pause();
            UpdateTempScale();


        }

        public void UpdateTempScale()
        {
            if (GameRoot.ScreenSize.X == GameRoot.VirtualScreenSize.X)
            {
                tempScale.X = 1;
            }
            if (GameRoot.ScreenSize.Y == GameRoot.VirtualScreenSize.Y)
            {
                tempScale.Y = 1;
            }
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
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime;

            switch (Menu.gameState)
            {
                case Menu.GameState.ingame:
                    {

                        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        {
                            MediaPlayer.Pause();
                            Menu.gameState = Menu.GameState.pause;

                            
                        }

                        Input.Update();

                        EnemySpawner.Update();

                        PlayerStatus.Update(); // do not update when paused

                        EntityManager.Update();

                        ParticleManager.Update();

                    }
                    break;

                case Menu.GameState.menu:
                    {
                        menu.Update(Content);

                        ParticleManager2.Update();
                    }
                    break;

                case Menu.GameState.gameover:
                    {

                    }
                    break;

                case Menu.GameState.pause:
                    {
                        pause.Update(Content);
                        ParticleManager2.Update();
                    }
                    break;


                case Menu.GameState.upgrades:
                    {
                        upgrades.Update(gameTime, Content);

                        ParticleManager2.Update();
                    }
                    break;

        }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            bloom.BeginDraw();

            
            graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, 
                null, null, null, null, SpriteScale);

            switch (Menu.gameState)
            {
                case Menu.GameState.ingame:
                    {
                        spriteBatch.Draw(Art.TitleScreenBg, Vector2.Zero, Color.White);
                        EntityManager.Draw(spriteBatch);
                        ParticleManager.Draw(spriteBatch);
                        PlayerShip.Instance.joystickMgr.DrawSight(spriteBatch);
                        
                    }
                    break;

                case Menu.GameState.menu:
                    {
                        ParticleManager2.Draw(spriteBatch);
                    }
                    break;

                case Menu.GameState.gameover:
                    {
                        
                    }
                    break;

                case Menu.GameState.pause:
                    {
                        spriteBatch.Draw(Art.TitleScreenBg, Vector2.Zero, Color.White);
                        EntityManager.Draw(spriteBatch);
                        ParticleManager.Draw(spriteBatch);
                        ParticleManager2.Draw(spriteBatch);
                    }
                    break;

                case Menu.GameState.upgrades:
                    {
                        upgrades.Draw(spriteBatch);
  
                        ParticleManager2.Draw(spriteBatch);
                    }
                    break;
            }
           
            

            
            spriteBatch.End();
            
            base.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive,
    null, DepthStencilState.None, null, null, SpriteScale);

            switch (Menu.gameState)
            {
                case Menu.GameState.ingame:
                    {
                        PlayerShip.Instance.joystickMgr.Draw(spriteBatch);

                        spriteBatch.DrawString(Art.Font, "Lives: " + PlayerStatus.Lives, new Vector2(5), Color.White);
                        DrawRightAlignedString("Score: " + PlayerStatus.Score, 5);
                        DrawRightAlignedString("Multiplier: " + PlayerStatus.Multiplier, 35 + font.MeasureString("Score: ").Y);

                        if (PlayerStatus.IsGameOver)
                        {
                            string text = "Game Over\n" +
                               "Your Score: " + PlayerStatus.Score + "\n" +
                               "High Score: " + PlayerStatus.HighScore;

                            Vector2 textSize = Art.Font.MeasureString(text);
                            spriteBatch.DrawString(Art.Font, text, VirtualScreenSize / 2 - textSize / 2, Color.White);
                        }
                    }
                    break;

                case Menu.GameState.menu:
                    {
                        menu.Draw(spriteBatch);
                    }
                    break;

                case Menu.GameState.upgrades:
                    {
                        upgrades.DrawBG(spriteBatch);
                    }
                    break;

                case Menu.GameState.gameover:
                    {
                        string text = "Game Over\n" +
                            "Your Score: " + PlayerStatus.Score + "\n" +
                            "High Score: " + PlayerStatus.HighScore;

                        Vector2 textSize = Art.Font.MeasureString(text);
                        spriteBatch.DrawString(Art.Font, text, VirtualScreenSize / 2 - textSize / 2, Color.White);
                    }
                    break;

                case Menu.GameState.pause:
                    {
                        spriteBatch.DrawString(Art.Font, "Lives: " + PlayerStatus.Lives, new Vector2(5), Color.White);
                        DrawRightAlignedString("Score: " + PlayerStatus.Score, 5);
                        DrawRightAlignedString("Multiplier: " + PlayerStatus.Multiplier, 35 + font.MeasureString("Score: ").Y);
                        pause.Draw(spriteBatch);
                    }
                    break;
            }


            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _frameCounter.Update(deltaTime);

            var fps = string.Format("{0} Current FPS", _frameCounter.CurrentFramesPerSecond);
            var avgfps = string.Format("{0} Average FPS", _frameCounter.AverageFramesPerSecond);

            if (showFPS)
            {
                spriteBatch.DrawString(fpsFont, fps, new Vector2(0, 40), Color.White);
                spriteBatch.DrawString(fpsFont, avgfps, new Vector2(0,80), Color.White);
            }

            spriteBatch.End();
        }

        private void DrawRightAlignedString(string text, float y)
        {
            var textWidth = Art.Font.MeasureString(text).X;
            spriteBatch.DrawString(Art.Font, text, new Vector2(VirtualScreenSize.X - textWidth - 5, y), Color.White);
        }
    }
}
