using UnityEngine;

public class Entity : MonoBehaviour
{
    public Vector3 velocity;
    public Bounds AABB;
    public void Initialize(Vector3 position, Vector3 velocity)
    {
        transform.position = position;
        this.velocity = velocity;
        MeshFilter mesh = GetComponent<MeshFilter>();
        AABB = new Bounds(mesh.mesh.bounds);
    }

    public void Move()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.Scale(AABB.Size, transform.lossyScale));
    }
}