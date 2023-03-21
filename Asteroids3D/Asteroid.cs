using Microsoft.Xna.Framework;
using Asteroids3D.Mesh;
using Asteroids3D.Physics;
using System;

namespace Asteroids3D
{
    public class Asteroid : GameObject
    {
        public event Action<Asteroid> Destroyed;

        public Asteroid(Game1 game) : base(game)
        {
            MeshRenderer = new MeshRenderer()
            {
                Mesh = new SphereMesh(game.GraphicsDevice, 40, 40, RandomAsteroidColor),
                Effect = game.DefaultFX
            };
            Transform.Scale = Vector3.One * 0.75f;
            Collider = new SphereCollider(Transform, Transform.Scale.X);
            Collider.Collided += OnCollision;
        }

        public override void Destroy()
        {
            Destroyed?.Invoke(this);
            base.Destroy();
        }

        private void OnCollision(CollisionInfo info)
        {
            Destroy();
        }

        private Color RandomAsteroidColor()
        {
            int gray = Random.Shared.Next(45, 125);
            return new Color(gray, gray, gray);
        }
    }
}
