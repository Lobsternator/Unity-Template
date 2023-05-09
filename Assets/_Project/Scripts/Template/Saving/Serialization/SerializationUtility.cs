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
        public static ReadOnlyCollection<Type> KnownUnserializableTypes { get; } = new ReadOnlyCollection<Type>(new List<Type>
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

        private static readonly Dictionary<Type, Type> _knownUnserializableTypeToKnownSerializableType = KnownSerializableTypes.ToDictionary(  (t) => KnownUnserializableTypes[KnownSerializableTypes.IndexOf(t)]);
        private static readonly Dictionary<Type, Type> _knownSerializableTypeToKnownUnserializableType = KnownUnserializableTypes.ToDictionary((t) => KnownSerializableTypes[KnownUnserializableTypes.IndexOf(t)]);

        public static void CompileAndCacheKnownCastDelegates()
        {
            for (int i = 0; i < KnownUnserializableTypes.Count; i++)
            {
                Type knownUnserializableType = KnownUnserializableTypes[i];
                Type knownSerializableType   = KnownSerializableTypes[i];

                CastUtility.GetOrCompileCastDelegate(knownUnserializableType, knownSerializableType);
                CastUtility.GetOrCompileCastDelegate(knownSerializableType, knownUnserializableType);
            }
        }

        public static bool TryGetKnownSerializableType(Type knownUnserializableType, out Type knownSerializableType)
        {
            return _knownUnserializableTypeToKnownSerializableType.TryGetValue(knownUnserializableType, out knownSerializableType);
        }
        public static bool TryGetKnownUnserializableType(Type knownSerializableType, out Type knownUnserializableType)
        {
            return _knownSerializableTypeToKnownUnserializableType.TryGetValue(knownSerializableType, out knownUnserializableType);
        }

        public static bool TryConvertToKnownSerializableType(object obj, Type currentType, out object result)
        {
            result = obj;

            if (!TryGetKnownSerializableType(currentType, out var knownSerializableType))
                return false;

            result = CastUtility.Cast(obj, knownSerializableType);

            return true;
        }
        public static bool TryConvertToKnownUnserializableType(object obj, Type currentType, out object result)
        {
            result = obj;

            if (!TryGetKnownUnserializableType(currentType, out var knownType))
                return false;

            result = CastUtility.Cast(obj, knownType);

            return true;
        }
    }
}
