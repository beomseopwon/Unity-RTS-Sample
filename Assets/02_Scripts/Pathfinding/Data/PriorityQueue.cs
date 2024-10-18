using System.Collections.Generic;

namespace RTS.Pathfinding
{
    public class PriorityQueue
    {
        private List<GridNode> _grids;
        private List<GridNode> _gridVector;
        private int _gridCount;

        public bool IsEmpty() { return _gridCount < 2; }
        public int GetSize() { return _gridCount - 1; }

        public PriorityQueue()
        {
            _gridCount = 1;
            _grids = new List<GridNode>();
            _gridVector = new List<GridNode>();
        }

        public void Clear()
        {
            Reset();
            _grids.Clear();
            _gridVector.Clear();
        }

        public void SetMaxPriority(int p)
        {
            if (_grids.Count == 0)
            {
                Clear();
                _grids = new List<GridNode>();
                for (int i = 0; i < p + 1; i++)
                {
                    _grids.Add(null);
                }
                _gridVector = new List<GridNode>();
                for (int i = 0; i < p; i++)
                {
                    _gridVector.Add(null);
                }
            }
        }

        public void Reset()
        {
            if (_gridCount > 1)
            {
                int i;
                for (i = 0; i < _gridVector.Count; i++)
                {
                    _gridVector[i] = null;
                    _grids[i] = null;
                }
                _grids[i] = null;
                _gridCount = 1;
            }
        }

        public void Push(GridNode val)
        {
            _gridVector[val.GetId()] = val;

            int pos = _gridCount;
            val.SetQpos(pos);
            _grids[pos] = val;
            while (pos > 1)
            {
                int parent = pos / 2;
                if (_grids[parent].GetF() > val.GetF())
                {
                    GridNode tval1 = _grids[pos];
                    GridNode tval2 = _grids[parent];
                    _grids[parent] = tval1;
                    _grids[parent].SetQpos(parent);
                    _grids[pos] = tval2;
                    _grids[pos].SetQpos(pos);
                    pos = parent;
                }
                else break;
            }
            _gridCount++;
        }

        public GridNode Pop()
        {
            GridNode ret = Top();
            _gridVector[ret.GetId()] = null;

            _grids[1] = _grids[_gridCount - 1];
            _grids[1].SetQpos(1);
            _grids[_gridCount - 1] = null;
            _gridCount--;
            int pos = 1;
            int child1 = 2 * pos;
            int child2 = 2 * pos + 1;
            while (child1 < _gridCount)
            {
                if (child2 < _gridCount)
                {
                    if (_grids[child1].GetF() < _grids[child2].GetF())
                    {
                        if (_grids[pos].GetF() > _grids[child1].GetF())
                        {
                            GridNode tval1 = _grids[pos];
                            GridNode tval2 = _grids[child1];
                            _grids[child1] = tval1; _grids[child1].SetQpos(child1);
                            _grids[pos] = tval2; _grids[pos].SetQpos(pos);
                            pos = child1;
                        }
                        else break;
                    }
                    else
                    {
                        if (_grids[pos].GetF() > _grids[child2].GetF())
                        {
                            GridNode tval1 = _grids[pos];
                            GridNode tval2 = _grids[child2];
                            _grids[child2] = tval1; _grids[child2].SetQpos(child2);
                            _grids[pos] = tval2; _grids[pos].SetQpos(pos);
                            pos = child2;
                        }
                        else break;
                    }
                }
                else
                {
                    if (_grids[pos].GetF() > _grids[child1].GetF())
                    {
                        GridNode tval1 = _grids[pos];
                        GridNode tval2 = _grids[child1];
                        _grids[child1] = tval1; _grids[child1].SetQpos(child1);
                        _grids[pos] = tval2; _grids[pos].SetQpos(pos);
                        pos = child1;
                    }
                    else break;
                }
                child1 = 2 * pos;
                child2 = 2 * pos + 1;
            }
            return ret;
        }

        public GridNode Top()
        {
            return _grids[1];
        }

        public GridNode Exists(int id)
        {
            return _gridVector[id];
        }

        public GridNode Exists(GridNode val)
        {
            return _gridVector[val.GetId()];
        }

        public void Update(GridNode val)
        {
            int pos = val.GetQpos();
            while (pos > 1)
            {
                int parent = pos / 2;
                if (_grids[parent].GetF() > val.GetF())
                {
                    GridNode tval1 = _grids[pos];
                    GridNode tval2 = _grids[parent];
                    _grids[parent] = tval1; _grids[parent].SetQpos(parent);
                    _grids[pos] = tval2; _grids[pos].SetQpos(pos);
                    pos = parent;
                }
                else break;
            }
        }
    }
}
