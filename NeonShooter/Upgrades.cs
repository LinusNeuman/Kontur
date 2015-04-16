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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using BloomPostprocess;

namespace NeonShooter
{
    public class PreviewShip
    {
        public Model model;
        public Vector3 modelPosition = new Vector3(0f, 0f, 0f);
        public float modelRotation = 0.0f;
    }

    public class Upgrades
    {
        #region Textures

        public static Model Model_Damage { get; private set; }
        public static Model Model_Speed { get; private set; }
        public static Model Model_Standard { get; private set; }
        public static Model Model_Tank { get; private set; }
        public static Texture2D BgTxt { get; private set; }
        public static Texture2D BuyButtonTxt { get; private set; }
        public static Texture2D CreditsButtonTxt { get; private set; }
        public static Texture2D GoBackButtonTxt { get; private set; }
        public static Texture2D LeftArrowButtonTxt { get; private set; }
        public static Texture2D RightArrowButtonTxt { get; private set; }
        public static Texture2D PlayButtonTxt { get; private set; }
        public static Texture2D DamageShipInfoTxt { get; private set; }

        #endregion

        static Random rand = new Random();
        

        Vector3 cameraPosition = new Vector3(0f, 0.5f, 1.5f);

        List<Button> buttonList = new List<Button>();

        public static PreviewShip previewShip;

        int selectedShip = 2;

        public Upgrades()
        {
            buttonList.Add(new Button()
            {
                texture = GoBackButtonTxt,
                Position = new Vector2(0 + 40, GameRoot.VirtualScreenSize.Y - GoBackButtonTxt.Height - 30),
                bgameState = NeonShooter.Button.bGameState.menu,
            });

            buttonList.Add(new Button()
            {
                texture = PlayButtonTxt,
                Position = new Vector2(GameRoot.VirtualScreenSize.X - PlayButtonTxt.Width - 40, GameRoot.VirtualScreenSize.Y - PlayButtonTxt.Height - 30),
                bgameState = NeonShooter.Button.bGameState.ingame,
            });

            buttonList.Add(new Button()
            {
                texture = BuyButtonTxt,
                Position = new Vector2(0 + 1035, GameRoot.VirtualScreenSize.Y - BuyButtonTxt.Height - 27),
                bgameState = NeonShooter.Button.bGameState.none,
            });

            buttonList.Add(new Button()
            {
                texture = LeftArrowButtonTxt,
                Position = new Vector2(0 + 345, GameRoot.VirtualScreenSize.Y - LeftArrowButtonTxt.Height - 400),
                bgameState = NeonShooter.Button.bGameState.none,
            });

            buttonList.Add(new Button()
            {
                texture = RightArrowButtonTxt,
                Position = new Vector2(GameRoot.VirtualScreenSize.X - 680, GameRoot.VirtualScreenSize.Y - RightArrowButtonTxt.Height - 400),
                bgameState = NeonShooter.Button.bGameState.none,
            });
        }

        public static void Load(ContentManager content)
        {
            Model_Damage = content.Load<Model>("3D/damage");
            Model_Speed = content.Load<Model>("3D/speed");
            Model_Standard = content.Load<Model>("3D/standard");
            Model_Tank = content.Load<Model>("3D/tank");

            previewShip = new PreviewShip();
            previewShip.model = Model_Standard;
        }

        public static void LoadButtons(ContentManager content)
        {
            BgTxt = content.Load<Texture2D>("Graphics/UpgradesMenuBG");
            BuyButtonTxt = content.Load<Texture2D>("Upgrades/Buy");
            CreditsButtonTxt = content.Load<Texture2D>("Upgrades/Credits");
            GoBackButtonTxt = content.Load<Texture2D>("Upgrades/GoBack");
            PlayButtonTxt = content.Load<Texture2D>("Upgrades/PlayUpg");
            LeftArrowButtonTxt = content.Load<Texture2D>("Upgrades/LeftArrow");
            RightArrowButtonTxt = content.Load<Texture2D>("Upgrades/RightArrow");
            DamageShipInfoTxt = content.Load<Texture2D>("Upgrades/DamageShipInfo");
        }

        public void Update(GameTime gameTime, ContentManager Content)
        {
            if (selectedShip == 0)
            {
                previewShip.model = Model_Speed;
            }
            if (selectedShip == 1)
            {
                previewShip.model = Model_Tank;
            }
            if(selectedShip == 2)
            {
                previewShip.model = Model_Standard;
            }
            if(selectedShip == 3)
            {
                previewShip.model = Model_Damage;
            }

            previewShip.modelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds *
                MathHelper.ToRadians(0.05f);

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

                        if (buttonList[i].bgameState == Button.bGameState.ingame)
                        {
                            for (int g = 0; g < buttonList.Count; g++)
                            {
                                buttonList[g].texture.Dispose();
                            }
                            
                            
                            Sound.MainTheme.Dispose();
                            Sound.Load(Content);
                            Bullet.Load(Content);
                            BlackHole.Load(Content);
                            PlayerStatus.selectedShip = selectedShip;
                            PlayerShip.Load(Content);
                            EntityManager.Add(PlayerShip.Instance);
                            EnemySpawner.Load(Content);

                            MediaPlayer.Volume = 0.8f;
                            MediaPlayer.Play(Sound.Music);
                            
                            MediaPlayer.IsRepeating = true;

                            int f = Array.FindIndex(BloomSettings.PresetSettings, row => row.Name == "Default");
                            GameRoot.Instance.bloom.Settings = BloomSettings.PresetSettings[f];

             

                            Menu.gameState = Menu.GameState.ingame;
                        }

                        if(buttonList[i].texture == LeftArrowButtonTxt)
                        {
                            if (selectedShip > 0)
                            {
                                selectedShip -= 1;
                            }
                        }

                        if (buttonList[i].texture == RightArrowButtonTxt)
                        {
                            if (selectedShip < 3)
                            {
                                selectedShip += 1;
                            }
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

        public void DrawBG(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BgTxt, Vector2.Zero, Color.White);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            
            Matrix[] transforms = new Matrix[previewShip.model.Bones.Count];
            previewShip.model.CopyAbsoluteBoneTransformsTo(transforms);


            foreach (ModelMesh mesh in previewShip.model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateRotationY(previewShip.modelRotation)
                        * Matrix.CreateTranslation(previewShip.modelPosition);
                    effect.View = Matrix.CreateLookAt(cameraPosition,
                        Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(55.0f), GameRoot.aspectRatio,
                        0.01f, 10000.0f);
                }

                mesh.Draw();
            }

            for (int i = 0; i < buttonList.Count; i++)
            {
                spriteBatch.Draw(buttonList[i].texture, buttonList[i].Position, Color.White);
            }

            spriteBatch.Draw(CreditsButtonTxt, new Vector2(0+721,971), Color.White);
            spriteBatch.Draw(DamageShipInfoTxt, new Vector2(0+ 721, 682), Color.White);

        }
    }
}