using PandeaGames;
using Terra.ViewModels;
using TMPro;
using UnityEngine;

namespace Terra.WorldContextUI
{
    public class HoldItemInHandContentUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField] 
        private GameObject _contextUIInfo;
        
        private PlayerStateViewModel _vm;
        private WorldContextViewModel _contextUIModel;
        
        private void Start()
        {
            _contextUIModel = Game.Instance.GetViewModel<WorldContextViewModel>(0);
            _vm = Game.Instance.GetViewModel<PlayerStateViewModel>(0);
        }

        private void Update()
        {
            _text.text = "[Q] Hold in Hand";
            _contextUIInfo.SetActive(_contextUIModel.CurrentContext == WorldContextViewModel.Context.PutInInventory);
        }
    }
}