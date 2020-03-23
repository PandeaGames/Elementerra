using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraGrassMonoView : MonoBehaviour
    {
        public float scaler;
        public void SetData(TerraGrassNode dataNode)
        {
            gameObject.SetActive(dataNode.Grass > 0);
            transform.localScale = new Vector3(
                1, dataNode.Grass * scaler, 1
                );
        }
    }
}