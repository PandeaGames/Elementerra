using System;
using UnityEngine;

namespace Terra.Utils
{
    public static class TerraPlayerPrefs
    {
        [Flags]
        public enum TerraTerrainDebugViewTypes
        {
            Pathable = 1 << 1,
            LocalPositions = 1 << 2,
            WorldPositions = 1 << 3,
            Pathfinding = Pathable | LocalPositions
        }

        public static TerraTerrainDebugViewTypes TerraTerrainDebugViewType
        {
            get
            {
                TerraTerrainDebugViewTypes value = (TerraTerrainDebugViewTypes)Enum.ToObject(typeof(TerraTerrainDebugViewTypes) , PlayerPrefs.GetInt("TerraTerrainDebugViewType"));
                return value;
            }
            set
            {
                PlayerPrefs.SetInt("TerraTerrainDebugViewType", (int) value);
            }
        }
    }
}