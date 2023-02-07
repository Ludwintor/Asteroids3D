using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids3D
{
    public class GameObject
    {
        public MeshRenderer MeshRenderer { get; set; }
        public BoxCollider Collider { get; set; }

        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = Vector3.One;
        public Quaternion Rotation { get; set; } = Quaternion.Identity;

        public Matrix Transform => Matrix.CreateTranslation(Position) * Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateScale(Scale);

        public void Update(GameTime time)
        {

        }

        public void Draw(GraphicsDevice device, Matrix view)
        {
            MeshRenderer.Draw(device, Transform, view);
        }
    }
}
