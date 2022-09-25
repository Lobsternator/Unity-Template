using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Template.Core;

namespace Template.Saving.Serialization
{
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -850)]
    public class SerializationManager : PersistentRuntimeSingleton<SerializationManager>
    {
        public readonly List<Type> knownTypes = new List<Type>
        {
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4),
            typeof(Quaternion),
            typeof(Keyframe),
            typeof(AnimationCurve),
        };
        public readonly List<Type> knownSerializableTypes = new List<Type>
        {
            typeof(SerializableVector2),
            typeof(SerializableVector3),
            typeof(SerializableVector4),
            typeof(SerializableQuaternion),
            typeof(SerializableKeyframe),
            typeof(SerializableAnimationCurve),
        };

        private readonly Dictionary<Tuple<Type, Type>, Func<object, object>> _castCache = new Dictionary<Tuple<Type, Type>, Func<object, object>>();

        private Func<object, object> CompileCastDelegate(Type from, Type to)
        {
            var p = Expression.Parameter(typeof(object));

            return Expression.Lambda<Func<object, object>>(
                Expression.Convert(Expression.ConvertChecked(Expression.Convert(p, from), to), typeof(object)), p)
                .Compile();
        }
        public Func<object, object> GetCastDelegate(Type from, Type to)
        {
            lock (_castCache)
            {
                var key = new Tuple<Type, Type>(from, to);
                Func<object, object> castDelegate;

                if (!_castCache.TryGetValue(key, out castDelegate))
                {
                    castDelegate = CompileCastDelegate(from, to);
                    _castCache.Add(key, castDelegate);
                }

                return castDelegate;
            }
        }

        public void CompileAndCacheKnownCastDelegates()
        {
            for (int i = 0; i < knownTypes.Count; i++)
            {
                Type knownType             = knownTypes[i];
                Type knownSerializableType = knownSerializableTypes[i];

                GetCastDelegate(knownType, knownSerializableType);
                GetCastDelegate(knownSerializableType, knownType);
            }
        }

        public object Cast(object obj, Type t)
        {
            return GetCastDelegate(obj.GetType(), t).Invoke(obj);
        }

        public bool TryConvertToKnownSerializableType(object obj, Type currentType, out object result)
        {
            result = obj;

            int index = knownTypes.FindIndex((t) => t == currentType);
            if (index == -1)
                return false;

            Type knownSerializableType = knownSerializableTypes[index];
            result                     = Cast(obj, knownSerializableType);

            return true;
        }
        public bool TryConvertToKnownType(object obj, Type currentType, out object result)
        {
            result = obj;

            int index = knownSerializableTypes.FindIndex((t) => t == currentType);
            if (index == -1)
                return false;

            Type knownType = knownTypes[index];
            result         = Cast(obj, knownType);

            return true;
        }

        public Type GetKnowSerializableType(Type knownType)
        {
            int index = knownTypes.FindIndex((t) => t == knownType);

            return index != -1 ? knownSerializableTypes[index] : null;
        }
        public Type GetKnowType(Type knownSerializableType)
        {
            int index = knownSerializableTypes.FindIndex((t) => t == knownSerializableType);

            return index != -1 ? knownTypes[index] : null;
        }

        protected override void Awake()
        {
            base.Awake();
            if (IsDuplicate)
                return;

            CompileAndCacheKnownCastDelegates();
        }
    }
}
