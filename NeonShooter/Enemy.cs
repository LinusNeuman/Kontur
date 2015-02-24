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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NeonShooter
{
    class Enemy : Entity
    {
        public static Random rand = new Random();
        private int timeUntilStart = 60;
        public bool IsActive { get { return timeUntilStart <= 0; } }

        private List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>();

        public int PointValue { get; private set; }

        public Enemy(Texture2D image, Vector2 position)
        {
            this.image = image;
            Position = position;
            Radius = image.Width / 2f;
            color = Color.Transparent;
        }

        public void HandleCollision(Enemy other)
        {
            var d = Position - other.Position;
            Velocity += 10 * d / (d.LengthSquared() + 1);
        }

        public override void Update()
        {
            if (timeUntilStart <= 0)
            {
                ApplyBehaviours();
            }
            else
            {
                timeUntilStart--;
                color = Color.White * (1 - timeUntilStart / 60f);
            }

            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);

            Velocity *= 0.8f;
        }

        public void WasShot()
        {
            IsExpired = true;

            float hue1 = rand.NextFloat(0, 6);
            float hue2 = (hue1 + rand.NextFloat(0, 2)) % 6f;
            Color color1 = ColorUtil.HSVToColor(hue1, 0.5f, 1);
            Color color2 = ColorUtil.HSVToColor(hue2, 0.5f, 1);


            for (int i = 0; i < 60; i++)
            {
                float speed = 12f * (1f - 1 / rand.NextFloat(1f, 10f));
                var state = new ParticleState()
                {
                    Velocity = rand.NextVector2(speed,speed),
                    Type = ParticleType.Enemy,
                    LengthMultiplier = 1f
                };

                Color color = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
                GameRoot.ParticleManager.CreateParticle(Art.LineParticle, Position, color, 190, new Vector2(1.5f,1.5f), state);
            }

            PlayerStatus.AddPoints(PointValue);
            PlayerStatus.IncreaseMultiplier();

            Sound.Explosion.Play(0.1f, rand.NextFloat(-0.2f, 0.2f), 0);
        }

        public static Enemy CreateSeeker(Vector2 position)
        {
            var enemy = new Enemy(Art.Follower, position);
            enemy.AddBehaviour(enemy.FollowPlayer());
            enemy.PointValue = 2;

            Sound.Spawn.Play(0.1f, rand.NextFloat(-0.2f, 0.2f), 0);

            return enemy;
        }

        public static Enemy CreateWanderer(Vector2 position)
        {
            var enemy = new Enemy(Art.Wanderer_Part1, position);
            enemy.AddBehaviour(enemy.MoveRandomly());
            enemy.PointValue = 1;

            Sound.Spawn.Play(0.1f, rand.NextFloat(-0.2f, 0.2f), 0);

            return enemy;
        }

        private void AddBehaviour(IEnumerable<int> behaviour)
        {
            behaviours.Add(behaviour.GetEnumerator());
        }

        private void ApplyBehaviours()
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (!behaviours[i].MoveNext())
                    behaviours.RemoveAt(i--);
            }
        }

        IEnumerable<int> MoveRandomly()
        {
            float direction = rand.NextFloat(0, MathHelper.TwoPi);

            while (true)
            {
                direction += rand.NextFloat(-0.1f, 0.1f);
                direction = MathHelper.WrapAngle(direction);

                for (int i = 0; i < 6; i++)
                {
                    Velocity += MathUtil.FromPolar(direction, 0.4f);
                    Orientation -= 0.05f;

                    var bounds = GameRoot.Viewport.Bounds;
                    bounds.Inflate(-image.Width, -image.Width);

                    // move away from bounds
                    if (!bounds.Contains(Position.ToPoint()))
                        direction = (GameRoot.ScreenSize / 2 - Position).ToAngle() + rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);

                    yield return 0;
                }
            }
        }

        IEnumerable<int> FollowPlayer(float acceleration = 1f)
        {
            while (true)
            {
                Velocity += (PlayerShip.Instance.Position - Position).ScaleTo(acceleration);
                if (Velocity != Vector2.Zero)
                Orientation = Velocity.ToAngle();

                yield return 0;
            }
        }

        IEnumerable<int> MoveInASquare()
        {
            const int framesPerSide = 30;
            while (true)
            {
                // right for 30 frames
                for (int i = 0; i < framesPerSide; i++)
                {
                    Velocity = Vector2.UnitX;
                    yield return 0;
                }

                // down
                for (int i = 0; i < framesPerSide; i++)
                {
                    Velocity = Vector2.UnitY;
                    yield return 0;
                }

                // left
                for (int i = 0; i < framesPerSide; i++)
                {
                    Velocity = Vector2.UnitX;
                    yield return 0;
                }

                // up
                for (int i = 0; i < framesPerSide; i++)
                {
                    Velocity = Vector2.UnitY;
                    yield return 0;
                }
            }
        }
    }
}