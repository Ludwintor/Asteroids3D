using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids3D
{
    public class ConeMesh : MeshBase
    {
        public ConeMesh(GraphicsDevice graphics, float height, float radius, Color color, Color headColor, int tessellation = 3)
        {
            AddVertices(height, radius, color, headColor, tessellation);
            AddIndices(tessellation);
            PrepareBuffers(graphics);
        }

        private void AddVertices(float height, float radius, Color color, Color headColor, int tessellation)
        {
            // Top vertex
            AddVertex(new VertexPositionColor(new Vector3(0f, height, 0f), headColor));
            // Center vertex
            AddVertex(new VertexPositionColor(Vector3.Zero, color));

            // Bottom circle verts
            for (int i = 0; i < tessellation; i++)
            {
                Vector3 position = GetCircleVector(i, tessellation) * radius;
                AddVertex(new VertexPositionColor(position, color));
            }
        }

        private void AddIndices(int tessellation)
        {
            for (int i = 0; i < tessellation; i++)
            {
                // + 2 to skip top and bottom verts
                int current = i + 2;
                int next = (i + 1) % tessellation + 2;

                // Tri from circle to top
                AddTri(0, current, next);
                // Tri from circle to center
                AddTri(current, 1, next);
            }
        }

        private static Vector3 GetCircleVector(int i, int tessellation)
        {
            float angle = i * MathHelper.TwoPi / tessellation;
            return new Vector3(MathF.Cos(angle), 0f, MathF.Sin(angle));
        }
    }
}
