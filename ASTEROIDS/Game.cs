using Raylib_cs;
using RayGuiCreator;
using System.Collections.Generic;
using System.Numerics;

namespace ASTEROIDS
{
    public class Game
    {
        private Player player;
        private List<Asteroid> asteroids;
        private List<Enemy> enemies;
        private List<Bullet> bullets;
        private int score = 0;
        private int level = 1;
        private Random random = new Random();
        private bool gameOver = false;
        public GameState state = GameState.MainMenu;
        public Game()
        {
            
        }

        private void InitGame()
        {
            
            AssetManager.LoadAssets();

            player = new Player(
                new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2),
                25
            );

            asteroids = new List<Asteroid>();
            enemies = new List<Enemy>();
            bullets = new List<Bullet>();


            // Reset score and level
            score = 0;
            level = 1;
            gameOver = false;

            StartLevel();

            Raylib.PlayMusicStream(AssetManager.BackgroundMusic);
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
                    //the enemies position, 50 from each corner + it checks if its 50 near the player
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
            

            if (!gameOver)
            {
                float deltaTime = Raylib.GetFrameTime();

                // HIGHLIGHT: Update music
                Raylib.UpdateMusicStream(AssetManager.BackgroundMusic);

                if (Raylib.IsKeyPressed(KeyboardKey.Space) && player.CurrentCooldown <= 0)
                {
                    bullets.Add(player.Shoot());
                    player.CurrentCooldown = player.ShootCooldown;
                }

                player.Update(deltaTime);

                foreach (var asteroid in asteroids)
                {
                    asteroid.Update(deltaTime);
                }

                foreach (var enemy in enemies)
                {
                    enemy.Update(deltaTime);

                    if (enemy.CurrentCooldown <= 0)
                    {
                        bullets.Add(enemy.Shoot());
                    }
                }

                for (int i = bullets.Count - 1; i >= 0; i--)
                {
                    bullets[i].Update(deltaTime);
                    if (!bullets[i].IsActive)
                    {
                        bullets.RemoveAt(i);
                    }
                }
                // Check collisions between player bullets and asteroids
                for (int i = bullets.Count - 1; i >= 0; i--)
                {
                    Bullet bullet = bullets[i];

                    if (bullet.Source != "player") continue;

                    // Check collision with asteroids
                    bool bulletHit = false;
                    for (int j = asteroids.Count - 1; j >= 0 && !bulletHit; j--)
                    {
                        Asteroid asteroid = asteroids[j];

                        if (bullet.CheckCollision(asteroid))
                        {
                            // Split asteroid into smaller ones
                            asteroids.AddRange(asteroid.Split());
                            asteroids.RemoveAt(j);
                            bullets.RemoveAt(i);
                            bulletHit = true;
                            // Add score based on asteroid size
                            score += (4 - asteroid.Size) * 100;

                            // Play explosion sound
                            Raylib.PlaySound(AssetManager.ExplosionSound);
                        }
                    }

                    // Check collision with enemies
                    if (!bulletHit && i < bullets.Count)
                    {
                        for (int j = enemies.Count - 1; j >= 0; j--)
                        {
                            Enemy enemy = enemies[j];

                            if (bullet.CheckCollision(enemy))
                            {
                                enemies.RemoveAt(j);
                                bullets.RemoveAt(i);
                                score += 500;

                                // Play explosion sound
                                Raylib.PlaySound(AssetManager.ExplosionSound);
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
                            state = GameState.LoseScreen;
                        }
                        else
                        {
                            player.Reset();
                        }

                        //Play explosion sound
                        Raylib.PlaySound(AssetManager.ExplosionSound);
                        break;
                    }
                }

                // Check player collision with enemy bullets

                for (int i = bullets.Count - 1; i >= 0; i--)
                {
                    Bullet bullet = bullets[i];

                    if (bullet.Source != "enemy") continue;

                    if (player.CheckCollision(bullet))
                    {
                        player.Lives--;
                        bullets.RemoveAt(i);

                        if (player.Lives <= 0)
                        {
                            gameOver = true;
                        }
                        else
                        {
                            player.Reset();
                        }

                        // Play explosion sound
                        Raylib.PlaySound(AssetManager.ExplosionSound);
                        break;
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
        public void DrawMainMenu()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            int buttonW= 200;
            int buttonH= 100;
            int buttonX= 300;
            int buttonY = 300;

            Raylib.DrawText("ASTEROIDS", buttonX-50, buttonY-100, 50, Color.White);
            MenuCreator mainMenu = new MenuCreator(buttonX, buttonY,16, buttonW);
            if(mainMenu.Button("Start Game"))
            {
                state = GameState.Game;
                InitGame();
            }
            if (mainMenu.Button("Quit"))
            {
                state= GameState.QuitGame;

            }
            Raylib.EndDrawing();

        }
        public void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
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
                foreach (var bullet in bullets)
                {
                    bullet.Draw();
                }

                // Draw UI elements
                Raylib.DrawText($"Score: {score}", 10, 10, 20, Color.White);
                Raylib.DrawText($"Level: {level}", 10, 40, 20, Color.White);
                Raylib.DrawText($"Lives: {player.Lives}", 10, 70, 20, Color.White);
                Raylib.EndDrawing();


        }
        public void DrawLoseScreen()
        {
            // Draw game over scree
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
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
                Color.White);
            if (Raylib.IsKeyPressed(KeyboardKey.Enter))
            {
                state = GameState.MainMenu;
            }
            Raylib.EndDrawing();
        }
        public void CleanUp()
        {
            // Unload all assets when game is closed
            AssetManager.UnloadAssets();
        }
    }
}
