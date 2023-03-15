using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Gameplay
{
    [BuiltinDamageType]
    [CreateAssetMenu(fileName = "ValidateMinDamage", menuName = "Damage/MinDamageTypes/Builtin/ValidateMinDamage")]
    public sealed class ValidateMinDamage_MinDamageType : MinDamageType
    {
        public override float GetModifiedDamage(float baseMinDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseMinDamage;
        }
    }
}
