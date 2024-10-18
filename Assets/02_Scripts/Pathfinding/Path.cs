using UnityEngine;
using System.Collections.Generic;

namespace RTS.Pathfinding
{
    public class Path
    {
        private List<Vector3> _nodes;

        private int _length;

        public int GetLength() { return _length; }
        public Vector3 GetNode(int i) { return _nodes[i]; }
        public void SetNode(int i, Vector3 v) { _nodes[i] = v; }

        public Path()
        {
            _nodes = new List<Vector3>();
            _length = 0;
        }

        public void Clear()
        {
            _nodes.Clear();
            _length = 0;
        }

        public void Clone(Path path)
        {
            Clear();
            for (int i = 0; i < path.GetLength(); i++)
            {
                AddNode(path.GetNode(i));
            }
        }

        public void AddNode(Vector3 node)
        {
            _nodes.Add(node);
            _length++;
        }

        public void ShiftNode(Vector3 node)
        {
            _nodes.Insert(0, node);
            _length++;
        }
    }
}
