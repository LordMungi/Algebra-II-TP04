using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Triangle : IPoint
{
    public Vector3 offset;

    private Vector3 _center;
    private Vector3 _v0;
    private Vector3 _v1;
    private Vector3 _v2;

    public bool isColliding = false;

    public float Area
    {
        get
        {
            return Vector3.Cross(_v1 - _v0, _v2 - _v0).magnitude / 2;
        }
    }
    public Vector3 w0 { get { return _v0 + offset; } }
    public Vector3 w1 { get { return _v1 + offset; } }
    public Vector3 w2 { get { return _v2 + offset; } }

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
        Color c = Color.yellow;
        if (isColliding)
            c = Color.red;

        Debug.DrawLine(_v0 + offset, _v1 + offset, c);
        Debug.DrawLine(_v1 + offset, _v2 + offset, c);
        Debug.DrawLine(_v2 + offset, _v0 + offset, c);
    }

    public bool IsColliding(Triangle other)
    {
        Plane plane = new Plane(w0, w1, w2);

        if (plane.GetSide(other.w0) && plane.GetSide(other.w1) && plane.GetSide(other.w2))
            return false;

        if (IsEdgeColliding(plane, other.w0, other.w1)) return true;
        if (IsEdgeColliding(plane, other.w1, other.w2)) return true;
        if (IsEdgeColliding(plane, other.w2, other.w0)) return true;
        return false;
    }

    private bool IsEdgeColliding(Plane plane, Vector3 v1, Vector3 v2)
    {
        Vector3 edge = v2 - v1;
        Ray ray = new Ray(v1, edge.normalized);

        if (plane.Raycast(ray, out float enter) && enter <= edge.magnitude)
        {
            Vector3 hit = ray.GetPoint(enter);

            Triangle t1 = new Triangle(hit, w0, w1);
            Triangle t2 = new Triangle(hit, w1, w2);
            Triangle t3 = new Triangle(hit, w2, w0);

            if (Mathf.Abs((t1.Area + t2.Area + t3.Area) - Area) <= 0.01f)
                return true;
        }
        return false;
    }

}