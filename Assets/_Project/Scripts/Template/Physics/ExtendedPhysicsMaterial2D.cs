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
    /// <summary>
    /// Checks what <see cref="PhysicsMaterial2D"/>s are being used by <see cref="ExtendedPhysicsMaterial2D"/>s as a base material and sets them to ReadOnly.
    /// </summary>
    public class ExtendedPhysicsMaterialReferenceChecker2D : AssetPostprocessor
    {
        private static HashSet<ExtendedPhysicsMaterial2D> _registeredMaterials = new HashSet<ExtendedPhysicsMaterial2D>();

        public static void RegisterExtendedMaterial(ExtendedPhysicsMaterial2D physicsMaterial)
        {
            _registeredMaterials.Add(physicsMaterial);
        }

        public static void CheckReferences(string[] searchInFolders)
        {
            HashSet<PhysicsMaterial2D> basePhysicsMaterials =
                _registeredMaterials
                .Select((m) => m.BaseMaterial)
                .ToHashSet();

            PhysicsMaterial2D[] physicsMaterials =
                AssetDatabase.FindAssets($"t: {nameof(PhysicsMaterial2D)}", searchInFolders)
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

        public static ExtendedPhysicsMaterial2D GetExtendedMaterialFromBase(PhysicsMaterial2D physicsMaterial)
        {
            return _registeredMaterials.FirstOrDefault((m) => m.BaseMaterial == physicsMaterial);
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (deletedAssets.Length > 0)
                _registeredMaterials.RemoveWhere((m) => !m);
        }
    }
#endif

    /// <summary>
    /// Wrapper around <see cref="PhysicsMaterial2D"/> which stores additional data.
    /// </summary>
    [CreateAssetMenu(fileName = "new ExtendedPhysicMaterial2D", menuName = "Physics/ExtendedMaterial2D")]
    public class ExtendedPhysicsMaterial2D : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] private PhysicsMaterial2D _lastBaseMaterial;
#endif

        [SerializeField] private PhysicsMaterial2D _baseMaterial;
        public PhysicsMaterial2D BaseMaterial => _baseMaterial;

        [SerializeField] private float _friction = 0.4f;
        public float Friction
        {
            get => _friction;
            set
            {
                _friction = Mathf.Max(value, 0.0f);
                if (_baseMaterial)
                    _baseMaterial.friction = _friction;
            }
        }

        [SerializeField] private float _bounciness;
        public float Bounciness
        {
            get => _bounciness;
            set
            {
                _bounciness = Mathf.Max(value, 0.0f);
                if (_baseMaterial)
                    _baseMaterial.bounciness = _bounciness;
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
            ExtendedPhysicsMaterialReferenceChecker2D.RegisterExtendedMaterial(this);

            if (_lastBaseMaterial != _baseMaterial)
            {
                ExtendedPhysicsMaterialReferenceChecker2D.CheckReferences(new string[] { Path.GetDirectoryName(AssetDatabase.GetAssetPath(this)).Replace('\\', '/') });
                _lastBaseMaterial = _baseMaterial;
            }

            _friction    = Mathf.Max(_friction, 0.0f);
            _bounciness  = Mathf.Max(_bounciness, 0.0f);
            _linearDrag  = Mathf.Max(_linearDrag, 0.0f);
            _angularDrag = Mathf.Max(_angularDrag, 0.0f);

            if (_baseMaterial)
            {
                _baseMaterial.friction   = Mathf.Max(_friction, 0.0f);
                _baseMaterial.bounciness = Mathf.Max(_bounciness, 0.0f);
            }
            else
                Debug.LogWarning($"{nameof(ExtendedPhysicsMaterial2D)} is missing a base material!", this);
        }
#endif
    }
}
