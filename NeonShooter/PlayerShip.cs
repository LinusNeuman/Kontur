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
    class PlayerShip : Entity
    {
        #region Textures

        public static Texture2D PlayerDmgShip { get; private set; }
        public static Texture2D PlayerSpdShip { get; private set; }
        public static Texture2D PlayerStndShip { get; private set; }
        public static Texture2D PlayerTnkShip { get; private set; }

        public static Texture2D Pixel { get; private set; }


        #endregion

        public static float cooldownFrames = 8;
        public int cooldownRemaining = 0;
        static Random rand = new Random();

        public int framesUntilRespawn = 0;
        public bool IsDead { get { return framesUntilRespawn > 0; } }

        public static float playerSpeed;
        public static float playerAccuracy;

        private static PlayerShip instance;
        public static PlayerShip Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerShip();

                return instance;
            }
        }

        public JoystickManager joystickMgr;

        private PlayerShip()
        {
            image = PlayerStndShip;

            Position = GameRoot.VirtualScreenSize / 2;
            Radius = 15;

            joystickMgr = new JoystickManager();
        }

        public static void Initialize()
        {
            
        }

        public static void SetStatsAndSpec()
        {
            if (PlayerStatus.selectedShip == 0)
            {
                PlayerShip.Instance.image = PlayerSpdShip;
                playerSpeed = 14 * 1.3f;
                cooldownFrames = 8 * 0.7f;
                playerAccuracy = 0.12f; //0.06
            }
            if (PlayerStatus.selectedShip == 1)
            {
                PlayerShip.Instance.image = PlayerTnkShip;
                playerSpeed = 14 * 0.7f;
                cooldownFrames = 8 * 1.3f;
                playerAccuracy = 0.01f;
            }
            if (PlayerStatus.selectedShip == 2)
            {
                PlayerShip.Instance.image = PlayerStndShip;
                playerSpeed = 14 * 1f;
                cooldownFrames = 8 * 1f;
                playerAccuracy = 0.06f;
            }
            if (PlayerStatus.selectedShip == 3)
            {
                PlayerShip.Instance.image = PlayerDmgShip;
                playerSpeed = 14 * 1f;
                cooldownFrames = 8 * 0.9f;
                playerAccuracy = 0.08f;
            }
        }

        public static void Load(ContentManager content)
        {
            PlayerDmgShip = content.Load<Texture2D>("Player/DamageShip");
            PlayerSpdShip = content.Load<Texture2D>("Player/SpeedShip");
            PlayerStndShip = content.Load<Texture2D>("Player/StandardShip");
            PlayerTnkShip = content.Load<Texture2D>("Player/TankShip");

            //go back here to maybe fix with other ships
            Pixel = new Texture2D(PlayerDmgShip.GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });

            if (PlayerStatus.selectedShip == 0)
            {
                PlayerShip.Instance.image = PlayerSpdShip;
            }
            if (PlayerStatus.selectedShip == 1)
            {
                PlayerShip.Instance.image = PlayerTnkShip;
            }
            if (PlayerStatus.selectedShip == 2)
            {
                PlayerShip.Instance.image = PlayerStndShip;
            }
            if (PlayerStatus.selectedShip == 3)
            {
                PlayerShip.Instance.image = PlayerDmgShip;
            }
        }

        public void ResetGame()
        {
            PlayerStatus.Reset();
            Position = GameRoot.VirtualScreenSize / 2;
        }

        public void Kill()
        {
            PlayerStatus.RemoveLife();
            joystickMgr.Reset();
            framesUntilRespawn = PlayerStatus.IsGameOver ? 300 : 120;
            JoystickManager.noDirection = true;

            

            Color yellow = new Color(0.8f, 0.8f, 0.4f);
            for (int i = 0; i < 30; i++)
            {
                float speed = 10f * (1f - 1 / rand.NextFloat(1f, 10f));
                var state = new ParticleState()
                {
                    Velocity = rand.NextVector2(1, 7),
                    Type = ParticleType.None,
                    LengthMultiplier = 1f
                };

                Color color = Color.Lerp(yellow, yellow, rand.NextFloat(0, 1));
                GameRoot.ParticleManager.CreateParticle(Art.LineParticle2, Position, color, 190, new Vector2(1.5f, 1.5f), state);
            }
            Position = GameRoot.VirtualScreenSize / 2;

        }

        public override void Update()
        {
            if (IsDead)
            {
                
                if (--framesUntilRespawn == 0)
                {
                    if (PlayerStatus.Lives == 0)
                    {
                        PlayerStatus.Reset();
                        Position = GameRoot.VirtualScreenSize / 2;
                    }
                    JoystickManager.noDirection = false;
                    return;
                }

                
            }

            if (PlayerStatus.IsGameOver)
            {
                //Menu.gameState = Menu.GameState.gameover;
            }

            joystickMgr.Update();

            var aim = Input.GetAimDirection(); // get aim
            if (aim.LengthSquared() > 0 && cooldownRemaining <= 0 && !IsDead)
            {
                cooldownRemaining = (int)cooldownFrames;
                float aimAngle = aim.ToAngle();
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                float randomSpread = rand.NextFloat(-playerAccuracy, playerAccuracy) + rand.NextFloat(-playerAccuracy, playerAccuracy);
                Vector2 vel = MathUtil.FromPolar(aimAngle + randomSpread, 20f);




                if (PlayerStatus.appliedEffects.Exists(ae => PlayerStatus.FindAE(ae, 1)))
                {
                    PlayerStatus.doubleBullets = true;
                }
                else
                {
                    PlayerStatus.doubleBullets = false;
                }

                if (PlayerStatus.doubleBullets == false)
                {
                    //Vector2 offset = Vector2.Transform(new Vector2(25, -16), aimQuat);
                    Vector2 offset = Vector2.Zero;
                    EntityManager.Add(new Bullet(Position + offset, vel));

                    //offset= Vector2.Transform(new Vector2(25, 16), aimQuat);
                    //EntityManager.Add(new Bullet(Position + offset, vel));
                }
                if(PlayerStatus.doubleBullets == true)
                {
                    Vector2 offset = Vector2.Transform(new Vector2(25, -16), aimQuat);
                    EntityManager.Add(new Bullet(Position + offset, vel));

                    offset= Vector2.Transform(new Vector2(25, 16), aimQuat);
                    EntityManager.Add(new Bullet(Position + offset, vel));
                }

                Sound.Shot.Play(0.4f, rand.NextFloat(-0.2f, 0.2f), 0); // change pitch
            }
            if (cooldownRemaining > 0)
                cooldownRemaining--;

            
            Velocity = Input.GetMovementDirection();
            Position += Velocity;
            if(Position.X + image.Width < 0)
            {
                Position.X = 1920;
            }
            if (Position.X - image.Width >= 1920)
            {
                Position.X = 0;
            }

            if(Position.Y + image.Height < 0)
            {
                Position.Y = 1080;
            }
            if(Position.Y - image.Height > 1080)
            {
                Position.Y = 0;
            }

            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            //MakeExhaustFire();
            Velocity = Vector2.Zero;
        }

        //private void MakeExhaustFire()
        //{
        //    if (Velocity.LengthSquared() > 0.1f)
        //    {
        //        // new variables
        //        Orientation = Velocity.ToAngle();
        //        Quaternion rot = Quaternion.CreateFromYawPitchRoll(0f, 0f, Orientation);

        //        double t = GameRoot.GameTime.TotalGameTime.TotalSeconds;
        //        // primary velocity is 3 pixels/frame direction = opposite of ship
        //        Vector2 baseVel = Velocity.ScaleTo(-3);
        //        // calculate sideways velocity for two sidestreams.
        //        Vector2 perpVel = new Vector2(baseVel.Y, -baseVel.X) * (0.6f * (float)Math.Sin(t * 10));
        //        Color sideColor = new Color(200, 38, 9);
        //        Color midColor = new Color(255, 187, 30);
        //        Vector2 pos = Position + Vector2.Transform(new Vector2(-25, 0), rot);
        //        const float alpha = 0.7f;

        //        //mid pfx stream
        //        Vector2 velMid = baseVel + rand.NextVector2(0, 1);
        //        GameRoot.ParticleManager.CreateParticle(Art.LineParticle, pos, Color.White * alpha, 60f, new Vector2(0.5f, 1),
        //            new ParticleState(velMid, ParticleType.Enemy));
        //        GameRoot.ParticleManager.CreateParticle(Art.Glow, pos, midColor * alpha, 60f, new Vector2(0.5f, 1),
        //            new ParticleState(velMid, ParticleType.Enemy));

        //        // side pfx stream
        //        Vector2 vel1 = baseVel + perpVel + rand.NextVector2(0, 0.3f);
        //        Vector2 vel2 = baseVel - perpVel + rand.NextVector2(0, 0.3f);
        //        GameRoot.ParticleManager.CreateParticle(Art.LineParticle, pos, Color.White * alpha, 60f, new Vector2(0.5f, 1),
        //            new ParticleState(vel1, ParticleType.Enemy));
        //        GameRoot.ParticleManager.CreateParticle(Art.LineParticle, pos, Color.White * alpha, 60f, new Vector2(0.5f, 1),
        //            new ParticleState(vel2, ParticleType.Enemy));

        //        GameRoot.ParticleManager.CreateParticle(Art.Glow, pos, sideColor * alpha, 60f, new Vector2(0.5f, 1),
        //            new ParticleState(vel1, ParticleType.Enemy));
        //        GameRoot.ParticleManager.CreateParticle(Art.Glow, pos, sideColor * alpha, 60f, new Vector2(0.5f, 1),
        //            new ParticleState(vel2, ParticleType.Enemy));
        //    }       
        //}

        public override void Draw(SpriteBatch spriteBatch)
        {

            
            //joystickMgr.Draw(spriteBatch);

            if(!IsDead)
                base.Draw(spriteBatch);
        }
    }
}