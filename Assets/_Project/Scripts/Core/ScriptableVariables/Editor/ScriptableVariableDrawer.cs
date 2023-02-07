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

        private ConstantSelectionEnum _constantSelection;

        private void DrawPopup(Rect position, SerializedProperty property)
        {
            SerializedProperty useConstantProperty = property.FindPropertyRelative("useConstant");

            position.width     = 32.0f;
            position.x         = EditorGUIUtility.labelWidth - 16.0f;
            _constantSelection = useConstantProperty.boolValue ? ConstantSelectionEnum.Constant : ConstantSelectionEnum.Reference;
            _constantSelection = (ConstantSelectionEnum)EditorGUI.EnumPopup(position, new GUIContent(""), _constantSelection);

            useConstantProperty.boolValue = _constantSelection == ConstantSelectionEnum.Constant;
        }
        private void DrawValueField(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_constantSelection == ConstantSelectionEnum.Constant)
                EditorGUI.PropertyField(position, property.FindPropertyRelative("constant"), label);
            else if (_constantSelection == ConstantSelectionEnum.Reference)
                EditorGUI.PropertyField(position, property.FindPropertyRelative("reference"), label);
            else
                throw new NotImplementedException();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawPopup(position, property);
            DrawValueField(position, property, label);
        }
    }
}
#endif
