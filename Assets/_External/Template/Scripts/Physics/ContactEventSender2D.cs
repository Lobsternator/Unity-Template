using System;
using System.Collections;
using System.Collections.Generic;
using Template.Core;
using UnityEngine;

namespace Template.Physics
{
    [Flags]
    public enum ContactEventFlags2D
    {
        OnCollisionEnter    = 0b000001,
        OnCollisionStay     = 0b000010,
        OnCollisionExit     = 0b000100,
        OnTriggerEnter      = 0b001000,
        OnTriggerStay       = 0b010000,
        OnTriggerExit       = 0b100000,
    }

    /// <summary>
    /// Makes an object able to receive contact events from external sources.
    /// </summary>
    public interface IContactEventReceiver2D
    {
        /// <summary>
        /// Which sender are we currently receiving contact events from? Null if current contact event originated from this object.
        /// </summary>
        public ContactEventSender2D CurrentContactEventSender { get; set; }

        public void OnCollisionEnter2D(Collision2D collision)
        {

        }
        public void OnCollisionStay2D(Collision2D collision)
        {

        }
        public void OnCollisionExit2D(Collision2D collision)
        {

        }
        public void OnTriggerEnter2D(Collider2D other)
        {

        }
        public void OnTriggerStay2D(Collider2D other)
        {

        }
        public void OnTriggerExit2D(Collider2D other)
        {

        }
    }

    /// <summary>
    /// Sends contact events to any <see cref="IContactEventReceiver2D"/>s on specified <see cref="GameObject"/>s.
    /// </summary>
    public class ContactEventSender2D : MonoBehaviour, IContactEventReceiver2D
    {
        public ContactEventSender2D CurrentContactEventSender { get; set; }

        public ContactEventFlags2D enabledContactEvents;
        public HashSet<GameObject> receivers;

        private List<IContactEventReceiver2D> _cachedReceivers;

        private void Awake()
        {
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
                yield return CoroutineUtility.WaitForFrames(1);
                ClearDeadReceivers();
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if ((enabledContactEvents & ContactEventFlags2D.OnCollisionEnter) == 0)
                return;

            foreach (GameObject obj in receivers)
            {
                if (obj == null)
                    continue;

                _cachedReceivers.Clear();
                obj.GetComponents(_cachedReceivers);

                foreach (IContactEventReceiver2D recipient in _cachedReceivers)
                {
                    if (recipient == null)
                        continue;

                    recipient.CurrentContactEventSender = this;
                    recipient.OnCollisionEnter2D(collision);
                    recipient.CurrentContactEventSender = null;
                }
            }
        }
        public void OnCollisionStay2D(Collision2D collision)
        {
            if ((enabledContactEvents & ContactEventFlags2D.OnCollisionStay) == 0)
                return;

            foreach (GameObject obj in receivers)
            {
                if (obj == null)
                    continue;

                _cachedReceivers.Clear();
                obj.GetComponents(_cachedReceivers);

                foreach (IContactEventReceiver2D recipient in _cachedReceivers)
                {
                    if (recipient == null)
                        continue;

                    recipient.CurrentContactEventSender = this;
                    recipient.OnCollisionStay2D(collision);
                    recipient.CurrentContactEventSender = null;
                }
            }
        }
        public void OnCollisionExit2D(Collision2D collision)
        {
            if ((enabledContactEvents & ContactEventFlags2D.OnCollisionExit) == 0)
                return;

            foreach (GameObject obj in receivers)
            {
                if (obj == null)
                    continue;

                _cachedReceivers.Clear();
                obj.GetComponents(_cachedReceivers);

                foreach (IContactEventReceiver2D recipient in _cachedReceivers)
                {
                    if (recipient == null)
                        continue;

                    recipient.CurrentContactEventSender = this;
                    recipient.OnCollisionExit2D(collision);
                    recipient.CurrentContactEventSender = null;
                }
            }
        }
        public void OnTriggerEnter2D(Collider2D other)
        {
            if ((enabledContactEvents & ContactEventFlags2D.OnTriggerEnter) == 0)
                return;

            foreach (GameObject obj in receivers)
            {
                if (obj == null)
                    continue;

                _cachedReceivers.Clear();
                obj.GetComponents(_cachedReceivers);

                foreach (IContactEventReceiver2D recipient in _cachedReceivers)
                {
                    if (recipient == null)
                        continue;

                    recipient.CurrentContactEventSender = this;
                    recipient.OnTriggerEnter2D(other);
                    recipient.CurrentContactEventSender = null;
                }
            }
        }
        public void OnTriggerStay2D(Collider2D other)
        {
            if ((enabledContactEvents & ContactEventFlags2D.OnTriggerStay) == 0)
                return;

            foreach (GameObject obj in receivers)
            {
                if (obj == null)
                    continue;

                _cachedReceivers.Clear();
                obj.GetComponents(_cachedReceivers);

                foreach (IContactEventReceiver2D recipient in _cachedReceivers)
                {
                    if (recipient == null)
                        continue;

                    recipient.CurrentContactEventSender = this;
                    recipient.OnTriggerStay2D(other);
                    recipient.CurrentContactEventSender = null;
                }
            }
        }
        public void OnTriggerExit2D(Collider2D other)
        {
            if ((enabledContactEvents & ContactEventFlags2D.OnTriggerExit) == 0)
                return;

            foreach (GameObject obj in receivers)
            {
                if (obj == null)
                    continue;

                _cachedReceivers.Clear();
                obj.GetComponents(_cachedReceivers);

                foreach (IContactEventReceiver2D recipient in _cachedReceivers)
                {
                    if (recipient == null)
                        continue;

                    recipient.CurrentContactEventSender = this;
                    recipient.OnTriggerExit2D(other);
                    recipient.CurrentContactEventSender = null;
                }
            }
        }
    }
}
