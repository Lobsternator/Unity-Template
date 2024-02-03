using Template.Core;
using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Builtin damage type for when the max damage changes due to it being validated.
    /// </summary>
    [SingletonAsset]
    [CreateAssetMenu(fileName = "ValidateMaxDamage", menuName = "Gameplay/Damage/DamageTypes/Builtin/ValidateMaxDamage")]
    public sealed class ValidateMaxDamage_DamageType : DamageType
    {
        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }
}
