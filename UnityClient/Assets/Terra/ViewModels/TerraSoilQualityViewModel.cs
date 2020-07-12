using System.Collections.Generic;
using PandeaGames.ViewModels;
using UnityEngine;

namespace Terra.ViewModels
{
    public class TerraSoilQualityGridPoint : GridDataPoint<Color>
    {
        
    }
    
    public class TerraSoilQualityViewModel : AbstractGridDataModel<Color, TerraSoilQualityGridPoint>
    {
        private TerraGrassPotentialViewModel _grassPotentialViewModel;
        public TerraSoilQualityViewModel(TerraGrassPotentialViewModel grassPotentialViewModel) : base((uint)grassPotentialViewModel.Width, (uint)grassPotentialViewModel.Height)
        {
            _grassPotentialViewModel = grassPotentialViewModel;
            _grassPotentialViewModel.OnDataHasChanged += UpdateData;
            UpdateData(_grassPotentialViewModel.AllData());
        }

        private void UpdateData(IEnumerable<TerraGrassPotentialNodeGridPoint> data)
        {
            foreach (TerraGrassPotentialNodeGridPoint dataPoint in data)
            {
                this[dataPoint.Vector] = GetValue(dataPoint);
            }
            
            DataHasChanged(ReportDataChangeForRange<TerraGrassPotentialNodeGridPoint, float>(data));
        }

        private Color GetValue(TerraGrassPotentialNodeGridPoint dataPoint)
        {
            float soilQualityValue = 0;

            soilQualityValue = dataPoint.Data;
                    
            return new Color(
                soilQualityValue, 
                soilQualityValue,
                soilQualityValue);
        }
    }
}