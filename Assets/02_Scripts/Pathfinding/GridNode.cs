using UnityEngine;
using System.Collections.Generic;

namespace RTS.Pathfinding
{
    public class GridNode
    {
        private int _id;
        private GridNode _parent;
        private List<Pair<GridNode, float>> _childs;
        private int _childCount;

        private int _queuePosition;
        private Vector3 _position;
        private Vector3 _spatialPosition;
        
        private float f, g, v;

        public int GetId() { return _id; }
        public int GetQpos() { return _queuePosition; }
        public Vector3 GetPos() { return _position; }
        public float GetF() { return f; }
        public float GetG() { return g; }
        public float GetV() { return v; }
        public float GetX() { return _position.x; }
        public float GetY() { return _position.y; }
        public float GetZ() { return _position.z; }
        public float GetSpatialX() { return _spatialPosition.x; }
        public float GetSpatialZ() { return _spatialPosition.z; }
        public int GetChildCount() { return _childCount; }
        public GridNode GetParent() { return _parent; }
        public GridNode GetChild(int i) { return _childs[i].First; }
        public float GetEdgeWeight(int i) { return _childs[i].Second; }

        public void SetId(int i) { _id = i; }
        public void SetQpos(int i) { _queuePosition = i; }
        public void SetF(float i) { f = i; }
        public void SetG(float i) { g = i; }
        public void SetV(float i) { v = i; }
        public void SetX(float f) { _position.x = f; }
        public void SetY(float f) { _position.y = f; }
        public void SetZ(float f) { _position.z = f; }
        public void SetNumChildren(int i) { _childCount = i; }
        public void SetParent(GridNode node) { _parent = node; }
        public void ClearChildren() { _childs.Clear(); }

        public GridNode(int id, float x, float y, float z, float v, GridNode parent)
        {
            this._id = id;
            _position.x = x;
            _position.y = y;
            _position.z = z;
            _spatialPosition.x = (int)(x / 8);
            _spatialPosition.y = 0;
            _spatialPosition.z = (int)(z / 8);
            this.v = v;
            this._parent = parent;
            _queuePosition = 0;
            f = 0;
            g = 0;
            _childCount = 0;
            _childs = new List<Pair<GridNode, float>>();
        }

        public void Clear()
        {
            _parent = null;
            _childs.Clear();
        }

        public void AddEdgeTo(GridNode to, float edgeWeight)
        {
            Pair<GridNode, float> p = new Pair<GridNode, float>(to, edgeWeight);
            _childs.Add(p);
            _childCount++;
        }

        public void RemoveEdge(GridNode to)
        {
            for (int i = 0; i < _childs.Count; i++)
            {
                if (_childs[i].First == to)
                {
                    _childs.RemoveAt(i);
                    _childCount--;
                    return;
                }
            }
        }

        public bool HasChild(GridNode node)
        {
            for (int i = 0; i < _childs.Count; i++)
            {
                if (_childs[i].First == node) return true;
            }
            return false;
        }
    }
}
