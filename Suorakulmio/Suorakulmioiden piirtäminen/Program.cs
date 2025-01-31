using Raylib_cs;
using System.Numerics;
namespace Suorakulmioiden_piirtäminen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Vector2 pointPosition = new Vector2(220, 170);
            int screenWidth = 600;
            int screenHeight = 400;
            Raylib.InitWindow(screenWidth, screenHeight, "Raylib");
            while (Raylib.WindowShouldClose() == false)
            {
                Rectangle collisionTest = new Rectangle(200, 150, 60, 60);
                Raylib.DrawRectangleRec(collisionTest, Color.Blue);

                Vector2 position = new Vector2(200, 150);
                Vector2 size = new Vector2(60, 80);

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                Raylib.DrawRectangleV(position, size, Color.Blue);

                Raylib.DrawCircle(220, 170 , 6, Color.White);
                if (Raylib.CheckCollisionPointRec(pointPosition, collisionTest))
                {
                    Raylib.DrawText("Collision!",220, 60, 32, Color.White);
                }

                Raylib.EndDrawing();
            }
        }
    }
}
