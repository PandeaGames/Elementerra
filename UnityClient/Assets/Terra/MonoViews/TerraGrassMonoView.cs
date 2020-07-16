using PandeaGames;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraGrassMonoView : MonoBehaviour
    {
        public float scaler;
        public Renderer[] _renderer;

        private TerraViewModel _terraViewModel;
        private TerraWorldStateViewModel _terraWorldStateViewModel;

        private void Start()
        {
            _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);
            _terraWorldStateViewModel = Game.Instance.GetViewModel<TerraWorldStateViewModel>(0);
            _terraWorldStateViewModel.OnWorldFlipChange += TerraWorldStateViewModelOnWorldFlipChange;
        }

        private TerraVector _vector;

        private void TerraWorldStateViewModelOnWorldFlipChange(bool IsWorldFipped)
        {
            bool rendererEnabled = IsWorldFipped ? !_terraViewModel.TerraAlterVerseViewModel[_vector]:_terraViewModel.TerraAlterVerseViewModel[_vector];
            foreach (Renderer renderer in _renderer)
            {
                renderer.enabled = rendererEnabled;
            }
        }

        public void SetData(TerraVector vector, TerraGrassNode dataNode)
        {
            _vector = vector;
            _terraWorldStateViewModel = Game.Instance.GetViewModel<TerraWorldStateViewModel>(0);
            _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);
            bool rendererEnabled = _terraWorldStateViewModel.IsWorldFipped ? !_terraViewModel.TerraAlterVerseViewModel[vector]:_terraViewModel.TerraAlterVerseViewModel[vector];
            foreach (Renderer renderer in _renderer)
            {
                renderer.enabled = rendererEnabled;
            }
            gameObject.SetActive(dataNode.Grass > 0);
            transform.localScale = new Vector3(
                1, dataNode.Scale, 1
                );
        }
    }
}