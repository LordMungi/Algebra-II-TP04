using System;
using System.Collections.Generic;
using UnityEngine;

public class Octree <T> where T : IPoint
{
    private const uint MAX_LEVEL = 8;

    private Bounds _localBounds;
    private Bounds _bounds;
    private uint _limit;
    private uint _level;

    private Vector3 _offset;

    private bool _isDivided = false;

    private List<T> _entities = new List<T>();
    private Octree<T>[] _branches;

    public int TotalCount
    {
        get
        {
            int count = 0;
            if (_isDivided)
            {
                foreach (Octree<T> branch in _branches)
                {
                    count += branch.TotalCount;
                }
            }
            return count + _entities.Count;
        }
    }

    public Octree(Bounds bounds, uint limit)
    {
        _bounds = bounds;
        _localBounds = bounds; 
        _limit = limit;
        _level = 1;
        _offset = Vector3.zero;
    }

    private Octree(Bounds bounds, uint limit, uint level, Vector3 position)
    {
        _bounds = bounds + position;
        _localBounds = bounds;
        _limit = limit;
        _level = level;
        _offset = position;
    }

    public bool Insert(T entity)
    {

        if (_bounds.Contains(entity.GetPosition()))
        {
            if (_isDivided)
            {
                InsertIntoBranch(entity);
                return true;
            }
            else if (_entities.Count + 1 >= _limit && _level < MAX_LEVEL)
            {
                Divide();
                InsertIntoBranch(entity);
                return true;
            }
            else
            {
                _entities.Add(entity);
                return true;
            }
        }
        return false;
    }
    public bool TryRemove(T entity)
    {
        if (_isDivided)
        {
            if (TryRemoveInBranch(entity))
            {
                if (TotalCount < _limit)
                    Merge();
                return true;
            }
        }

        if (_entities.Contains(entity))
        {
            if (!_bounds.Contains(entity.GetPosition()))
            {
                _entities.Remove(entity);
                return true;
            }
        }
        return false;
    }

    public List<T> GetEntitiesInSameArea(T entity)
    {
        if (_isDivided)
            foreach (Octree<T> branch in _branches)
            {
                List<T> aux = branch.GetEntitiesInSameArea(entity);
                if (aux != null)
                    return aux;
            }
        else if (_bounds.Contains(entity.GetPosition()))
            return _entities;

        return null;
    }

    public void Reposition(Vector3 position)
    {
        _offset = position;
        _bounds = _localBounds + _offset;

        if (_isDivided)
        {
            foreach (Octree<T> branch in _branches)
            {
                branch.Reposition(position);
            }
        }
    }

    private void Divide()
    {
        _branches = new Octree<T>[8];

        Vector3 min = _localBounds.min;
        Vector3 max = _localBounds.max;
        Vector3 center = _localBounds.Center;

        Bounds newBounds = new Bounds(min.x, min.y, min.z, center.x, center.y, center.z);
        _branches[0] = new Octree<T>(newBounds, _limit, _level + 1, _offset);
        newBounds = new Bounds(min.x, min.y, center.z, center.x, center.y, max.z);
        _branches[1] = new Octree<T>(newBounds, _limit, _level + 1, _offset);
        newBounds = new Bounds(center.x, min.y, center.z, max.x, center.y, max.z);
        _branches[2] = new Octree<T>(newBounds, _limit, _level + 1, _offset);
        newBounds = new Bounds(center.x, min.y, min.z, max.x, center.y, center.z);
        _branches[3] = new Octree<T>(newBounds, _limit, _level + 1, _offset);
        newBounds = new Bounds(min.x, center.y, min.z, center.x, max.y, center.z);
        _branches[4] = new Octree<T>(newBounds, _limit, _level + 1, _offset);
        newBounds = new Bounds(min.x, center.y, center.z, center.x, max.y, max.z);
        _branches[5] = new Octree<T>(newBounds, _limit, _level + 1, _offset);
        newBounds = new Bounds(center.x, center.y, center.z, max.x, max.y, max.z);
        _branches[6] = new Octree<T>(newBounds, _limit, _level + 1, _offset);
        newBounds = new Bounds(center.x, center.y, min.z, max.x, max.y, center.z);
        _branches[7] = new Octree<T>(newBounds, _limit, _level + 1, _offset);

        _isDivided = true;

        foreach (T entity in _entities)
        {
            InsertIntoBranch(entity);
        }
        _entities.Clear();
    }
    private void Merge()
    {
        foreach (Octree<T> branch in _branches)
        {
            foreach (T entity in branch._entities)
            {
                _entities.Add(entity);
            }            
        }
        Array.Clear(_branches, 0, _branches.Length);
        _branches = null;
        _isDivided = false;
    }

    private bool InsertIntoBranch(T entity)
    {
        foreach (Octree<T> branch in _branches)
        {
            if (branch.Insert(entity)) return true;
        }
        return false;
    }
    private bool TryRemoveInBranch(T entity)
    {
        foreach (Octree<T> branch in _branches)
        {
            if (branch.TryRemove(entity)) return true;
        }
        return false;
    }

    public void Draw()
    {
        Vector3 min = _bounds.min;
        Vector3 max = _bounds.max;

        Debug.DrawLine(new Vector3(min.x, min.y, min.z), new Vector3(min.x, min.y, max.z), Color.green);
        Debug.DrawLine(new Vector3(min.x, min.y, max.z), new Vector3(max.x, min.y, max.z), Color.green);
        Debug.DrawLine(new Vector3(max.x, min.y, max.z), new Vector3(max.x, min.y, min.z), Color.green);
        Debug.DrawLine(new Vector3(max.x, min.y, min.z), new Vector3(min.x, min.y, min.z), Color.green);
        Debug.DrawLine(new Vector3(min.x, min.y, min.z), new Vector3(min.x, max.y, min.z), Color.green);
        Debug.DrawLine(new Vector3(min.x, min.y, max.z), new Vector3(min.x, max.y, max.z), Color.green);
        Debug.DrawLine(new Vector3(max.x, min.y, max.z), new Vector3(max.x, max.y, max.z), Color.green);
        Debug.DrawLine(new Vector3(max.x, min.y, min.z), new Vector3(max.x, max.y, min.z), Color.green);
        Debug.DrawLine(new Vector3(min.x, max.y, min.z), new Vector3(min.x, max.y, max.z), Color.green);
        Debug.DrawLine(new Vector3(min.x, max.y, max.z), new Vector3(max.x, max.y, max.z), Color.green);
        Debug.DrawLine(new Vector3(max.x, max.y, max.z), new Vector3(max.x, max.y, min.z), Color.green);
        Debug.DrawLine(new Vector3(max.x, max.y, min.z), new Vector3(min.x, max.y, min.z), Color.green);

        //Debug.DrawLine(_bounds.Center + _offset - Vector3.up * 0.5f, _bounds.Center + _offset + Vector3.up * 0.5f, Color.red);
        //Debug.DrawLine(_bounds.Center + _offset - Vector3.right * 0.5f, _bounds.Center + _offset + Vector3.right * 0.5f, Color.red);
        //Debug.DrawLine(_bounds.Center + _offset - Vector3.forward * 0.5f, _bounds.Center + _offset + Vector3.forward * 0.5f, Color.red);

        if (_isDivided)
        {
            foreach (Octree<T> octree in _branches)
            {
                octree.Draw();
            }
        }
    }
}
