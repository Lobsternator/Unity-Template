using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Gameplay
{
    public static class DamageExtensions
    {
        public static float GetDamageDifference<TDamageType>(this IDamageEventArgs<TDamageType> damageEventArgs) where TDamageType : IDamageType
        {
            return damageEventArgs.NewDamage - damageEventArgs.OldDamage;
        }
    }
}
