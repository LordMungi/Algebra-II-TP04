using System.Collections.Generic;
using UnityEngine;

public class Octree
{
    private Bounds _bounds;
    private uint _limit;
    private uint _level;

    private bool _isDivided = false;

    private List<Entity> _entities = new List<Entity>();

    public Octree(Bounds bounds, uint limit, uint level)
    {
        _bounds = bounds;
        _limit = limit;
        _level = level;
    }

    public bool Insert(Entity entity)
    {
        return false;
    }

    private void Divide()
    {

    }
}
