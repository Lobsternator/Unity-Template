using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    [CreateAssetMenu(fileName = "new ExtendedPhysicMaterial", menuName = "Physics/ExtendedMaterial")]
    public class ExtendedPhysicsMaterial : ScriptableObject
    {
        [SerializeField, HideInInspector] private PhysicMaterial _lastBaseMaterial;
        [field: SerializeField] public PhysicMaterial BaseMaterial { get; private set; }

        [SerializeField] private float _dynamicFriction = 0.6f;
        public float DynamicFriction
        {
            get => _dynamicFriction;
            set
            {
                _dynamicFriction = Mathf.Max(value, 0.0f);
                if (BaseMaterial)
                    BaseMaterial.dynamicFriction = _dynamicFriction;
            }
        }

        [SerializeField] private float _staticFriction = 0.6f;
        public float StaticFriction
        {
            get => _staticFriction;
            set
            {
                _staticFriction = Mathf.Max(value, 0.0f);
                if (BaseMaterial)
                    BaseMaterial.staticFriction = _staticFriction;
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
                    BaseMaterial.bounciness = Mathf.Clamp01(value);
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
                if (BaseMaterial)
                    BaseMaterial.frictionCombine = value;
            }
        }

        [SerializeField] private PhysicMaterialCombine _bounceCombine;
        public PhysicMaterialCombine BounceCombine
        {
            get => _bounceCombine;
            set
            {
                _bounceCombine = value;
                if (BaseMaterial)
                    BaseMaterial.bounceCombine = value;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_lastBaseMaterial && BaseMaterial != _lastBaseMaterial)
                _lastBaseMaterial.hideFlags = HideFlags.None;

            _lastBaseMaterial = BaseMaterial;

            _dynamicFriction = Mathf.Max(_dynamicFriction, 0.0f);
            _staticFriction  = Mathf.Max(_staticFriction, 0.0f);
            _bounciness      = Mathf.Max(_bounciness, 0.0f);
            _linearDrag      = Mathf.Max(_linearDrag, 0.0f);
            _angularDrag     = Mathf.Max(_angularDrag, 0.0f);

            if (BaseMaterial)
            {
                BaseMaterial.dynamicFriction = Mathf.Max(_dynamicFriction, 0.0f);
                BaseMaterial.staticFriction  = Mathf.Max(_staticFriction, 0.0f);
                BaseMaterial.bounciness      = Mathf.Clamp01(_bounciness);
                BaseMaterial.frictionCombine = _frictionCombine;
                BaseMaterial.bounceCombine   = _bounceCombine;
                BaseMaterial.hideFlags       = HideFlags.NotEditable;
            }
        }
#endif
    }
}
