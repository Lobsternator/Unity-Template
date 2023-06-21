using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace Template.Core
{
#if UNITY_EDITOR
    /// <summary>
    /// Makes a serialized field ReadOnly in the inspector.
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute
    {

    }
#else
    /// <summary>
    /// Makes a serialized field ReadOnly in the inspector.
    /// </summary>
    public class ReadOnlyAttribute : System.Attribute
    {

    }
#endif

#if UNITY_EDITOR
    /// <summary>
    /// <see cref="PropertyDrawer"/> for <see cref="ReadOnlyAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
            GUI.enabled = true;
        }
    }
#endif
}
