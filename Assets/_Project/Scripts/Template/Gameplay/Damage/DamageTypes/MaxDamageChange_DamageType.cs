using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Gameplay
{
    /// <summary>
    /// Builtin damage type for when the damage changes due to the max damage changing.
    /// </summary>
    [CreateAssetMenu(fileName = "MaxDamageChange", menuName = "Gameplay/Damage/DamageTypes/Builtin/MaxDamageChange")]
    public sealed class MaxDamageChange_DamageType : DamageBoundsChange_DamageType
    {
        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }
}
