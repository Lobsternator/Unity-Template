#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Template.Physics
{
    /// <summary>
    /// <see cref="Editor"/> for <see cref="WorldUpOverride"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(WorldUpOverride))]
    public class WorldUpOverrideEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty transformProperty = property.FindPropertyRelative("transform");

            EditorGUI.PropertyField(position, transformProperty, label);
        }
    }
}
#endif
