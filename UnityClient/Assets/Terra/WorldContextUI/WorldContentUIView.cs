using System;
using PandeaGames;
using UnityEngine;

namespace Terra.WorldContextUI
{
    public class WorldContentUIView : MonoBehaviour
    {
        [SerializeField]
        private ContextControlUI _contextControlUI;
        
        private WorldContextViewModel _contextUIModel;
        
        private void Start()
        {
            _contextUIModel = Game.Instance.GetViewModel<WorldContextViewModel>(0);
            _contextUIModel.OnChange += ContextUiModelOnChange;
        }

        private void ContextUiModelOnChange(Vector3 vector3, WorldContextViewModel.Context context, int data)
        {
            
        }

        private void Update()
        {
        }
    }
}