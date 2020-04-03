using UnityEngine;

namespace Terra
{
    public class TerraArea
    {
        public int x;
        public int y;
        public int width;
        public int height;

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
    }
}