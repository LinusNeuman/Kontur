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

namespace NeonShooter
{
    class PlayerShip : Entity
    {
        const int cooldownFrames = 8;
        public int cooldownRemaining = 0;
        static Random rand = new Random();

        int framesUntilRespawn = 0;
        public bool IsDead { get { return framesUntilRespawn > 0; } }

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
            image = Art.PlayerDmgShip;
            Position = GameRoot.ScreenSize / 2;
            Radius = 10;

            joystickMgr = new JoystickManager();
        }

        public void Kill()
        {
            PlayerStatus.RemoveLife();
            joystickMgr.Reset();
            framesUntilRespawn = PlayerStatus.IsGameOver ? 300 : 120;
            

            Color yellow = new Color(0.8f, 0.8f, 0.4f);
            for (int i = 0; i < 200; i++)
            {
                float speed = 18f * (1f - 1 / rand.NextFloat(1f, 10f));

                Color color = Color.Lerp(Color.White, yellow, rand.NextFloat(0, 1));
                var state = new ParticleState()
                {
                    Velocity = rand.NextVector2(speed, speed),
                    Type = ParticleType.None,
                    LengthMultiplier = 1
                };

                GameRoot.ParticleManager.CreateParticle(Art.LineParticle, Position, color, 190, new Vector2(1.5f, 1.5f), state);
            }

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
                        Position = GameRoot.ScreenSize / 2;
                    }
                    GameRoot.Grid.ApplyDirectedForce(new Vector3(0, 0, 5000), new Vector3(Position, 0), 50);
                    return;
                }

                
            }

            joystickMgr.Update();

            var aim = Input.GetAimDirection(); // get aim
            if (aim.LengthSquared() > 0 && cooldownRemaining <= 0)
            {
                cooldownRemaining = cooldownFrames;
                float aimAngle = aim.ToAngle();
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                float randomSpread = rand.NextFloat(-0.04f, 0.04f) + rand.NextFloat(-0.04f, 0.04f);
                Vector2 vel = MathUtil.FromPolar(aimAngle + randomSpread, 11f);

                Vector2 offset = Vector2.Transform(new Vector2(25, -16), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, vel));

                offset= Vector2.Transform(new Vector2(25, 16), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, vel));

                Sound.Shot.Play(0.2f, rand.NextFloat(-0.2f, 0.2f), 0); // change pitch
            }
            if (cooldownRemaining > 0)
                cooldownRemaining--;

            const float speed = 7;
            Velocity = speed * Input.GetMovementDirection();
            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);

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