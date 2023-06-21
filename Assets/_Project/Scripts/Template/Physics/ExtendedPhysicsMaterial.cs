using System;
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
    /// <summary>
    /// Checks what <see cref="PhysicMaterial"/>s are being used by <see cref="ExtendedPhysicsMaterial"/>s as a base material and sets them to ReadOnly.
    /// </summary>
    public class ExtendedPhysicsMaterialReferenceChecker : AssetPostprocessor
    {
        private static HashSet<ExtendedPhysicsMaterial> _registeredMaterials = new HashSet<ExtendedPhysicsMaterial>();

        public static void RegisterExtendedMaterial(ExtendedPhysicsMaterial physicsMaterial)
        {
            _registeredMaterials.Add(physicsMaterial);
        }

        public static void CheckReferences(string[] searchInFolders)
        {
            HashSet<PhysicMaterial> basePhysicsMaterials =
                _registeredMaterials
                .Select((m) => m.BaseMaterial)
                .ToHashSet();

            PhysicMaterial[] physicsMaterials =
                AssetDatabase.FindAssets($"t: {nameof(PhysicMaterial)}", searchInFolders)
                .Select((guid) => AssetDatabase.LoadAssetAtPath<PhysicMaterial>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();

            for (int i = 0; i < physicsMaterials.Length; i++)
            {
                PhysicMaterial physicsMaterial = physicsMaterials[i];

                if (basePhysicsMaterials.Contains(physicsMaterial))
                    physicsMaterial.hideFlags = HideFlags.NotEditable;
                else
                    physicsMaterial.hideFlags = HideFlags.None;
            }
        }

        public static ExtendedPhysicsMaterial GetExtendedMaterialFromBase(PhysicMaterial physicsMaterial)
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
    /// Wrapper around <see cref="PhysicMaterial"/> which stores additional data.
    /// </summary>
    [CreateAssetMenu(fileName = "new ExtendedPhysicMaterial", menuName = "Physics/ExtendedMaterial")]
    public class ExtendedPhysicsMaterial : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] private PhysicMaterial _lastBaseMaterial;
#endif

        [SerializeField] private PhysicMaterial _baseMaterial;
        public PhysicMaterial BaseMaterial => _baseMaterial;

        [SerializeField] private float _dynamicFriction = 0.6f;
        public float DynamicFriction
        {
            get => _dynamicFriction;
            set
            {
                _dynamicFriction = Mathf.Max(value, 0.0f);
                if (_baseMaterial)
                    _baseMaterial.dynamicFriction = _dynamicFriction;
            }
        }

        [SerializeField] private float _staticFriction = 0.6f;
        public float StaticFriction
        {
            get => _staticFriction;
            set
            {
                _staticFriction = Mathf.Max(value, 0.0f);
                if (_baseMaterial)
                    _baseMaterial.staticFriction = _staticFriction;
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
                    _baseMaterial.bounciness = Mathf.Clamp01(value);
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

        [SerializeField] private PhysicMaterialCombine _frictionCombine;
        public PhysicMaterialCombine FrictionCombine
        {
            get => _frictionCombine;
            set
            {
                _frictionCombine = value;
                if (_baseMaterial)
                    _baseMaterial.frictionCombine = value;
            }
        }

        [SerializeField] private PhysicMaterialCombine _bounceCombine;
        public PhysicMaterialCombine BounceCombine
        {
            get => _bounceCombine;
            set
            {
                _bounceCombine = value;
                if (_baseMaterial)
                    _baseMaterial.bounceCombine = value;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            ExtendedPhysicsMaterialReferenceChecker.RegisterExtendedMaterial(this);

            if (_lastBaseMaterial != _baseMaterial)
            {
                ExtendedPhysicsMaterialReferenceChecker.CheckReferences(new string[] { Path.GetDirectoryName(AssetDatabase.GetAssetPath(this)).Replace('\\', '/') });
                _lastBaseMaterial = _baseMaterial;
            }

            _dynamicFriction = Mathf.Max(_dynamicFriction, 0.0f);
            _staticFriction  = Mathf.Max(_staticFriction, 0.0f);
            _bounciness      = Mathf.Max(_bounciness, 0.0f);
            _linearDrag      = Mathf.Max(_linearDrag, 0.0f);
            _angularDrag     = Mathf.Max(_angularDrag, 0.0f);

            if (_baseMaterial)
            {
                _baseMaterial.dynamicFriction = Mathf.Max(_dynamicFriction, 0.0f);
                _baseMaterial.staticFriction  = Mathf.Max(_staticFriction, 0.0f);
                _baseMaterial.bounciness      = Mathf.Clamp01(_bounciness);
                _baseMaterial.frictionCombine = _frictionCombine;
                _baseMaterial.bounceCombine   = _bounceCombine;
            }
            else
                Debug.LogWarning($"{nameof(ExtendedPhysicsMaterial)} is missing a base material!", this);
        }
#endif
    }
}
