using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Gameplay
{
    [BuiltinDamageType]
    [CreateAssetMenu(fileName = "MaxDamageChange", menuName = "Damage/DamageTypes/Builtin/MaxDamageChange")]
    public sealed class MaxDamageChange_DamageType : DamageType
    {
        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }
}
