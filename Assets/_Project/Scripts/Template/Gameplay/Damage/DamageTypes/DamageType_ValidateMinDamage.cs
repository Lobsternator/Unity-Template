using Template.Core;
using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Builtin damage type for when the min damage changes due to it being validated.
    /// </summary>
    [SingletonAsset]
    [CreateAssetMenu(fileName = "DamageType_ValidateMinDamage", menuName = "Gameplay/Damage/DamageTypes/Builtin/ValidateMinDamage")]
    public sealed class DamageType_ValidateMinDamage : DamageType
    {
        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }
}
