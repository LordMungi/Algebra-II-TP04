using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollisionsManager : MonoBehaviour
{
    [SerializeField] List<Entity> entities = new List<Entity>();
    [SerializeField] Bounds worldBounds;

    private Octree _baseOctree;

    private bool _debugView = false;

    void Start()
    {
        worldBounds = new Bounds(worldBounds);
        _baseOctree = new Octree(worldBounds, 10);
    }

    void Update()
    {
        if (_debugView)
            _baseOctree.Draw();

        if (Keyboard.current.tabKey.wasPressedThisFrame)
            _debugView = !_debugView;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(worldBounds.Center, worldBounds.Size);
    }
}
