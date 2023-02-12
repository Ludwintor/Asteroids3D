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
        public Vector3 Pivot { get; set; } = Vector3.Down / 2f;

        public Matrix Transform => Matrix.CreateTranslation(Pivot) * Matrix.CreateScale(Scale) *
                                   Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateTranslation(Position);

        public void Update(GameTime time)
        {

        }

        public void Draw(GraphicsDevice device, Matrix view)
        {
            MeshRenderer.Draw(device, Transform, view);
        }
    }
}
