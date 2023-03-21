using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asteroids3D.Core;
using Microsoft.Xna.Framework;

namespace Asteroids3D.Physics
{
    public abstract class Collider
    {
        public event Action<CollisionInfo> Collided;

        public Collider(Transform transform)
        {
            Transform = transform;
        }

        public Transform Transform { get; }
        
        public abstract BoundingBox BoxBounds { get; }

        public abstract bool Intersects(Collider other);

        public abstract void Collide(Collider other);

        public abstract void Draw(Color color);

        protected void RaiseCollided(CollisionInfo info)
        {
            Collided?.Invoke(info);
        }
    }
}
