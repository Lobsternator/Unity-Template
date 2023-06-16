using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Gameplay
{
    /// <summary>
    /// Builtin damage type for when the damage changes due to the min damage changing.
    /// </summary>
    [CreateAssetMenu(fileName = "MinDamageChange", menuName = "Gameplay/Damage/DamageTypes/Builtin/MinDamageChange")]
    public sealed class MinDamageChange_DamageType : DamageBoundsChange_DamageType
    {
        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }
}
