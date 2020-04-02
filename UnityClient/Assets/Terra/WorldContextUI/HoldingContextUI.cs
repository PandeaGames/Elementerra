using PandeaGames;
using PandeaGames.Data;
using Terra.SerializedData.GameData;
using Terra.StaticData;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.WorldContextUI
{
    public class HoldingContextUI: MonoBehaviour
    {
        private WorldContextViewModel _contextUIModel;
        private PlayerStateViewModel _playerStateViewModel;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField] private GameObject _planUI;
        [SerializeField] private GameObject _dropUI;
        
        public float smoothTime = 0.3f;
        float yVelocity = 0.0f;
        
        private void Start()
        {
            _contextUIModel = Game.Instance.GetViewModel<WorldContextViewModel>(0);
            _playerStateViewModel = Game.Instance.GetViewModel<PlayerStateViewModel>(0);
        }

        private void Update()
        {
            switch (_contextUIModel.CurrentContext)
            {
                case WorldContextViewModel.Context.None:
                case WorldContextViewModel.Context.PickUp:
                {
                    _canvasGroup.alpha = Mathf.SmoothDamp(_canvasGroup.alpha, 0, ref yVelocity, smoothTime);
                    break;
                }
                case WorldContextViewModel.Context.Holding:
                {
                    _canvasGroup.alpha = Mathf.SmoothDamp(_canvasGroup.alpha, 1, ref yVelocity, smoothTime);
                    break;
                }
            }

            TerraEntityTypeData entityData = TerraGameResources.Instance.TerraEntityPrefabConfig
                .GetEntityConfig(_playerStateViewModel);

            if (entityData != null)
            {
                bool isPlantable = TerraGameResources.Instance.TerraEntityPrefabConfig
                    .GetEntityConfig(_playerStateViewModel).IsPlantable;
            
                _planUI.SetActive(isPlantable);
                //_dropUI.SetActive(!isPlantable);
            }
        }
    }
}