#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Template.Physics
{
    [CustomEditor(typeof(ExtendedPhysicsMaterial))]
    public class ExtendedPhysicsMaterialEditor : Editor
    {
        private SerializedProperty _baseMaterialProperty;
        private SerializedProperty _dynamicFrictionProperty;
        private SerializedProperty _staticFrictionProperty;
        private SerializedProperty _bouncinessProperty;
        private SerializedProperty _linearDragProperty;
        private SerializedProperty _angularDragProperty;
        private SerializedProperty _frictionCombineProperty;
        private SerializedProperty _bounceCombineProperty;

        private void OnEnable()
        {
            _baseMaterialProperty    = serializedObject.FindProperty("_baseMaterial");
            _dynamicFrictionProperty = serializedObject.FindProperty("_dynamicFriction");
            _staticFrictionProperty  = serializedObject.FindProperty("_staticFriction");
            _bouncinessProperty      = serializedObject.FindProperty("_bounciness");
            _linearDragProperty      = serializedObject.FindProperty("_linearDrag");
            _angularDragProperty     = serializedObject.FindProperty("_angularDrag");
            _frictionCombineProperty = serializedObject.FindProperty("_frictionCombine");
            _bounceCombineProperty   = serializedObject.FindProperty("_bounceCombine");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_baseMaterialProperty);
            EditorGUILayout.PropertyField(_dynamicFrictionProperty);
            EditorGUILayout.PropertyField(_staticFrictionProperty);
            EditorGUILayout.PropertyField(_bouncinessProperty);
            EditorGUILayout.PropertyField(_linearDragProperty);
            EditorGUILayout.PropertyField(_angularDragProperty);
            EditorGUILayout.PropertyField(_frictionCombineProperty);
            EditorGUILayout.PropertyField(_bounceCombineProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomEditor(typeof(ExtendedPhysicsMaterial2D))]
    public class ExtendedPhysicsMaterial2DEditor : Editor
    {
        private SerializedProperty _baseMaterialProperty;
        private SerializedProperty _frictionProperty;
        private SerializedProperty _bouncinessProperty;
        private SerializedProperty _linearDragProperty;
        private SerializedProperty _angularDragProperty;

        private void OnEnable()
        {
            _baseMaterialProperty = serializedObject.FindProperty("_baseMaterial");
            _frictionProperty     = serializedObject.FindProperty("_friction");
            _bouncinessProperty   = serializedObject.FindProperty("_bounciness");
            _linearDragProperty   = serializedObject.FindProperty("_linearDrag");
            _angularDragProperty  = serializedObject.FindProperty("_angularDrag");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_baseMaterialProperty);
            EditorGUILayout.PropertyField(_frictionProperty);
            EditorGUILayout.PropertyField(_bouncinessProperty);
            EditorGUILayout.PropertyField(_linearDragProperty);
            EditorGUILayout.PropertyField(_angularDragProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
