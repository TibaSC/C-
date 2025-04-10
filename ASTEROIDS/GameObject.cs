using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class GameObject
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Rotation;
        public bool IsActive = true;
        public float Radius;

        public GameObject(Vector2 position, float radius)
        {
            Position = position;
            Velocity = new Vector2(0, 0);
            Rotation = 0;
            Radius = radius;
        }

        public virtual void Update(float deltaTime)
        {
            Position += Velocity * deltaTime;

            // Screen wrapping
            int screenWidth = Raylib.GetScreenWidth();
            int screenHeight = Raylib.GetScreenHeight();

            if (Position.X < 0 - Radius) Position.X = screenWidth + Radius;
            if (Position.X > screenWidth + Radius) Position.X = 0 - Radius;
            if (Position.Y < 0 - Radius) Position.Y = screenHeight + Radius;
            if (Position.Y > screenHeight + Radius) Position.Y = 0 - Radius;
        }

        public virtual void Draw()
        {
            
        }

        public bool CheckCollision(GameObject other)
        {
            return Vector2.Distance(Position, other.Position) < (Radius + other.Radius);
        }
    }
}
