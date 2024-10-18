using UnityEngine;
using System.Collections.Generic;

namespace RTS.Pathfinding
{
    public class GridManager
    {
        private const float MAX_HEIGHT = 0.5f;

        private List<GridNode> _nodes;
        private int _totalNodeCount;
        private int _maxNodeCount;
        private PriorityQueue _openList;
        private PriorityQueue _closedList;
        private Path _path;

        public int GetTotalNodes() { return _totalNodeCount; }
        public GridNode GetNode(int i) { if (i > -1 && i < _totalNodeCount) return _nodes[i]; else return null; }

        public GridNode IsInOpenlist(GridNode g) { return _openList.Exists(g); }
        public GridNode IsInClosedlist(GridNode g) { return _closedList.Exists(g); }

        public GridManager(int mapSize, float scaleFactor)
        {
            _maxNodeCount = mapSize * mapSize;
            _totalNodeCount = 0;
            _nodes = new List<GridNode>();
            _openList = new PriorityQueue();
            _closedList = new PriorityQueue();
            _openList.SetMaxPriority(_maxNodeCount);
            _closedList.SetMaxPriority(_maxNodeCount);
            _path = new Path();
        }

        public void Clear()
        {
            _openList.Clear();
            _openList = null;
            _closedList.Clear();
            _closedList = null;
            for (int i = 0; i < _totalNodeCount; i++)
            {
                _nodes[i].Clear();
                _nodes[i] = null;
            }
            _nodes.Clear();
            _nodes = null;
            _totalNodeCount = 0;
            _path.Clear();
            _path = null;
        }

        public void ClearNodes()
        {
            _openList.Reset();
            _closedList.Reset();
            for (int i = 0; i < _totalNodeCount; i++)
            {
                _nodes[i].Clear();
            }
            _nodes.Clear();
            _totalNodeCount = 0;
        }

        public GridNode GetNodeFromPos(Vector3 pos, bool checkIfNull)
        {
            int sx = (int)(pos.x / 8);
            int sz = (int)(pos.z / 8);

            float mdist = float.MaxValue;
            int index = -1;
            for (int i = 0; i < _totalNodeCount; i++)
            {
                if (sx == _nodes[i].GetSpatialX() && sz == _nodes[i].GetSpatialZ())
                {
                    float dist = MathUtility.distanceXZ2(pos, _nodes[i].GetPos());
                    if (dist < mdist)
                    {
                        mdist = dist;
                        index = i;
                    }
                }
            }

            if (index != -1)
            {
                return _nodes[index];
            }

            if (checkIfNull)
            {
                mdist = float.MaxValue;
                index = -1;
                for (int i = 0; i < _totalNodeCount; i++)
                {
                    float dist = MathUtility.distanceXZ2(pos, _nodes[i].GetPos());
                    if (dist < mdist)
                    {
                        mdist = dist;
                        index = i;
                    }
                }
                if (index != -1)
                {
                    return _nodes[index];
                }
            }

            return null;
        }

        public void AddNode(GridNode node)
        {
            if (_totalNodeCount < _maxNodeCount)
            {
                _nodes.Add(node);
                _totalNodeCount++;
            }
        }

