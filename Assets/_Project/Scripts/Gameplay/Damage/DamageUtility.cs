using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Template.Gameplay;
using Type = System.Type;

namespace Template
{
    public static class DamageUtility
    {
        private static Dictionary<Type, Object> _cachedDamageTypes = new Dictionary<Type, Object>();

        public static TDamageType GetBuiltinDamageType<TDamageType>() where TDamageType : ScriptableObject, IDamageType
        {
            if (typeof(TDamageType).GetCustomAttribute<BuiltinDamageTypeAttribute>() is null)
                return null;

            if (_cachedDamageTypes.TryGetValue(typeof(TDamageType), out var damageType))
            {
                if (!damageType)
                {
                    damageType = Resources.LoadAll("", typeof(TDamageType)).FirstOrDefault();
                    _cachedDamageTypes[typeof(TDamageType)] = damageType;
                }

                return damageType as TDamageType;
            }
            else
            {
                damageType = Resources.LoadAll("", typeof(TDamageType)).FirstOrDefault();
                _cachedDamageTypes.Add(typeof(TDamageType), damageType);

                return damageType as TDamageType;
            }
        }
    }
}
