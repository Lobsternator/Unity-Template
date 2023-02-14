#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Template.Core
{
    public struct SerializedPropertyPathInfo
    {
        public object Object { get; }
        public Type Type { get; }
        public FieldInfo FieldInfo { get; }

        public SerializedPropertyPathInfo(object obj, Type type, FieldInfo fieldInfo)
        {
            Object    = obj;
            Type      = type;
            FieldInfo = fieldInfo;
        }
    }

    public static class ExtendedEditorUtility
    {
        private static BindingFlags _fieldBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private static Regex _arrayDataRegex           = new Regex(@"data\[(\d*)\]");

        private static List<string> GetCleanedSerializedPropertyPath(string path)
        {
            List<string> cleanedPath = path.Split('.').ToList();

            for (int i = 0; i < cleanedPath.Count; i++)
            {
                string pathSegment = cleanedPath[i];
                if (pathSegment.EndsWith(']'))
                    cleanedPath.RemoveAt(--i);
            }

            return cleanedPath;
        }

        public static SerializedPropertyPathInfo GetSerializedPropertyPathInfo(object obj, string propertyPath)
        {
            Type type                = obj.GetType();
            FieldInfo fieldInfo      = null;
            List<string> cleanedPath = GetCleanedSerializedPropertyPath(propertyPath);

            for (int i = 0; i < cleanedPath.Count; i++)
            {
                string pathSegment = cleanedPath[i];
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
                    return new SerializedPropertyPathInfo(obj, type, fieldInfo);
            }

            return new SerializedPropertyPathInfo(obj, type, fieldInfo);
        }
        public static SerializedPropertyPathInfo GetSerializedPropertyPathInfo(SerializedProperty property) => GetSerializedPropertyPathInfo(property.serializedObject.targetObject, property.propertyPath);

        public static Type GetSerializedPropertyType(SerializedProperty property)
        {
            return GetSerializedPropertyPathInfo(property).Type;
        }
    }
}
#endif
