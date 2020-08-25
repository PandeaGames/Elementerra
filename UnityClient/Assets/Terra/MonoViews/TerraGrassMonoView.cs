using System;
using System.Collections.Generic;
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

        public Material primaryMaterial;
        public Material secondaryMaterial;

        [Serializable]
        public struct Config
        {
            public GameObject gameObject;
            public float threshold;
        }

        public List<Config> _config;

        private void Start()
        {
            _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);
            _config.Sort((a, b) =>
            {
                if (a.threshold == b.threshold)
                {
                    return 0;
                }

                return a.threshold > b.threshold ? 1 : -1;
            });
        }
        
        public void SetData(TerraVector vector, TerraGrassNode dataNode)
        {
            _terraWorldStateViewModel = Game.Instance.GetViewModel<TerraWorldStateViewModel>(0);
            _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);

            foreach (Renderer renderer in _renderer)
            {
                renderer.material = _terraViewModel.TerraAlterVerseViewModel[vector]
                    ? secondaryMaterial
                    : primaryMaterial;
                /*renderer.gameObject.layer = _terraViewModel.TerraAlterVerseViewModel[vector]
                    ? LayerMask.NameToLayer("BetaDimension")
                    : LayerMask.NameToLayer("AlphaDimension");*/
            }

            bool foundGrassThreshold = false;
            foreach (Config config in _config)
            {
                if (!foundGrassThreshold && dataNode.Grass <= config.threshold)
                {
                    config.gameObject.SetActive(true);
                    foundGrassThreshold = true;
                    continue;
                }
                
                config.gameObject.SetActive(false);
            }
            
            //gameObject.SetActive(dataNode.Grass > 0);
            /*transform.localScale = new Vector3(
                1, dataNode.Scale, 1
                );*/
                
        }
    }
}