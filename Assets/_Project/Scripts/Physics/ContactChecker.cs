using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Physics
{
    [DisallowMultipleComponent]
    public class ContactChecker : MonoBehaviour
    {
        private List<ContactInfo> _contacts = new List<ContactInfo>();
        public ReadOnlyCollection<ContactInfo> Contacts => _contacts.AsReadOnly();

        public event Action FrameProcessed;
        public event Action PhysicsFrameProcessed;

        public void ClearDeadContacts()
        {
            for (int i = 0; i < _contacts.Count; i++)
            {
                Collider collider = _contacts[i].Collider;

                if (!collider || !collider.enabled || !collider.gameObject.activeInHierarchy)
                    _contacts.RemoveAt(i--);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            _contacts.Add(new ContactInfo(collision.collider, ContactType.Collision));
        }
        private void OnCollisionExit(Collision collision)
        {
            _contacts.Remove(new ContactInfo(collision.collider, ContactType.Collision));
        }

        private void OnTriggerEnter(Collider other)
        {
            _contacts.Add(new ContactInfo(other, ContactType.Trigger));
        }
        private void OnTriggerExit(Collider other)
        {
            _contacts.Remove(new ContactInfo(other, ContactType.Trigger));
        }

        private void Update()
        {
            ClearDeadContacts();

            FrameProcessed?.Invoke();
        }
        private void FixedUpdate()
        {
            ClearDeadContacts();

            PhysicsFrameProcessed?.Invoke();
        }
    }
}
