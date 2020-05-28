using System.Collections.Generic;
using UnityEngine;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public abstract class AbstractPandeaStateCondition : MonoBehaviour
    {
#if UNITY_EDITOR
        [HideInInspector]
        public List<string> EvaluationLog = new List<string>(); 
#endif
        public abstract bool Evaluate(float secondsInCurrentState);
    }
}