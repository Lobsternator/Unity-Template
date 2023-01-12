using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Template.Core;

namespace Template.Physics
{
    [RequireComponent(typeof(Collider2D), typeof(ContactChecker2D))]
    public class ExtendedPhysics2D : MonoBehaviour
    {
        private ContactChecker2D _oldContactChecker;
        [SerializeField] private ContactChecker2D _contactChecker;
        [SerializeField] public ContactChecker2D ContactChecker
        {
            get => _contactChecker;
            set
            {
                if (value && value.gameObject != gameObject)
                {
                    Debug.LogWarning($"Cannot assign {typeof(ContactChecker2D).Name} from a different object!");
                    return;
                }

                _contactChecker = value;
            }
        }

        [SerializeField] private ExtendedPhysicsMaterial2D _physicMaterial;
        public ExtendedPhysicsMaterial2D PhysicMaterial
        {
            get => _physicMaterial;
            set
            {
                _physicMaterial = value;
                UpdateAffectedColliders();
            }
        }

        public bool ignoreTriggerOverlaps = true;

        [SerializeField] private List<Collider2D> _affectedColliders = new List<Collider2D>();
        public ReadOnlyCollection<Collider2D> AffectedColliders => _affectedColliders.AsReadOnly();

        public void UpdateAffectedColliders()
        {
            for (int i = 0; i < _affectedColliders.Count; i++)
            {
                Collider2D collider = _affectedColliders[i];
                if (!collider)
                    continue;

                collider.sharedMaterial = _physicMaterial ? _physicMaterial.BaseMaterial : null;
            }
        }

        private void Awake()
        {
            UpdateAffectedColliders();
        }

#if UNITY_EDITOR
        private void Reset()
        {
            ContactChecker = GetComponent<ContactChecker2D>();
        }

        private void OnValidate()
        {
            if (ContactChecker && ContactChecker.gameObject != gameObject)
            {
                Debug.LogWarning($"Cannot assign {typeof(ContactChecker2D).Name} from a different object!", gameObject);
                ContactChecker = _oldContactChecker;
            }

            _oldContactChecker = _contactChecker;

            _affectedColliders.RemoveAll((c) => !c || c.gameObject != gameObject);
            _affectedColliders = _affectedColliders.Distinct().ToList();
            UpdateAffectedColliders();
        }
#endif

        private void FixedUpdate()
        {
            if (!PhysicMaterial)
                return;

            HashSet<Rigidbody2D> affectedBodies = new HashSet<Rigidbody2D>();
            for (int i = 0; i < ContactChecker.TouchingColliders.Count; i++)
            {
                Collider2D collider   = ContactChecker.TouchingColliders[i];
                Rigidbody2D rigidbody = collider.attachedRigidbody;

                if (collider.isTrigger && ignoreTriggerOverlaps || rigidbody == null || affectedBodies.Contains(rigidbody))
                    continue;

                rigidbody.velocity        *= Mathf.Pow(1.0f / (PhysicMaterial.LinearDrag + 1.0f), Time.fixedDeltaTime);
                rigidbody.angularVelocity *= Mathf.Pow(1.0f / (PhysicMaterial.AngularDrag + 1.0f), Time.fixedDeltaTime);

                affectedBodies.Add(rigidbody);
            }
        }
    }
}
