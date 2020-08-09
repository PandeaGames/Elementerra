
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public struct TerraVector
{
    public int x;
    public int y;
    
    public TerraVector(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override int GetHashCode()
    {
        //https://math.stackexchange.com/questions/23503/create-unique-number-from-2-numbers
        return (x * x) * (y * y * y);
    }

    public static implicit operator TerraVector(int radius)
    {
        return new TerraVector(radius, radius);
    }

    public static bool operator ==(TerraVector a, TerraVector b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(TerraVector a, TerraVector b)
    {
        return !(a == b);
    }

    public override string ToString()
    {
        return $"{x}:{y}";
    }

    public static float Distance(TerraVector a, TerraVector b)
    {
        float dx = b.x - a.x;
        float dy = b.y - a.y;

        return Mathf.Sqrt(dy * dy + dx * dx);
    }
    
    
}

public interface ITerraShape
{
    TerraVector[] TerraVectors { get; }
}

public static class TerraVectorExtensions
{
    //https://stackoverflow.com/questions/4243042/c-sharp-point-in-polygon
    public static bool IsInPolygon(this TerraVector point, IEnumerable<TerraVector> polygon)
    {
        bool result = false;
        var a = polygon.Last();
        foreach (var b in polygon)
        {
            if ((b.x == point.x) && (b.y == point.y))
                return true;

            if ((b.y == a.y) && (point.y == a.y) && (a.x <= point.x) && (point.x <= b.x))
                return true;

            if ((b.y < point.y) && (a.y >= point.y) || (a.y < point.y) && (b.y >= point.y))
            {
                if (b.x + (point.y - b.y) / (a.y - b.y) * (a.x - b.x) <= point.x)
                    result = !result;
            }
            a = b;
        }
        return result;
    }
}