        public void DeleteNode(GridNode node)
        {
            for (int i = 0; i < _totalNodeCount; i++)
            {
                _nodes[i].RemoveEdge(node);
            }
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i] == node)
                {
                    _nodes[i].Clear();
                    _nodes.RemoveAt(i);
                    _totalNodeCount--;
                    break;
                }
            }
            ResetIds();
        }

        public void ResetIds()
        {
            for (int i = 0; i < _totalNodeCount; i++)
            {
                _nodes[i].SetId(i);
            }
        }

        public void AddEdge(GridNode from, GridNode to, bool doubleSided)
        {
            float w = NodeDistance(from, to);
            from.AddEdgeTo(to, w);
            if (doubleSided) to.AddEdgeTo(from, w);
        }

        public void Build8GridGraph(float stepsize)
        {
            for (int i = 0; i < _totalNodeCount; i++)
            {
                for (int j = 0; j < _totalNodeCount; j++)
                {
                    if (_nodes[i].GetX() + stepsize == _nodes[j].GetX() && _nodes[i].GetZ() == _nodes[j].GetZ())
                    {
                        AddEdge(_nodes[i], _nodes[j], false);
                    }
                    else if (_nodes[i].GetX() - stepsize == _nodes[j].GetX() && _nodes[i].GetZ() == _nodes[j].GetZ())
                    {
                        AddEdge(_nodes[i], _nodes[j], false);
                    }
                    else if (_nodes[i].GetX() == _nodes[j].GetX() && _nodes[i].GetZ() + stepsize == _nodes[j].GetZ())
                    {
                        AddEdge(_nodes[i], _nodes[j], false);
                    }
                    else if (_nodes[i].GetX() == _nodes[j].GetX() && _nodes[i].GetZ() - stepsize == _nodes[j].GetZ())
                    {
                        AddEdge(_nodes[i], _nodes[j], false);
                    }
                    else if (_nodes[i].GetX() + stepsize == _nodes[j].GetX() && _nodes[i].GetZ() + stepsize == _nodes[j].GetZ())
                    {
                        AddEdge(_nodes[i], _nodes[j], false);
                    }
                    else if (_nodes[i].GetX() + stepsize == _nodes[j].GetX() && _nodes[i].GetZ() - stepsize == _nodes[j].GetZ())
                    {
                        AddEdge(_nodes[i], _nodes[j], false);
                    }
                    else if (_nodes[i].GetX() - stepsize == _nodes[j].GetX() && _nodes[i].GetZ() + stepsize == _nodes[j].GetZ())
                    {
                        AddEdge(_nodes[i], _nodes[j], false);
                    }
                    else if (_nodes[i].GetX() - stepsize == _nodes[j].GetX() && _nodes[i].GetZ() - stepsize == _nodes[j].GetZ())
                    {
                        AddEdge(_nodes[i], _nodes[j], false);
                    }
                }
            }
        }

        public void ReturnPath(GridNode from, GridNode to, ref Path path, bool bw)
        {
            path.Clear();
            _openList.Reset();
            _closedList.Reset();
            GridNode _currentNode = from;
            _currentNode.SetParent(null);
            _currentNode.SetG(0);
            _currentNode.SetF(0);
            _openList.Push(_currentNode);
            //
            while (!_openList.IsEmpty())
            {
                _currentNode = _openList.Pop();
                if (_currentNode.GetId() == to.GetId()) break;
                _closedList.Push(_currentNode);
                for (int i = 0; i < (int)_currentNode.GetChildCount(); i++)
                {
                    if (_closedList.Exists(_currentNode.GetChild(i)) == null && _currentNode.GetChild(i).GetV() < _maxNodeCount)
                    {
                        float g = _currentNode.GetG() + _currentNode.GetEdgeWeight(i);
                        if (!bw) g += _currentNode.GetChild(i).GetV();
                        float f = g + NodeDistance(_currentNode.GetChild(i), to);
                        if (_openList.Exists(_currentNode.GetChild(i)) == null)
                        {
                            _currentNode.GetChild(i).SetF(f);
                            _currentNode.GetChild(i).SetG(g);
                            _currentNode.GetChild(i).SetParent(_currentNode);
                            _openList.Push(_currentNode.GetChild(i));
                        }
                        else if (g < _currentNode.GetChild(i).GetG())
                        {
                            _currentNode.GetChild(i).SetF(f);
                            _currentNode.GetChild(i).SetG(g);
                            _currentNode.GetChild(i).SetParent(_currentNode);
                            _openList.Update(_currentNode.GetChild(i));
                        }
                    }
                }
            }

            while (_currentNode.GetParent() != null)
            {
                path.AddNode(_currentNode.GetPos());
                _currentNode = _currentNode.GetParent();
            }
        }

        public bool NodesPassable(Vector3 start, Vector3 dest)
        {
            float mess = MathUtility.distanceXZ(start, dest);
            float addition_x = (float)((dest.x - start.x) / mess);
            float addition_z = (float)((dest.z - start.z) / mess);
            for (float i = 0; i < mess; i += 0.75f)
            {
                float npos_x = start.x + i * addition_x + 0.75f;
                float npos_z = start.z + i * addition_z + 0.75f;
                float y = Terrain.activeTerrain.SampleHeight(new Vector3(npos_x, 0, npos_z));
                if (y >= MAX_HEIGHT)
                {
                    return false;
                }
            }
            return true;
        }

        public void SmoothPath(Vector3 start, Vector3 end, ref Path path)
        {
            int len = path.GetLength();
            if (len > 1)
            {
                _path.Clone(path);
                _path.ShiftNode(end);
                path.Clear();
                Vector3 current = start;
                int index = 1;
                while (index < _path.GetLength())
                {
                    if (NodesPassable(current, _path.GetNode(index)))
                    {
                        index++;
                    }
                    else
                    {
                        current = _path.GetNode(index - 1);
                        path.AddNode(current);
                        index++;
                    }
                }
                //final target
                if (path.GetLength() == 0)
                {
                    path.ShiftNode(end);
                }
            }
            else
            {
                path.ShiftNode(end);
            }
        }

        private float NodeDistance(GridNode from, GridNode to)
        {
            return MathUtility.distanceXZ(from.GetPos(), to.GetPos());
        }
    }
}
