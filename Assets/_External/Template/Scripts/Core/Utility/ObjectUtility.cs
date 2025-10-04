using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Various utilities related to finding objects.
    /// </summary>
    public static class ObjectUtility
    {
        public static T FindFirstObjectWithInterface<T>(FindObjectsInactive findObjectsInactive) where T : class
        {
            var objects = Object.FindObjectsByType<Object>(findObjectsInactive, FindObjectsSortMode.InstanceID);
            foreach (Object obj in objects)
            {
                if (obj.GetType().HasInterface<T>())
                    return obj as T;
            }

            return null;
        }
        public static T FindFirstObjectWithInterface<T>() where T : class
        {
            return FindFirstObjectWithInterface<T>(FindObjectsInactive.Exclude);
        }

        public static T FindAnyObjectWithInterface<T>(FindObjectsInactive findObjectsInactive) where T : class
        {
            var objects = Object.FindObjectsByType<Object>(findObjectsInactive, FindObjectsSortMode.None);
            foreach (Object obj in objects)
            {
                if (obj.GetType().HasInterface<T>())
                    return obj as T;
            }

            return null;
        }
        public static T FindAnyObjectWithInterface<T>() where T : class
        {
            return FindFirstObjectWithInterface<T>(FindObjectsInactive.Exclude);
        }

        public static List<T> FindObjectsWithInterface<T>(FindObjectsInactive findObjectsInactive, FindObjectsSortMode findObjectsSortMode) where T : class
        {
            var objects              = Object.FindObjectsByType<Object>(findObjectsInactive, findObjectsSortMode);
            var objectsWithInterface = new List<T>();
            foreach (Object obj in objects)
            {
                if (obj.GetType().HasInterface<T>())
                    objectsWithInterface.Add(obj as T);
            }

            return objectsWithInterface;
        }
        public static List<T> FindObjectsWithInterface<T>(FindObjectsSortMode findObjectsSortMode) where T : class
        {
            return FindObjectsWithInterface<T>(FindObjectsInactive.Exclude, findObjectsSortMode);
        }
    }
}
