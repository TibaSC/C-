using Raylib_cs;

namespace ASTEROIDS
{

    class Program
    {
        static void Main(string[] args)
        {
            Raylib.InitWindow(800, 600, "Asteroids");
            Raylib.InitAudioDevice();
            Raylib.SetTargetFPS(60);

            Game game = new Game();

            while (!Raylib.WindowShouldClose())
            {
                game.Update();
                game.Draw();
            }
            game.CleanUp();
            Raylib.CloseWindow();
        }
    }
}
