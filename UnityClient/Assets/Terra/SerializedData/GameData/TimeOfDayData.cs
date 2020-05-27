using System;
using UnityEngine;

namespace Terra.SerializedData.GameData
{
    [Serializable]
    public class TimeOfDayData
    {
        [Serializable]
        public struct TimeOfDay
        {
            [Range(0, 1)]
            public float Time;
            public string ID;
        }
        
        public float DayLengthSeconds;
        public TimeOfDay[] TimesOfDay;
    }
}