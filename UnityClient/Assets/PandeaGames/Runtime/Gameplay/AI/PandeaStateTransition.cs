using System;

namespace PandeaGames.Runtime.Gameplay.AI
{
    [Serializable]
    public struct PandeaStateTransition
    {
        public AbstractPandeaStateCondition Condition;
        public AbstractPandeaState AbstractPandeaState;
    }
}