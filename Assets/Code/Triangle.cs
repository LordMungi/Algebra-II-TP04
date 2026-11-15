using UnityEngine;

public struct Triangle : IPoint
{
    public Vector3 offset;

    private Vector3 _center;
    private Vector3 _v0;
    private Vector3 _v1;
    private Vector3 _v2;

    public Triangle(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        offset = Vector3.zero;

        _v0 = v0;
        _v1 = v1;
        _v2 = v2;

        _center = (_v0 + _v1 + _v2) / 3; 
    }

    public void SetOffset(Vector3 offset)
    {
        this.offset = offset; 
    }

    public Vector3 GetPosition()
    {
        return _center + offset;
    }

    public void Draw()
    {
        Debug.DrawLine(_v0 + offset, _v1 + offset, Color.yellow);
        Debug.DrawLine(_v1 + offset, _v2 + offset, Color.yellow);
        Debug.DrawLine(_v2 + offset, _v0 + offset, Color.yellow);
    }
}