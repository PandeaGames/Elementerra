using Terra.SerializedData.Entities;
using UnityEngine;

namespace Terra.MonoViews.AI
{
    public class AttackDef
    {
        public readonly int Damage;
        public Vector3 Force;
        public RuntimeTerraEntity AttackingEntity;

        public AttackDef(int damage, Vector3 force, RuntimeTerraEntity attackingEntity)
        {
            Damage = damage;
            Force = force;
            AttackingEntity = attackingEntity;
        }
    }
}