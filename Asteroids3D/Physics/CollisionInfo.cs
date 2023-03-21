using Microsoft.Xna.Framework;

namespace Asteroids3D.Physics
{
    public readonly struct CollisionInfo
    {
        public Collider Other { get; }

        public Vector3 Normal { get; }

        public CollisionInfo(Collider other, Vector3 normal)
        {
            Other = other;
            Normal = normal;
        }
    }
}
