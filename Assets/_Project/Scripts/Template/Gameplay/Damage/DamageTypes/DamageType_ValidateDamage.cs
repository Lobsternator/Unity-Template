using Template.Core;
using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Builtin damage type for when the damage changes due to it being validated.
    /// </summary>
    [SingletonAsset]
    [CreateAssetMenu(fileName = "DamageType_ValidateDamage", menuName = "Gameplay/Damage/DamageTypes/Builtin/ValidateDamage")]
    public sealed class DamageType_ValidateDamage : DamageType
    {
        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }
}
