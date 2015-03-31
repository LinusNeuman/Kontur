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

        Matrix SpriteScale;

        BloomComponent bloom;

        public static ParticleManager<ParticleState> ParticleManager { get; private set; }

        public static GameRoot Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static Vector2 VirtualScreenSize { get { return new Vector2(1920, 1080); } }
        public static GameTime GameTime { get; private set; }

        Menu menu;

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

            EntityManager.Add(PlayerShip.Instance);

            AchievementManager.pInstance.Initialize();

            const int maxGridPoints = 1600;
            Vector2 gridSpacing = new Vector2((float)Math.Sqrt(Viewport.Width * Viewport.Height / maxGridPoints));

            float screenscaleX =
                (((float)ScreenSize.X / VirtualScreenSize.X));
            float screenscaleY =
                (((float)ScreenSize.Y / VirtualScreenSize.Y));
            SpriteScale = Matrix.CreateScale(screenscaleX, screenscaleY, 1);

            
            MediaPlayer.Play(Sound.Music);
            MediaPlayer.IsRepeating = true;

            ParticleManager = new ParticleManager<ParticleState>(1024 * 20, ParticleState.UpdateParticle);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("Fonts/spriteFont1");

            Art.Load(Content);
            Sound.Load(Content);

            menu = new Menu();
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
                            NeonShooter.Activity1 activity = GameRoot.Activity as NeonShooter.Activity1;
                            if (activity.pGooglePlayClient.IsConnected)
                                activity.StartActivityForResult(GamesClass.Achievements.GetAchievementsIntent(activity.pGooglePlayClient), Activity1.REQUEST_ACHIEVEMENTS);
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
                        menu.Update();
                    }
                    break;

                case Menu.GameState.gameover:
                    {

                    }
                    break;

                case Menu.GameState.pause:
                    {

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
                        Grid.Draw(spriteBatch);
                    }
                    break;

                case Menu.GameState.menu:
                    {

                    }
                    break;

                case Menu.GameState.gameover:
                    {
                        
                    }
                    break;

                case Menu.GameState.pause:
                    {

                    }
                    break;
            }
           
            
<<<<<<< HEAD
            //spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            spriteBatch.Draw(Art.TitleScreenBg, Vector2.Zero, Color.White);
            EntityManager.Draw(spriteBatch);
            ParticleManager.Draw(spriteBatch);
=======
>>>>>>> ed7d7381c81270b70e2d0dd09b6b079625219021
            
            spriteBatch.End();
            
            base.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive,
null, null, null, null, SpriteScale);

            switch (Menu.gameState)
            {
                case Menu.GameState.ingame:
                    {
                        PlayerShip.Instance.joystickMgr.Draw(spriteBatch);

                        spriteBatch.DrawString(Art.Font, "Lives: " + PlayerStatus.Lives, new Vector2(5), Color.White);
                        DrawRightAlignedString("Score: " + PlayerStatus.Score, 5);
                        DrawRightAlignedString("Multiplier: " + PlayerStatus.Multiplier, 35 + font.MeasureString("Score: ").Y);
                    }
                    break;

                case Menu.GameState.menu:
                    {
                        
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

                    }
                    break;
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
