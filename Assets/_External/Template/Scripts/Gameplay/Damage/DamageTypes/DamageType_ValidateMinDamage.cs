using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Builtin damage type for when the min damage changes due to it being validated.
    /// </summary>
    public struct DamageType_ValidateMinDamage : IDamageType
    {
        public float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }
}
