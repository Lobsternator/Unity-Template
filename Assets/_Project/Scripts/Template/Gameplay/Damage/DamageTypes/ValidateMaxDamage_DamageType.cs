using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Gameplay
{
    [SingletonAsset]
    [CreateAssetMenu(fileName = "ValidateMaxDamage", menuName = "Gameplay/Damage/DamageTypes/Builtin/ValidateMaxDamage")]
    public sealed class ValidateMaxDamage_DamageType : DamageType
    {
        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }
}
