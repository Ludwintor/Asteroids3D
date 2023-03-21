using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids3D.Mesh
{
    public abstract class MeshBase : IDisposable
    {
        private readonly List<VertexPositionColor> _vertices;
        private readonly List<int> _indices;
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;

        public MeshBase()
        {
            _vertices = new List<VertexPositionColor>();
            _indices = new List<int>();
        }

        public int VertexCount => _vertices.Count;
        public int IndexCount => _indices.Count;

        public void ReadBuffers(GraphicsDevice device)
        {
            device.SetVertexBuffer(_vertexBuffer);
            device.Indices = _indexBuffer;
        }

        protected void AddVertex(VertexPositionColor vertex)
        {
            _vertices.Add(vertex);
        }

        protected void AddIndex(int index)
        {
            _indices.Add(index);
        }

        protected void AddTri(int a, int b, int c)
        {
            _indices.Add(a);
            _indices.Add(b);
            _indices.Add(c);
        }

        protected void AddQuad(int a, int b, int c, int d)
        {
            _indices.Add(a);
            _indices.Add(b);
            _indices.Add(c);
            _indices.Add(b);
            _indices.Add(d);
            _indices.Add(c);
        }

        protected void PrepareBuffers(GraphicsDevice device)
        {
            _vertexBuffer = new VertexBuffer(device, VertexPositionColor.VertexDeclaration, _vertices.Count, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(_vertices.ToArray());

            _indexBuffer = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits, _indices.Count, BufferUsage.WriteOnly);
            _indexBuffer.SetData(_indices.ToArray());
        }

        public void Dispose()
        {
            _vertexBuffer?.Dispose();
            _indexBuffer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
