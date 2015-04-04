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

namespace NeonShooter
{
    public class Upgrades
    {
        #region Textures

        public static Model PreviewShip { get; private set; }
        public static Texture2D BgTxt { get; private set; }

        #endregion

        static Random rand = new Random();

        Vector3 modelPosition = new Vector3(-0.6f, 0, 0f);
        float modelRotation = 0.0f;

        Vector3 cameraPosition = new Vector3(0f, 0.5f, 1.5f);

        List<Button> buttonList = new List<Button>();

        public Upgrades()
        {
            //buttonList.Add(new Button()
            //{
            //    texture = PlayTxt,
            //    Position = new Vector2(GameRoot.VirtualScreenSize.X - PlayTxt.Width - 140, GameRoot.VirtualScreenSize.Y - PlayTxt.Height - 680),
            //    bgameState = NeonShooter.Button.bGameState.upgrades,
            //});
        }

        public static void Load(ContentManager content)
        {
            PreviewShip = content.Load<Model>("Player/3Ddmg");
            BgTxt = content.Load<Texture2D>("Graphics/UpgradesMenuBG");

        }

        public void Update(GameTime gameTime, ContentManager Content)
        {
            modelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds *
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

                    }
                    else
                    {
                        if (gesture.GestureType == GestureType.Tap)
                        {
                            Color yellow = new Color(47, 206, 251);
                            //Color color2 = ColorUtil.HSVToColor(5, 0.5f, 0.8f);

                            for (int o = 0; o < 5; o++)
                            {
                                float speed = 8f * (1f - 1 / rand.NextFloat(1f, 10f));

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
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BgTxt, Vector2.Zero, Color.White);

            Matrix[] transforms = new Matrix[PreviewShip.Bones.Count];
            PreviewShip.CopyAbsoluteBoneTransformsTo(transforms);


            foreach (ModelMesh mesh in PreviewShip.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateRotationY(modelRotation)
                        * Matrix.CreateTranslation(modelPosition);
                    effect.View = Matrix.CreateLookAt(cameraPosition,
                        Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(55.0f), GameRoot.aspectRatio,
                        0.01f, 10000.0f);
                    effect.DiffuseColor = Color.Red.ToVector3();
                    effect.AmbientLightColor = Color.Gray.ToVector3();
                }

                mesh.Draw();
            }

        }
    }
}