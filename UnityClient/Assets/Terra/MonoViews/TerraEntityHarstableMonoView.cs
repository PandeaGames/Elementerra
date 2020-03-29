using PandeaGames;
using Terra.MonoViews.Utility;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraEntityHarstableMonoView : AbstractTerraMonoComponent
    {
        [SerializeField] private GameObject _showWhenRipe;
        [SerializeField] private GameObject _hideWhenRipe;
        
        private void Update()
        {
            if (Initialized)
            {
                bool isRipe = Entity.IsRipe();
                _showWhenRipe.SetActive(isRipe);
                _hideWhenRipe.SetActive(!isRipe);
            }
        }
    }
}