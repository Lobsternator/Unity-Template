using System;
using UnityEngine;

namespace Template.Gameplay
{
    public interface IDamageType
    {
        public float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser);
    }

    [Serializable]
    public abstract class DamageType : ScriptableObject, IDamageType
    {
        public abstract float GetModifiedDamage(float baseDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser);
    }

    [Serializable]
    public abstract class MinDamageType : ScriptableObject, IDamageType
    {
        public abstract float GetModifiedDamage(float baseMinDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser);
    }

    [Serializable]
    public abstract class MaxDamageType : ScriptableObject, IDamageType
    {
        public abstract float GetModifiedDamage(float baseMaxDamage, DamagePool damagePool, MonoBehaviour eventInstigator, MonoBehaviour damageCauser);
    }
}
