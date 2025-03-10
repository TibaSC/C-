using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class GameObject
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Rotation;
        public bool IsActive = true;
        public float Radius;

        public GameObject(Vector2 position, float radius)
        {
            Position = position;
            Velocity = new Vector2(0, 0);
            Rotation = 0;
            Radius = radius;
        }

        public virtual void Update(float deltaTime)
        {
            Position += Velocity * deltaTime;

            // Screen wrapping
            int screenWidth = Raylib.GetScreenWidth();
            int screenHeight = Raylib.GetScreenHeight();

            if (Position.X < 0 - Radius) Position.X = screenWidth + Radius;
            if (Position.X > screenWidth + Radius) Position.X = 0 - Radius;
            if (Position.Y < 0 - Radius) Position.Y = screenHeight + Radius;
            if (Position.Y > screenHeight + Radius) Position.Y = 0 - Radius;
        }

        public virtual void Draw()
        {
            
        }

        public bool CheckCollision(GameObject other)
        {
            return Vector2.Distance(Position, other.Position) < (Radius + other.Radius);
        }
    }

    public class Player : GameObject
    {
        public Sound shootSound;
        public float RotationSpeed = 5.0f;
        public float Acceleration = 200.0f;
        public float MaxSpeed = 300.0f;
        public float Friction = 0.98f;
        public Texture2D Texture;
        public int Lives = 3;
        public List<Bullet> Bullets = new List<Bullet>();
        public float ShootCooldown = 0.25f;
        public float CurrentCooldown = 0;

        public Player(Vector2 position, float radius) : base(position, radius)
        {
            // HIGHLIGHT: Player ship texture loading
            Texture = Raylib.LoadTexture("Images/playerShip.png");
            shootSound = Raylib.LoadSound("Sounds/laser.wav");
        }

        public override void Update(float deltaTime)
        {
            // Rotation
            if (Raylib.IsKeyDown(KeyboardKey.Left)) Rotation -= RotationSpeed * deltaTime;
            if (Raylib.IsKeyDown(KeyboardKey.Right)) Rotation += RotationSpeed * deltaTime;

            // Thrust
            if (Raylib.IsKeyDown(KeyboardKey.Up))
            {
                Vector2 direction = new Vector2(
                    (float)Math.Sin(Rotation),
                    -(float)Math.Cos(Rotation)
                );

                Velocity += direction * Acceleration * deltaTime;

                float currentSpeed = Vector2.Distance(Vector2.Zero, Velocity);
                if (currentSpeed > MaxSpeed)
                {
                    Velocity = Vector2.Normalize(Velocity) * MaxSpeed;
                }
            }

            Velocity *= Friction;

            // Shooting
            if (CurrentCooldown > 0)
                CurrentCooldown -= deltaTime;

            if (Raylib.IsKeyPressed(KeyboardKey.Space) && CurrentCooldown <= 0)
            {
                Shoot();
                CurrentCooldown = ShootCooldown;
            }

            // Update bullets
            for (int i = Bullets.Count - 1; i >= 0; i--)
            {
                Bullets[i].Update(deltaTime);
                if (!Bullets[i].IsActive)
                {
                    Bullets.RemoveAt(i);
                }
            }

            base.Update(deltaTime);
        }

        public void Shoot()
        {
            Vector2 direction = new Vector2(
            (float)Math.Sin(Rotation),
                -(float)Math.Cos(Rotation)
            );

            Vector2 bulletPos = Position + direction * 30;
            Bullet bullet = new Bullet(bulletPos, direction, Rotation);
            Bullets.Add(bullet);
            Raylib.PlaySound(shootSound);
        }

        public override void Draw()
        {
            // Draw bullets
            foreach (var bullet in Bullets)
            {
                bullet.Draw();
            }

            // Draw ship with rotation
            Raylib.DrawTexturePro(
                Texture,
                new Rectangle(0, 0, Texture.Width, Texture.Height),
                new Rectangle(Position.X, Position.Y, Radius * 2, Radius * 2),
                new Vector2(Radius, Radius),
                Rotation * Raylib.RAD2DEG,
                Color.White
            );

        }

        public void Reset()
        {
            Position = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
            Velocity = new Vector2(0, 0);
            Rotation = 0;
            Bullets.Clear();
        }
    }

    public class Bullet : GameObject
    {
        public float Speed = 500.0f;
        public float LifeTime = 2.0f;
        public float CurrentLife;

        public Bullet(Vector2 position, Vector2 direction, float rotation) : base(position, 5)
        {
            Velocity = direction * Speed;
            Rotation = rotation;
            CurrentLife = LifeTime;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Decrease lifetime
            CurrentLife -= deltaTime;
            if (CurrentLife <= 0)
            {
                IsActive = false;
            }
        }

        public override void Draw()
        {
            Raylib.DrawCircle((int)Position.X, (int)Position.Y, Radius, Color.White);
        }
    }

    public class Asteroid : GameObject
    {
        public int Size; // 3 = large, 2 = medium, 1 = small
        public Texture2D Texture;

        public Asteroid(Vector2 position, int size) : base(position, size * 20)
        {
            Size = size;

            // Random velocity
            Random rand = new Random();
            float angle = (float)(rand.NextDouble() * Math.PI * 2);
            float speed = (float)(rand.NextDouble() * 30) + 20;
            Velocity = new Vector2(
                (float)Math.Sin(angle) * speed,
                (float)Math.Cos(angle) * speed
            );

            Rotation = (float)(rand.NextDouble() * Math.PI * 2);

            switch (size)
            {
                case 3:
                    Texture = Raylib.LoadTexture("Images/meteorBig.png");
                    break;
                case 2:
                    Texture = Raylib.LoadTexture("Images/meteorMedium.png");
                    break;
                case 1:
                    Texture = Raylib.LoadTexture("Images/meteorSmall.png"); 
                    break;
            }
        }

        public override void Draw()
        {
            
            Raylib.DrawTexturePro(
                Texture,
                new Rectangle(0, 0, Texture.Width, Texture.Height),
                new Rectangle(Position.X, Position.Y, Radius * 2, Radius * 2),
                new Vector2(Radius, Radius),
                Rotation * Raylib.RAD2DEG,
                Color.White
            );

        }

        public List<Asteroid> Split()
        {
            List<Asteroid> newAsteroids = new List<Asteroid>();

            if (Size > 1)
            {
                // Create 2 smaller asteroids
                for (int i = 0; i < 2; i++)
                {
                    Asteroid newAsteroid = new Asteroid(Position, Size - 1);
                    newAsteroids.Add(newAsteroid);
                }
            }

            return newAsteroids;
        }
    }

    public class Enemy : GameObject
    {
        public Texture2D Texture;
        public float ShootCooldown = 2.0f;
        public float CurrentCooldown = 0;
        public List<Bullet> Bullets = new List<Bullet>();
        private Vector2 moveDirection;
        private float moveTimer;
        private Player player;

        public Enemy(Vector2 position, Player targetPlayer) : base(position, 25)
        {
            Texture = Raylib.LoadTexture("Images/enemyShip.png");
            player = targetPlayer;

            Random rand = new Random();
            float angle = (float)(rand.NextDouble() * Math.PI * 2);
            moveDirection = new Vector2(
                (float)Math.Sin(angle),
                (float)Math.Cos(angle)
            );
            moveTimer = (float)rand.NextDouble() * 3 + 1;
        }

        public override void Update(float deltaTime)
        {
            moveTimer -= deltaTime;
            if (moveTimer <= 0)
            {
                Random rand = new Random();
                float angle = (float)(rand.NextDouble() * Math.PI * 2);
                moveDirection = new Vector2(
                    (float)Math.Sin(angle),
                    (float)Math.Cos(angle)
                );
                moveTimer = (float)rand.NextDouble() * 3 + 1;
            }

            Velocity = moveDirection * 50;

            // Always face the player
            Vector2 direction = Vector2.Normalize(player.Position - Position);
            Rotation = (float)Math.Atan2(direction.Y, direction.X) + MathF.PI / 2;

            // Shooting logic
            if (CurrentCooldown > 0)
                CurrentCooldown -= deltaTime;

            if (CurrentCooldown <= 0)
            {
                Shoot();
                CurrentCooldown = ShootCooldown;
            }

            // Update bullets
            for (int i = Bullets.Count - 1; i >= 0; i--)
            {
                Bullets[i].Update(deltaTime);
                if (!Bullets[i].IsActive)
                {
                    Bullets.RemoveAt(i);
                }
            }

            base.Update(deltaTime);
        }

        private void Shoot()
        {
            Vector2 direction = Vector2.Normalize(player.Position - Position);
            Vector2 bulletPos = Position + direction * 30;
            Bullet bullet = new Bullet(bulletPos, direction, Rotation);
            bullet.Speed = 200; 
            Bullets.Add(bullet);
        }

        public override void Draw()
        {
            // Draw bullets
            foreach (var bullet in Bullets)
            {
                bullet.Draw();
            }

            // Draw enemy ship
            Raylib.DrawTexturePro(
                Texture,
                new Rectangle(0, 0, Texture.Width, Texture.Height),
                new Rectangle(Position.X, Position.Y, Radius * 2, Radius * 2),
                new Vector2(Radius, Radius),
                Rotation * Raylib.RAD2DEG,
                Color.White
            );

        }
    }

    public class Game
    {
        private Player player;
        private List<Asteroid> asteroids;
        private List<Enemy> enemies;
        private int score = 0;
        private int level = 1;
        private Random random = new Random();
        private Music backgroundMusic;
        private Sound explosionSound;
        private bool gameOver = false;

        public Game()
        {
            InitGame();
        }

        private void InitGame()
        {
            player = new Player(
                new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2),
                25
            );

            asteroids = new List<Asteroid>();
            enemies = new List<Enemy>();

            // Reset score and level
            score = 0;
            level = 1;
            gameOver = false;

            StartLevel();

            // HIGHLIGHT: Load sound effects
            
            backgroundMusic = Raylib.LoadMusicStream("Sounds/background.wav");
            explosionSound = Raylib.LoadSound("Sounds/explosion.wav");
            Raylib.PlayMusicStream(backgroundMusic);
        }

        private void StartLevel()
        {
            // Clear existing asteroids and enemies
            asteroids.Clear();
            enemies.Clear();

            player.Reset();

            int numAsteroids = 2 + level;
            for (int i = 0; i < numAsteroids; i++)
            {
                Vector2 position;
                do
                {
                    position = new Vector2(
                        random.Next(50, Raylib.GetScreenWidth() - 50),
                        random.Next(50, Raylib.GetScreenHeight() - 50)
                    );
                } while (Vector2.Distance(position, player.Position) < 150);

                asteroids.Add(new Asteroid(position, 3));
            }

            int numEnemies = (level - 1) / 2;
            for (int i = 0; i < numEnemies; i++)
            {
                Vector2 position;
                do
                {
                    position = new Vector2(
                        random.Next(50, Raylib.GetScreenWidth() - 50),
                        random.Next(50, Raylib.GetScreenHeight() - 50)
                    );
                } while (Vector2.Distance(position, player.Position) < 200);

                enemies.Add(new Enemy(position, player));
            }
        }

        public void Update()
        {
            if (gameOver && Raylib.IsKeyPressed(KeyboardKey.Enter))
            {
                InitGame();
                return;
            }

            if (!gameOver)
            {
                float deltaTime = Raylib.GetFrameTime();

                // HIGHLIGHT: Update music
                Raylib.UpdateMusicStream(backgroundMusic);

                player.Update(deltaTime);

                foreach (var asteroid in asteroids)
                {
                    asteroid.Update(deltaTime);
                }

                foreach (var enemy in enemies)
                {
                    enemy.Update(deltaTime);
                }

                // Check collisions between player bullets and asteroids
                for (int i = player.Bullets.Count - 1; i >= 0; i--)
                {
                    Bullet bullet = player.Bullets[i];

                    // Check collision with asteroids
                    for (int j = asteroids.Count - 1; j >= 0; j--)
                    {
                        Asteroid asteroid = asteroids[j];

                        if (bullet.CheckCollision(asteroid))
                        {
                            // Split asteroid into smaller ones
                            asteroids.AddRange(asteroid.Split());

                            asteroids.RemoveAt(j);

                            player.Bullets.RemoveAt(i);

                            // Add score based on asteroid size
                            score += (4 - asteroid.Size) * 100;

                            // HIGHLIGHT: Play explosion sound
                             Raylib.PlaySound(explosionSound);

                            break; // Bullet can only hit one asteroid
                        }
                    }

                    // Check collision with enemies
                    if (i < player.Bullets.Count)
                    {
                        for (int j = enemies.Count - 1; j >= 0; j--)
                        {
                            Enemy enemy = enemies[j];

                            if (bullet.CheckCollision(enemy))
                            {
                                enemies.RemoveAt(j);

                                player.Bullets.RemoveAt(i);

                                score += 500;

                                // HIGHLIGHT: Play explosion sound
                                 Raylib.PlaySound(explosionSound);

                                break; // Bullet can only hit one enemy
                            }
                        }
                    }
                }

                // Check player collision with asteroids
                foreach (var asteroid in asteroids)
                {
                    if (player.CheckCollision(asteroid))
                    {
                        player.Lives--;

                        if (player.Lives <= 0)
                        {
                            gameOver = true;
                        }
                        else
                        {
                            player.Reset();
                        }

                        //Play explosion sound
                         Raylib.PlaySound(explosionSound);

                        break;
                    }
                }

                // Check player collision with enemy bullets
                foreach (var enemy in enemies)
                {
                    for (int i = enemy.Bullets.Count - 1; i >= 0; i--)
                    {
                        Bullet bullet = enemy.Bullets[i];

                        if (player.CheckCollision(bullet))
                        {
                            player.Lives--;
                            enemy.Bullets.RemoveAt(i);

                            if (player.Lives <= 0)
                            {
                                gameOver = true;
                            }
                            else
                            {
                                player.Reset();
                            }

                            // Play explosion sound
                             Raylib.PlaySound(explosionSound);

                            break;
                        }
                    }
                }

                // Check if all asteroids are destroyed to move to next level
                if (asteroids.Count == 0)
                {
                    level++;
                    StartLevel();
                }
            }
        }

        public void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            // Draw game elements
            if (!gameOver)
            {
                // Draw player
                player.Draw();

                // Draw asteroids
                foreach (var asteroid in asteroids)
                {
                    asteroid.Draw();
                }

                // Draw enemies
                foreach (var enemy in enemies)
                {
                    enemy.Draw();
                }

                // Draw UI elements
                Raylib.DrawText($"Score: {score}", 10, 10, 20, Color.White);
                Raylib.DrawText($"Level: {level}", 10, 40, 20, Color.White);
                Raylib.DrawText($"Lives: {player.Lives}", 10, 70, 20, Color.White);
            }
            else
            {
                // Draw game over screen
                string gameOverText = "GAME OVER";
                int textWidth = Raylib.MeasureText(gameOverText, 50);
                Raylib.DrawText(
                    gameOverText,
                    Raylib.GetScreenWidth() / 2 - textWidth / 2,
                    Raylib.GetScreenHeight() / 2 - 50,
                    50,
                    Color.Red
                );

                string scoreText = $"Final Score: {score}";
                int scoreWidth = Raylib.MeasureText(scoreText, 30);
                Raylib.DrawText(
                    scoreText,
                    Raylib.GetScreenWidth() / 2 - scoreWidth / 2,
                    Raylib.GetScreenHeight() / 2 + 10,
                    30,
                    Color.White
                );

                string restartText = "Press ENTER to restart";
                int restartWidth = Raylib.MeasureText(restartText, 20);
                Raylib.DrawText(
                    restartText,
                    Raylib.GetScreenWidth() / 2 - restartWidth / 2,
                    Raylib.GetScreenHeight() / 2 + 60,
                    20,
                    Color.White
                );
            }

            Raylib.EndDrawing();
        }
    }

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

            Raylib.CloseWindow();
        }
    }
}
