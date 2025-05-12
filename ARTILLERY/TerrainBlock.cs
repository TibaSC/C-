using Raylib_cs;

namespace ARTILLERY
{
    class TerrainBlock
    {
        public static int Width = 10; 
        public int Height;
        public int X;
        public Color color;
        public TerrainBlock(int Height, int X)
        {
            this.Height = Height;
            this.X = X;
        }

        public void Draw()
        {
            Raylib.DrawRectangle(X, Height, Width, Game.ScreenHeight - Height, Color.DarkGreen);
        }

        public bool CheckCollision(Ammus ammus)
        {
            return ammus.position.X >= X && ammus.position.X < X + Width && ammus.position.Y >= Height;
        }

        public void Damage(int explosionSize)
        {
            Height += explosionSize; 
            if (Height >= Game.ScreenHeight)
            {
                Height = Game.ScreenHeight - 1;
            }
        }
    }
}
