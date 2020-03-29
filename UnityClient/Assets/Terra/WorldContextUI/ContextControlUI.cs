using PandeaGames;
using UnityEngine;

namespace Terra.WorldContextUI
{
    public class ContextControlUI : MonoBehaviour
    {
        private WorldContextViewModel _contextUIModel;

        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        public float smoothTime = 0.3f;
        float yVelocity = 0.0f;
        
        private void Start()
        {
            _contextUIModel = Game.Instance.GetViewModel<WorldContextViewModel>(0);
        }

        private void Update()
        {
            switch (_contextUIModel.CurrentContext)
            {
                case WorldContextViewModel.Context.None:
                {
                    _canvasGroup.alpha = Mathf.SmoothDamp(_canvasGroup.alpha, 0, ref yVelocity, smoothTime);
                    break;
                }
                case WorldContextViewModel.Context.PickUp:
                {
                    _canvasGroup.alpha = Mathf.SmoothDamp(_canvasGroup.alpha, 1, ref yVelocity, smoothTime);
                    break;
                }
            }
            
            transform.position = Camera.main.WorldToScreenPoint(_contextUIModel.CurrentTransform);
        }
    }
}