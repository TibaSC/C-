using Raylib_cs;
using System.Numerics;
using System.Text.Json;

namespace ARTILLERY
{
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
            string[] filePaths = {
                "SmallShell.json",
                "MediumShell.json",
                "HeavyShell.json"
                };

            foreach (string filePath in filePaths)
            {
                if (File.Exists(filePath))
                {
                    string jsonContent;

                    try
                    {
                        jsonContent = File.ReadAllText(filePath);
                        Ammus ammo = JsonSerializer.Deserialize<Ammus>(jsonContent);
                        ammunitionTypes.Add(ammo);
                    }
                    catch
                    {
                        Console.WriteLine($"Couldn't read ammo file {filePath}");
                    }


                }
                else
                {
                    Console.WriteLine($"Warning: Ammunition file {filePath} not found.");
                }
            }

            if (ammunitionTypes.Count == 0)
            {
                Console.WriteLine("No ammunition files found. Using default ammunition.");

                Ammus smallShell = new Ammus (name: "Small Shell", explosionSize: 3, color : Color.Yellow, damage : 20, weight : 1);
                Ammus mediumShell = new Ammus (name : "Medium Shell", explosionSize : 5, color : Color.Red, damage : 30, weight : 1.5f );
                Ammus heavyShell = new Ammus ( name : "Heavy Shell", explosionSize : 8, color : Color.Blue, damage : 50, weight : 2 );

                ammunitionTypes.Add(smallShell);
                ammunitionTypes.Add(mediumShell);
                ammunitionTypes.Add(heavyShell);
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

                TerrainBlock block = new TerrainBlock(newHeight, x);

                terrain.Add(block);
                lastHeight = newHeight;
            }
        }

        private void AddPlayers()
        {
            Random random = new Random();

            Tykki player1 = new Tykki(new Vector2(random.Next(5, ScreenWidth / 3), 0), 100, MathF.PI / 4, "Player 1", Color.Blue);

            Tykki player2 = new Tykki(new Vector2(random.Next(ScreenWidth * 2 / 3, ScreenWidth - 5), 0), 100, MathF.PI * 3 / 4 / 4, "Player 2", Color.Red);

            SetCannonOnTerrain(player1);
            SetCannonOnTerrain(player2);

            cannons.Add(player1);
            cannons.Add(player2);
        }

        private void SetCannonOnTerrain(Tykki cannon)
        {
            int terrainIndex = (int)(cannon.position.X / TerrainBlock.Width);
            if (terrainIndex < terrain.Count)
            {
                cannon.position.Y = terrain[terrainIndex].Height - 1;
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
                Raylib.DrawCircleV(currentProjectile.position, 5, Color.Red);

            }

            Tykki currentPlayer = cannons[currentPlayerIndex];

            Raylib.DrawText($"Turn: {currentPlayer.PlayerName}", 10, 10, 20, currentPlayer.RaylibColor);
            Raylib.DrawText($"Health: {currentPlayer.Health}", 10, 35, 20, currentPlayer.RaylibColor);
            Raylib.DrawText($"Angle: {Math.Round(currentPlayer.Angle * Raylib.RAD2DEG)}°", 10, 60, 20, Color.Black);
            Raylib.DrawText($"Power: {currentPlayer.shotPower}", 10, 85, 20, Color.Black);

            Raylib.DrawText("Ammunition:", 10, 110, 20, Color.Black);
            for (int i = 0; i < ammunitionTypes.Count; i++)
            {
                string ammoText = $"{i + 1}. {ammunitionTypes[i].Name} (Dmg: {ammunitionTypes[i].Damage}, Exp: {ammunitionTypes[i].ExplosionSize})";
                Color textColor = currentPlayer.selectedBulletI== i ? Color.Red : Color.Black;
                Raylib.DrawText(ammoText, 10, 140 + i * 30, 20, textColor);
            }

        }

        private void ProcessPlayerInput()
        {
            if (currentProjectile != null) return;

            Tykki currentPlayer = cannons[currentPlayerIndex];

            if (Raylib.IsKeyDown(KeyboardKey.Left))
            {
                currentPlayer.Move(-1, terrain);
            }
            else if (Raylib.IsKeyDown(KeyboardKey.Right))
            {
                currentPlayer.Move(1, terrain);
            }

            if (Raylib.IsKeyDown(KeyboardKey.Up))
            {
                currentPlayer.AdjustAngle(0.05f);
            }
            else if (Raylib.IsKeyDown(KeyboardKey.Down))
            {
                currentPlayer.AdjustAngle(-0.05f);
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
            if (Raylib.IsKeyPressed(KeyboardKey.KpSubtract)) 
            {
                currentPlayer.shotPower -= 15;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KpAdd)) 
            {
                currentPlayer.shotPower += 15;

            }

        }

        private void SelectAmmunition(int index)
        {
            if (index >= 0 && index < ammunitionTypes.Count)
            {
                Tykki currentPlayer = cannons[currentPlayerIndex];
                currentPlayer.selectedBulletI = index;
            }
        }

        private void FireCannon()
        {
            Tykki currentPlayer = cannons[currentPlayerIndex];

            Ammus selectedAmmo = ammunitionTypes.Find(a => a.position.Y == -1);
            if (selectedAmmo == null)
            {
                selectedAmmo = ammunitionTypes[0];
            }

            float power = 100;

            currentProjectile = currentPlayer.Fire(selectedAmmo);
        }

        private void UpdateProjectile()
        {
            if (currentProjectile == null) return;

            currentProjectile.Update();

            if (currentProjectile.position.X < 0 || currentProjectile.position.X >= ScreenWidth ||
                currentProjectile.position.Y >= ScreenHeight)
            {
                EndTurn();
                return;
            }

            int terrainIndex = (int)(currentProjectile.position.X / TerrainBlock.Width);
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
                    Math.Pow(cannon.position.X - currentProjectile.position.X, 2) +
                    Math.Pow(cannon.position.Y - currentProjectile.position.Y, 2)
                );

                if (distance <= currentProjectile.ExplosionSize * 5)
                {
                    int damage = (int)(currentProjectile.Damage * (1 - distance / currentProjectile.ExplosionSize * 5));
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
