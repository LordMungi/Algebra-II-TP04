using System;
using UnityEngine;

[Serializable]
public struct Bounds 
{
    public Vector3 min;
    public Vector3 max;

    public Vector3 Center
    {
        get { return Size / 2 + min; }
        set { }
    }
    public Vector3 Size
    {
        get { return max - min; }
        set { }
    }

    public Bounds(Bounds other)
    {
        this.min = other.min;
        this.max = other.max;

        InvertIfImpossible(ref min.x, ref max.x);
        InvertIfImpossible(ref min.y, ref max.y);
        InvertIfImpossible(ref min.z, ref max.z);
    }

    public Bounds(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
    {
        min.x = minX;
        min.y = minY;
        min.z = minZ;
        max.x = maxX;
        max.y = maxY;
        max.z = maxZ;

        InvertIfImpossible(ref min.x, ref max.x);
        InvertIfImpossible(ref min.y, ref max.y);
        InvertIfImpossible(ref min.z, ref max.z);
    }
    public Bounds(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;

        InvertIfImpossible(ref min.x, ref max.x);
        InvertIfImpossible(ref min.y, ref max.y);
        InvertIfImpossible(ref min.z, ref max.z);
    }

    public bool Contains(Vector3 point)
    {
        return 
            (
            point.x >= min.x && point.x <= max.x &&
            point.y >= min.y && point.y <= max.y &&
            point.z >= max.z && point.z <= max.z
            );
    }

    private void InvertIfImpossible(ref float minf, ref float maxf)
    {
        float aux = 0;
        if (minf > maxf)
        {
            aux = maxf;
            maxf = minf;
            minf = aux; 
        }
    }
}