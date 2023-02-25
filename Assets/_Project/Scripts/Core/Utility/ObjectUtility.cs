using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public static class ObjectUtility
    {
        public static T FindObjectWithInterface<T>(bool includeInactive) where T : class
        {
            var objects = Object.FindObjectsOfType<Object>(includeInactive);
            foreach (Object obj in objects)
            {
                if (obj.GetType().HasInterface(typeof(T)))
                    return obj as T;
            }

            return null;
        }
        public static T FindObjectWithInterface<T>() where T : class
        {
            return FindObjectWithInterface<T>(false);
        }

        public static List<T> FindObjectsWithInterface<T>(bool includeInactive) where T : class
        {
            var objects              = Object.FindObjectsOfType<Object>(includeInactive);
            var objectsWithInterface = new List<T>();
            foreach (Object obj in objects)
            {
                if (obj.GetType().HasInterface(typeof(T)))
                    objectsWithInterface.Add(obj as T);
            }

            return objectsWithInterface;
        }
        public static List<T> FindObjectsWithInterface<T>() where T : class
        {
            return FindObjectsWithInterface<T>(false);
        }
    }
}
