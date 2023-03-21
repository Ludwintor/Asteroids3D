using Microsoft.Xna.Framework;

namespace Asteroids3D.Core
{
    public class Transform
    {
        public Vector3 Position { get; set; } = Vector3.One;
        public Vector3 Scale { get; set; } = Vector3.One;
        public Quaternion Rotation { get; set; } = Quaternion.Identity;
        public Vector3 Pivot { get; set; } = Vector3.Zero;

        public Matrix Matrix => Matrix.CreateTranslation(Pivot) * Matrix.CreateScale(Scale) *
                                Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateTranslation(Position);
    }
}
