using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollisionsManager : MonoBehaviour
{
    [SerializeField] Bounds worldBounds;
    [SerializeField] Entity baseEntity;

    private List<Entity> _entities;
    private Octree _baseOctree;

    private bool _debugView = true;

    void Start()
    {
        _entities = new List<Entity>(); 
        worldBounds = new Bounds(worldBounds);
        _baseOctree = new Octree(worldBounds, 4);
    }

    void Update()
    {
        if (_debugView)
            _baseOctree.Draw();

        if (Keyboard.current.tabKey.wasPressedThisFrame)
            _debugView = !_debugView;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Entity e = Instantiate(baseEntity, transform);
            e.Relocate(new Vector3(Random.Range(worldBounds.min.x, worldBounds.max.x),
                                   Random.Range(worldBounds.min.y, worldBounds.max.y),
                                   Random.Range(worldBounds.min.z, worldBounds.max.z)));
            _entities.Add(e);
            _baseOctree.Insert(e);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(worldBounds.Center, worldBounds.Size);
    }
}
