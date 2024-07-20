#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;

namespace Template.Core
{
    /// <summary>
    /// Stores property info.
    /// </summary>
    public struct PropertyPathInfo
    {
        public object Object { get; }
        public Type Type { get; }
        public FieldInfo FieldInfo { get; }

        public PropertyPathInfo(object obj, Type type, FieldInfo fieldInfo)
        {
            Object    = obj;
            Type      = type;
            FieldInfo = fieldInfo;
        }
    }

    /// <summary>
    /// Various utilities related to editors.
    /// </summary>
    public static class ExtendedEditorUtility
    {
        private static BindingFlags _fieldBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private static Regex _arrayDataRegex           = new Regex(@"data\[(\d*)\]");

        private static List<string> GetProperPath(string propertyPath)
        {
            List<string> properPath = propertyPath.Split('.').ToList();

            for (int i = 0; i < properPath.Count; i++)
            {
                string pathSegment = properPath[i];
                if (pathSegment.EndsWith(']'))
                    properPath.RemoveAt(--i);
            }

            return properPath;
        }

        public static PropertyPathInfo GetPropertyPathInfo(object obj, string propertyPath)
        {
            Type type               = obj.GetType();
            FieldInfo fieldInfo     = null;
            List<string> properPath = GetProperPath(propertyPath);

            for (int i = 0; i < properPath.Count; i++)
            {
                string pathSegment = properPath[i];
                Match match        = _arrayDataRegex.Match(pathSegment);

                if (match.Success)
                {
                    int index              = Convert.ToInt32(match.Groups[1].Value);
                    IEnumerator enumerator = ((IEnumerable)obj).GetEnumerator();

                    for (int j = 0; j < index + 1; j++)
                        enumerator.MoveNext();

                    obj  = enumerator.Current;
                    type = obj.GetType();
                    continue;
                }
                else
                    fieldInfo = type.GetField(pathSegment, _fieldBindingFlags);

                if (fieldInfo != null)
                {
                    type = fieldInfo.FieldType;
                    obj  = fieldInfo.GetValue(obj);
                }
                else
                    return new PropertyPathInfo(obj, type, fieldInfo);
            }

            return new PropertyPathInfo(obj, type, fieldInfo);
        }
        public static PropertyPathInfo GetPropertyPathInfo(SerializedProperty property) => GetPropertyPathInfo(property.serializedObject.targetObject, property.propertyPath);

        public static Type GetPropertyType(SerializedProperty property)
        {
            return GetPropertyPathInfo(property).Type;
        }
    }
}
#endif
