using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Template.Core
{
    /// <summary>
    /// Various utilities related to casting.
    /// </summary>
    public static class CastUtility
    {
        private static readonly Dictionary<Tuple<Type, Type>, Func<object, object>> _castCache = new Dictionary<Tuple<Type, Type>, Func<object, object>>();

        private static Func<object, object> CompileCastDelegate(Type from, Type to)
        {
            var p = Expression.Parameter(typeof(object));

            return Expression.Lambda<Func<object, object>>(
                Expression.Convert(Expression.ConvertChecked(Expression.Convert(p, from), to), typeof(object)), p)
                .Compile();
        }
        public static Func<object, object> GetOrCompileCastDelegate(Type from, Type to)
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

        public static object Cast(object obj, Type t)
        {
            return GetOrCompileCastDelegate(obj.GetType(), t).Invoke(obj);
        }

        public static void ClearCastCache()
        {
            _castCache.Clear();
        }
    }
}
