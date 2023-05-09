using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Gameplay
{
    public static class DamageExtensions
    {
        public static float GetDamageDifference(this DamageEventArgs damageEventArgs)
        {
            return damageEventArgs.NewDamage - damageEventArgs.OldDamage;
        }

        public static void SetDamage(this DamagePool damagePool, float damage)
        {
            damagePool.SetDamage(damage, null, null, null);
        }
        public static void ApplyDamage(this DamagePool damagePool, float baseDamage)
        {
            damagePool.ApplyDamage(baseDamage, null, null, null);
        }
        public static void SetMinDamage(this DamagePool damagePool, float minDamage)
        {
            damagePool.SetMinDamage(minDamage, null, null, null);
        }
        public static void ApplyMinDamage(this DamagePool damagePool, float baseMinDamage)
        {
            damagePool.ApplyMinDamage(baseMinDamage, null, null, null);
        }
        public static void SetMaxDamage(this DamagePool damagePool, float maxDamage)
        {
            damagePool.SetMaxDamage(maxDamage, null, null, null);
        }
        public static void ApplyMaxDamage(this DamagePool damagePool, float baseMaxDamage)
        {
            damagePool.ApplyMaxDamage(baseMaxDamage, null, null, null);
        }
    }
}
