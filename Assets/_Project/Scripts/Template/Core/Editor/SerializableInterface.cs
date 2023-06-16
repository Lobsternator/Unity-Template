using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Base class for <see cref="SerializableInterface{TInterface, TCovering}"/>.
    /// </summary>
    [Serializable]
    public abstract class SerializableInterface : IEquatable<SerializableInterface>
    {
        [SerializeField] protected UnityEngine.Object _value;

        public bool Equals(SerializableInterface other)
        {
            return _value.Equals(other._value);
        }
    }

    /// <summary>
    /// Exposes an interface in the inspector.
    /// Value must have TCovering as a base class.
    /// </summary>
    /// <typeparam name="TInterface">The interface type.</typeparam>
    /// <typeparam name="TCovering">The type to use to serialize the value in the backend.</typeparam>
    [Serializable]
    public class SerializableInterface<TInterface, TCovering> : SerializableInterface, IEquatable<TInterface> where TInterface : class where TCovering : UnityEngine.Object
    {
        public TInterface Value
        {
            get => _value as TInterface;
            set => _value = value as TCovering;
        }

        public SerializableInterface()
        {
            _value = null;
        }
        public SerializableInterface(TInterface value)
        {
            _value = value as TCovering;
        }

        public bool Equals(TInterface other)
        {
            return _value.Equals(other);
        }
    }

    /// <summary>
    /// Exposes an interface in the inspector.
    /// Value must have <see cref="UnityEngine.Object"/> as a base class.
    /// </summary>
    /// <typeparam name="TInterface">The interface type.</typeparam>
    [Serializable]
    public class SerializableInterface<TInterface> : SerializableInterface<TInterface, UnityEngine.Object> where TInterface : class
    {
        public SerializableInterface() : base() { }
        public SerializableInterface(TInterface value) : base(value) { }
    }

#if UNITY_EDITOR
    /// <summary>
    /// <see cref="PropertyDrawer"/> for <see cref="SerializableInterface{TInterface, TCovering}"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializableInterface<,>), true)]
    public class SerializableInterfaceDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 1;
        }

        private void HandleDragAndDropLogic(Rect position, Type interfaceType, Type coveringType)
        {
            Event e = Event.current;
            if (DragAndDrop.objectReferences.Length == 0 || !position.Contains(e.mousePosition))
                return;

            var draggedObject = DragAndDrop.objectReferences[0];
            Type objectType   = draggedObject.GetType();

            if (draggedObject is GameObject)
            {
                GameObject gameObject = (GameObject)draggedObject;

                if (gameObject.GetComponent(interfaceType))
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                else
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
            }
            else if (objectType.IsSubclassOf(coveringType) && objectType.HasInterface(interfaceType))
                DragAndDrop.visualMode = DragAndDropVisualMode.Link;
            else
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative("_value");
            Type valueType                   = ExtendedEditorUtility.GetPropertyType(property);
            Type interfaceType               = valueType.GenericTypeArguments[0];
            Type coveringType                = valueType.GenericTypeArguments.Length > 1 ? valueType.GenericTypeArguments[1] : typeof(UnityEngine.Object);

            HandleDragAndDropLogic(position, interfaceType, coveringType);

            valueProperty.objectReferenceValue = EditorGUI.ObjectField(position, label, valueProperty.objectReferenceValue, coveringType, true);

            if (valueProperty.objectReferenceValue)
            {
                UnityEngine.Object obj = valueProperty.objectReferenceValue;
                Type objectType        = obj.GetType();

                if (obj && !objectType.HasInterface(interfaceType))
                {
                    bool isGameObject = obj is GameObject;
                    if (isGameObject)
                    {
                        GameObject gameObject              = (GameObject)obj;
                        valueProperty.objectReferenceValue = gameObject.GetComponent(interfaceType);
                    }

                    bool isComponent = objectType.IsSubclassOf(typeof(Component));
                    if (isComponent)
                    {
                        Component component                = (Component)obj;
                        valueProperty.objectReferenceValue = component.GetComponent(interfaceType);
                    }

                    if (!valueProperty.objectReferenceValue || !(isComponent || isGameObject))
                    {
                        valueProperty.objectReferenceValue = null;
                        Debug.LogWarning($"Component needs to inherit from {interfaceType.Name}!");
                    }
                }
            }
        }
    }
#endif
}
