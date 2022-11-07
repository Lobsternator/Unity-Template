using UnityEngine;

namespace Template.Gameplay
{
    public static class DamageableObjectExtensions
    {
        public static void SetDamage(this DamageManager damageManager, float damage, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            damageManager.ApplyDamage(damage - damageManager.Damage, null, eventInstigator, damageCauser);
        }
        public static void AddMinDamage(this DamageManager damageManager, float minDamage, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            damageManager.SetMinDamage(damageManager.MinDamage + minDamage, eventInstigator, damageCauser);
        }
        public static void AddMaxDamage(this DamageManager damageManager, float maxDamage, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            damageManager.SetMaxDamage(damageManager.MaxDamage + maxDamage, eventInstigator, damageCauser);
        }

        public static bool IsDestroyed(this IDamageableObject damageableObject)
        {
            return damageableObject.DamageManager.IsDestroyed;
        }

        public static void ApplyDamage(this IDamageableObject damageableObject, float baseDamage, DamageType damageType, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            damageableObject.DamageManager.ApplyDamage(baseDamage, damageType, eventInstigator, damageCauser);
        }
        public static void SetDamage(this IDamageableObject damageableObject, float damage, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            damageableObject.DamageManager.SetDamage(damage, eventInstigator, damageCauser);
        }

        public static void SetMinDamage(this IDamageableObject damageableObject, float minDamage, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            damageableObject.DamageManager.SetMinDamage(minDamage, eventInstigator, damageCauser);
        }
        public static void AddMinDamage(this IDamageableObject damageableObject, float minDamage, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            damageableObject.DamageManager.AddMinDamage(minDamage, eventInstigator, damageCauser);
        }

        public static void SetMaxDamage(this IDamageableObject damageableObject, float maxDamage, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            damageableObject.DamageManager.SetMaxDamage(maxDamage, eventInstigator, damageCauser);
        }
        public static void AddMaxDamage(this IDamageableObject damageableObject, float maxDamage, MonoBehaviour eventInstigator, MonoBehaviour damageCauser)
        {
            damageableObject.DamageManager.AddMaxDamage(maxDamage, eventInstigator, damageCauser);
        }
    }
}
