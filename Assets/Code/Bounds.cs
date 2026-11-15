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
    }
    public Vector3 Size
    {
        get { return max - min; }
    }

    public Bounds(Bounds other)
    {
        min = other.min;
        max = other.max;

        InvertIfImpossible(ref min.x, ref max.x);
        InvertIfImpossible(ref min.y, ref max.y);
        InvertIfImpossible(ref min.z, ref max.z);
    }

    public Bounds(UnityEngine.Bounds other)
    {
        min = other.min;
        max = other.max;

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
            point.x >= min.x && point.x <= max.x &&
            point.y >= min.y && point.y <= max.y &&
            point.z >= min.z && point.z <= max.z;
    }
    public bool Collides(Bounds other)
    {
        return
            Center.x + Size.x / 2 > other.Center.x - other.Size.x / 2 &&
            Center.x - Size.x / 2 < other.Center.x + other.Size.x / 2 &&
            Center.y + Size.y / 2 > other.Center.y - other.Size.y / 2 &&
            Center.y - Size.y / 2 < other.Center.y + other.Size.y / 2 &&
            Center.z + Size.z / 2 > other.Center.z - other.Size.z / 2 &&
            Center.z - Size.z / 2 < other.Center.z + other.Size.z / 2;
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