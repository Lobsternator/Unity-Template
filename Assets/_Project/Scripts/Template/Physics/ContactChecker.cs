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
    public class ContactChecker : MonoBehaviour, IContactEventReceiver
    {
        public ContactEventSender CurrentContactEventSender { get; set; }

        private List<ContactInfo> _contacts = new List<ContactInfo>();
        public ReadOnlyCollection<ContactInfo> Contacts => _contacts.AsReadOnly();

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

        public void OnCollisionEnter(Collision collision)
        {
            _contacts.Add(new ContactInfo(collision.collider, ContactType.Collision));
        }
        public void OnCollisionExit(Collision collision)
        {
            _contacts.Remove(new ContactInfo(collision.collider, ContactType.Collision));
        }

        public void OnTriggerEnter(Collider other)
        {
            _contacts.Add(new ContactInfo(other, ContactType.Trigger));
        }
        public void OnTriggerExit(Collider other)
        {
            _contacts.Remove(new ContactInfo(other, ContactType.Trigger));
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
