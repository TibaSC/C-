using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class Asteroid : GameObject
    {
        public int Size; // 3 = large, 2 = medium, 1 = small

        public Asteroid(Vector2 position, int size) : base(position, size * 20)
        {
            Size = size;

            // Random velocity
            Random rand = new Random();
            float angle = (float)(rand.NextDouble() * Math.PI * 2);
            float speed = (float)(rand.NextDouble() * 30) + 20;
            Velocity = new Vector2(
                (float)Math.Sin(angle) * speed,
                (float)Math.Cos(angle) * speed
            );

            Rotation = (float)(rand.NextDouble() * Math.PI * 2);
        }

        public override void Draw()
        {
            Texture2D texture;
            switch (Size)
            {
                case 3:
                    texture = AssetManager.MeteorBigTexture;
                    break;
                case 2:
                    texture = AssetManager.MeteorMediumTexture;
                    break;
                default:
                    texture = AssetManager.MeteorSmallTexture;
                    break;
            }
            Raylib.DrawTexturePro(
                texture,
                new Rectangle(0, 0, texture.Width, texture.Height),
                new Rectangle(Position.X, Position.Y, Radius * 2, Radius * 2),
                new Vector2(Radius, Radius),
                Rotation * Raylib.RAD2DEG,
                Color.White
            );

        }

        public List<Asteroid> Split()
        {
            List<Asteroid> newAsteroids = new List<Asteroid>();

            if (Size > 1)
            {
                // Create 2 smaller asteroids
                for (int i = 0; i < 2; i++)
                {
                    Asteroid newAsteroid = new Asteroid(Position, Size - 1);
                    newAsteroids.Add(newAsteroid);
                }
            }

            return newAsteroids;
        }
    }
}
