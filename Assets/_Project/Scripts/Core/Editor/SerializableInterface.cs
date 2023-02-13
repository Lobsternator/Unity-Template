using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Template.Core
{
    [Serializable]
    public sealed class SerializableInterface<TInterface> where TInterface : class
    {
        [SerializeField] private MonoBehaviour _value;
        public TInterface Value
        {
            get => _value as TInterface;
            set => _value = value as MonoBehaviour;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SerializableInterface<>))]
    public class SerializableInterfaceDrawer : PropertyDrawer
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
            else if (draggedObject is MonoBehaviour)
            {
                MonoBehaviour behaviour = (MonoBehaviour)draggedObject;
                Type behaviourType      = behaviour.GetType();

                if (!(behaviourType.IsSubclassOf(restrictedType) || behaviourType.GetInterfaces().Contains(restrictedType)))
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
            }
            else
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative("_value");
            Type valueType                   = ExtendedEditorUtility.GetSerializedPropertyType(property);
            Type interfaceType               = valueType.GenericTypeArguments[0];

            HandleDragAndDropLogic(position, interfaceType);

            valueProperty.objectReferenceValue = EditorGUI.ObjectField(position, label, valueProperty.objectReferenceValue, typeof(MonoBehaviour), true);

            // If a value was set through other means (e.g ObjectPicker)
            if (valueProperty.objectReferenceValue)
            {
                MonoBehaviour behaviour = valueProperty.objectReferenceValue as MonoBehaviour;
                Type behaviourType      = behaviour.GetType();

                if (behaviour && !(behaviourType.IsSubclassOf(interfaceType) || behaviourType.GetInterfaces().Contains(interfaceType)))
                {
                    valueProperty.objectReferenceValue = behaviour.GetComponent(interfaceType);

                    if (!valueProperty.objectReferenceValue)
                        Debug.LogWarning($"Component needs to inherit from {interfaceType.Name}!");
                }
            }
        }
    }
#endif
}
