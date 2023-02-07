using Microsoft.Xna.Framework;

namespace Asteroids3D
{
    public class BoxCollider
    {
        public BoundingBox BoundingBox { get; }

        public BoxCollider(BoundingBox box)
        {
            BoundingBox = box;
        }
    }
}
