using Asteroids3D.Core;
using Asteroids3D.Mesh;
using Asteroids3D.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids3D
{
    public abstract class GameObject
    {
        public GameObject(Game1 game)
        {
            Game = game;
        }

        public MeshRenderer MeshRenderer { get; set; }
        public SphereCollider Collider { get; set; }
        public Transform Transform { get; } = new Transform();

        protected Game1 Game { get; }

        public virtual void Update(GameTime time) { }

        public virtual void Draw()
        {
            MeshRenderer?.Draw(Game.GraphicsDevice, Transform.Matrix, Game.MainCamera.View);
        }

        public virtual void Destroy()
        {
            MeshRenderer?.Dispose();
        }
    }
}
