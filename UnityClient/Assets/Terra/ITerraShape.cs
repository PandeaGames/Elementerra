
using System;


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
}

public interface ITerraShape
{
    TerraVector[] TerraVectors { get; }
}
