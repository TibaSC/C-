using Raylib_cs;

namespace ASTEROIDS
{
    public enum GameState
    {
        MainMenu,
        Game,
        LoseScreen,
        QuitGame

    }
    class Program
    {
        static void Main(string[] args)
        {
            Raylib.InitWindow(800, 600, "Asteroids");
            Raylib.InitAudioDevice();
            Raylib.SetTargetFPS(60);

            Game game = new Game();
            bool gameIsRunning = true;
            while (!Raylib.WindowShouldClose() && gameIsRunning)
            {
                switch (game.state)
                {
                    case GameState.MainMenu:
                        
                        game.DrawMainMenu(); 
                        break;

                    case GameState.Game:
                        game.Update();
                        game.Draw();
                        break;

                    case GameState.LoseScreen:
                        game.DrawLoseScreen();
                        break;
                    case GameState.QuitGame:
                        gameIsRunning = false; 
                        break;
                }
            }
            game.CleanUp();
            Raylib.CloseWindow();
        }
    }
}
