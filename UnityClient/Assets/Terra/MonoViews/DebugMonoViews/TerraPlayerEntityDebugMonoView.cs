using System.Collections.Generic;
using UnityEngine;

namespace Terra.MonoViews.DebugMonoViews
{
    public class TerraPlayerEntityDebugMonoView : MonoBehaviour
    {
        [SerializeField]
        private List<MonoBehaviour> _componentsToDisableInEditMode;

    }
}