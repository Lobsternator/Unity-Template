using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Physics
{
    public class ContactChecker2D : MonoBehaviour
    {
        private List<Collider2D> _touchingColliders             = new List<Collider2D>();
        public ReadOnlyCollection<Collider2D> TouchingColliders => _touchingColliders.AsReadOnly();

        public event Action FrameProcessed;
        public event Action PhysicsFrameProcessed;

        public void ClearDeadColliders()
        {
            for (int i = 0; i < _touchingColliders.Count; i++)
            {
                Collider2D collider = _touchingColliders[i];

                if (!collider || !collider.enabled || !collider.gameObject.activeInHierarchy)
                    _touchingColliders.RemoveAt(i--);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _touchingColliders.Add(collision.collider);
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            _touchingColliders.Remove(collision.collider);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.isTrigger)
                return;

            _touchingColliders.Add(other);
        }
        private void OnTriggerExit2D(Collider2D other)
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
