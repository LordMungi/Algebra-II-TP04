using UnityEngine;

public class Entity : MonoBehaviour
{
    public Vector3 velocity;
    public Bounds AABB;
    public bool isColliding = false;
    private bool _isFast = false;
    private bool _isPaused = false;
    private bool _isReversed = false;

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
            if (_isFast) delta *= 2;
            if (_isReversed) delta *= -1;

            transform.position += delta;
            AABB.Move(delta);
        }
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

    private void OnDrawGizmos()
    {
        if (isColliding)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(transform.position, AABB.Size);
    }
}