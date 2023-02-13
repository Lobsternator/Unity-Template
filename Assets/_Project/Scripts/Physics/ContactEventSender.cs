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

    public interface IContactEventReceiver
    {
        /// <summary>
        /// Only set this if you absolutely know what you're doing!
        /// </summary>
        public ContactEventSender ActiveSender { get; set; }

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

    public class ContactEventSender : MonoBehaviour, IContactEventReceiver
    {
        public ContactEventSender ActiveSender { get; set; }

        public ContactEventFlags enabledContactEvents;
        public List<SerializableInterface<IContactEventReceiver>> receivers;

        public void OnCollisionEnter(Collision collision)
        {
            if ((enabledContactEvents & ContactEventFlags.OnCollisionEnter) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].Value;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = this;
                recipient.OnCollisionEnter(collision);
                recipient.ActiveSender = null;
            }
        }
        public void OnCollisionStay(Collision collision)
        {
            if ((enabledContactEvents & ContactEventFlags.OnCollisionStay) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].Value;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = this;
                recipient.OnCollisionStay(collision);
                recipient.ActiveSender = null;
            }
        }
        public void OnCollisionExit(Collision collision)
        {
            if ((enabledContactEvents & ContactEventFlags.OnCollisionExit) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].Value;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = this;
                recipient.OnCollisionExit(collision);
                recipient.ActiveSender = null;
            }
        }
        public void OnParticleCollision(GameObject other)
        {
            if ((enabledContactEvents & ContactEventFlags.OnParticleCollision) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].Value;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = this;
                recipient.OnParticleCollision(other);
                recipient.ActiveSender = null;
            }
        }
        public void OnTriggerEnter(Collider other)
        {
            if ((enabledContactEvents & ContactEventFlags.OnTriggerEnter) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].Value;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = this;
                recipient.OnTriggerEnter(other);
                recipient.ActiveSender = null;
            }
        }
        public void OnTriggerStay(Collider other)
        {
            if ((enabledContactEvents & ContactEventFlags.OnTriggerStay) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].Value;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = this;
                recipient.OnTriggerStay(other);
                recipient.ActiveSender = null;
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if ((enabledContactEvents & ContactEventFlags.OnTriggerExit) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].Value;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = this;
                recipient.OnTriggerExit(other);
                recipient.ActiveSender = null;
            }
        }
        public void OnParticleTrigger()
        {
            if ((enabledContactEvents & ContactEventFlags.OnParticleTrigger) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].Value;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = this;
                recipient.OnParticleTrigger();
                recipient.ActiveSender = null;
            }
        }
    }
}
