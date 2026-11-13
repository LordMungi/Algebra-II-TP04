using UnityEngine;

public class Entity : MonoBehaviour
{
    public Vector3 velocity;

    public void Initialize(Vector3 position, Vector3 velocity)
    {
        transform.position = position;
        this.velocity = velocity; 
    }

    public void Move()
    {
        transform.position += velocity * Time.deltaTime;
    }
}