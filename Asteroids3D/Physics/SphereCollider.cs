using System;
using Asteroids3D.Core;
using Microsoft.Xna.Framework;

namespace Asteroids3D.Physics
{
    public class SphereCollider : Collider
    {
        private float _radius;

        public SphereCollider(Transform transform, float radius) : base (transform)
        {
            _radius = radius;
        }

        public override BoundingBox BoxBounds => new(Transform.Position - Vector3.One * _radius, Transform.Position + Vector3.One * _radius);
        public BoundingSphere Bounds => new(Transform.Position, _radius);

        public override bool Intersects(Collider other)
        {
            return other switch
            {
                SphereCollider sphere => Bounds.Intersects(sphere.Bounds),
                _ => throw new NotImplementedException()
            };
        }

        public override void Collide(Collider other)
        {
            Vector3 diff = Transform.Position - other.Transform.Position;
            RaiseCollided(new CollisionInfo(other, Vector3.Normalize(diff)));
        }

        public override void Draw(Color color)
        {
            Gizmos.DrawSphere(Transform.Position, _radius, color);
        }
    }
}
