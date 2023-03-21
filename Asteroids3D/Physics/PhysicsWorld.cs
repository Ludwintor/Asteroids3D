using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Asteroids3D.Physics
{
    public class PhysicsWorld
    {
        private Octree _octree;
        private List<Collider> _colliders;
        private List<Pair<Collider>> _intersectionsBuffer;

        public PhysicsWorld(BoundingBox octreeBox)
        {
            _octree = new Octree(octreeBox);
            _colliders = new List<Collider>();
            _intersectionsBuffer = new List<Pair<Collider>>();
        }

        public void Update(GameTime time)
        {
            _octree.Clear();
            foreach (Collider collider in _colliders)
                _octree.Add(collider);
            _intersectionsBuffer.Clear();
            _octree.FindAllIntersections(_intersectionsBuffer);
            ResolveCollisions();
        }

        public void AddCollider(Collider collider)
        {
            _colliders.Add(collider);
        }

        public void RemoveCollider(Collider collider)
        {
            _colliders.Remove(collider);
        }

        public void Draw()
        {
            _octree.Draw();
            foreach (Collider collider in _colliders)
                collider.Draw(Color.Yellow);
        }

        private void ResolveCollisions()
        {
            foreach (Pair<Collider> pair in _intersectionsBuffer)
            {
                pair.Left.Collide(pair.Right);
                pair.Right.Collide(pair.Left);
            }
        }
    }
}
