using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Builtin damage type for when the damage changes due to the min damage changing.
    /// </summary>
    [CreateAssetMenu(fileName = "DamageType_MinDamageChange", menuName = "Gameplay/Damage/DamageTypes/Builtin/MinDamageChange")]
    public sealed class DamageType_MinDamageChange : DamageType_DamageBoundsChange
    {
        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }
}
