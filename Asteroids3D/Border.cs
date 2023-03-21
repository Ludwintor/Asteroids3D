using System.Diagnostics;
using Asteroids3D.Physics;
using Asteroids3D.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids3D
{
    public class Border
    {
        private BoundingBox _box;
        private PlayerController _player;

        public Border(BoundingBox box, PlayerController player)
        {
            _box = box;
            _player = player;
        }

        public void Update(GameTime time)
        {
            Vector3 playerSize = _player.Collider.BoxBounds.Max - _player.Collider.BoxBounds.Min;
            _player.Transform.Position = Vector3.Clamp(_player.Transform.Position, _box.Min + playerSize / 2f, _box.Max - playerSize / 2f);
        }

        public void Draw()
        {
            Vector3 min = _box.Min;
            Vector3 max = _box.Max;
            // Left
            DrawSide(new BoundingBox(new Vector3(min.X, min.Y, min.Z), new Vector3(min.X, max.Y, max.Z)));
            // Right
            DrawSide(new BoundingBox(new Vector3(max.X, min.Y, min.Z), new Vector3(max.X, max.Y, max.Z)));
            // Front
            DrawSide(new BoundingBox(new Vector3(min.X, min.Y, max.Z), new Vector3(max.X, max.Y, max.Z)));
            // Back
            DrawSide(new BoundingBox(new Vector3(min.X, min.Y, min.Z), new Vector3(max.X, max.Y, min.Z)));
            // Up
            DrawSide(new BoundingBox(new Vector3(min.X, max.Y, min.Z), new Vector3(max.X, max.Y, max.Z)));
            // Down
            DrawSide(new BoundingBox(new Vector3(min.X, min.Y, min.Z), new Vector3(max.X, min.Y, max.Z)));
        }

        private void DrawSide(BoundingBox box)
        {
            float divFactor = 6f;
            Color color = Color.Gray;
            color.A = 200;
            Vector3 cellSize = (box.Max - box.Min) / divFactor;
            Vector3 cellCenter = box.Min + cellSize / 2f;
            for (int z = 0; z < divFactor; z++)
                for (int y = 0; y < divFactor; y++)
                    for (int x = 0; x < divFactor; x++)
                    {
                        Vector3 center = new(cellCenter.X + cellSize.X * x, cellCenter.Y + cellSize.Y * y, cellCenter.Z + cellSize.Z * z);
                        Gizmos.DrawWireframeCube(center, cellSize, color);
                    }
        }
    }
}
