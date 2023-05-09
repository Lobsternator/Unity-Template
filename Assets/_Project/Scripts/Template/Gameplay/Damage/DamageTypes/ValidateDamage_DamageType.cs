using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Gameplay
{
    [SingletonAsset]
    [CreateAssetMenu(fileName = "ValidateDamage", menuName = "Damage/DamageTypes/Builtin/ValidateDamage")]
    public sealed class ValidateDamage_DamageType : DamageType
    {
        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }
}
