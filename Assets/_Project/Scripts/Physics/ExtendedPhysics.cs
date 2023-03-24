using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ContactChecker))]
    public class ExtendedPhysics : MonoBehaviour, IContactEventReceiver
    {
        public ContactEventSender CurrentContactEventSender { get; set; }

        [SerializeField] private ExtendedPhysicsMaterial _physicsMaterial;
        public ExtendedPhysicsMaterial PhysicsMaterial
        {
            get => _physicsMaterial;
            set
            {
                _physicsMaterial = value;
                UpdateColliderMaterials();
            }
        }

        public bool ignoreTriggerOverlaps = true;
        
        private ContactChecker _contactChecker;

        public void UpdateColliderMaterials()
        {
            Collider[] colliders = GetComponents<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider collider              = colliders[i];
                PhysicMaterial physicsMaterial = _physicsMaterial ? _physicsMaterial.BaseMaterial : null;

                if (collider.sharedMaterial != physicsMaterial)
                    collider.sharedMaterial = physicsMaterial;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateColliderMaterials();
        }
#endif

        private void Awake()
        {
            _contactChecker = GetComponent<ContactChecker>();
        }

        public void OnCollisionEnter(Collision collision)
        {
            Rigidbody otherRigidbody = collision.rigidbody;
            if (!otherRigidbody)
                return;

            ExtendedPhysics otherExtendedPhysics = otherRigidbody.GetComponent<ExtendedPhysics>();
            ContactPoint[] contacts              = new ContactPoint[collision.contactCount];
            collision.GetContacts(contacts);

            for (int i = 0; i < contacts.Length; i++)
            {
                ref ContactPoint contact = ref contacts[i];
                Collider ourCollider     = contact.thisCollider;
                Collider otherCollider   = contact.otherCollider;

                PhysicMaterialCombine bounceCombine = otherCollider.sharedMaterial ? otherCollider.sharedMaterial.bounceCombine : PhysicMaterialCombine.Average;
                float ourBaseBounciness             = ourCollider.sharedMaterial ? ourCollider.sharedMaterial.bounciness : 0.0f;
                float ourExtendedBounciness         = PhysicsMaterial ? PhysicsMaterial.Bounciness : ourBaseBounciness;
                float otherBaseBounciness           = otherCollider.sharedMaterial ? otherCollider.sharedMaterial.bounciness : 0.0f;
                float otherExtendedBounciness       = otherExtendedPhysics ? otherExtendedPhysics.PhysicsMaterial ? otherExtendedPhysics.PhysicsMaterial.Bounciness : otherBaseBounciness : otherBaseBounciness;

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
                if (Mathf.Approximately(residualBounciness, 0.0f))
                    continue;

                Vector3 relativeVelocity = otherRigidbody.GetRelativePointVelocity(contact.point);
                otherRigidbody.AddForceAtPosition(contact.normal * (relativeVelocity.magnitude * residualBounciness * 2.0f), contact.point, ForceMode.VelocityChange);
            }
        }

        private void FixedUpdate()
        {
            if (!PhysicsMaterial)
                return;

            HashSet<Rigidbody> affectedBodies = new HashSet<Rigidbody>();
            for (int i = 0; i < _contactChecker.Contacts.Count; i++)
            {
                Rigidbody rigidbody     = _contactChecker.Contacts[i].Collider.attachedRigidbody;
                ContactType contactType = _contactChecker.Contacts[i].ContactType;

                if (rigidbody == null || (contactType == ContactType.Trigger && ignoreTriggerOverlaps) || affectedBodies.Contains(rigidbody))
                    continue;

                rigidbody.velocity        *= Mathf.Pow(1.0f / (PhysicsMaterial.LinearDrag  + 1.0f), Time.fixedDeltaTime);
                rigidbody.angularVelocity *= Mathf.Pow(1.0f / (PhysicsMaterial.AngularDrag + 1.0f), Time.fixedDeltaTime);

                affectedBodies.Add(rigidbody);
            }
        }
    }
}
