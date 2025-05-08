using Raylib_cs;
using System.Numerics;

namespace ARTILLERY
{
    class Ammus
    {
        public Vector2 position;
        public Vector2 Velocity;
        public int ExplosionSize ;
        public Color Color ;
        public int Damage ;
        public float Weight ;
        public string Name ;

        public Ammus( int explosionSize, Color color, int damage, float weight, string name)
        {
            
            ExplosionSize = explosionSize;
            Color = color;
            Damage = damage;
            Weight = weight;
            Name = name;
        }

        public Ammus Clone()
        {
            return new Ammus( ExplosionSize, Color, Damage, Weight, Name);
        }
        public void Update()
        {
            float gravity = 20;
            Velocity.Y += gravity * Raylib.GetFrameTime()*4;

            position += Velocity* Raylib.GetFrameTime()*4;
        }
        
    }
}
