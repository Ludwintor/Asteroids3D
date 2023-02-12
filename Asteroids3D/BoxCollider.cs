using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids3D
{
    public class BoxCollider
    {
        public BoundingBox BoundingBox { get; }

        public BoxCollider(BoundingBox box)
        {
            BoundingBox = box;
        }

        public void Draw(GraphicsDevice device, Matrix view)
        {
            Vector3 center = (BoundingBox.Min + BoundingBox.Max) / 2f;
            Vector3 size = BoundingBox.Max - BoundingBox.Min;
            Gizmos.DrawWireframeCube(center, size, Color.Green);
        }
    }
}
