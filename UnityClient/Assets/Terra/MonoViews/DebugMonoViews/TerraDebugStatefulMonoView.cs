using System.Collections.Generic;
using UnityEngine;

namespace Terra.MonoViews.DebugMonoViews
{
    public class TerraDebugStatefulMonoView : StateMachine<TerraDebugWindowMonoView.EditorStates>
    {
        [SerializeField]
        private List<MonoBehaviour> _offStateComponents;
        
        [SerializeField]
        private List<MonoBehaviour> _freeFlyStateComponents;

        [SerializeField] 
        private List<MonoBehaviour> _lockedStateComponents;

        protected override void Start()
        {
           //
        }

        protected override void EnterState(TerraDebugWindowMonoView.EditorStates state)
        {
            switch (state)
            {
                case TerraDebugWindowMonoView.EditorStates.Locked:
                {
                    SetEnabled(_freeFlyStateComponents, false);
                    SetEnabled(_offStateComponents, false);
                    SetEnabled(_lockedStateComponents, true);
                    
                    break;
                }
                case TerraDebugWindowMonoView.EditorStates.Off:
                {
                    SetEnabled(_freeFlyStateComponents, false);
                    SetEnabled(_lockedStateComponents, false);
                    SetEnabled(_offStateComponents, true);
                    break;
                }
                case TerraDebugWindowMonoView.EditorStates.FreeFly:
                {
                    SetEnabled(_lockedStateComponents, false);
                    SetEnabled(_offStateComponents, false);
                    SetEnabled(_freeFlyStateComponents, true);
                    break;
                }
            }
        }

        protected override void LeaveState(TerraDebugWindowMonoView.EditorStates state)
        {
            
        }

        private void SetEnabled(List<MonoBehaviour> components, bool enabled)
        {
            foreach (MonoBehaviour monoBehaviour in components)
            {
                monoBehaviour.enabled = enabled;
            }
        }
    }
}