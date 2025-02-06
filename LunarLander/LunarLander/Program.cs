using Raylib_cs;
using System.Numerics;

namespace LunarLander
{
    internal class Ship
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public bool EngineOn { get; private set; }
        private float fuel = 100.0f;
        private const float ENGINE_POWER = 200.0f;
        private const float FUEL_CONSUMPTION = 10.0f;

        public Ship(Vector2 startPosition)
        {
            Position = startPosition;
            Velocity = Vector2.Zero;
        }

        public void Update(float deltaTime)
        {
            EngineOn = Raylib.IsKeyDown(KeyboardKey.Up) && fuel > 0;

            if (EngineOn)
            {
                Velocity -= new Vector2(0, ENGINE_POWER * deltaTime);
                fuel -= FUEL_CONSUMPTION * deltaTime;
            }

            Position += Velocity * deltaTime;
        }

        public void Draw()
        {

            Vector2 p1 = Position + new Vector2(0, -20);
            Vector2 p2 = Position + new Vector2(-15, 10);
            Vector2 p3 = Position + new Vector2(15, 10);

            Raylib.DrawTriangle(p1, p2, p3, Color.White);


            if (EngineOn && fuel > 0)
            {
                // Alternative method using regular coordinates
                float flameBaseY = Position.Y + 15;
                float flamePointY = Position.Y + 30;

                Raylib.DrawTriangle(
                    new Vector2(Position.X, flamePointY),         // Bottom point
                    new Vector2(Position.X + 8, flameBaseY),      // Left point
                    new Vector2(Position.X - 8, flameBaseY),      // Right point
                    Color.Orange
                );
            }


            DrawHUD();
        }
        private void DrawHUD()
        {
            
            Raylib.DrawRectangle(10, 10, (int)(fuel * 2), 20, Color.Green);

            
            string speedText = $"Speed: {Math.Abs(Velocity.Y):F1} m/s";
            string fuelText = $"Fuel: {Math.Max(fuel, 0):F1}%";

            Raylib.DrawText(speedText, 10, 40, 20, Color.Lime);
            Raylib.DrawText(fuelText, 10, 70, 20, Color.Lime);
        }
    }

    internal class Lander
    {
        private const int WINDOW_WIDTH = 800;
        private const int WINDOW_HEIGHT = 600;
        private const float GRAVITY = 100.0f;
        private const float LANDING_PLATFORM_Y = 550;
        private Ship ship;
        private bool gameOver = false;
        private bool victory = false;

        static void Main(string[] args)
        {
            Lander game = new Lander();
            game.Init();
            game.GameLoop();
        }

        void Init()
        {
            Raylib.InitWindow(WINDOW_WIDTH, WINDOW_HEIGHT, "Lunar Lander");
            Raylib.SetTargetFPS(60);

            ship = new Ship(new Vector2(WINDOW_WIDTH / 2, 100));
        }

        void GameLoop()
        {
            while (!Raylib.WindowShouldClose())
            {
                Update();
                Draw();
            }

            Raylib.CloseWindow();
        }

        void Update()
        {
            if (gameOver) return;

            float deltaTime = Raylib.GetFrameTime();

            ship.Velocity += new Vector2(0, GRAVITY * deltaTime);
            ship.Update(deltaTime);


            if (ship.Position.Y >= LANDING_PLATFORM_Y)
            {
                gameOver = true;
                victory = ship.Velocity.Y < 150;
            }
        }

        void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);


            Raylib.DrawRectangle(WINDOW_WIDTH / 4, (int)LANDING_PLATFORM_Y,
                               WINDOW_WIDTH / 2, 20, Color.Gray);

            ship.Draw();


            if (gameOver)
            {
                string message = victory ? "Successful Landing!" : "The Ship Crashed!";
                Raylib.DrawText(message,
                              WINDOW_WIDTH / 2 - 100,
                              WINDOW_HEIGHT / 2,
                              30,
                              victory ? Color.Green : Color.Red);
            }

            Raylib.EndDrawing();
        }
    }
}