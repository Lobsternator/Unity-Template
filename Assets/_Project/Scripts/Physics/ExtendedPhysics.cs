using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Physics
{
    [RequireComponent(typeof(Collider), typeof(ContactChecker))]
    public class ExtendedPhysics : MonoBehaviour
    {
        public ExtendedPhysicMaterial _physicMaterial;
        public ExtendedPhysicMaterial PhysicMaterial
        {
            get => _physicMaterial;
            set
            {
                _physicMaterial = value;
                UpdateAffectedColliders();
            }
        }

        public bool affectTriggers                                 = false;
        [SerializeField] private List<Collider> _affectedColliders = new List<Collider>();
        public ReadOnlyCollection<Collider> AffectedColliders => _affectedColliders.AsReadOnly();

        private ContactChecker _contactChecker;

        public void UpdateAffectedColliders()
        {
            for (int i = 0; i < _affectedColliders.Count; i++)
            {
                Collider collider = _affectedColliders[i];
                if (!collider)
                    continue;

                collider.sharedMaterial = _physicMaterial ? _physicMaterial.baseMaterial : null;
            }
        }

        private void Awake()
        {
            _contactChecker = GetComponent<ContactChecker>();
            UpdateAffectedColliders();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _affectedColliders.RemoveAll((c) => c && c.gameObject != gameObject);
            UpdateAffectedColliders();
        }
#endif

        private void FixedUpdate()
        {
            HashSet<Rigidbody> affectedBodies = new HashSet<Rigidbody>();
            for (int i = 0; i < _contactChecker.TouchingColliders.Count; i++)
            {
                Collider collider = _contactChecker.TouchingColliders[i];
                if (collider.isTrigger && !affectTriggers || collider.attachedRigidbody == null || affectedBodies.Contains(collider.attachedRigidbody))
                    continue;

                collider.attachedRigidbody.velocity        *= Mathf.Pow(1.0f - PhysicMaterial.LinearDrag, Time.fixedDeltaTime);
                collider.attachedRigidbody.angularVelocity *= Mathf.Pow(1.0f - PhysicMaterial.AngularDrag, Time.fixedDeltaTime);

                affectedBodies.Add(collider.attachedRigidbody);
            }
        }
    }
}
