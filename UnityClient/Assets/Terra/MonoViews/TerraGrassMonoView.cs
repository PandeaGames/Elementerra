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
        }
        
        public void SetData(TerraVector vector, TerraGrassNode dataNode)
        {
            _terraWorldStateViewModel = Game.Instance.GetViewModel<TerraWorldStateViewModel>(0);
            _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);

            foreach (Renderer renderer in _renderer)
            {
                renderer.gameObject.layer = _terraViewModel.TerraAlterVerseViewModel[vector]
                    ? LayerMask.NameToLayer("BetaDimension")
                    : LayerMask.NameToLayer("AlphaDimension");
            }
            gameObject.SetActive(dataNode.Grass > 0);
            transform.localScale = new Vector3(
                1, dataNode.Scale, 1
                );
                
        }
    }
}