using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Gameplay
{
    [SingletonAsset]
    [CreateAssetMenu(fileName = "ValidateMaxDamage", menuName = "Damage/MaxDamageTypes/Builtin/ValidateMaxDamage")]
    public sealed class ValidateMaxDamage_MaxDamageType : MaxDamageType
    {
        public override float GetModifiedDamage(float baseMaxDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseMaxDamage;
        }
    }
}
