using UnityEngine;

public class Entity : MonoBehaviour
{
    public Vector3 velocity;
    public Bounds AABB;
    public bool isColliding = false;
    private bool _isFast = false;
    private bool _isPaused = false;

    public void Initialize(Vector3 position, Vector3 velocity)
    {
        transform.position = position;
        this.velocity = velocity;
        MeshFilter mesh = GetComponent<MeshFilter>();
        AABB = new Bounds(mesh.mesh.bounds);
        AABB.Move(transform.position);
    }

    public void Move()
    {
        if (!_isPaused)
        {
            Vector3 delta = velocity * Time.deltaTime;
            transform.position += delta;
            AABB.Move(delta);
        }
    }

    public void Reverse()
    {
        velocity *= -1;
    }

    public void ToggleFast()
    {
        if (_isFast)
        {
            velocity *= 0.5f;
            _isFast = false;
        }
        else
        {
            velocity *= 2;
            _isFast = true;
        }
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
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