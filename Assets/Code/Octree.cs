using System.Collections.Generic;
using UnityEngine;

public class Octree 
{
    private const uint MAX_LEVEL = 8;

    private Bounds _bounds;
    private uint _limit;
    private uint _level;

    private bool _isDivided = false;

    private List<Entity> _entities = new List<Entity>();
    private Octree[] _branches;

    public Octree(Bounds bounds, uint limit)
    {
        _bounds = bounds;
        _limit = limit;
        _level = 1;
    }

    private Octree(Bounds bounds, uint limit, uint level)
    {
        _bounds = bounds;
        _limit = limit;
        _level = level;
    }

    public bool Insert(Entity entity)
    {

        if (_bounds.Contains(entity.transform.position))
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
                Debug.Log("Entity in "+ entity.transform.position + " Inserted into level " + _level + ".");
                return true;
            }
        }
        return false;
    }

    private void Divide()
    {
        _branches = new Octree[8];

        Vector3 min = _bounds.min;
        Vector3 max = _bounds.max;
        Vector3 center = _bounds.Center;

        _branches[0] = new Octree(new Bounds(min.x, min.y, min.z, center.x, center.y, center.z), _limit, _level + 1);
        _branches[1] = new Octree(new Bounds(min.x, min.y, center.z, center.x, center.y, max.z), _limit, _level + 1);
        _branches[2] = new Octree(new Bounds(center.x, min.y, center.z, max.x, center.y, max.z), _limit, _level + 1);
        _branches[3] = new Octree(new Bounds(center.x, min.y, min.z, max.x, center.y, center.z), _limit, _level + 1);
        _branches[4] = new Octree(new Bounds(min.x, center.y, min.z, center.x, max.y, center.z), _limit, _level + 1);
        _branches[5] = new Octree(new Bounds(min.x, center.y, center.z, center.x, max.y, max.z), _limit, _level + 1);
        _branches[6] = new Octree(new Bounds(center.x, center.y, center.z, max.x, max.y, max.z), _limit, _level + 1);
        _branches[7] = new Octree(new Bounds(center.x, center.y, min.z, max.x, max.y, center.z), _limit, _level + 1);

        _isDivided = true;

        foreach (Entity entity in _entities)
        {
            InsertIntoBranch(entity);
        }
        _entities.Clear();
    }

    private bool InsertIntoBranch(Entity entity)
    {
        foreach (Octree branch in _branches)
        {
            if (branch.Insert(entity)) return true;
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

        if (_isDivided)
        {
            foreach (Octree octree in _branches)
            {
                octree.Draw();
            }
        }
    }
}
