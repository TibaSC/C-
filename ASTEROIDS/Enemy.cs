using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class Enemy : GameObject
    {
        public float ShootCooldown = 2.0f;
        public float CurrentCooldown = 0;
        private Vector2 moveDirection;
        private float moveTimer;
        private Player player;

        public Enemy(Vector2 position, Player targetPlayer) : base(position, 25)
        {
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

            base.Update(deltaTime);
        }

        public Bullet Shoot()
        {
            Vector2 direction = Vector2.Normalize(player.Position - Position);
            Vector2 bulletPos = Position + direction * 30;
            Bullet bullet = new Bullet(bulletPos, direction, Rotation, "enemy");
            CurrentCooldown = ShootCooldown;

            return bullet;
        }

        public override void Draw()
        {
            // Draw enemy ship
            Raylib.DrawTexturePro(
                AssetManager.EnemyShipTexture,
                new Rectangle(0, 0, AssetManager.EnemyShipTexture.Width, AssetManager.EnemyShipTexture.Height),
                new Rectangle(Position.X, Position.Y, Radius * 2, Radius * 2),
                new Vector2(Radius, Radius),
                Rotation * Raylib.RAD2DEG,
                Color.White
            );

        }
    }
}
