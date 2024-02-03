#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Template.Physics
{
    /// <summary>
    /// <see cref="Editor"/> for <see cref="PhysicMaterial"/>.
    /// </summary>
    [CustomEditor(typeof(PhysicMaterial))]
    public class PhysicsMaterialEditor : Editor
    {
        private ExtendedPhysicsMaterial _extendedPhysicsMaterial;

        private void OnEnable()
        {
            _extendedPhysicsMaterial = ExtendedPhysicsMaterialReferenceChecker.GetExtendedMaterialFromBase(target as PhysicMaterial);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_extendedPhysicsMaterial)
                EditorGUILayout.HelpBox($"Is being used as a base material by {AssetDatabase.GetAssetPath(_extendedPhysicsMaterial)}.", MessageType.Info);
        }
    }
    /// <summary>
    /// <see cref="Editor"/> for <see cref="PhysicsMaterial2D"/>.
    /// </summary>
    [CustomEditor(typeof(PhysicsMaterial2D))]
    public class PhysicsMaterial2DEditor : Editor
    {
        private ExtendedPhysicsMaterial2D _extendedPhysicsMaterial;

        private void OnEnable()
        {
            _extendedPhysicsMaterial = ExtendedPhysicsMaterialReferenceChecker2D.GetExtendedMaterialFromBase(target as PhysicsMaterial2D);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_extendedPhysicsMaterial)
                EditorGUILayout.HelpBox($"Is being used as a base material by {AssetDatabase.GetAssetPath(_extendedPhysicsMaterial)}.", MessageType.Info);
        }
    }
}
#endif
