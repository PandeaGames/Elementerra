using System;
using System.Linq;
using Terra.Inventory;
using Terra.Inventory.UnityData;
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
        private int _totalHealth;
        public int TotalHealth => _totalHealth;
        
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
        private bool _isUniverseGateway;
        public bool IsUniverseGateway => _isUniverseGateway;
        
        [SerializeField]
        private int _universeGatewayRadius;
        public int UniverseGatewayRadius => _universeGatewayRadius;
        
        [SerializeField]
        private AnimationCurve _grassPotentialReductionCurve;
        public AnimationCurve GrassPotentialReductionCurve => _grassPotentialReductionCurve;
        
        [SerializeField]
        private float _ripeTimeSeconds;
        public float RipeTimeSeconds => _ripeTimeSeconds;
        
        [SerializeField]
        private float _lifespanSeconds;
        public float LifespanSeconds => _lifespanSeconds;
        
        [SerializeField]
        private TerraEntityTypeSO _entityToSpawnAfterDeath;
        public TerraEntityTypeSO EntityToSpawnAfterDeath => _entityToSpawnAfterDeath;
        
        [SerializeField]
        private InventoryItemDataSO _inventoryItemDataSO;
        public InventoryItemDataSO InventoryItemDataSO => _inventoryItemDataSO;
        
        [SerializeField]
        private string[] _labels = new string[]{};
        public string[] Labels => _labels;
        
        [SerializeField]
        private string _aggroLabel;
        public string AggroLabel => _aggroLabel;
        
        [SerializeField]
        private float _attackRange;
        public float AttackRange => _attackRange;
        
        [SerializeField]
        private int _attackDamage;
        public int AttackDamage => _attackDamage;
        
        [SerializeField]
        private bool _isSlavable;
        public bool IsSlavable => _isSlavable;
        
        [SerializeField]
        private int _numberOfLevels;
        public int NumberOfLevels => _numberOfLevels;

        [SerializeField] private TerraEntityTypeSO _entityToSpawnAfterUpgraded;
        public TerraEntityTypeSO EntityToSpawnAfterUpgraded => _entityToSpawnAfterUpgraded;
        
        [SerializeField]
        private InventoryDataTypeSO _inventory;
        public IInventoryDataType Inventory => _inventory == null ? null:_inventory.Data;
        
        public bool IsPlantable => !string.IsNullOrEmpty(PlantableEntityId);
        public bool IsSpawnable => !string.IsNullOrEmpty(SpawnableEntityId);
        
        public override string ToString()
        {
            return _entityId;
        }
        
        public bool HasLabel(string label)
        {
            return _labels.Contains(label);
        }
    }
}