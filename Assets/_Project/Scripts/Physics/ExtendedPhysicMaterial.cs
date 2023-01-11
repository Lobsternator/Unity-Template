using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    [CreateAssetMenu(fileName = "new ExtendedPhysicMaterial", menuName = "Physics/ExtendedMaterial")]
    public class ExtendedPhysicMaterial : ScriptableObject
    {
        public PhysicMaterial baseMaterial;

        [SerializeField] private float _linearDrag;
        public float LinearDrag
        {
            get => _linearDrag;
            set => _linearDrag = Mathf.Clamp01(value);
        }

        [SerializeField] private float _angularDrag;
        public float AngularDrag
        {
            get => _angularDrag;
            set => _angularDrag = Mathf.Clamp01(value);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _linearDrag  = Mathf.Clamp01(_linearDrag);
            _angularDrag = Mathf.Clamp01(_angularDrag);
        }
#endif
    }
}
