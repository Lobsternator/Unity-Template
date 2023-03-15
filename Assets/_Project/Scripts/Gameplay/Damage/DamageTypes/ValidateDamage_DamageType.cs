using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Gameplay
{
    [BuiltinDamageType]
    [CreateAssetMenu(fileName = "ValidateDamage", menuName = "Damage/DamageTypes/Builtin/ValidateDamage")]
    public sealed class ValidateDamage_DamageType : DamageType
    {
        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }
}
