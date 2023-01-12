using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Template.Core;

namespace Template.Physics
{
    [RequireComponent(typeof(Collider), typeof(ContactChecker))]
    public class ExtendedPhysics : MonoBehaviour
    {
        private ContactChecker _oldContactChecker;
        [SerializeField] private ContactChecker _contactChecker;
        [SerializeField] public ContactChecker ContactChecker
        {
            get => _contactChecker;
            set
            {
                if (value && value.gameObject != gameObject)
                {
                    Debug.LogWarning($"Cannot assign {typeof(ContactChecker).Name} from a different object!");
                    return;
                }

                _contactChecker = value;
            }
        }

        [SerializeField] private ExtendedPhysicsMaterial _physicMaterial;
        public ExtendedPhysicsMaterial PhysicMaterial
        {
            get => _physicMaterial;
            set
            {
                _physicMaterial = value;
                UpdateAffectedColliders();
            }
        }

        public bool ignoreTriggerOverlaps = true;

        [SerializeField] private List<Collider> _affectedColliders = new List<Collider>();
        public ReadOnlyCollection<Collider> AffectedColliders => _affectedColliders.AsReadOnly();

        public void UpdateAffectedColliders()
        {
            for (int i = 0; i < _affectedColliders.Count; i++)
            {
                Collider collider = _affectedColliders[i];
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
            ContactChecker = GetComponent<ContactChecker>();
        }

        private void OnValidate()
        {
            if (ContactChecker && ContactChecker.gameObject != gameObject)
            {
                Debug.LogWarning($"Cannot assign {typeof(ContactChecker).Name} from a different object!", gameObject);
                ContactChecker = _oldContactChecker;
            }

            _oldContactChecker = _contactChecker;

            _affectedColliders.RemoveAll((c) => !c || c.gameObject != gameObject);
            _affectedColliders = _affectedColliders.Distinct().ToList();
            UpdateAffectedColliders();
        }
#endif

        private void OnCollisionEnter(Collision collision)
        {
            Rigidbody otherRigidbody = collision.rigidbody;
            if (!otherRigidbody)
                return;

            ExtendedPhysics otherExtendedPhysics = otherRigidbody.GetComponent<ExtendedPhysics>();
            ContactPoint[] contacts              = new ContactPoint[collision.contactCount];
            collision.GetContacts(contacts);

            for (int i = 0; i < contacts.Length; i++)
            {
                ref ContactPoint contact             = ref contacts[i];
                Collider ourCollider                 = contact.thisCollider;
                Collider otherCollider               = contact.otherCollider;

                PhysicMaterialCombine bounceCombine = otherCollider.sharedMaterial ? otherCollider.sharedMaterial.bounceCombine : PhysicMaterialCombine.Average;
                float ourBaseBounciness             = ourCollider.sharedMaterial ? ourCollider.sharedMaterial.bounciness : 0.0f;
                float ourExtendedBounciness         = AffectedColliders.Contains(ourCollider) ? PhysicMaterial ? PhysicMaterial.Bounciness : ourBaseBounciness : ourBaseBounciness;
                float otherBaseBounciness           = otherCollider.sharedMaterial ? otherCollider.sharedMaterial.bounciness : 0.0f;
                float otherExtendedBounciness       = otherExtendedPhysics ? otherExtendedPhysics.AffectedColliders.Contains(otherCollider) ? otherExtendedPhysics.PhysicMaterial ? otherExtendedPhysics.PhysicMaterial.Bounciness : otherBaseBounciness : otherBaseBounciness : otherBaseBounciness;

                float baseFinalBounciness;
                float extendedFinalBounciness;

                if (bounceCombine == PhysicMaterialCombine.Average)
                {
                    baseFinalBounciness     = (ourBaseBounciness + otherBaseBounciness) * 0.5f;
                    extendedFinalBounciness = (ourExtendedBounciness + otherExtendedBounciness) * 0.5f;
                }
                else if (bounceCombine == PhysicMaterialCombine.Maximum)
                {
                    baseFinalBounciness     = Mathf.Max(ourBaseBounciness, otherBaseBounciness);
                    extendedFinalBounciness = Mathf.Max(ourExtendedBounciness, otherExtendedBounciness);
                }
                else if (bounceCombine == PhysicMaterialCombine.Minimum)
                {
                    baseFinalBounciness     = Mathf.Min(ourBaseBounciness, otherBaseBounciness);
                    extendedFinalBounciness = Mathf.Min(ourExtendedBounciness, otherExtendedBounciness);
                }
                else if (bounceCombine == PhysicMaterialCombine.Multiply)
                {
                    baseFinalBounciness     = ourBaseBounciness * otherBaseBounciness;
                    extendedFinalBounciness = ourExtendedBounciness * otherExtendedBounciness;
                }
                else
                    throw new System.NotImplementedException();

                float residualBounciness = baseFinalBounciness - extendedFinalBounciness;
                Vector3 relativeVelocity = otherRigidbody.GetRelativePointVelocity(contact.point);

                otherRigidbody.AddForceAtPosition(contact.normal * (relativeVelocity.magnitude * residualBounciness * 2.0f), contact.point, ForceMode.VelocityChange);
            }
        }

        private void FixedUpdate()
        {
            if (!PhysicMaterial)
                return;

            HashSet<Rigidbody> affectedBodies = new HashSet<Rigidbody>();
            for (int i = 0; i < ContactChecker.TouchingColliders.Count; i++)
            {
                Collider collider   = ContactChecker.TouchingColliders[i];
                Rigidbody rigidbody = collider.attachedRigidbody;

                if (collider.isTrigger && ignoreTriggerOverlaps || rigidbody == null || affectedBodies.Contains(rigidbody))
                    continue;

                rigidbody.velocity        *= Mathf.Pow(1.0f / (PhysicMaterial.LinearDrag + 1.0f), Time.fixedDeltaTime);
                rigidbody.angularVelocity *= Mathf.Pow(1.0f / (PhysicMaterial.AngularDrag + 1.0f), Time.fixedDeltaTime);

                affectedBodies.Add(rigidbody);
            }
        }
    }
}
