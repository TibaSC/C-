using System.Collections.Generic;
using Raylib_cs;

namespace ASTEROIDS
{
    public static class AssetManager
    {
        // Textures
        public static Texture2D PlayerShipTexture;
        public static Texture2D EnemyShipTexture;
        public static Texture2D MeteorBigTexture;
        public static Texture2D MeteorMediumTexture;
        public static Texture2D MeteorSmallTexture;

        // Sounds
        public static Sound LaserSound;
        public static Sound ExplosionSound;
        public static Music BackgroundMusic;

        // Initialize all game assets
        public static void LoadAssets()
        {
            // Load textures
            PlayerShipTexture = Raylib.LoadTexture("Images/playerShip.png");
            EnemyShipTexture = Raylib.LoadTexture("Images/enemyShip.png");
            MeteorBigTexture = Raylib.LoadTexture("Images/meteorBig.png");
            MeteorMediumTexture = Raylib.LoadTexture("Images/meteorMedium.png");
            MeteorSmallTexture = Raylib.LoadTexture("Images/meteorSmall.png");

            // Load sounds
            LaserSound = Raylib.LoadSound("Sounds/laser.wav");
            ExplosionSound = Raylib.LoadSound("Sounds/explosion.wav");
            BackgroundMusic = Raylib.LoadMusicStream("Sounds/background.wav");
        }

        // Clean up resources
        public static void UnloadAssets()
        {
            Raylib.UnloadTexture(PlayerShipTexture);
            Raylib.UnloadTexture(EnemyShipTexture);
            Raylib.UnloadTexture(MeteorBigTexture);
            Raylib.UnloadTexture(MeteorMediumTexture);
            Raylib.UnloadTexture(MeteorSmallTexture);

            Raylib.UnloadSound(LaserSound);
            Raylib.UnloadSound(ExplosionSound);
            Raylib.UnloadMusicStream(BackgroundMusic);
        }
    }
}
