using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Physics
{
    [RequireComponent(typeof(Rigidbody))]
    public class ContactChecker : MonoBehaviour
    {
        private List<Collider> _touchingColliders             = new List<Collider>();
        public ReadOnlyCollection<Collider> TouchingColliders => _touchingColliders.AsReadOnly();

        public event Action FrameProcessed;

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
            for (int i = 0; i < _touchingColliders.Count; i++)
            {
                Collider collider = _touchingColliders[i];

                if (!collider || !collider.enabled || !collider.gameObject.activeInHierarchy)
                    _touchingColliders.RemoveAt(i--);
            }

            FrameProcessed?.Invoke();
        }
    }
}
