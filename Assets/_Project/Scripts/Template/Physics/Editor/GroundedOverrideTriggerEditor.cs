#if UNITY_EDITOR
using UnityEditor;

namespace Template.Physics
{
    /// <summary>
    /// <see cref="Editor"/> for <see cref="GroundedOverrideProximityTrigger"/>.
    /// </summary>
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
    /// <summary>
    /// <see cref="Editor"/> for <see cref="GroundedOverrideProximityTrigger2D"/>.
    /// </summary>
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

    /// <summary>
    /// <see cref="Editor"/> for <see cref="GroundedOverrideTargetedTrigger"/>.
    /// </summary>
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
    /// <summary>
    /// <see cref="Editor"/> for <see cref="GroundedOverrideTargetedTrigger2D"/>.
    /// </summary>
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
