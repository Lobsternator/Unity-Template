using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Template.Core;

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

        private static readonly Dictionary<Type, Type> _knownTypeToKnownSerializableType = KnownSerializableTypes.ToDictionary((t) => KnownTypes[KnownSerializableTypes.IndexOf(t)]);
        private static readonly Dictionary<Type, Type> _knownSerializableTypeToKnownType = KnownTypes.ToDictionary(            (t) => KnownSerializableTypes[KnownTypes.IndexOf(t)]);

        public static void CompileAndCacheKnownCastDelegates()
        {
            for (int i = 0; i < KnownTypes.Count; i++)
            {
                Type knownType             = KnownTypes[i];
                Type knownSerializableType = KnownSerializableTypes[i];

                CastUtility.GetOrCompileCastDelegate(knownType, knownSerializableType);
                CastUtility.GetOrCompileCastDelegate(knownSerializableType, knownType);
            }
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

            result = CastUtility.Cast(obj, knownSerializableType);

            return true;
        }
        public static bool TryConvertToKnownType(object obj, Type currentType, out object result)
        {
            result = obj;

            if (!TryGetKnownType(currentType, out var knownType))
                return false;

            result = CastUtility.Cast(obj, knownType);

            return true;
        }
    }
}
