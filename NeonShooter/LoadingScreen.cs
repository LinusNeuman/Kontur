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
using Microsoft.Xna.Framework.Content;
// test for in app billing

namespace NeonShooter
{
    public class LoadingScreen
    {
        public static Texture2D LoadTxt;

        public static bool hasStartedGameLoad;

        public static int timer = 0;

        public LoadingScreen()
        {
        }


        public static void Load(ContentManager Content)
        {
            LoadTxt = Content.Load<Texture2D>("Graphics/Loading");
            
        }

        public static void LoadTheGame(ContentManager Content)
        {
            GameRoot.Instance.font = Content.Load<SpriteFont>("Fonts/spriteFont1");
            GameRoot.Instance.fpsFont = Content.Load<SpriteFont>("Fonts/FPSFont");

            Art.Load(Content);
            Sound.LoadTheme(Content);
            Menu.Load(Content);
            Pause.Load(Content);
            About.Load(Content);
            JoystickManager.Load(Content);
            JoystickManager.Initialize();
            GameOver.Load(Content);

            GameRoot.Instance.about = new About();
            GameRoot.Instance.gameOver = new GameOver();
            GameRoot.Instance.menu = new Menu();
            GameRoot.Instance.upgrades = new Upgrades();
            GameRoot.Instance.pause = new Pause();
            GameRoot.UpdateTempScale();

            Bullet.Load(Content);

            PlayerShip.Load(Content);
            EnemySpawner.Load(Content);
            Upgrades.LoadButtons(Content);



            Upgrades.Load(Content);
            Upgrades.LoadButtons(Content);
            Upgrades.ReloadButtons();

            PlayerShip.Load(Content);
            EnemySpawner.Load(Content);
            Sound.Load(Content);
            Bullet.Load(Content);

            GameRoot.Instance.loadingGame = false;
            hasStartedGameLoad = false;

            NeonShooter.Activity1 activity = GameRoot.Activity as NeonShooter.Activity1;
            activity.ConnectP();
        }

        public static void Update(ContentManager Content)
        {

            if(GameRoot.Instance.loadingLoad == false && GameRoot.Instance.loadingGame == true && hasStartedGameLoad == false)
            {
                timer++;
                if(timer >= 100)
                {
                LoadTheGame(Content);
                hasStartedGameLoad = true;
                timer = 0;
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(LoadTxt, Vector2.Zero, Color.White);
        }
    }
}