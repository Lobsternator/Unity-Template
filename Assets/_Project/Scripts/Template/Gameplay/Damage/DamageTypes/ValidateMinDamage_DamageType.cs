using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Gameplay
{
    [SingletonAsset]
    [CreateAssetMenu(fileName = "ValidateMinDamage", menuName = "Damage/DamageTypes/Builtin/ValidateMinDamage")]
    public sealed class ValidateMinDamage_DamageType : DamageType
    {
        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }
}
