using System.Numerics;
using Raylib_cs;

namespace ARTILLERY
{
    class Tykki
    {
        public Vector2 position;
        public int Health;
        public float Angle;
        public string PlayerName;
        public Color RaylibColor;
        public int shotPower;
        public int selectedBulletI;
        public Tykki(Vector2 position, int health, float angle, string playerName, Color raylibColor)
        {
            this.position = position;
            Health = health;
            Angle = angle;
            PlayerName = playerName;
            RaylibColor = raylibColor;
            selectedBulletI = 0;
            shotPower = 50;
        }

        public void Move(int direction, List<TerrainBlock> terrain)
        {
            float newX = position.X + direction * 2;
            if (newX >= 0 && newX < Console.WindowWidth - 2)
            {
                position.X = newX;
                int terrainIndex = (int)(position.X / TerrainBlock.Width);
                if (terrainIndex < terrain.Count)
                {
                    position.Y = terrain[terrainIndex].Height - 1;
                }
            }
        }

        public void AdjustAngle(float adjustment)
        {
            Angle += adjustment;
            if (Angle < 0) Angle = 0;
            if (Angle > Math.PI) Angle = MathF.PI;
        }

        public Ammus Fire(Ammus ammusTemplate)
        {
            Ammus ammus = ammusTemplate.Clone();
            ammus.position.X = position.X;
            ammus.position.Y = position.Y;
            ammus.Velocity = Vector2.Transform(Vector2.UnitX,Matrix3x2.CreateRotation(-Angle)) * shotPower; 
            return ammus;
        }
        public void Draw()
        {
            // Draw cannon base
            Raylib.DrawRectangle((int)position.X - 5, (int)position.Y - 5, 10, 10, RaylibColor);

            // Draw cannon barrel
            int barrelLength = 15;
            int barrelEndX = (int)(position.X + Math.Cos(Angle) * barrelLength);
            int barrelEndY = (int)(position.Y - Math.Sin(Angle) * barrelLength);
            Raylib.DrawLineEx(new Vector2((float)position.X, (float)position.Y), new Vector2(barrelEndX, barrelEndY), 3, RaylibColor);
        }

    }
}
