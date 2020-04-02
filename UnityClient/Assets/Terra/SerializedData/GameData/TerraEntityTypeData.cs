using System;
using Terra.SerializedData.Entities;
using UnityEngine;

namespace Terra.SerializedData.GameData
{
    [Serializable]
    public class TerraEntityTypeData : ITerraEntityType
    {
        [SerializeField]
        private EntityComponent component;
        public EntityComponent Component => component;
        
        [SerializeField]
        private string _entityId;
        public string EntityID => _entityId;
        
        [SerializeField]
        private string _plantableEntityId;
        public string PlantableEntityId => _plantableEntityId;
        
        [SerializeField]
        private string _spawnableEntityId;
        public string SpawnableEntityId => _spawnableEntityId;
        
        [SerializeField]
        private Sprite _debugImage;
        public Sprite DebugImage => _debugImage;
        
        [SerializeField]
        private int _grassPotentialReductionRadius;
        public int GrassPotentialReductionRadius => _grassPotentialReductionRadius;
        
        [SerializeField]
        private AnimationCurve _grassPotentialReductionCurve;
        public AnimationCurve GrassPotentialReductionCurve => _grassPotentialReductionCurve;
        
        [SerializeField]
        private float _ripeTimeSeconds;
        public float RipeTimeSeconds => _ripeTimeSeconds;

        public bool IsPlantable => !string.IsNullOrEmpty(PlantableEntityId);
        public bool IsSpawnable => !string.IsNullOrEmpty(SpawnableEntityId);

        public override string ToString()
        {
            return _entityId;
        }
    }
}