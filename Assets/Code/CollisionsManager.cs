using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollisionsManager : MonoBehaviour
{
    [SerializeField] Bounds worldBounds;
    [SerializeField] Entity baseEntity;
    [SerializeField] float maxSpeed;

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
            e.Initialize(new Vector3(Random.Range(worldBounds.min.x, worldBounds.max.x),
                                     Random.Range(worldBounds.min.y, worldBounds.max.y),
                                     Random.Range(worldBounds.min.z, worldBounds.max.z)),
                         new Vector3(Random.Range(-maxSpeed, maxSpeed),
                                     Random.Range(-maxSpeed, maxSpeed),
                                     Random.Range(-maxSpeed, maxSpeed)));
            _entities.Add(e);
            _baseOctree.Insert(e);
        }

        foreach (Entity entity in _entities)
        {
            entity.Move();
            if (entity.transform.position.x <= worldBounds.min.x)
                entity.velocity.x = Mathf.Abs(entity.velocity.x);
            if (entity.transform.position.x >= worldBounds.max.x)
                entity.velocity.x = Mathf.Abs(entity.velocity.x) * -1;

            if (entity.transform.position.y <= worldBounds.min.y)
                entity.velocity.y = Mathf.Abs(entity.velocity.y);
            if (entity.transform.position.y >= worldBounds.max.y)
                entity.velocity.y = Mathf.Abs(entity.velocity.y) * -1;

            if (entity.transform.position.z <= worldBounds.min.z)
                entity.velocity.z = Mathf.Abs(entity.velocity.z);
            if (entity.transform.position.z >= worldBounds.max.z)
                entity.velocity.z = Mathf.Abs(entity.velocity.z) * -1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(worldBounds.Center, worldBounds.Size);
    }
}
