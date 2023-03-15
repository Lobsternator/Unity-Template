using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Gameplay
{
    [BuiltinDamageType]
    [CreateAssetMenu(fileName = "ValidateMaxDamage", menuName = "Damage/MaxDamageTypes/Builtin/ValidateMaxDamage")]
    public sealed class ValidateMaxDamage_MaxDamageType : MaxDamageType
    {
        public override float GetModifiedDamage(float baseMaxDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseMaxDamage;
        }
    }
}
