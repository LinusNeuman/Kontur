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

        JoystickManager joystickMgr;

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
            for (int i = 0; i < 1200; i++)
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
                }
                return;
            }


            const float speed = 8;
            Velocity = speed * Input.GetMovementDirection();
            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);

            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();


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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {


            joystickMgr.Draw(spriteBatch);

            if(!IsDead)
                base.Draw(spriteBatch);
        }
    }
}