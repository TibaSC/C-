using Raylib_cs;
using System.Numerics;


namespace TANKS_
{
    class Program
    {
        static void Main()
        {
            const int screenWidth = 800;
            const int screenHeight = 450;

            Raylib.InitWindow(screenWidth, screenHeight, "Tank Game");
            Raylib.SetTargetFPS(60);


            Tank player1 = new Tank(new Vector2(100, 225), Color.Blue);
            Tank player2 = new Tank(new Vector2(700, 225), Color.Yellow);
            Wall[] walls = new Wall[]
            {
            new Wall(210, 100, 40, 250),
            new Wall(550, 100, 40, 250)
            };

            while (!Raylib.WindowShouldClose())
            {

                player1.Update(KeyboardKey.W, KeyboardKey.S, KeyboardKey.A,
                              KeyboardKey.D, KeyboardKey.Space);
                player2.Update(KeyboardKey.Up, KeyboardKey.Down, KeyboardKey.Left,
                              KeyboardKey.Right, KeyboardKey.Enter);

                // Check collisions with walls
                foreach (Wall wall in walls)
                {
                    player1.CheckWallCollision(wall);
                    player2.CheckWallCollision(wall);
                    player1.UpdateProjectileWallCollisions(wall);
                    player2.UpdateProjectileWallCollisions(wall);
                }

                // Check tank collisions with projectiles
                player1.CheckProjectileCollision(player2);
                player2.CheckProjectileCollision(player1);

                // Drawing
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Lime);

                // Draw walls
                foreach (Wall wall in walls)
                {
                    wall.Draw();
                }

                // Draw tanks and projectiles
                player1.Draw();
                player2.Draw();

                // Draw scores
                Raylib.DrawText($"Player 1: {player1.Score}", 10, 10, 20, Color.Blue);
                Raylib.DrawText($"Player 2: {player2.Score}", screenWidth - 150, 10, 20, Color.Yellow);

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }

    class Tank
    {
        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }
        public Color Color { get; private set; }
        public int Score { get; set; }
        private List<Projectile> projectiles;
        private double lastShootTime;
        private const double shootInterval = 1.0;
        private const float speed = 5.0f;
        private Rectangle Bounds => new Rectangle(Position.X - 15, Position.Y - 15, 30, 30);

        public Tank(Vector2 startPosition, Color color)
        {
            Position = startPosition;
            Direction = new Vector2(1, 0);
            Color = color;
            Score = 0;
            projectiles = new List<Projectile>();
            lastShootTime = 0;
        }

        public void Update(KeyboardKey up, KeyboardKey down, KeyboardKey left,
                          KeyboardKey right, KeyboardKey shoot)
        {
            Vector2 movement = Vector2.Zero;

            if (Raylib.IsKeyDown(up)) movement.Y = -1;
            if (Raylib.IsKeyDown(down)) movement.Y = 1;
            if (Raylib.IsKeyDown(left)) movement.X = -1;
            if (Raylib.IsKeyDown(right)) movement.X = 1;

            if (movement != Vector2.Zero)
            {
                Direction = Vector2.Normalize(movement);
                Position += Direction * speed;
            }

            // Keep tank in bounds
            Position = new Vector2(
                Math.Clamp(Position.X, 20, 780),
                Math.Clamp(Position.Y, 20, 430)
            );

            // Shooting
            if (Raylib.IsKeyPressed(shoot) &&
                Raylib.GetTime() - lastShootTime > shootInterval)
            {
                projectiles.Add(new Projectile(Position, Direction));
                lastShootTime = Raylib.GetTime();
            }

            // Update projectiles
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update();
                if (projectiles[i].ShouldDestroy)
                {
                    projectiles.RemoveAt(i);
                }
            }
        }

        public void Draw()
        {
            // Draw tank body
            Raylib.DrawRectangleV(Position - new Vector2(15, 15), new Vector2(30, 30), Color);

            // Draw cannon
            Vector2 cannonEnd = Position + Direction * 25;
            Raylib.DrawLineEx(Position, cannonEnd, 7, Color.DarkGray);

            // Draw tracks
            Raylib.DrawRectangleV(Position - new Vector2(18, 20), new Vector2(36, 5), Color.DarkGray);
            Raylib.DrawRectangleV(Position + new Vector2(-18, 15), new Vector2(36, 5), Color.DarkGray);

            // Draw projectiles
            foreach (var projectile in projectiles)
            {
                projectile.Draw();
            }
        }

        public void CheckWallCollision(Wall wall)
        {
            if (Raylib.CheckCollisionRecs(Bounds, wall.Bounds))
            {

                if (Position.X < wall.Position.X)
                    Position = new Vector2(wall.Position.X - 16, Position.Y);
                else if (Position.X > wall.Position.X + wall.Width)
                    Position = new Vector2(wall.Position.X + wall.Width + 16, Position.Y);

                if (Position.Y < wall.Position.Y)
                    Position = new Vector2(Position.X, wall.Position.Y - 16);
                else if (Position.Y > wall.Position.Y + wall.Height)
                    Position = new Vector2(Position.X, wall.Position.Y + wall.Height + 16);
            }
        }

        public void UpdateProjectileWallCollisions(Wall wall)
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                if (projectiles[i].CheckWallCollision(wall))
                {
                    projectiles.RemoveAt(i);
                }
            }
        }

        public void CheckProjectileCollision(Tank other)
        {
            foreach (var projectile in other.projectiles.ToList())
            {
                if (Raylib.CheckCollisionCircleRec(projectile.Position, 5, Bounds))
                {
                    other.Score++;
                    ResetPosition();
                    other.projectiles.Clear();
                    break;
                }
            }
        }

        private void ResetPosition()
        {
            Position = new Vector2(Position.X < 400 ? 100 : 700, 225);
        }
    }

    class Projectile
    {
        public Vector2 Position { get; private set; }
        private Vector2 Direction;
        private const float Speed = 10.0f;
        public bool ShouldDestroy { get; private set; }

        public Projectile(Vector2 startPosition, Vector2 direction)
        {
            Position = startPosition;
            Direction = direction;
            ShouldDestroy = false;
        }

        public void Update()
        {
            Position += Direction * Speed;

            // Destroy if out of bounds
            if (Position.X < 0 || Position.X > 800 ||
                Position.Y < 0 || Position.Y > 450)
            {
                ShouldDestroy = true;
            }
        }

        public void Draw()
        {
            Raylib.DrawCircleV(Position, 5, Color.Black);
        }

        public bool CheckWallCollision(Wall wall)
        {
            return Raylib.CheckCollisionCircleRec(Position, 5, wall.Bounds);
        }
    }

    class Wall
    {
        public Vector2 Position { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public Rectangle Bounds => new Rectangle(Position.X, Position.Y, Width, Height);

        public Wall(float x, float y, float width, float height)
        {
            Position = new Vector2(x, y);
            Width = width;
            Height = height;
        }

        public void Draw()
        {
            Raylib.DrawRectangleV(Position, new Vector2(Width, Height), Color.Maroon);
        }
    }
}

