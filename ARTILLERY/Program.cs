
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Raylib_cs;

namespace ARTILLERY
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
    class Ammus
    {
        public double X ;
        public double Y ;
        public double VelocityX ;
        public double VelocityY ;
        public int ExplosionSize ;
        public string Color ;
        public int Damage ;
        public double Weight ;
        public string Name ;

        public Ammus() { }

        public Ammus Clone()
        {
            return new Ammus
            {
                ExplosionSize = this.ExplosionSize,
                Color = this.Color,
                Damage = this.Damage,
                Weight = this.Weight,
                Name = this.Name
            };
        }
        public void Update()
        {
            VelocityY += 0.1 * Weight;

            X += VelocityX;
            Y += VelocityY;
        }
        
    }

    class Tykki
    {
        public double X;
        public double Y;
        public int Health;
        public double Angle;
        public string PlayerName;
        public Color RaylibColor;

        public void Move(int direction, List<TerrainBlock> terrain)
        {
            double newX = X + direction * 2;
            if (newX >= 0 && newX < Console.WindowWidth - 2)
            {
                X = newX;
                int terrainIndex = (int)(X / TerrainBlock.Width);
                if (terrainIndex < terrain.Count)
                {
                    Y = terrain[terrainIndex].Height - 1;
                }
            }
        }

        public void AdjustAngle(double adjustment)
        {
            Angle += adjustment;
            if (Angle < 0) Angle = 0;
            if (Angle > Math.PI) Angle = Math.PI;
        }

        public Ammus Fire(Ammus ammusTemplate, double power)
        {
            Ammus ammus = ammusTemplate.Clone();
            ammus.X = X;
            ammus.Y = Y;
            ammus.VelocityX = Math.Cos(Angle) * power;
            ammus.VelocityY = -Math.Sin(Angle) * power; 

            return ammus;
        }
        public void Draw()
        {
            // Draw cannon base
            Raylib.DrawRectangle((int)X - 5, (int)Y - 5, 10, 10, RaylibColor);

            // Draw cannon barrel
            int barrelLength = 15;
            int barrelEndX = (int)(X + Math.Cos(Angle) * barrelLength);
            int barrelEndY = (int)(Y - Math.Sin(Angle) * barrelLength);
            Raylib.DrawLineEx(new Vector2((float)X, (float)Y), new Vector2(barrelEndX, barrelEndY), 3, RaylibColor);
        }

    }

    class TerrainBlock
    {
        public static int Width = 10; 
        public int Height;
        public int X;

        public void Draw()
        {
            Raylib.DrawRectangle(X, Height, Width, Game.ScreenHeight - Height, Color.DarkGreen);
        }

        public bool CheckCollision(Ammus ammus)
        {
            return ammus.X >= X && ammus.X < X + Width && ammus.Y >= Height;
        }

        public void Damage(int explosionSize)
        {
            Height += explosionSize / 3; 
            if (Height >= Game.ScreenHeight)
            {
                Height = Game.ScreenHeight - 1;
            }
        }
    }

    class Game
    {
        public static int ScreenWidth = 800;
        public static int ScreenHeight = 600;
        private List<Tykki> cannons = new List<Tykki>();
        private List<TerrainBlock> terrain = new List<TerrainBlock>();
        private List<Ammus> ammunitionTypes = new List<Ammus>();
        private Ammus currentProjectile = null;
        private int currentPlayerIndex = 0;
        private bool gameOver = false;

        public void Start()
        {
            Raylib.InitWindow(ScreenWidth, ScreenHeight, "Artillery Game");
            Raylib.SetTargetFPS(60);

            LoadAmmunition();

            GenerateTerrain();

            AddPlayers();

            GameLoop();

            Raylib.CloseWindow();
        }

        private void LoadAmmunition()
        {
            try
            {
                string[] filePaths = {
                "SmallShell.json",
                "MediumShell.json",
                "HeavyShell.json"
                };

                foreach (string filePath in filePaths)
                {
                    if (File.Exists(filePath))
                    {
                        string jsonContent = File.ReadAllText(filePath);
                        Ammus ammo = JsonSerializer.Deserialize<Ammus>(jsonContent);
                        ammunitionTypes.Add(ammo);
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Ammunition file {filePath} not found.");
                    }
                }

                if (ammunitionTypes.Count == 0)
                {
                    Console.WriteLine("No ammunition files found. Using default ammunition.");
           
                    Ammus smallShell = new Ammus { Name = "Small Shell", ExplosionSize = 3, Color = "Yellow", Damage = 20, Weight = 1 };
                    Ammus mediumShell = new Ammus { Name = "Medium Shell", ExplosionSize = 5, Color = "Red", Damage = 30, Weight = 1.5 };
                    Ammus heavyShell = new Ammus { Name = "Heavy Shell", ExplosionSize = 8, Color = "Blue", Damage = 50, Weight = 2 };

                    ammunitionTypes.Add(smallShell);
                    ammunitionTypes.Add(mediumShell);
                    ammunitionTypes.Add(heavyShell);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading ammunition: {ex.Message}");
                
                Ammus smallShell = new Ammus { Name = "Small Shell", ExplosionSize = 3, Color = "Yellow", Damage = 20, Weight = 1 };
                Ammus mediumShell = new Ammus { Name = "Medium Shell", ExplosionSize = 5, Color = "Red", Damage = 30, Weight = 1.5 };
                Ammus heavyShell = new Ammus { Name = "Heavy Shell", ExplosionSize = 8, Color = "Blue", Damage = 50, Weight = 2 };

                ammunitionTypes.Add(smallShell);
                ammunitionTypes.Add(mediumShell);
                ammunitionTypes.Add(heavyShell);
            }
            foreach (var ammo in ammunitionTypes)
            {
                if (ammo.Weight < 0.5 || ammo.Weight > 5)
                {
                    ammo.Weight = 0.6;
                }
            }
        }

        private void GenerateTerrain()
        {
            Random random = new Random();
            int lastHeight = ScreenHeight / 2 + 150;

            for (int x = 0; x < ScreenWidth; x += TerrainBlock.Width)
            {
                
                int newHeight = lastHeight + random.Next(-2, 3);

                if (newHeight < ScreenHeight / 3) newHeight = ScreenHeight / 3;
                if (newHeight > ScreenHeight - 5) newHeight = ScreenHeight - 5;

                TerrainBlock block = new TerrainBlock
                {
                    X = x,
                    Height = newHeight
                };

                terrain.Add(block);
                lastHeight = newHeight;
            }
        }

        private void AddPlayers()
        {
            Random random = new Random();

            Tykki player1 = new Tykki
            {
                X = random.Next(5, ScreenWidth / 3),
                Angle = Math.PI / 4,
                Health = 100,
                PlayerName = "Player 1",
                RaylibColor = Color.Blue
            };

            Tykki player2 = new Tykki
            {
                X = random.Next(ScreenWidth * 2 / 3, ScreenWidth - 5),
                Angle = Math.PI * 3 / 4,
                Health = 100,
                PlayerName = "Player 2",
                RaylibColor = Color.Red
            };

            SetCannonOnTerrain(player1);
            SetCannonOnTerrain(player2);

            cannons.Add(player1);
            cannons.Add(player2);
        }

        private void SetCannonOnTerrain(Tykki cannon)
        {
            int terrainIndex = (int)(cannon.X / TerrainBlock.Width);
            if (terrainIndex < terrain.Count)
            {
                cannon.Y = terrain[terrainIndex].Height - 1;
            }
        }

        private void GameLoop()
        {
            while (!Raylib.WindowShouldClose() && !gameOver)
            {
                ProcessPlayerInput();
                UpdateProjectile();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.SkyBlue);

                DrawGame();

                Raylib.EndDrawing();
            }
            if (gameOver)
            {
                ShowGameOverScreen();
            }

        }

        private void DrawGame()
        {
            foreach (var block in terrain)
            {
                block.Draw();
            }
            foreach (var cannon in cannons)
            {
                cannon.Draw();
            }

            if (currentProjectile != null)
            {
                int projX = (int)currentProjectile.X;
                int projY = (int)currentProjectile.Y;

                if (projX >= 0 && projX < ScreenWidth && projY >= 0 && projY < ScreenHeight)
                {
                    Raylib.DrawCircle(projX, projY, 5, Color.Red);
                }
            }

            Tykki currentPlayer = cannons[currentPlayerIndex];

            Raylib.DrawText($"Turn: {currentPlayer.PlayerName}", 10, 10, 20, currentPlayer.RaylibColor);
            Raylib.DrawText($"Health: {currentPlayer.Health}", 10, 40, 20, currentPlayer.RaylibColor);
            Raylib.DrawText($"Angle: {Math.Round(currentPlayer.Angle * 180 / Math.PI, 1)}°", 10, 70, 20, Color.Black);

            Raylib.DrawText("Ammunition:", 10, 110, 20, Color.Black);
            for (int i = 0; i < ammunitionTypes.Count; i++)
            {
                string ammoText = $"{i + 1}. {ammunitionTypes[i].Name} (Dmg: {ammunitionTypes[i].Damage}, Exp: {ammunitionTypes[i].ExplosionSize})";
                Color textColor = ammunitionTypes[i].Y == -1 ? Color.Red : Color.Black;
                Raylib.DrawText(ammoText, 10, 140 + i * 30, 20, textColor);
            }

        }

        private void ProcessPlayerInput()
        {
            if (currentProjectile != null) return; 

            Tykki currentPlayer = cannons[currentPlayerIndex];

            if (Raylib.IsKeyPressed(KeyboardKey.Left) || Raylib.IsKeyDown(KeyboardKey.Left))
            {
                currentPlayer.Move(-1, terrain);
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.Right) || Raylib.IsKeyDown(KeyboardKey.Right))
            {
                currentPlayer.Move(1, terrain);
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Up) || Raylib.IsKeyDown(KeyboardKey.Up))
            {
                currentPlayer.AdjustAngle(0.05);
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.Down) || Raylib.IsKeyDown(KeyboardKey.Down))
            {
                currentPlayer.AdjustAngle(-0.05);
            }

            if (Raylib.IsKeyPressed(KeyboardKey.One))
            {
                SelectAmmunition(0);
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.Two))
            {
                SelectAmmunition(1);
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.Three))
            {
                SelectAmmunition(2);
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                FireCannon();
            }
        }

        private void SelectAmmunition(int index)
        {
            if (index >= 0 && index < ammunitionTypes.Count)
            {
                // Reset all ammunition
                foreach (var ammo in ammunitionTypes)
                {
                    ammo.Y = 0;
                }

                ammunitionTypes[index].Y = -1;
            }
        }
            
        private void FireCannon()
        {
            Tykki currentPlayer = cannons[currentPlayerIndex];

            Ammus selectedAmmo = ammunitionTypes.Find(a => a.Y == -1);
            if (selectedAmmo == null)
            {
                selectedAmmo = ammunitionTypes[0];
            }

            Random random = new Random();
            double power = 3 + random.NextDouble() * 3;

            currentProjectile = currentPlayer.Fire(selectedAmmo, power);
        }

        private void UpdateProjectile()
        {
            if (currentProjectile == null) return;

            currentProjectile.Update();

            if (currentProjectile.X < 0 || currentProjectile.X >= ScreenWidth ||
                currentProjectile.Y >= ScreenHeight)
            {
                EndTurn();
                return;
            }

            int terrainIndex = (int)(currentProjectile.X / TerrainBlock.Width);
            if (terrainIndex >= 0 && terrainIndex < terrain.Count)
            {
                if (terrain[terrainIndex].CheckCollision(currentProjectile))
                {
                    //Raylib.PlaySound(explosionsSound);

                    DamageTerrain(terrainIndex, currentProjectile.ExplosionSize);

                    CheckPlayerHit();

                    EndTurn();
                    return;
                }
            }
        }



        private void DamageTerrain(int centerIndex, int explosionSize)
        {
            terrain[centerIndex].Damage(explosionSize);

            for (int i = 1; i <= explosionSize / 2; i++)
            {
                if (centerIndex - i >= 0)
                {
                    terrain[centerIndex - i].Damage(explosionSize - i);
                }

                if (centerIndex + i < terrain.Count)
                {
                    terrain[centerIndex + i].Damage(explosionSize - i);
                }
            }
        }

        private void CheckPlayerHit()
        {
            foreach (var cannon in cannons)
            {
                double distance = Math.Sqrt(
                    Math.Pow(cannon.X - currentProjectile.X, 2) +
                    Math.Pow(cannon.Y - currentProjectile.Y, 2)
                );

                if (distance <= currentProjectile.ExplosionSize *5)
                {
                    int damage = (int)(currentProjectile.Damage * (1 - distance / currentProjectile.ExplosionSize *5));
                    cannon.Health -= damage;

                    if (cannon.Health <= 0)
                    {
                        cannon.Health = 0;
                        CheckGameOver();
                    }
                }
            }
        }

        private void EndTurn()
        {
            currentProjectile = null;

            currentPlayerIndex = (currentPlayerIndex + 1) % cannons.Count;

            while (cannons[currentPlayerIndex].Health <= 0)
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % cannons.Count;
            }

            foreach (var ammo in ammunitionTypes)
            {
                ammo.Y = 0;
            }
        }

        private void CheckGameOver()
        {
            int playersAlive = 0;
            int lastAliveIndex = -1;

            for (int i = 0; i < cannons.Count; i++)
            {
                if (cannons[i].Health > 0)
                {
                    playersAlive++;
                    lastAliveIndex = i;
                }
            }

            if (playersAlive <= 1)
            {
                gameOver = true;
            }
        }
        private void ShowGameOverScreen()
        {
            int playersAlive = 0;
            int lastAliveIndex = -1;

            for (int i = 0; i < cannons.Count; i++)
            {
                if (cannons[i].Health > 0)
                {
                    playersAlive++;
                    lastAliveIndex = i;
                }
            }

            string gameOverText;
            Color textColor;

            if (playersAlive == 1)
            {
                gameOverText = $"{cannons[lastAliveIndex].PlayerName} WINS!";
                textColor = cannons[lastAliveIndex].RaylibColor;
            }
            else
            {
                gameOverText = "DRAW GAME!";
                textColor = Color.Green;
            }

            for (int i = 0; i < 180; i++)
            {
                if (Raylib.WindowShouldClose()) break;

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                int textWidth = Raylib.MeasureText(gameOverText, 40);
                Raylib.DrawText(gameOverText, ScreenWidth / 2 - textWidth / 2, ScreenHeight / 2 - 20, 40, textColor);
                Raylib.DrawText("Press ESC to exit", ScreenWidth / 2 - 100, ScreenHeight / 2 + 40, 20, Color.White);

                Raylib.EndDrawing();

                if (Raylib.IsKeyPressed(KeyboardKey.Escape)) break;
            }
        }
    }
}
