using System;
using UnityEngine;

namespace Terra
{
    [Serializable]
    public class TerraArea
    {
        public int x;
        public int y;
        public int width;
        public int height;

        public int Left => x;
        public int Right => x+width;
        public int Top => y;
        public int Bottom => y+height;
    

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

        public override string ToString()
        {
            return $"[x:{x}, y:{y}, width:{width}, height:{height}]";
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