using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IPoint
{
    public Vector3 velocity;
    public Bounds AABB;
    public float radius;
    public bool isCollidingAABB = false;
    public bool isCollidingRadius = false;
    private bool _isFast = false;
    private bool _isPaused = false;
    private bool _isReversed = false;

    public List<Triangle> triangles = new List<Triangle>();

    public Octree<Triangle> meshTree;

    public void Initialize(Vector3 position, Vector3 velocity)
    {
        transform.position = position;
        this.velocity = velocity;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        AABB = new Bounds(mesh.bounds);
        AABB.Scale(transform.lossyScale);
        meshTree = new Octree<Triangle>(AABB, 10);
        AABB.Move(transform.position);

        radius = Mathf.Abs((transform.position - AABB.max).magnitude) * 2;
        for (int i = 0; i < mesh.triangles.Length / 3; i++)
        {
            int index = i * 3;
            triangles.Add(new Triangle(Vector3.Scale(mesh.vertices[mesh.triangles[index]], transform.lossyScale), 
                                        Vector3.Scale(mesh.vertices[mesh.triangles[index + 1]], transform.lossyScale), 
                                        Vector3.Scale(mesh.vertices[mesh.triangles[index + 2]], transform.lossyScale)));
        }

        foreach (Triangle triangle in triangles)
        {
            meshTree.Insert(triangle);
            triangle.SetOffset(transform.position);
        }
        meshTree.Relocate(transform.position);
    }

    public void Update()
    {
        if (!_isPaused)
        {
            Vector3 delta = velocity * Time.deltaTime;
            if (_isFast) delta *= 2;
            if (_isReversed) delta *= -1;

            transform.position += delta;
            AABB.Move(delta);
            foreach (Triangle triangle in triangles)
            {
            }
        }
        foreach (Triangle triangle in triangles)
        {
            triangle.SetOffset(transform.position);
            triangle.Draw();
        }
        meshTree.Relocate(transform.position);
        meshTree.Draw();
    }

    public void SetReverse(bool value)
    {
        _isReversed = value;
    }

    public void SetFast(bool value)
    {
        _isFast = value;
    }

    public void SetPause(bool value)
    {
        _isPaused = value;
    }
    public bool IsInRadiusOfOther(Entity other)
    {
        return Mathf.Abs((transform.position - other.transform.position).magnitude) <= radius + other.radius;
    }

    private void OnDrawGizmos()
    {
        if (isCollidingAABB)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, AABB.Size);

        if (isCollidingRadius)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}