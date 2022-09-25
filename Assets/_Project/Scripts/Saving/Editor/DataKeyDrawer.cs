#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Guid = System.Guid;

namespace Template.Saving
{
    [CustomPropertyDrawer(typeof(DataKey))]
    public class DataKeyDrawer : PropertyDrawer
    {
        private void DrawValueField(ref Rect position, SerializedProperty valueProperty)
        {
            EditorGUI.PropertyField(position, valueProperty);
        }

        private void DrawRegenerateButton(ref Rect position, SerializedProperty valueProperty)
        {
            position.width -= 16;
            position.x     += 16;
            position.y     += EditorGUIUtility.singleLineHeight * 1.35f;

            if (GUI.Button(position, "Regenerate Data Key"))
                valueProperty.stringValue = Guid.NewGuid().ToString();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
                return EditorGUIUtility.singleLineHeight * 3.75f;

            else
                return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.BeginProperty(position, label, property);
            {
                property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(position, property.isExpanded, label);

                if (property.isExpanded)
                {
                    SerializedProperty valueProperty = property.FindPropertyRelative("_value");

                    position.y += EditorGUIUtility.singleLineHeight * 1.15f;

                    EditorGUI.indentLevel += 1;
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        DrawValueField(ref position, valueProperty);
                        EditorGUI.EndDisabledGroup();

                        DrawRegenerateButton(ref position, valueProperty);
                    }
                    EditorGUI.indentLevel -= 1;
                }

                EditorGUI.EndFoldoutHeaderGroup();
            }
            EditorGUI.EndProperty();

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
