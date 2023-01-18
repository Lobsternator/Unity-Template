using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Template.Physics
{
#if UNITY_EDITOR
    public static class ExtendedPhysicsMaterial2DReferenceChecker
    {
        public static void OnValidate(string[] searchInFolders)
        {
            HashSet<PhysicsMaterial2D> basePhysicsMaterials =
                AssetDatabase.FindAssets($"t: {typeof(ExtendedPhysicsMaterial2D).Name}", searchInFolders)
                .Select((guid) => AssetDatabase.LoadAssetAtPath<ExtendedPhysicsMaterial2D>(AssetDatabase.GUIDToAssetPath(guid)).BaseMaterial)
                .ToHashSet();

            PhysicsMaterial2D[] physicsMaterials =
                AssetDatabase.FindAssets($"t: {typeof(PhysicsMaterial2D).Name}", searchInFolders)
                .Select((guid) => AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();

            for (int i = 0; i < physicsMaterials.Length; i++)
            {
                PhysicsMaterial2D physicsMaterial = physicsMaterials[i];

                if (basePhysicsMaterials.Contains(physicsMaterial))
                    physicsMaterial.hideFlags = HideFlags.NotEditable;
                else
                    physicsMaterial.hideFlags = HideFlags.None;
            }
        }
    }
#endif

    [CreateAssetMenu(fileName = "new ExtendedPhysicMaterial2D", menuName = "Physics/ExtendedMaterial2D")]
    public class ExtendedPhysicsMaterial2D : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField, HideInInspector] private PhysicsMaterial2D _lastBaseMaterial;
#endif

        [field: SerializeField] public PhysicsMaterial2D BaseMaterial { get; private set; }

        [SerializeField] private float _friction = 0.4f;
        public float Friction
        {
            get => _friction;
            set
            {
                _friction = Mathf.Max(value, 0.0f);
                if (BaseMaterial)
                    BaseMaterial.friction = _friction;
            }
        }

        [SerializeField] private float _bounciness;
        public float Bounciness
        {
            get => _bounciness;
            set
            {
                _bounciness = Mathf.Max(value, 0.0f);
                if (BaseMaterial)
                    BaseMaterial.bounciness = _bounciness;
            }
        }

        [SerializeField] private float _linearDrag;
        public float LinearDrag
        {
            get => _linearDrag;
            set => _linearDrag = Mathf.Max(value, 0.0f);
        }

        [SerializeField] private float _angularDrag;
        public float AngularDrag
        {
            get => _angularDrag;
            set => _angularDrag = Mathf.Max(value, 0.0f);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_lastBaseMaterial != BaseMaterial)
            {
                ExtendedPhysicsMaterial2DReferenceChecker.OnValidate(new string[] { Path.GetDirectoryName(AssetDatabase.GetAssetPath(this)).Replace('\\', '/') });
                _lastBaseMaterial = BaseMaterial;
                EditorUtility.SetDirty(this);
            }

            _friction    = Mathf.Max(_friction, 0.0f);
            _bounciness  = Mathf.Max(_bounciness, 0.0f);
            _linearDrag  = Mathf.Max(_linearDrag, 0.0f);
            _angularDrag = Mathf.Max(_angularDrag, 0.0f);

            if (BaseMaterial)
            {
                BaseMaterial.friction   = Mathf.Max(_friction, 0.0f);
                BaseMaterial.bounciness = Mathf.Max(_bounciness, 0.0f);
            }
        }
#endif
    }
}
