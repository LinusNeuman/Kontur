using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

using BloomPostprocess;

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


            float screenscaleX =
                (((float)graphics.PreferredBackBufferWidth / 1920));
            float screenscaleY =
                (((float)graphics.PreferredBackBufferHeight / 1080));
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
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime;

            Input.Update();

            EnemySpawner.Update();

            PlayerStatus.Update(); // do not update when paused

            EntityManager.Update();

            ParticleManager.Update();

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
            //spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            spriteBatch.Draw(Art.TitleScreenBg, Vector2.Zero, Color.White);
            EntityManager.Draw(spriteBatch);
            ParticleManager.Draw(spriteBatch);
            

            spriteBatch.End();

            base.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive,
    null, null, null, null, SpriteScale);

            spriteBatch.DrawString(Art.Font, "Lives: " + PlayerStatus.Lives, new Vector2(5), Color.White);
            DrawRightAlignedString("Score: " + PlayerStatus.Score, 5);
            DrawRightAlignedString("Multiplier: " + PlayerStatus.Multiplier, 35 + font.MeasureString("Score: ").Y);

            if (PlayerStatus.IsGameOver)
            {
                string text = "Game Over\n" +
                    "Your Score: " + PlayerStatus.Score + "\n" +
                    "High Score: " + PlayerStatus.HighScore;

                Vector2 textSize = Art.Font.MeasureString(text);
                spriteBatch.DrawString(Art.Font, text, ScreenSize / 2 - textSize / 2, Color.White);
            }

            spriteBatch.End();
        }

        private void DrawRightAlignedString(string text, float y)
        {
            var textWidth = Art.Font.MeasureString(text).X;
            spriteBatch.DrawString(Art.Font, text, new Vector2(1920 - textWidth - 5, y), Color.White);
        }
    }
}
