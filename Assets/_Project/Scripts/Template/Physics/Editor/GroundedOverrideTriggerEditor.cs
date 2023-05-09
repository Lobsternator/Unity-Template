#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Template.Physics
{
    [CustomEditor(typeof(GroundedOverrideProximityTrigger))]
    public class GroundedOverrideTriggerEditor : Editor
    {
        private SerializedProperty _ignoreTriggerOverlapsProperty;
        private SerializedProperty _forceGroundedStateProperty;

        private void OnEnable()
        {
            _ignoreTriggerOverlapsProperty = serializedObject.FindProperty("_ignoreTriggerOverlaps");
            _forceGroundedStateProperty    = serializedObject.FindProperty("_forceGroundedState");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_ignoreTriggerOverlapsProperty);
            EditorGUILayout.PropertyField(_forceGroundedStateProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
    [CustomEditor(typeof(GroundedOverrideProximityTrigger2D))]
    public class GroundedOverrideTrigger2DEditor : Editor
    {
        private SerializedProperty _ignoreTriggerOverlapsProperty;
        private SerializedProperty _forceGroundedStateProperty;

        private void OnEnable()
        {
            _ignoreTriggerOverlapsProperty = serializedObject.FindProperty("_ignoreTriggerOverlaps");
            _forceGroundedStateProperty    = serializedObject.FindProperty("_forceGroundedState");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_ignoreTriggerOverlapsProperty);
            EditorGUILayout.PropertyField(_forceGroundedStateProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomEditor(typeof(GroundedOverrideTargetedTrigger))]
    public class GroundedOverrideTargetedTriggerEditor : Editor
    {
        private SerializedProperty _tallyCounterProperty;
        private SerializedProperty _ignoreTriggerOverlapsProperty;
        private SerializedProperty _forceGroundedStateProperty;

        private void OnEnable()
        {
            _tallyCounterProperty          = serializedObject.FindProperty("_tallyCounter");
            _ignoreTriggerOverlapsProperty = serializedObject.FindProperty("_ignoreTriggerOverlaps");
            _forceGroundedStateProperty    = serializedObject.FindProperty("_forceGroundedState");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_tallyCounterProperty);
            EditorGUILayout.PropertyField(_ignoreTriggerOverlapsProperty);
            EditorGUILayout.PropertyField(_forceGroundedStateProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
    [CustomEditor(typeof(GroundedOverrideTargetedTrigger2D))]
    public class GroundedOverrideTargetedTrigger2DEditor : Editor
    {
        private SerializedProperty _tallyCounterProperty;
        private SerializedProperty _ignoreTriggerOverlapsProperty;
        private SerializedProperty _forceGroundedStateProperty;

        private void OnEnable()
        {
            _tallyCounterProperty          = serializedObject.FindProperty("_tallyCounter");
            _ignoreTriggerOverlapsProperty = serializedObject.FindProperty("_ignoreTriggerOverlaps");
            _forceGroundedStateProperty    = serializedObject.FindProperty("_forceGroundedState");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_tallyCounterProperty);
            EditorGUILayout.PropertyField(_ignoreTriggerOverlapsProperty);
            EditorGUILayout.PropertyField(_forceGroundedStateProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
