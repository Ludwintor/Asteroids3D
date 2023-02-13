using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids3D
{
    public class MeshRenderer
    {
        public MeshBase Mesh { get; set; }
        public BasicEffect Effect { get; set; }
        public Quaternion Rotation { get; set; }

        public void Draw(GraphicsDevice device, Matrix transform, Matrix view)
        {
            Mesh.ReadBuffers(device);
            Effect.World = transform * Matrix.CreateFromQuaternion(Rotation);
            Effect.View = view;
            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Mesh.IndexCount / 3);
            }
        }
    }
}
