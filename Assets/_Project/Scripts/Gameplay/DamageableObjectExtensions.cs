namespace Template.Gameplay
{
    public static class DamageableObjectExtensions
    {
        public static bool IsDestroyed(this IDamageableObject damageableObject)
        {
            return damageableObject.DamageManager.IsDestroyed;
        }

        public static void SetDamage(this IDamageableObject damageableObject, float damage)
        {
            damageableObject.DamageManager.Damage = damage;
        }
        public static void AddDamage(this IDamageableObject damageableObject, float damage)
        {
            damageableObject.DamageManager.Damage += damage;
        }

        public static void SetMinDamage(this IDamageableObject damageableObject, float minDamage)
        {
            damageableObject.DamageManager.MinDamage = minDamage;
        }
        public static void AddMinDamage(this IDamageableObject damageableObject, float minDamage)
        {
            damageableObject.DamageManager.MinDamage += minDamage;
        }

        public static void SetMaxDamage(this IDamageableObject damageableObject, float maxDamage)
        {
            damageableObject.DamageManager.MaxDamage = maxDamage;
        }
        public static void AddMaxDamage(this IDamageableObject damageableObject, float maxDamage)
        {
            damageableObject.DamageManager.MaxDamage += maxDamage;
        }
    }
}
