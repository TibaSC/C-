using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class Bullet : GameObject
    {
        public float Speed = 500.0f;
        public float LifeTime = 2.0f;
        public float CurrentLife;

        public string Source;

        public Bullet(Vector2 position, Vector2 direction, float rotation, string source = "player") : base(position, 5)
        {
            Velocity = direction * Speed;
            Rotation = rotation;
            CurrentLife = LifeTime;
            Source = source;

            if (source == "enemy")
            {
                Speed = 200;
                Velocity = direction * Speed;
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Decrease lifetime
            CurrentLife -= deltaTime;
            if (CurrentLife <= 0)
            {
                IsActive = false;
            }
        }

        public override void Draw()
        {
            Raylib.DrawCircle((int)Position.X, (int)Position.Y, Radius, Color.White);
        }
    }
}
