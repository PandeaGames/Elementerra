using UnityEngine;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public abstract class AbstractPandeaStateCondition : MonoBehaviour
    {
        public abstract bool Evaluate(float secondsInCurrentState);
    }
}