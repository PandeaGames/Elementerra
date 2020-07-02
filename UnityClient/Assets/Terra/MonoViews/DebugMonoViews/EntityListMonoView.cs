using System.Collections.Generic;
using PandeaGames;
using PandeaGames.Data;
using Terra.SerializedData.GameData;

namespace Terra.MonoViews.DebugMonoViews
{
    public class EntityListMonoView : AbstractListView<TerraEntityTypeData, EntityMonoView>
    {
        private TerraDebugControlViewModel _vm;
        protected override void Start()
        {
            _vm = Game.Instance.GetViewModel<TerraDebugControlViewModel>(0);
            List<TerraEntityTypeData> data = new List<TerraEntityTypeData>();
            foreach (TerraEntityTypeSO entityTypeSO in TerraGameResources.Instance.TerraEntityPrefabConfig.DataConfig)
            {
                data.Add(entityTypeSO.Data);
            }
            
            base.Start();
            SetData(data);
            gameObject.SetActive(false);
        }

        private void Update()
        {
            _itemViewContainer.gameObject.SetActive(_vm.CurrentState == TerraDebugControlViewModel.States.PlaceEntity);
        }
        
        protected override void OnItemSelect(IListItem<TerraEntityTypeData> listItem)
        {
            base.OnItemSelect(listItem);
            _vm.PlaceEntity(listItem.GetData());
        }
    }
}