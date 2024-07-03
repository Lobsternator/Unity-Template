using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Builtin damage type for when the damage changes due to the max damage changing.
    /// </summary>
    [CreateAssetMenu(fileName = "DamageType_MaxDamageChange", menuName = "Gameplay/Damage/DamageTypes/Builtin/MaxDamageChange")]
    public sealed class DamageType_MaxDamageChange : DamageType_DamageBoundsChange
    {
        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }
}
