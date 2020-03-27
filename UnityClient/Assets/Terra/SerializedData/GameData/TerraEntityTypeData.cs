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
        private Sprite _debugImage;
        public Sprite DebugImage => _debugImage;
        
        [SerializeField]
        private int _grassPotentialReductionRadius;
        public int GrassPotentialReductionRadius => _grassPotentialReductionRadius;
        
        [SerializeField]
        private AnimationCurve _grassPotentialReductionCurve;
        public AnimationCurve GrassPotentialReductionCurve => _grassPotentialReductionCurve;

        public override string ToString()
        {
            return _entityId;
        }
    }
}