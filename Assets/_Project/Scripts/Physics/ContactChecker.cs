using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Physics
{
    public class ContactChecker : MonoBehaviour
    {
        private List<Collider> _touchingColliders             = new List<Collider>();
        public ReadOnlyCollection<Collider> TouchingColliders => _touchingColliders.AsReadOnly();

        public event Action FrameProcessed;
        public event Action PhysicsFrameProcessed;

        public void ClearDeadColliders()
        {
            for (int i = 0; i < _touchingColliders.Count; i++)
            {
                Collider collider = _touchingColliders[i];

                if (!collider || !collider.enabled || !collider.gameObject.activeInHierarchy)
                    _touchingColliders.RemoveAt(i--);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            _touchingColliders.Add(collision.collider);
        }
        private void OnCollisionExit(Collision collision)
        {
            _touchingColliders.Remove(collision.collider);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger)
                return;

            _touchingColliders.Add(other);
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.isTrigger)
                return;

            _touchingColliders.Remove(other);
        }

        private void Update()
        {
            ClearDeadColliders();

            FrameProcessed?.Invoke();
        }
        private void FixedUpdate()
        {
            ClearDeadColliders();

            PhysicsFrameProcessed?.Invoke();
        }
    }
}
