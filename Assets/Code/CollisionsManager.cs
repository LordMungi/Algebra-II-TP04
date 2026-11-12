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

        foreach (Entity entity in entities)
        {
            _baseOctree.Insert(entity);
        }
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
