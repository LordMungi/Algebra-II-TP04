using System.Collections.Generic;
using UnityEngine;

public class CollisionsManager : MonoBehaviour
{
    [SerializeField] List<Entity> entities = new List<Entity>();
    [SerializeField] Bounds worldBounds;

    private Octree _baseOctree;


    void Start()
    {
        worldBounds = new Bounds(worldBounds);
        _baseOctree = new Octree(worldBounds, 10);
    }

    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(worldBounds.Center, worldBounds.Size);
    }
}
