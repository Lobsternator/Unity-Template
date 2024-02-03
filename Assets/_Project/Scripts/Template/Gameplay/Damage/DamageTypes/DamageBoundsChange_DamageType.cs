using Template.Core;
using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Builtin damage type for when the damage changes due to the bounds changing.
    /// </summary>
    [SingletonAsset]
    public abstract class DamageBoundsChange_DamageType : DamageType
    {
        public override float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            return baseDamage;
        }
    }
}
