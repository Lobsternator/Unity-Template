using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    [CreateAssetMenu(fileName = "new ExtendedPhysicMaterial2D", menuName = "Physics/ExtendedMaterial2D")]
    public class ExtendedPhysicsMaterial2D : ScriptableObject
    {
        [SerializeField, HideInInspector] private PhysicsMaterial2D _lastBaseMaterial;
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
            if (_lastBaseMaterial && BaseMaterial != _lastBaseMaterial)
                _lastBaseMaterial.hideFlags = HideFlags.None;

            _lastBaseMaterial = BaseMaterial;

            _friction    = Mathf.Max(_friction, 0.0f);
            _bounciness  = Mathf.Max(_bounciness, 0.0f);
            _linearDrag  = Mathf.Max(_linearDrag, 0.0f);
            _angularDrag = Mathf.Max(_angularDrag, 0.0f);

            if (BaseMaterial)
            {
                BaseMaterial.friction   = Mathf.Max(_friction, 0.0f);
                BaseMaterial.bounciness = Mathf.Max(_bounciness, 0.0f);
                BaseMaterial.hideFlags  = HideFlags.NotEditable;
            }
        }
#endif
    }
}
