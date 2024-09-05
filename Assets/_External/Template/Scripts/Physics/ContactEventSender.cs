using System;
using System.Collections;
using System.Collections.Generic;
using Template.Core;
using UnityEngine;

namespace Template.Physics
{
    [Flags]
    public enum ContactEventFlags
    {
        OnCollisionEnter    = 0b00000001,
        OnCollisionStay     = 0b00000010,
        OnCollisionExit     = 0b00000100,
        OnParticleCollision = 0b00001000,
        OnTriggerEnter      = 0b00010000,
        OnTriggerStay       = 0b00100000,
        OnTriggerExit       = 0b01000000,
        OnParticleTrigger   = 0b10000000
    }

    /// <summary>
    /// Makes an object able to receive contact events from external sources.
    /// </summary>
    public interface IContactEventReceiver
    {
        /// <summary>
        /// Which sender are we currently receiving contact events from? Null if current contact event originated from this object.
        /// </summary>
        public ContactEventSender CurrentContactEventSender { get; set; }

        public void OnCollisionEnter(Collision collision)
        {

        }
        public void OnCollisionStay(Collision collision)
        {

        }
        public void OnCollisionExit(Collision collision)
        {

        }
        public void OnParticleCollision(GameObject other)
        {

        }
        public void OnTriggerEnter(Collider other)
        {

        }
        public void OnTriggerStay(Collider other)
        {

        }
        public void OnTriggerExit(Collider other)
        {

        }
        public void OnParticleTrigger()
        {

        }
    }

    /// <summary>
    /// Sends contact events to any <see cref="IContactEventReceiver"/>s on specified <see cref="GameObject"/>s.
    /// </summary>
    public class ContactEventSender : MonoBehaviour, IContactEventReceiver
    {
        public ContactEventSender CurrentContactEventSender { get; set; }

        public ContactEventFlags enabledContactEvents;
        public HashSet<GameObject> receivers = new HashSet<GameObject>();

        [SerializeField]
        private List<GameObject> receiversToAddOnAwake = new List<GameObject>();

        private List<IContactEventReceiver> _cachedReceivers = new List<IContactEventReceiver>();

        private void Awake()
        {
            foreach (var receiver in receiversToAddOnAwake)
                receivers.Add(receiver);

            StartCoroutine(ClearDeadReceivers_UpdateRoutine());
        }

        public void ClearDeadReceivers()
        {
            receivers.RemoveWhere((r) => !r);
        }
        private IEnumerator ClearDeadReceivers_UpdateRoutine()
        {
            while (true)
            {
                ClearDeadReceivers();
                yield return CoroutineUtility.WaitForFrames(1);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            if ((enabledContactEvents & ContactEventFlags.OnCollisionEnter) == 0)
                return;

            foreach (GameObject obj in receivers)
            {
                if (obj == null)
                    continue;

                _cachedReceivers.Clear();
                obj.GetComponents(_cachedReceivers);

                foreach (IContactEventReceiver recipient in _cachedReceivers)
                {
                    if (recipient == null)
                        continue;

                    recipient.CurrentContactEventSender = this;
                    recipient.OnCollisionEnter(collision);
                    recipient.CurrentContactEventSender = null;
                }
            }
        }
        public void OnCollisionStay(Collision collision)
        {
            if ((enabledContactEvents & ContactEventFlags.OnCollisionStay) == 0)
                return;

            foreach (GameObject obj in receivers)
            {
                if (obj == null)
                    continue;

                _cachedReceivers.Clear();
                obj.GetComponents(_cachedReceivers);

                foreach (IContactEventReceiver recipient in _cachedReceivers)
                {
                    if (recipient == null)
                        continue;

                    recipient.CurrentContactEventSender = this;
                    recipient.OnCollisionStay(collision);
                    recipient.CurrentContactEventSender = null;
                }
            }
        }
        public void OnCollisionExit(Collision collision)
        {
            if ((enabledContactEvents & ContactEventFlags.OnCollisionExit) == 0)
                return;

            foreach (GameObject obj in receivers)
            {
                if (obj == null)
                    continue;

                _cachedReceivers.Clear();
                obj.GetComponents(_cachedReceivers);

                foreach (IContactEventReceiver recipient in _cachedReceivers)
                {
                    if (recipient == null)
                        continue;

                    recipient.CurrentContactEventSender = this;
                    recipient.OnCollisionExit(collision);
                    recipient.CurrentContactEventSender = null;
                }
            }
        }
        public void OnParticleCollision(GameObject other)
        {
            if ((enabledContactEvents & ContactEventFlags.OnParticleCollision) == 0)
                return;

            foreach (GameObject obj in receivers)
            {
                if (obj == null)
                    continue;

                _cachedReceivers.Clear();
                obj.GetComponents(_cachedReceivers);

                foreach (IContactEventReceiver recipient in _cachedReceivers)
                {
                    if (recipient == null)
                        continue;

                    recipient.CurrentContactEventSender = this;
                    recipient.OnParticleCollision(other);
                    recipient.CurrentContactEventSender = null;
                }
            }
        }
        public void OnTriggerEnter(Collider other)
        {
            if ((enabledContactEvents & ContactEventFlags.OnTriggerEnter) == 0)
                return;

            foreach (GameObject obj in receivers)
            {
                if (obj == null)
                    continue;

                _cachedReceivers.Clear();
                obj.GetComponents(_cachedReceivers);

                foreach (IContactEventReceiver recipient in _cachedReceivers)
                {
                    if (recipient == null)
                        continue;

                    recipient.CurrentContactEventSender = this;
                    recipient.OnTriggerEnter(other);
                    recipient.CurrentContactEventSender = null;
                }
            }
        }
        public void OnTriggerStay(Collider other)
        {
            if ((enabledContactEvents & ContactEventFlags.OnTriggerStay) == 0)
                return;

            foreach (GameObject obj in receivers)
            {
                if (obj == null)
                    continue;

                _cachedReceivers.Clear();
                obj.GetComponents(_cachedReceivers);

                foreach (IContactEventReceiver recipient in _cachedReceivers)
                {
                    if (recipient == null)
                        continue;

                    recipient.CurrentContactEventSender = this;
                    recipient.OnTriggerStay(other);
                    recipient.CurrentContactEventSender = null;
                }
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if ((enabledContactEvents & ContactEventFlags.OnTriggerExit) == 0)
                return;

            foreach (GameObject obj in receivers)
            {
                if (obj == null)
                    continue;

                _cachedReceivers.Clear();
                obj.GetComponents(_cachedReceivers);

                foreach (IContactEventReceiver recipient in _cachedReceivers)
                {
                    if (recipient == null)
                        continue;

                    recipient.CurrentContactEventSender = this;
                    recipient.OnTriggerExit(other);
                    recipient.CurrentContactEventSender = null;
                }
            }
        }
        public void OnParticleTrigger()
        {
            if ((enabledContactEvents & ContactEventFlags.OnParticleTrigger) == 0)
                return;

            foreach (GameObject obj in receivers)
            {
                if (obj == null)
                    continue;

                _cachedReceivers.Clear();
                obj.GetComponents(_cachedReceivers);

                foreach (IContactEventReceiver recipient in _cachedReceivers)
                {
                    if (recipient == null)
                        continue;

                    recipient.CurrentContactEventSender = this;
                    recipient.OnParticleTrigger();
                    recipient.CurrentContactEventSender = null;
                }
            }
        }
    }
}
