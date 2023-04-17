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
    }
}
