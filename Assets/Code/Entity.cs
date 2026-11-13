using UnityEngine;

public class Entity : MonoBehaviour
{
    public void Relocate(Vector3 position)
    {
        transform.position = position;
    }
}