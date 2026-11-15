using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollisionsManager : MonoBehaviour
{
    [SerializeField] Bounds localBounds;
    [SerializeField] Entity baseEntity;
    [SerializeField] float maxSpeed;

    private Bounds _worldBounds;

    private List<Entity> _entities;
    private Octree<Entity> _baseOctree;

    private bool _debugView = true;

    private bool _isPaused = false;
    private bool _isFast = false;
    private bool _isReversed = false;

    void Start()
    {
        _entities = new List<Entity>(); 
        _worldBounds = new Bounds(localBounds);
        _baseOctree = new Octree<Entity>(_worldBounds, 4);
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
            e.Initialize(new Vector3(Random.Range(_worldBounds.min.x, _worldBounds.max.x),
                                     Random.Range(_worldBounds.min.y, _worldBounds.max.y),
                                     Random.Range(_worldBounds.min.z, _worldBounds.max.z)),
                         new Vector3(Random.Range(-maxSpeed, maxSpeed),
                                     Random.Range(-maxSpeed, maxSpeed),
                                     Random.Range(-maxSpeed, maxSpeed)));
            _entities.Add(e);
            _baseOctree.Insert(e);
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
            _isReversed = !_isReversed;

        if (Keyboard.current.qKey.wasPressedThisFrame)
            _isFast = !_isFast;

        if (Keyboard.current.wKey.wasPressedThisFrame)
            _isPaused = !_isPaused;

        // Move
        foreach (Entity entity in _entities)
        {
            entity.SetReverse(_isReversed);
            entity.SetFast(_isFast);
            entity.SetPause(_isPaused);

                entity.Move();
            if (entity.transform.position.x <= _worldBounds.min.x)
            {
                entity.transform.position = new Vector3(_worldBounds.min.x, entity.transform.position.y, entity.transform.position.z);
                entity.velocity.x = entity.velocity.x * -1;
            }
            if (entity.transform.position.x >= _worldBounds.max.x)
            {
                entity.transform.position = new Vector3(_worldBounds.max.x, entity.transform.position.y, entity.transform.position.z);
                entity.velocity.x = entity.velocity.x * -1;
            }

            if (entity.transform.position.y <= _worldBounds.min.y)
            {
                entity.transform.position = new Vector3(entity.transform.position.x, _worldBounds.min.y, entity.transform.position.z);
                entity.velocity.y = entity.velocity.y * -1;
            }
            if (entity.transform.position.y >= _worldBounds.max.y)
            {
                entity.transform.position = new Vector3(entity.transform.position.x, _worldBounds.max.y, entity.transform.position.z);
                entity.velocity.y = entity.velocity.y * -1;
            }

            if (entity.transform.position.z <= _worldBounds.min.z)
            {
                entity.transform.position = new Vector3(entity.transform.position.x, entity.transform.position.y,_worldBounds.min.z);
                entity.velocity.z = entity.velocity.z * -1;
            }
            if (entity.transform.position.z >= _worldBounds.max.z)
            {
                entity.transform.position = new Vector3(entity.transform.position.x, entity.transform.position.y, _worldBounds.max.z);
                entity.velocity.z = entity.velocity.z * -1;
            }

            if (_baseOctree.TryRemove(entity))
                _baseOctree.Insert(entity);
        }

        // Collide
        foreach (Entity entity in _entities)
        {
            entity.isCollidingAABB = false;
            entity.isCollidingRadius = false;

            foreach (Entity other in _baseOctree.GetEntitiesInSameArea(entity))
            {
                if (entity == other) continue;
                if (entity.IsInRadiusOfOther(other))
                {
                    entity.isCollidingRadius = true;
                    other.isCollidingRadius = true;

                    if (entity.AABB.Collides(other.AABB))
                    {
                        entity.isCollidingAABB = true;
                        other.isCollidingAABB = true;
                    }
                }
            }
        }

        _worldBounds.min = localBounds.min + transform.position;
        _worldBounds.max = localBounds.max + transform.position;

        _baseOctree.Reposition(transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(localBounds.Center, localBounds.Size);
    }


}
