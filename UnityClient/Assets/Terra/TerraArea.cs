using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terra
{
    [Serializable]
    public class TerraArea : IEnumerable<TerraVector>
    {
        public int x;
        public int y;
        public int width;
        public int height;

        public int Left => x;
        public int Right => x+width;
        public int Top => y;
        public int Bottom => y+height;

        public int Area => width * height;

        public TerraArea()
        {
            
        }
        
        public TerraArea(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public IEnumerator<TerraVector> GetEnumerator()
        {
            for (int x = Left; x < Right; x++)
            {
                for (int y = Top; y < Bottom; y++)
                {
                    yield return new TerraVector(x, y);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is TerraArea)
            {
                return this == (TerraArea)obj;
            }
            return base.Equals(obj);
        }

        public static bool operator ==(TerraArea a, TerraArea b)
        {
            return a.x == b.x && a.y == b.y && a.width == b.width && a.height == b.height;
        }

        public static bool operator !=(TerraArea a, TerraArea b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return $"[x:{x}, y:{y}, width:{width}, height:{height}]";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(TerraVector terraVector, int bevel = 0)
        {
            return x + bevel < terraVector.x && x + width - bevel > terraVector.x && y + bevel < terraVector.y && y + height - bevel > terraVector.y;
        }
        
        public bool Contains(Vector3 position, float bevel = 0)
        {
            return x + bevel < position.x && x + width - bevel > position.x && y + bevel < position.z && y + height - bevel > position.z;
        }
        
        public bool Contains(TerraArea area)
        {
            return (x < area.x + area.width) && (y < area.y + area.height) &&
                   (area.x < x + width) &&
                   (area.y < y + height);
            return Left < area.Right && Right > area.Left &&
                   Top > area.Bottom && Bottom < area.Top;
        }
    }
}