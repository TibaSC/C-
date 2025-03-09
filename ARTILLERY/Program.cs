using System.Text.Json;

namespace ARTILLERY
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Game game = new Game();
            game.Start();
        }
    }
    class Ammus
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }
        public int ExplosionSize { get; set; }
        public string Color { get; set; }
        public int Damage { get; set; }
        public double Weight { get; set; }
        public string Name { get; set; }

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
        public double X { get; set; }
        public double Y { get; set; }
        public int Health { get; set; }
        public double Angle { get; set; }
        public string PlayerName { get; set; }
        public ConsoleColor Color { get; set; }

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

        // تغيير زاوية المدفع
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

            Console.Beep(300, 100); 
            return ammus;
        }

        public void Draw()
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = Color;

            Console.SetCursorPosition((int)X, (int)Y);
            Console.Write("▀");

            int barrelX = (int)(X + Math.Cos(Angle) * 2);
            int barrelY = (int)(Y - Math.Sin(Angle) * 2);
            if (barrelX >= 0 && barrelX < Console.WindowWidth && barrelY >= 0 && barrelY < Console.WindowHeight)
            {
                Console.SetCursorPosition(barrelX, barrelY);
                Console.Write("■");
            }

            Console.ForegroundColor = originalColor;
        }
    }

    class TerrainBlock
    {
        public static int Width = 2; 
        public int Height { get; set; }
        public int X { get; set; }

        public void Draw()
        {
            Console.SetCursorPosition(X, Height);
            for (int i = 0; i < Width; i++)
            {
                Console.Write("█");
            }


        }

        public bool CheckCollision(Ammus ammus)
        {
            return ammus.X >= X && ammus.X < X + Width && ammus.Y >= Height;
        }

        public void Damage(int explosionSize)
        {
            Height += explosionSize / 3; 
            if (Height >= Console.WindowHeight)
            {
                Height = Console.WindowHeight - 1;
            }
        }
    }

    class Game
    {
        private List<Tykki> cannons = new List<Tykki>();
        private List<TerrainBlock> terrain = new List<TerrainBlock>();
        private List<Ammus> ammunitionTypes = new List<Ammus>();
        private Ammus currentProjectile = null;
        private int currentPlayerIndex = 0;
        private bool gameOver = false;

        public void Start()
        {
            Console.CursorVisible = false;
            Console.Clear();

            LoadAmmunition();

            GenerateTerrain();

            AddPlayers();

            GameLoop();
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
                        Thread.Sleep(1000);
                    }
                }

                if (ammunitionTypes.Count == 0)
                {
                    Console.WriteLine("No ammunition files found. Using default ammunition.");
                    Thread.Sleep(1000);

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
                Thread.Sleep(2000);

                Ammus smallShell = new Ammus { Name = "Small Shell", ExplosionSize = 3, Color = "Yellow", Damage = 20, Weight = 1 };
                Ammus mediumShell = new Ammus { Name = "Medium Shell", ExplosionSize = 5, Color = "Red", Damage = 30, Weight = 1.5 };
                Ammus heavyShell = new Ammus { Name = "Heavy Shell", ExplosionSize = 8, Color = "Blue", Damage = 50, Weight = 2 };

                ammunitionTypes.Add(smallShell);
                ammunitionTypes.Add(mediumShell);
                ammunitionTypes.Add(heavyShell);
            }
        }

        private void GenerateTerrain()
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            Random random = new Random();
            int lastHeight = height / 2;

            for (int x = 0; x < width; x += TerrainBlock.Width)
            {
                
                int newHeight = lastHeight + random.Next(-2, 3);

                if (newHeight < height / 3) newHeight = height / 3;
                if (newHeight > height - 5) newHeight = height - 5;

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
                X = random.Next(5, Console.WindowWidth / 3),
                Angle = Math.PI / 4,
                Health = 100,
                PlayerName = "Player 1",
                Color = ConsoleColor.Blue
            };

            Tykki player2 = new Tykki
            {
                X = random.Next(Console.WindowWidth * 2 / 3, Console.WindowWidth - 5),
                Angle = Math.PI * 3 / 4,
                Health = 100,
                PlayerName = "Player 2",
                Color = ConsoleColor.Red
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
            while (!gameOver)
            {
                DrawGame();

                ProcessPlayerInput();

                UpdateProjectile();

                Thread.Sleep(50); 
            }
        }

        private void DrawGame()
        {
            Console.Clear();

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
                ConsoleColor originalColor = Console.ForegroundColor;
                switch (currentProjectile.Color.ToLower())
                {
                    case "red": Console.ForegroundColor = ConsoleColor.Red; break;
                    case "blue": Console.ForegroundColor = ConsoleColor.Blue; break;
                    case "yellow": Console.ForegroundColor = ConsoleColor.Yellow; break;
                    default: Console.ForegroundColor = ConsoleColor.White; break;
                }

                int projX = (int)currentProjectile.X;
                int projY = (int)currentProjectile.Y;

                if (projX >= 0 && projX < Console.WindowWidth && projY >= 0 && projY < Console.WindowHeight)
                {
                    Console.SetCursorPosition(projX, projY);
                    Console.Write("●");
                }

                Console.ForegroundColor = originalColor;
            }

            Tykki currentPlayer = cannons[currentPlayerIndex];
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = currentPlayer.Color;
            Console.WriteLine($"Turn: {currentPlayer.PlayerName} - Health: {currentPlayer.Health}");
            Console.WriteLine($"Angle: {Math.Round(currentPlayer.Angle * 180 / Math.PI, 1)}°");

            Console.SetCursorPosition(0, 2);
            Console.WriteLine("Ammunition:");
            for (int i = 0; i < ammunitionTypes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {ammunitionTypes[i].Name} (Dmg: {ammunitionTypes[i].Damage}, Exp: {ammunitionTypes[i].ExplosionSize})");
            }
        }

        private void ProcessPlayerInput()
        {
            if (currentProjectile != null) return; 

            Tykki currentPlayer = cannons[currentPlayerIndex];

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        currentPlayer.AdjustAngle(0.1);
                        break;
                    case ConsoleKey.DownArrow:
                        currentPlayer.AdjustAngle(-0.1);
                        break;

                    case ConsoleKey.LeftArrow:
                        currentPlayer.Move(-1, terrain);
                        break;
                    case ConsoleKey.RightArrow:
                        currentPlayer.Move(1, terrain);
                        break;

                    case ConsoleKey.D1:
                    case ConsoleKey.D2:
                    case ConsoleKey.D3:
                        int ammoIndex = (int)key.Key - (int)ConsoleKey.D1;
                        if (ammoIndex >= 0 && ammoIndex < ammunitionTypes.Count)
                        {
                            SelectAmmunition(ammoIndex);
                        }
                        break;

                    case ConsoleKey.Spacebar:
                        FireCannon();
                        break;
                }
            }
        }

        private void SelectAmmunition(int index)
        {
            Console.SetCursorPosition(0, 6);
            Console.WriteLine($"Selected: {ammunitionTypes[index].Name}      ");
            
            ammunitionTypes[index].Y = -1; 
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

            if (currentProjectile.X < 0 || currentProjectile.X >= Console.WindowWidth ||
                currentProjectile.Y >= Console.WindowHeight)
            {
                EndTurn();
                return;
            }

            int terrainIndex = (int)(currentProjectile.X / TerrainBlock.Width);
            if (terrainIndex >= 0 && terrainIndex < terrain.Count)
            {
                if (terrain[terrainIndex].CheckCollision(currentProjectile))
                {
                    Console.Beep(150, 100);

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

                if (distance <= currentProjectile.ExplosionSize)
                {
                    int damage = (int)(currentProjectile.Damage * (1 - distance / currentProjectile.ExplosionSize));
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

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2);

                if (playersAlive == 1)
                {
                    Console.WriteLine($"{cannons[lastAliveIndex].PlayerName} WINS!");
                    
                    Console.Beep(523, 100);
                    Console.Beep(659, 100);
                    Console.Beep(784, 300);
                }
                else
                {
                    Console.WriteLine("DRAW GAME!");
                }

                Console.ReadKey();
            }
        }
    }
}
