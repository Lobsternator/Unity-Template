using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Template.Core;

namespace Template.Physics
{
    /// <summary>
    /// Collects and stores information about active contacts.
    /// </summary>
    [DisallowMultipleComponent]
    public class ContactChecker2D : MonoBehaviour, IContactEventReceiver2D
    {
        public ContactEventSender2D CurrentContactEventSender { get; set; }

        private List<ContactInfo2D> _contacts = new List<ContactInfo2D>();
        public ReadOnlyCollection<ContactInfo2D> Contacts => _contacts.AsReadOnly();

        public event Action FrameProcessed;
        public event Action PhysicsFrameProcessed;

        public void ClearDeadContacts()
        {
            _contacts.RemoveAll((c) => !c.Collider || !c.Collider.enabled || !c.Collider.gameObject.activeInHierarchy);
        }

        private IEnumerator ClearDeadContacts_UpdateRoutine()
        {
            while (true)
            {
                yield return CoroutineUtility.WaitForFrames(1);

                ClearDeadContacts();
                FrameProcessed?.Invoke();
            }
        }
        private IEnumerator ClearDeadContacts_FixedRoutine()
        {
            while (true)
            {
                yield return CoroutineUtility.WaitForFixedFrames(1);

                ClearDeadContacts();
                PhysicsFrameProcessed?.Invoke();
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            _contacts.Add(new ContactInfo2D(collision.collider, ContactType.Collision));
        }
        public void OnCollisionExit2D(Collision2D collision)
        {
            _contacts.Remove(new ContactInfo2D(collision.collider, ContactType.Collision));
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            _contacts.Add(new ContactInfo2D(other, ContactType.Trigger));
        }
        public void OnTriggerExit2D(Collider2D other)
        {
            _contacts.Remove(new ContactInfo2D(other, ContactType.Trigger));
        }

        private void OnEnable()
        {
            StartCoroutine(ClearDeadContacts_UpdateRoutine());
            StartCoroutine(ClearDeadContacts_FixedRoutine());
        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}
