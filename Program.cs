using Raylib_cs;
using System.Numerics;
namespace Raylib_testi
    
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int width = 600;
            int height = 400;
            Vector2 A = new Vector2(width/2, 0);
            Vector2 B = new Vector2(0,height/2);
            Vector2 C = new Vector2(width, height*3/4);

            Vector2  Amove = new Vector2(1, 1);
            Vector2  Bmove = new Vector2(1, -1);
            Vector2  Cmove = new Vector2(-1, 1);
            Console.WriteLine("Hello, World!");
            Raylib.InitWindow(600, 400 ,"Raylib Testi");
            while(Raylib.WindowShouldClose() == false)
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                Raylib.DrawCircle(0,0, 30,Color.Green);

                Raylib.DrawLineV(A, B, Color.Green);
                Raylib.DrawLineV(B, C, Color.Yellow);
                Raylib.DrawLineV(C, A, Color.SkyBlue);
                Raylib.DrawText("HELLO", 200, 200, 32, Color.White);
                Raylib.EndDrawing();
                Vector2 Anew = Amove * 50 * Raylib.GetFrameTime();
                A = A + Anew;
                if(A.X < 0 || A.X > width)
                {
                    Amove.X = Amove.X * -1;
                }
                if(A.Y < 0 || A.Y > height)
                {
                    Amove.Y = Amove.Y * -1;
                }
                Vector2 Bnew = Bmove * 50 * Raylib.GetFrameTime();
                B = B + Bnew;
                if(B.X < 0 || B.X > width)
                {
                    Bmove.X = Bmove.X * -1;
                }
                if(B.Y < 0 || B.Y > height)
                {
                    Bmove.Y = Bmove.Y * -1;
                }
                Vector2 Cnew = Cmove * 50 * Raylib.GetFrameTime();
                C = C + Cnew;
                if(C.X < 0 || C.X > width)
                {
                    Cmove.X = Cmove.X * -1;
                }
                if(C.Y < 0 || C.Y > height)
                {
                    Cmove.Y = Cmove.Y * -1;
                }
                
            }
            Raylib.CloseWindow();
        }
    }
}
