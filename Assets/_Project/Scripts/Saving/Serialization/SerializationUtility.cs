using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using Template.Core;
using System.Diagnostics;

namespace Template.Saving.Serialization
{
    public static class SerializationUtility
    {
        public static ReadOnlyCollection<Type> KnownTypes { get; } = new ReadOnlyCollection<Type>(new List<Type>
        {
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4),
            typeof(Quaternion),
            typeof(Keyframe),
            typeof(AnimationCurve),
        });
        public static ReadOnlyCollection<Type> KnownSerializableTypes { get; } = new ReadOnlyCollection<Type>(new List<Type>
        {
            typeof(SerializableVector2),
            typeof(SerializableVector3),
            typeof(SerializableVector4),
            typeof(SerializableQuaternion),
            typeof(SerializableKeyframe),
            typeof(SerializableAnimationCurve),
        });

        private static readonly Dictionary<Type, Type> _knownTypeToKnownSerializableType       = KnownSerializableTypes.ToDictionary((t) => KnownTypes[KnownSerializableTypes.IndexOf(t)]);
        private static readonly Dictionary<Type, Type> _knownSerializableTypeToKnownType       = KnownTypes.ToDictionary(            (t) => KnownSerializableTypes[KnownTypes.IndexOf(t)]);
        private static readonly Dictionary<Tuple<Type, Type>, Func<object, object>> _castCache = new Dictionary<Tuple<Type, Type>, Func<object, object>>();

        private static Func<object, object> CompileCastDelegate(Type from, Type to)
        {
            var p = Expression.Parameter(typeof(object));

            return Expression.Lambda<Func<object, object>>(
                Expression.Convert(Expression.ConvertChecked(Expression.Convert(p, from), to), typeof(object)), p)
                .Compile();
        }
        public static Func<object, object> GetCastDelegate(Type from, Type to)
        {
            lock (_castCache)
            {
                var key = new Tuple<Type, Type>(from, to);
                if (_castCache.TryGetValue(key, out var castDelegate))
                    return castDelegate;

                castDelegate = CompileCastDelegate(from, to);
                _castCache.Add(key, castDelegate);
                return castDelegate;
            }
        }

        public static void CompileAndCacheKnownCastDelegates()
        {
            _castCache.Clear();

            for (int i = 0; i < KnownTypes.Count; i++)
            {
                Type knownType             = KnownTypes[i];
                Type knownSerializableType = KnownSerializableTypes[i];

                GetCastDelegate(knownType, knownSerializableType);
                GetCastDelegate(knownSerializableType, knownType);
            }
        }

        public static object Cast(object obj, Type t)
        {
            return GetCastDelegate(obj.GetType(), t).Invoke(obj);
        }

        public static bool TryGetKnownSerializableType(Type knownType, out Type knownSerializableType)
        {
            return _knownTypeToKnownSerializableType.TryGetValue(knownType, out knownSerializableType);
        }
        public static bool TryGetKnownType(Type knownSerializableType, out Type knownType)
        {
            return _knownSerializableTypeToKnownType.TryGetValue(knownSerializableType, out knownType);
        }

        public static bool TryConvertToKnownSerializableType(object obj, Type currentType, out object result)
        {
            result = obj;

            if (!TryGetKnownSerializableType(currentType, out var knownSerializableType))
                return false;

            result = Cast(obj, knownSerializableType);

            return true;
        }
        public static bool TryConvertToKnownType(object obj, Type currentType, out object result)
        {
            result = obj;

            if (!TryGetKnownType(currentType, out var knownType))
                return false;

            result = Cast(obj, knownType);

            return true;
        }
    }
}
