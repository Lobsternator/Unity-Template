#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Template.Core
{
    [CustomPropertyDrawer(typeof(PersistentRuntimeObjectSettings))]
    public class PersistentRuntimeObjectSettingsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.indentLevel--;
            {
                property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, "Settings", true);
                if (property.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    {
                        var activeProp  = property.FindPropertyRelative("_active");
                        var staticProp  = property.FindPropertyRelative("_static");
                        var enabledProp = property.FindPropertyRelative("_enabled");

                        enabledProp.boolValue = EditorGUILayout.Toggle(enabledProp.displayName, enabledProp.boolValue);
                        activeProp.boolValue  = EditorGUILayout.Toggle(activeProp.displayName,  activeProp.boolValue);
                        staticProp.boolValue  = EditorGUILayout.Toggle(staticProp.displayName,  staticProp.boolValue);

                        EditorGUILayout.Space(5);

                        var overrideDefaultNameProp       = property.FindPropertyRelative("_shouldOverrideDefaultName");
                        overrideDefaultNameProp.boolValue = EditorGUILayout.Toggle(overrideDefaultNameProp.displayName, overrideDefaultNameProp.boolValue);
                        if (overrideDefaultNameProp.boolValue)
                        {
                            var nameOverrideProp         = property.FindPropertyRelative("_nameOverride");
                            nameOverrideProp.stringValue = EditorGUILayout.TextField(nameOverrideProp.displayName, nameOverrideProp.stringValue);
                        }
                    }
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUI.indentLevel++;

            EditorGUILayout.Space(5);
        }
    }
}
#endif
