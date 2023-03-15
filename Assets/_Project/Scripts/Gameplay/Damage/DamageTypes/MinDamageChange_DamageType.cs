using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Gameplay
{
    [BuiltinDamageType]
    [CreateAssetMenu(fileName = "MinDamageChange", menuName = "Damage/DamageTypes/Builtin/MinDamageChange")]
    public sealed class MinDamageChange_DamageType : DamageType
    {
        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }
}
