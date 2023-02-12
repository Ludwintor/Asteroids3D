using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids3D
{
    public static class Gizmos
    {
        private static GraphicsDevice _device;
        private static Camera _camera;
        private static BasicEffect _effect;
        private static VertexPosition[] _wireframeVerts;
        private static int[] _wireframeIndices;
        private static VertexBuffer _wireframeVertBuffer;
        private static IndexBuffer _wireframeIndexBuffer;

        public static void Init(GraphicsDevice device, Camera camera)
        {
            _device = device;
            _camera = camera;
            _effect = new BasicEffect(device);
            _wireframeVerts = new VertexPosition[8];
            _wireframeIndices = new int[24];
            _wireframeVerts[0] = new VertexPosition(-Vector3.One);
            _wireframeVerts[1] = new VertexPosition(new Vector3(1f, -1f, -1f));
            _wireframeVerts[2] = new VertexPosition(new Vector3(1f, -1f, 1f));
            _wireframeVerts[3] = new VertexPosition(new Vector3(-1f, -1f, 1f));
            _wireframeVerts[4] = new VertexPosition(new Vector3(-1f, 1f, -1f));
            _wireframeVerts[5] = new VertexPosition(new Vector3(1f, 1f, -1f));
            _wireframeVerts[6] = new VertexPosition(Vector3.One);
            _wireframeVerts[7] = new VertexPosition(new Vector3(-1f, 1f, 1f));
            for (int i = 0; i < 4; i++)
            {
                // Connect lower plane
                _wireframeIndices[i * 6 + 0] = i;
                _wireframeIndices[i * 6 + 1] = (i + 1) % 4;
                // Connect top plane
                _wireframeIndices[i * 6 + 2] = i + 4;
                _wireframeIndices[i * 6 + 3] = (i + 1) % 4 + 4;
                // Connect down and top points
                _wireframeIndices[i * 6 + 4] = i;
                _wireframeIndices[i * 6 + 5] = i + 4;
            }
            _wireframeVertBuffer = new VertexBuffer(device, VertexPosition.VertexDeclaration, _wireframeVerts.Length, BufferUsage.WriteOnly);
            _wireframeVertBuffer.SetData(_wireframeVerts);
            _wireframeIndexBuffer = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits, _wireframeIndices.Length, BufferUsage.WriteOnly);
            _wireframeIndexBuffer.SetData(_wireframeIndices);
        }

        public static void DrawWireframeCube(Vector3 centerPosition, Vector3 size, Color color)
        {
            _effect.World = Matrix.CreateScale(size / 2f) * Matrix.CreateTranslation(centerPosition);
            _effect.DiffuseColor = color.ToVector3();
            DrawLines(_wireframeVertBuffer, _wireframeIndexBuffer);
        }

        private static void DrawLines(VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
        {
            _device.SetVertexBuffer(vertexBuffer);
            _device.Indices = indexBuffer;
            _effect.Projection = _camera.Projection;
            _effect.View = _camera.View;
            _effect.CurrentTechnique.Passes[0].Apply();
            _device.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, _wireframeIndexBuffer.IndexCount / 2);
        }
    }
}
