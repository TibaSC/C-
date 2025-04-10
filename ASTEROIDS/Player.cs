using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class Player : GameObject
    {
        public float RotationSpeed = 5.0f;
        public float Acceleration = 200.0f;
        public float MaxSpeed = 300.0f;
        public float Friction = 0.98f;
        public int Lives = 3;
        public float ShootCooldown = 0.25f;
        public float CurrentCooldown = 0;

        public Player(Vector2 position, float radius) : base(position, radius)
        {
        }

        public override void Update(float deltaTime)
        {
            // Rotation
            if (Raylib.IsKeyDown(KeyboardKey.Left)) Rotation -= RotationSpeed * deltaTime;
            if (Raylib.IsKeyDown(KeyboardKey.Right)) Rotation += RotationSpeed * deltaTime;

            // Thrust
            if (Raylib.IsKeyDown(KeyboardKey.Up))
            {
                Vector2 direction = new Vector2(
                    (float)Math.Sin(Rotation),
                    -(float)Math.Cos(Rotation)
                );

                Velocity += direction * Acceleration * deltaTime;

                float currentSpeed = Vector2.Distance(Vector2.Zero, Velocity);
                if (currentSpeed > MaxSpeed)
                {
                    Velocity = Vector2.Normalize(Velocity) * MaxSpeed;
                }
            }

            Velocity *= Friction;

            // Shooting
            if (CurrentCooldown > 0)
                CurrentCooldown -= deltaTime;

            base.Update(deltaTime);
        }

        public Bullet Shoot()
        {
            Vector2 direction = new Vector2(
            (float)Math.Sin(Rotation),
             -(float)Math.Cos(Rotation)
            );

            Vector2 bulletPos = Position + direction * 30;
            Bullet bullet = new Bullet(bulletPos, direction, Rotation, "player");
            Raylib.PlaySound(AssetManager.LaserSound);

            return bullet;
        }

        public override void Draw()
        {
            // Draw ship with rotation
            Raylib.DrawTexturePro(
                AssetManager.PlayerShipTexture,
                new Rectangle(0, 0, AssetManager.PlayerShipTexture.Width, AssetManager.PlayerShipTexture.Height),
                new Rectangle(Position.X, Position.Y, Radius * 2, Radius * 2),
                new Vector2(Radius, Radius),
                Rotation * Raylib.RAD2DEG,
                Color.White
            );

        }

        public void Reset()
        {
            Position = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
            Velocity = new Vector2(0, 0);
            Rotation = 0;
        }
    }
}
