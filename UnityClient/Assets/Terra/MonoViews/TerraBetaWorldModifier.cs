using System;
using PandeaGames;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraBetaWorldModifier:MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;

        [SerializeField] private float _massAlpha;
        [SerializeField] private float _massBeta;
        
        private TerraViewModel _vm;
        private void Start()
        {
            _vm = Game.Instance.GetViewModel<TerraViewModel>(0);
        }
        private void Update()
        {
            TerraVector vector = _vm.Chunk.WorldToLocal(transform.position);
            if (_vm.TerraAlterVerseViewModel[vector])
            {
                _rb.mass = _massBeta;
            }
            else
            {
                _rb.mass = _massAlpha;
            }
        }
    }
}