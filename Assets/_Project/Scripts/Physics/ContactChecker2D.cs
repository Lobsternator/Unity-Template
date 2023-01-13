using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Physics
{
    [DisallowMultipleComponent]
    public class ContactChecker2D : MonoBehaviour
    {
        private List<ContactInfo2D> _contacts = new List<ContactInfo2D>();
        public ReadOnlyCollection<ContactInfo2D> Contacts => _contacts.AsReadOnly();

        public event Action FrameProcessed;
        public event Action PhysicsFrameProcessed;

        public void ClearDeadContacts()
        {
            for (int i = 0; i < _contacts.Count; i++)
            {
                Collider2D collider = _contacts[i].Collider;

                if (!collider || !collider.enabled || !collider.gameObject.activeInHierarchy)
                    _contacts.RemoveAt(i--);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _contacts.Add(new ContactInfo2D(collision.collider, ContactType.Collision));
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            _contacts.Remove(new ContactInfo2D(collision.collider, ContactType.Collision));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _contacts.Add(new ContactInfo2D(other, ContactType.Trigger));
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            _contacts.Remove(new ContactInfo2D(other, ContactType.Trigger));
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
