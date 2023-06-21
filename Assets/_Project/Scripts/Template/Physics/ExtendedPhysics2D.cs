using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    /// <summary>
    /// Provides a variety of additional physics capabilities. Allows use of an <see cref="ExtendedPhysicsMaterial2D"/>.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ContactChecker2D))]
    public class ExtendedPhysics2D : MonoBehaviour
    {
        [SerializeField] private ExtendedPhysicsMaterial2D _physicsMaterial;
        public ExtendedPhysicsMaterial2D PhysicsMaterial
        {
            get => _physicsMaterial;
            set
            {
                _physicsMaterial = value;
                UpdateColliderMaterials();
            }
        }

        private ContactChecker2D _contactChecker;

        public void UpdateColliderMaterials()
        {
            Collider2D[] colliders = GetComponents<Collider2D>();
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider2D collider               = colliders[i];
                PhysicsMaterial2D physicsMaterial = _physicsMaterial ? _physicsMaterial.BaseMaterial : null;

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
            _contactChecker = GetComponent<ContactChecker2D>();
        }

        public void FixedUpdate()
        {
            if (!PhysicsMaterial)
                return;

            HashSet<Rigidbody2D> affectedBodies = new HashSet<Rigidbody2D>();
            for (int i = 0; i < _contactChecker.Contacts.Count; i++)
            {
                Rigidbody2D rigidbody   = _contactChecker.Contacts[i].Collider.attachedRigidbody;
                ContactType contactType = _contactChecker.Contacts[i].ContactType;

                if (rigidbody == null || contactType == ContactType.Trigger || affectedBodies.Contains(rigidbody))
                    continue;

                rigidbody.velocity        *= Mathf.Pow(1.0f / (PhysicsMaterial.LinearDrag  + 1.0f), Time.fixedDeltaTime);
                rigidbody.angularVelocity *= Mathf.Pow(1.0f / (PhysicsMaterial.AngularDrag + 1.0f), Time.fixedDeltaTime);

                affectedBodies.Add(rigidbody);
            }
        }
    }
}
