#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Template.Core
{
    [Serializable]
    public class TypeRestrictedObjectReference<TRestrict>
    {
        public UnityEngine.Object value;
    }

    [CustomPropertyDrawer(typeof(TypeRestrictedObjectReference<>), true)]
    public class TypeRestrictedObjectReferenceDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 1;
        }

        private void HandleDragAndDropLogic(Rect position, Type restrictedType)
        {
            Event e = Event.current;
            if (DragAndDrop.objectReferences.Length == 0 || !position.Contains(e.mousePosition))
                return;

            var draggedObject = DragAndDrop.objectReferences[0];
            if (draggedObject is GameObject)
            {
                GameObject gameObject = (GameObject)draggedObject;

                if (!gameObject.GetComponent(restrictedType))
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                else
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;
            }
            else if (draggedObject is Component)
            {
                Component component = (Component)draggedObject;
                Type componentType  = component.GetType();

                if (!(componentType.IsSubclassOf(restrictedType) || componentType.GetInterfaces().Contains(restrictedType)))
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
            }
            else
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative("value");
            Type valueType                   = ExtendedEditorUtility.GetSerializedPropertyType(property);
            Type restrictedType              = valueType.GenericTypeArguments[0];

            HandleDragAndDropLogic(position, restrictedType);

            valueProperty.objectReferenceValue = EditorGUI.ObjectField(position, label, valueProperty.objectReferenceValue, restrictedType, true);

            // If a value was set through other means (e.g ObjectPicker)
            if (valueProperty.objectReferenceValue != null)
            {
                Component component = valueProperty.objectReferenceValue as Component;
                Type componentType  = component.GetType();

                if (component && !(componentType.IsSubclassOf(restrictedType) || componentType.GetInterfaces().Contains(restrictedType)))
                    valueProperty.objectReferenceValue = null;
            }
        }
    }
}
#endif
