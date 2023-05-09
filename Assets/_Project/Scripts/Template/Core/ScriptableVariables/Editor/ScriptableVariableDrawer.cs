#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Template.Core
{
    [CustomPropertyDrawer(typeof(ScriptableVariable<>), true)]
    public class ScriptableVariableDrawer : PropertyDrawer
    {
        public enum ConstantSelectionEnum
        {
            Constant,
            Reference
        }

        private void DrawLabel(Rect position, GUIContent label)
        {
            EditorGUI.PrefixLabel(position, label);
        }
        private void DrawPopup(Rect position, SerializedProperty useConstantProperty)
        {
            GUIStyle popupStyle          = new GUIStyle(EditorStyles.popup);
            popupStyle.normal.textColor  = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            popupStyle.hover.textColor   = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            popupStyle.focused.textColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            popupStyle.active.textColor  = new Color(0.0f, 0.0f, 0.0f, 0.0f);

            position.width                          = 20.0f;
            position.x                              = EditorGUIUtility.labelWidth - 4.0f;
            ConstantSelectionEnum constantSelection = useConstantProperty.boolValue ? ConstantSelectionEnum.Constant : ConstantSelectionEnum.Reference;
            constantSelection                       = (ConstantSelectionEnum)EditorGUI.EnumPopup(position, new GUIContent(""), constantSelection, popupStyle);

            useConstantProperty.boolValue = constantSelection == ConstantSelectionEnum.Constant;
        }
        private void DrawValueField(Rect position, SerializedProperty property, SerializedProperty useConstantProperty)
        {
            if (useConstantProperty.boolValue)
                EditorGUI.PropertyField(position, property.FindPropertyRelative("constant"), new GUIContent(" "), true);
            else
                EditorGUI.PropertyField(position, property.FindPropertyRelative("reference"), new GUIContent(" "));
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty useConstantProperty = property.FindPropertyRelative("useConstant");

            if (useConstantProperty.boolValue)
                return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("constant"), label, true);
            else
                return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("reference"), label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty useConstantProperty = property.FindPropertyRelative("useConstant");

            DrawLabel(position, label);
            DrawPopup(position, useConstantProperty);
            DrawValueField(position, property, useConstantProperty);
        }
    }
}
#endif
