using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids3D
{
    public class Octree
    {
        private const int THRESHOLD = 16;
        private const int MAX_DEPTH = 8;

        private BoundingBox _box;
        private Node _root;

        public Octree(BoundingBox box)
        {
            _box = box;
            _root = new Node();
        }

        public void Add(BoxCollider value)
        {
            Add(_root, 0, _box, value);
        }

        public void Remove(BoxCollider value)
        {
            Remove(_root, null, _box, value);
        }

        public void Clear()
        {
            ClearNode(_root);
        }

        public void Query(BoundingBox queryBox, List<BoxCollider> results)
        {
            Query(_root, _box, queryBox, results);
        }

        public void FindAllIntersections(List<Pair<BoxCollider>> results)
        {

        }

        public void Draw(GraphicsDevice graphics, Matrix view)
        {
            Draw(graphics, _root, _box);
        }

        private void Draw(GraphicsDevice graphics, Node node, BoundingBox box)
        {
            Vector3 center = (box.Min + box.Max) / 2f;
            Vector3 size = box.Max - box.Min;
            Gizmos.DrawWireframeCube(center, size, Color.Green);

            if (!node.IsLeaf)
                for (int i = 0; i < Node.CHILDREN_COUNT; i++)
                {
                    BoundingBox childBox = ComputeBox(box, (Octant)i);
                    Draw(graphics, node.Children[i], childBox);
                }
        }

        private BoundingBox ComputeBox(BoundingBox box, Octant octant)
        {
            Vector3 origin = box.Min;
            Vector3 childSize = (box.Max - box.Min) / 2f;
            Vector3 childOrigin;
            switch (octant)
            {
                case Octant.NorthEastDown:
                    childOrigin = new(origin.X + childSize.X, origin.Y, origin.Z + childSize.Z);
                    return new BoundingBox(childOrigin, childOrigin + childSize);
                case Octant.NorthWestDown:
                    childOrigin = new(origin.X, origin.Y, origin.Z + childSize.Z);
                    return new BoundingBox(childOrigin, childOrigin + childSize);
                case Octant.SouthWestDown:
                    return new BoundingBox(origin, origin + childSize);
                case Octant.SouthEastDown:
                    childOrigin = new(origin.X + childSize.X, origin.Y, origin.Z);
                    return new BoundingBox(childOrigin, childOrigin + childSize);
                case Octant.NorthEastUp:
                    childOrigin = new(origin.X + childSize.X, origin.Y + childSize.Y, origin.Z + childSize.Z);
                    return new BoundingBox(childOrigin, childOrigin + childSize);
                case Octant.NorthWestUp:
                    childOrigin = new(origin.X, origin.Y + childSize.Y, origin.Z + childSize.Z);
                    return new BoundingBox(childOrigin, childOrigin + childSize);
                case Octant.SouthWestUp:
                    childOrigin = new(origin.X, origin.Y + childSize.Y, origin.Z);
                    return new BoundingBox(childOrigin, childOrigin + childSize);
                case Octant.SouthEastUp:
                    childOrigin = new(origin.X + childSize.X, origin.Y + childSize.Y, origin.Z);
                    return new BoundingBox(childOrigin, childOrigin + childSize);
                default:
                    throw new Exception("Wrong octant value");
            }
        }

        private Octant GetOctant(BoundingBox nodeBox, BoundingBox valueBox)
        {
            Vector3 center = (nodeBox.Min + nodeBox.Max) / 2f;
            // West
            if (valueBox.Max.X <= center.X)
            {
                // North West
                if (valueBox.Min.Z >= center.Z)
                    return valueBox.Min.Y >= center.Y ? Octant.NorthWestUp :
                           valueBox.Max.Y <= center.Y ? Octant.NorthWestDown : Octant.None;
                // South West
                else if (valueBox.Max.Z <= center.Z)
                    return valueBox.Min.Y >= center.Y ? Octant.SouthWestUp :
                           valueBox.Max.Y <= center.Y ? Octant.SouthWestDown : Octant.None;
            }
            // East
            else if (valueBox.Min.X >= center.X)
            {
                // North East
                if (valueBox.Min.Z >= center.Z)
                    return valueBox.Min.Y >= center.Y ? Octant.NorthEastUp :
                           valueBox.Max.Y <= center.Y ? Octant.NorthEastDown : Octant.None;
                // South East
                else if (valueBox.Max.Z <= center.Z)
                    return valueBox.Min.Y >= center.Y ? Octant.SouthEastUp :
                           valueBox.Max.Y <= center.Y ? Octant.SouthEastDown : Octant.None;
            }
            // Not in any octant or intersects more than one
            return Octant.None;
        }

        private void Add(Node node, int depth, BoundingBox box, BoxCollider value)
        {
            Debug.Assert(node != null);
            Debug.Assert(box.Contains(value.BoundingBox) == ContainmentType.Contains);
            if (node.IsLeaf)
            {
                if (depth >= MAX_DEPTH || node.Values.Count < THRESHOLD)
                {
                    node.Values.Add(value);
                }
                else
                {
                    Split(node, box);
                    Add(node, depth, box, value);
                }
            }
            else
            {
                Octant octant = GetOctant(box, value.BoundingBox);
                if (octant != Octant.None)
                    Add(node.GetChild(octant), depth + 1, ComputeBox(box, octant), value);
                else
                    node.Values.Add(value);
            }
        }

        private void Split(Node node, BoundingBox box)
        {
            Debug.Assert(node != null);
            Debug.Assert(node.IsLeaf);
            node.PrepareChildren();

            for (int i = 0; i < node.Values.Count; i++)
            {
                BoxCollider value = node.Values[i];
                Octant octant = GetOctant(box, value.BoundingBox);
                if (octant != Octant.None)
                {
                    node.GetChild(octant).Values.Add(value);
                    node.Values.RemoveAt(i);
                    i--;
                }
            }
        }

        private void Remove(Node node, Node parent, BoundingBox box, BoxCollider value)
        {
            Debug.Assert(node != null);
            Debug.Assert(box.Contains(value.BoundingBox) == ContainmentType.Contains);
            if (node.IsLeaf)
            {
                RemoveValue(node, value);
                if (parent != null)
                    TryMerge(parent);
            }
            else
            {
                Octant octant = GetOctant(box, value.BoundingBox);
                if (octant != Octant.None)
                    Remove(node.GetChild(octant), node, ComputeBox(box, octant), value);
                else
                    RemoveValue(node, value);
            }
        }

        private void RemoveValue(Node node, BoxCollider value)
        {
            int index = node.Values.IndexOf(value);
            node.Values[index] = node.Values[^1];
            node.Values.RemoveAt(node.Values.Count - 1);
        }

        private void TryMerge(Node node)
        {
            Debug.Assert(node != null);
            Debug.Assert(!node.IsLeaf);
            int totalValues = node.Values.Count;
            foreach (Node child in node.Children)
            {
                if (!child.IsLeaf)
                    return;
                totalValues += child.Values.Count;
            }
            if (totalValues <= THRESHOLD)
            {
                node.Values.EnsureCapacity(totalValues);
                foreach (Node child in node.Children)
                {
                    node.Values.AddRange(child.Values);
                    child.Reset();
                }
            }
        }

        private void Query(Node node, BoundingBox box, BoundingBox queryBox, List<BoxCollider> results)
        {
            Debug.Assert(node != null);
            Debug.Assert(queryBox.Intersects(box));
            foreach (BoxCollider value in node.Values)
                if (queryBox.Intersects(value.BoundingBox))
                    results.Add(value);
            if (!node.IsLeaf)
            {
                for (int i = 0; i < Node.CHILDREN_COUNT; i++)
                {
                    BoundingBox childBox = ComputeBox(box, (Octant)i);
                    if (queryBox.Intersects(childBox))
                        Query(node.Children[i], childBox, queryBox, results);
                }
            }
        }

        private void ClearNode(Node node)
        {
            if (!node.IsLeaf)
                foreach (Node child in node.Children)
                    ClearNode(child);
            node.Reset();
        }

        private class Node
        {
            public const int CHILDREN_COUNT = 8;

            public Node[] Children;
            public List<BoxCollider> Values = new();

            public bool IsLeaf { get; private set; }

            public Node()
            {
                IsLeaf = true;
            }

            public Node GetChild(Octant octant)
            {
                return Children[(int)octant];
            }

            public void PrepareChildren()
            {
                if (Children == null)
                {
                    Children = new Node[CHILDREN_COUNT];
                    for (int i = 0; i < CHILDREN_COUNT; i++)
                        Children[i] = new Node();
                }
                IsLeaf = false;
            }

            public void Reset()
            {
                Values.Clear();
                IsLeaf = true;
            }
        }

        private enum Octant
        {
            None = -1,
            NorthEastDown = 0,
            NorthWestDown = 1,
            SouthWestDown = 2,
            SouthEastDown = 3,
            NorthEastUp = 4,
            NorthWestUp = 5,
            SouthWestUp = 6,
            SouthEastUp = 7
        }
    }
}
