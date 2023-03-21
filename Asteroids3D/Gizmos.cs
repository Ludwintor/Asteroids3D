using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids3D
{
    public static class Gizmos
    {
        private const int CIRCLE_RES = 50;

        private static GraphicsDevice _device;
        private static Camera _camera;
        private static BasicEffect _effect;

        private static VertexBuffer _wireframeVertBuffer;
        private static IndexBuffer _wireframeIndexBuffer;

        private static VertexBuffer _circleVertBuffer;

        private static VertexPosition[] _lineVerts;

        public static void Init(GraphicsDevice device, Camera camera)
        {
            _device = device;
            _camera = camera;
            _effect = new BasicEffect(device);
            #region Wireframe
            VertexPosition[] wireframeVerts = new VertexPosition[8];
            int[] wireframeIndices = new int[24];
            wireframeVerts[0] = new VertexPosition(-Vector3.One);
            wireframeVerts[1] = new VertexPosition(new Vector3(1f, -1f, -1f));
            wireframeVerts[2] = new VertexPosition(new Vector3(1f, -1f, 1f));
            wireframeVerts[3] = new VertexPosition(new Vector3(-1f, -1f, 1f));
            wireframeVerts[4] = new VertexPosition(new Vector3(-1f, 1f, -1f));
            wireframeVerts[5] = new VertexPosition(new Vector3(1f, 1f, -1f));
            wireframeVerts[6] = new VertexPosition(Vector3.One);
            wireframeVerts[7] = new VertexPosition(new Vector3(-1f, 1f, 1f));
            for (int i = 0; i < 4; i++)
            {
                // Connect lower plane
                wireframeIndices[i * 6 + 0] = i;
                wireframeIndices[i * 6 + 1] = (i + 1) % 4;
                // Connect top plane
                wireframeIndices[i * 6 + 2] = i + 4;
                wireframeIndices[i * 6 + 3] = (i + 1) % 4 + 4;
                // Connect down and top points
                wireframeIndices[i * 6 + 4] = i;
                wireframeIndices[i * 6 + 5] = i + 4;
            }
            _wireframeVertBuffer = new VertexBuffer(device, VertexPosition.VertexDeclaration, wireframeVerts.Length, BufferUsage.WriteOnly);
            _wireframeVertBuffer.SetData(wireframeVerts);
            _wireframeIndexBuffer = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits, wireframeIndices.Length, BufferUsage.WriteOnly);
            _wireframeIndexBuffer.SetData(wireframeIndices);
            #endregion

            #region Circle
            VertexPosition[] circleVerts = new VertexPosition[CIRCLE_RES + 1];
            for (int i = 0; i < CIRCLE_RES; i++)
            {
                float angle = MathHelper.TwoPi * i / CIRCLE_RES;
                float x = MathF.Cos(angle);
                float y = MathF.Sin(angle);
                circleVerts[i] = new VertexPosition(new Vector3(x, y, 0f));
            }
            circleVerts[CIRCLE_RES] = circleVerts[0]; 
            _circleVertBuffer = new VertexBuffer(_device, VertexPosition.VertexDeclaration, CIRCLE_RES + 1, BufferUsage.WriteOnly);
            _circleVertBuffer.SetData(circleVerts);
            #endregion

            _lineVerts = new VertexPosition[2];
        }

        public static void DrawWireframeCube(Vector3 centerPosition, Vector3 size, Color color)
        {
            _effect.World = Matrix.CreateScale(size / 2f) * Matrix.CreateTranslation(centerPosition);
            _effect.DiffuseColor = color.ToVector3();
            _effect.Alpha = color.ToVector4().W;
            DrawLines(_wireframeVertBuffer, _wireframeIndexBuffer);
        }

        public static void DrawSphere(Vector3 centerPosition, float radius, Color color)
        {
            _device.SetVertexBuffer(_circleVertBuffer);
            Matrix world = Matrix.CreateScale(radius) * Matrix.CreateTranslation(centerPosition);
            _effect.DiffuseColor = color.ToVector3();
            _effect.Alpha = color.ToVector4().W;
            DrawCircle(world);
            world = Matrix.CreateRotationX(MathHelper.PiOver2) * world;
            DrawCircle(world);
            world = Matrix.CreateRotationY(MathHelper.PiOver2) * world;
            DrawCircle(world);
        }

        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            _effect.World = Matrix.Identity;
            _effect.DiffuseColor = color.ToVector3();
            _effect.Alpha = color.ToVector4().W;
            _lineVerts[0] = new VertexPosition(start);
            _lineVerts[1] = new VertexPosition(end);
            _device.DrawUserPrimitives(PrimitiveType.LineList, _lineVerts, 0, 1);
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

        private static void DrawCircle(Matrix world)
        {
            _effect.World = world;
            _effect.Projection = _camera.Projection;
            _effect.View = _camera.View;
            _effect.CurrentTechnique.Passes[0].Apply();
            _device.DrawPrimitives(PrimitiveType.LineStrip, 0, CIRCLE_RES);
        }
    }
}
