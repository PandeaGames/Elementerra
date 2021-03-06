using System;
using Data;
using Terra.Inventory.UnityData;
using Terra.StaticData;
using UnityEditor;
using UnityEngine;
    
namespace PandeaGames.Data
{
    public class TerraGameResources : ScriptableObjectSingleton<TerraGameResources>, ILoadableObject
    {
        public const int PLAYER_INSTANCE_ID = Int32.MaxValue;
#if UNITY_EDITOR
        public const string AssetPath = "Assets/Resources/Terra/TerraGameResources.asset";
        
        [MenuItem("Terra/Data/Generate Game Resources")]
        public static void CreateAsset()
        {
            TerraGameResources gameResources = ScriptableObjectFactory.CreateInstance<TerraGameResources>();
            AssetDatabase.CreateAsset(gameResources, AssetPath);
            EditorUtility.DisplayDialog("Asset Created", string.Format("Asset has been added at '{0}'", AssetPath),
                "OK");
        }
#endif
        [SerializeField] private TerraEntityPrefabConfigSO _terraEntityPrefabConfigSO;
        public TerraEntityPrefabConfig TerraEntityPrefabConfig
        {
            get => _terraEntityPrefabConfigSO.Data;
        }
        
        [SerializeField] private Material _terrainMaterial;
        public Material TerrainMaterial
        {
            get => _terrainMaterial;
        }
        
        [SerializeField] private AnimationCurve _soilQualityCurve;
        public AnimationCurve SoilQualityCurve
        {
            get => _soilQualityCurve;
        }
        
        [SerializeField] private GameObject _terraDebugWindow;
        public GameObject TerraDebugWindow
        {
            get => _terraDebugWindow;
        }
        
        [SerializeField] private GameObject _terraWorldView;
        public GameObject TerraWorldView
        {
            get => _terraWorldView;
        }
        
        [SerializeField] 
        private string _layerForTerrain;
        public string LayerForTerrain
        {
            get => _layerForTerrain;
        }

        [SerializeField]
        private TimeOfDayConfigSO _timeOfDayConfigSO;
        public TimeOfDayConfigSO TimeOfDayConfigSO
        {
            get => _timeOfDayConfigSO;
        }
        
        [SerializeField]
        private InventoryDataTypeSO _playerInventoryType;
        public InventoryDataTypeSO PlayerInventoryType
        {
            get => _playerInventoryType;
        }

        [SerializeField]
        private InventoryTypesSO _inventoryTypesSo;
        public InventoryTypesSO InventoryTypesSO => _inventoryTypesSo;
        
        [SerializeField]
        private InventoryItemDataListSO _InventoryItemDataListSO;
        public InventoryItemDataListSO InventoryItemDataListSO => _InventoryItemDataListSO;
        
        [SerializeField]
        private GameObject _mainMenuView;
        public GameObject MainMenuView
        {
            get => _mainMenuView;
        }
        
        public void LoadAsync(LoadSuccess onLoadSuccess, LoadError onLoadFailed)
        {
            LoaderGroup loaderGroup = new LoaderGroup();
            loaderGroup.LoadAsync(onLoadSuccess, onLoadFailed);
        }

        public bool IsLoaded { get; }
    }
}