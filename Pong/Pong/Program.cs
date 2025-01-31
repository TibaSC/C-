using Raylib_cs;
using System.Numerics;

namespace Pong
{
    internal class Program
    {

        const int screenWidth = 800;
        const int screenHeight = 450;

        static void Main()
        {


            Raylib.InitWindow(screenWidth, screenHeight, "Pong");
            Raylib.SetTargetFPS(60);

            int paddleWidth = 30, paddleHeight = 120;
            int paddleSpeed = 5;
            int ballSpeed = 6;

            Vector2 player1 = new Vector2(20, screenHeight / 2 - paddleHeight / 2);
            Vector2 player2 = new Vector2(screenWidth - 30, screenHeight / 2 - paddleHeight / 2);
            Vector2 ball = new Vector2(screenWidth / 2, screenHeight / 2);
            Vector2 ballVelocity = new Vector2(ballSpeed, ballSpeed);

            int score1 = 0, score2 = 0;

            while (!Raylib.WindowShouldClose())
            {
                // Pelaajien liikkuminen
                if (Raylib.IsKeyDown(KeyboardKey.W) && player1.Y > 0) player1.Y -= paddleSpeed;
                if (Raylib.IsKeyDown(KeyboardKey.S) && player1.Y < screenHeight - paddleHeight) player1.Y += paddleSpeed;
                if (Raylib.IsKeyDown(KeyboardKey.Up) && player2.Y > 0) player2.Y -= paddleSpeed;
                if (Raylib.IsKeyDown(KeyboardKey.Down) && player2.Y < screenHeight - paddleHeight) player2.Y += paddleSpeed;

                // Pallon liike
                ball += ballVelocity;

                // Törmäys ylä- ja alareunoihin
                if (ball.Y <= 0 || ball.Y >= screenHeight) ballVelocity.Y *= -1;

                // Törmäys pelaajien mailoihin
                if ((ball.X <= player1.X + paddleWidth && ball.Y >= player1.Y && ball.Y <= player1.Y + paddleHeight) ||
                    (ball.X >= player2.X - paddleWidth && ball.Y >= player2.Y && ball.Y <= player2.Y + paddleHeight))
                {
                    ballVelocity.X *= -1;
                }

                // Pisteen lasku ja pallon resetointi
                if (ball.X <= 0) { score2++; ball = new Vector2(screenWidth / 2, screenHeight / 2); ballVelocity.X = ballSpeed; }
                if (ball.X >= screenWidth) { score1++; ball = new Vector2(screenWidth / 2, screenHeight / 2); ballVelocity.X = -ballSpeed; }

                // Drawing the game
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                Raylib.DrawRectangle((int)player1.X, (int)player1.Y, paddleWidth, paddleHeight, Color.Blue);
                Raylib.DrawRectangle((int)player2.X, (int)player2.Y, paddleWidth, paddleHeight, Color.Yellow);
                Raylib.DrawCircle((int)ball.X, (int)ball.Y, 16, Color.White);


                Raylib.DrawText(score1.ToString(), screenWidth / 2-50 , 20, 30, Color.Blue);
                Raylib.DrawText(score2.ToString(), screenWidth / 2+50, 20, 30, Color.Yellow);
                Raylib.DrawLine(screenWidth / 2, 0, screenWidth / 2, screenHeight, Color.White);

                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }
    }
}
