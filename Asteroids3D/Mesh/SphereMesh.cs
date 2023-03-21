using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.MediaFoundation;

namespace Asteroids3D.Mesh
{
    public class SphereMesh : MeshBase
    {
        public SphereMesh(GraphicsDevice graphics, int slices, int stacks, Func<Color> colorFunc)
        {
            AddVertices(slices, stacks, colorFunc);
            AddIndices(slices, stacks);
            PrepareBuffers(graphics);
        }

        private void AddVertices(int slices, int stacks, Func<Color> colorFunc)
        {
            // Top vert
            AddVertex(new VertexPositionColor(Vector3.Up, colorFunc()));
            // Bottom vert
            AddVertex(new VertexPositionColor(Vector3.Down, colorFunc()));

            // Generate verts per stack / slice
            for (int i = 0; i < stacks - 1; i++)
            {
                float stackAngle = MathHelper.Pi * (i + 1) / stacks;
                for (int j = 0; j < slices; j++)
                {
                    float slicesAngle = MathHelper.TwoPi * j / slices;
                    float x = MathF.Sin(stackAngle) * MathF.Cos(slicesAngle);
                    float y = MathF.Cos(stackAngle);
                    float z = MathF.Sin(stackAngle) * MathF.Sin(slicesAngle);
                    AddVertex(new VertexPositionColor(new Vector3(x, y, z), colorFunc()));
                }
            }
        }

        private void AddIndices(int slices, int stacks)
        {
            // Add top / bottom tris
            for (int i = 0; i < slices; i++)
            {
                // + 2 to skip top and bottom verts
                int a = i + 2;
                int b = (i + 1) % slices + 2;
                AddTri(b, 0, a);
                a = i + slices * (stacks - 2) + 2;
                b = (i + 1) % slices + slices * (stacks - 2) + 2;
                AddTri(a, 1, b);
            }

            // Add quads per stack / slice
            for (int i = 0; i < stacks - 2; i++)
            {
                int i0 = i * slices + 2;
                int i1 = (i + 1) * slices + 2;
                for (int j = 0; j < slices; j++)
                {
                    int a = i0 + j;
                    int b = i1 + j;
                    int c = i0 + (j + 1) % slices;
                    int d = i1 + (j + 1) % slices;
                    AddQuad(a, b, c, d);
                }
            }
        }
    }
}
