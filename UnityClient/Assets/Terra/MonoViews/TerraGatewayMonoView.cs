using System;
using PandeaGames;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraGatewayMonoView : MonoBehaviour
    {
        public void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Game.Instance.GetViewModel<TerraWorldStateViewModel>(0).IsWorldFipped =
                    !Game.Instance.GetViewModel<TerraWorldStateViewModel>(0).IsWorldFipped;
            }
        }
    }
}