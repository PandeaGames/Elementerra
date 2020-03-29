using System;
using PandeaGames;
using Terra.SerializedData.World;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews.TimeOfDayMonoViews
{
    [Serializable]
    public struct TODLightColor
    {
        public Color Color;
        
        [Range(0f, 1f)]
        public float TimeOfDay;
        
        public Transform Transform;
        [Range(0f, 1f)]
        public float Intensity;
    }
    
    public class TerraTODLightColor : MonoBehaviour
    {
        [SerializeField] 
        private Light _light;

        public TODLightColor[] TODLightColor;

        private TerraWorldStateViewModel _vm;

        private void Start()
        {
            _vm = Game.Instance.GetViewModel<TerraWorldStateViewModel>(0);
        }

        private void Update()
        {
            int startIndex = 0;
            int endIndex = 1;
            
            for (int i = 0; i < TODLightColor.Length; i++)
            {
                if (TODLightColor[i].TimeOfDay < _vm.CurrentDayProgress)
                {
                    startIndex = i;
                    endIndex = i == TODLightColor.Length - 1 ? 0 : i + 1;
                }
                else if(_vm.CurrentDayProgress !=0)
                {
                    endIndex = i;
                    break;
                }
            }

            TODLightColor start = TODLightColor[startIndex];
            TODLightColor end = TODLightColor[endIndex];

            float progressBetweenConfig =
                (_vm.CurrentDayProgress - start.TimeOfDay) / (end.TimeOfDay - start.TimeOfDay);
            
            _light.color = new Color(
                start.Color.r + (end.Color.r - start.Color.r) * progressBetweenConfig,
                start.Color.g + (end.Color.g - start.Color.g) * progressBetweenConfig,
                start.Color.b + (end.Color.b - start.Color.b) * progressBetweenConfig
                );

            _light.intensity = start.Intensity + (end.Intensity - start.Intensity) * progressBetweenConfig;
            if (start.Transform != null)
            {
                _light.transform.rotation = Quaternion.Euler(
                    start.Transform.rotation.eulerAngles.x + (end.Transform.rotation.eulerAngles.x - start.Transform.rotation.eulerAngles.x) * progressBetweenConfig,
                    start.Transform.rotation.eulerAngles.y + (end.Transform.rotation.eulerAngles.y - start.Transform.rotation.eulerAngles.y) * progressBetweenConfig,
                    start.Transform.rotation.eulerAngles.z + (end.Transform.rotation.eulerAngles.z - start.Transform.rotation.eulerAngles.z) * progressBetweenConfig
                );
            }
        }
    }
}