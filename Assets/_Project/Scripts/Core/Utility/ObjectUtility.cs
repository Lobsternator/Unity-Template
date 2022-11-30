using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Convert = System.Convert;

namespace Template.Core
{
    public static class ObjectUtility
    {
        private static T ConvertObjectToType<T>(Object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }

        public static T FindObjectWithInterface<T>(bool includeInactive)
        {
            var objects             = Object.FindObjectsOfType<Object>(includeInactive);
            var objectWithInterface = objects.FirstOrDefault(o =>
                    o.GetType().GetInterfaces().Contains(typeof(T)));

            return ConvertObjectToType<T>(objectWithInterface);
        }
        public static T FindObjectWithInterface<T>()
        {
            return FindObjectWithInterface<T>(false);
        }

        public static T[] FindObjectsWithInterface<T>(bool includeInactive)
        {
            var objects              = Object.FindObjectsOfType<Object>(includeInactive);
            var objectsWithInterface = objects.Where(o =>
                    o.GetType().GetInterfaces().Contains(typeof(T)));

            return objectsWithInterface.Cast<T>().ToArray();
        }
        public static T[] FindObjectsWithInterface<T>()
        {
            return FindObjectsWithInterface<T>(false);
        }
    }
}
