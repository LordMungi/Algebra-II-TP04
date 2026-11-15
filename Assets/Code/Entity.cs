using UnityEngine;

public class Entity : MonoBehaviour
{
    public Vector3 velocity;
    public Bounds AABB;
    public bool isColliding = false;
    public void Initialize(Vector3 position, Vector3 velocity)
    {
        transform.position = position;
        this.velocity = velocity;
        MeshFilter mesh = GetComponent<MeshFilter>();
        AABB = new Bounds(mesh.mesh.bounds);
    }

    public void Move()
    {
        Vector3 delta = velocity * Time.deltaTime;
        transform.position += delta;
        AABB.Move(delta);
    }

    private void OnDrawGizmos()
    {
        if (isColliding)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(transform.position, AABB.Size);
    }
}