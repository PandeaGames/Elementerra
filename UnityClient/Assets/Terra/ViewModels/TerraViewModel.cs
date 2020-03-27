﻿using System;
using PandeaGames;
using PandeaGames.ViewModels;
using Terra.SerializedData.World;

namespace Terra.ViewModels
{
    public class TerraViewModel : IViewModel
    {
        public Action<TerraTerrainGeometryDataModel> OnGeometryUpdate;
        
        private TerraWorldViewModel _worldViewModel;
        public TerraWorldChunk Chunk { get; private set; }
        
        public TerraTerrainGeometryDataModel Geometry { get; private set; }
        public TerraGrassViewModel Grass { get; private set; }
        public TerraGrassPotentialViewModel GrassPotential { get; private set; }

        public TerraViewModel()
        {
            _worldViewModel = Game.Instance.GetViewModel<TerraWorldViewModel>(0);
        }

        public void SetChunk(TerraWorldChunk chunk)
        {
            Chunk = chunk;
            Geometry = new TerraTerrainGeometryDataModel(chunk);
            GrassPotential = new TerraGrassPotentialViewModel(Geometry, Game.Instance.GetViewModel<TerraEntitiesViewModel>(0), chunk);
            Grass =  new TerraGrassViewModel(Geometry, GrassPotential);
            OnGeometryUpdate?.Invoke(Geometry);
        }
        
        public void Reset()
        {
            
        }
    }
}