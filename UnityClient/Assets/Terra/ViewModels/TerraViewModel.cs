using System;
using System.Numerics;
using PandeaGames;
using PandeaGames.ViewModels;
using Terra.MonoViews;
using Terra.SerializedData.Entities;
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
        public TerraEntityMonoView PlayerEntity { get; private set; }
        public TerraPathfinderViewModel TerraPathfinderViewModel { get; private set; }
        public TerraSoilQualityViewModel TerraSoilQualityViewModel { get; private set; }
        public TerraAlterVerseViewModel TerraAlterVerseViewModel { get; private set; }

        public TerraViewModel()
        {
            _worldViewModel = Game.Instance.GetViewModel<TerraWorldViewModel>(0);
        }

        public void SetChunk(TerraWorldChunk chunk)
        {
            TerraEntitiesViewModel entitiesModel = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);
            Chunk = chunk;
            TerraAlterVerseViewModel = new TerraAlterVerseViewModel(entitiesModel, chunk);
            TerraPathfinderViewModel = new TerraPathfinderViewModel(chunk);
            Geometry = new TerraTerrainGeometryDataModel(chunk);
            GrassPotential = new TerraGrassPotentialViewModel(Geometry, entitiesModel, chunk);
            Grass =  new TerraGrassViewModel(Geometry, GrassPotential);
            TerraSoilQualityViewModel = new TerraSoilQualityViewModel(GrassPotential);
            OnGeometryUpdate?.Invoke(Geometry);
        }
                
        public void RegisterEntity(TerraEntityMonoView view)
        {
            PlayerEntity = view;
        }
        
        public void Reset()
        {
            
        }
    }
}