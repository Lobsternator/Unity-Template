using UnityEngine;

namespace Template.Gameplay
{
    public static class DamageableObjectExtensions
    {
        public static bool IsAtMinDamage(this DamagePool damagePool)
        {
            return damagePool.DamageManager.IsAtMinDamage;
        }
        public static bool IsAtMaxDamage(this DamagePool damagePool)
        {
            return damagePool.DamageManager.IsAtMaxDamage;
        }
        public static float GetDamage(this DamagePool damagePool)
        {
            return damagePool.DamageManager.Damage;
        }
        public static float GetMinDamage(this DamagePool damagePool)
        {
            return damagePool.DamageManager.MinDamage;
        }
        public static float GetMaxDamage(this DamagePool damagePool)
        {
            return damagePool.DamageManager.MaxDamage;
        }

        public static void SetDamage(this DamagePool damagePool, float damage, DamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            damagePool.DamageManager.SetDamage(damage, damageType, eventInstigator, damageCauser);
        }
        public static void ApplyDamage(this DamagePool damagePool, float baseDamage, DamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            damagePool.DamageManager.ApplyDamage(baseDamage, damageType, eventInstigator, damageCauser);
        }

        public static void SetMinDamage(this DamagePool damagePool, float minDamage, MinDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            damagePool.DamageManager.SetMinDamage(minDamage, damageType, eventInstigator, damageCauser);
        }
        public static void ApplyMinDamage(this DamagePool damagePool, float baseMinDamage, MinDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            damagePool.DamageManager.ApplyMinDamage(baseMinDamage, damageType, eventInstigator, damageCauser);
        }

        public static void SetMaxDamage(this DamagePool damagePool, float maxDamage, MaxDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            damagePool.DamageManager.SetMaxDamage(maxDamage, damageType, eventInstigator, damageCauser);
        }
        public static void ApplyMaxDamage(this DamagePool damagePool, float baseMaxDamage, MaxDamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            damagePool.DamageManager.ApplyMaxDamage(baseMaxDamage, damageType, eventInstigator, damageCauser);
        }
    }
}
