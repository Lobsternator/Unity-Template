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

        public ContactEventFlags enabledContactEventFlags;
        public List<TypeRestrictedObjectReference<IContactEventReceiver>> receivers;

        public void OnCollisionEnter(Collision collision)
        {
            if ((enabledContactEventFlags & ContactEventFlags.OnCollisionEnter) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].value as IContactEventReceiver;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = ActiveSender;
                recipient.OnCollisionEnter(collision);
                recipient.ActiveSender = null;
            }
        }
        public void OnCollisionStay(Collision collision)
        {
            if ((enabledContactEventFlags & ContactEventFlags.OnCollisionStay) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].value as IContactEventReceiver;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = ActiveSender;
                recipient.OnCollisionStay(collision);
                recipient.ActiveSender = null;
            }
        }
        public void OnCollisionExit(Collision collision)
        {
            if ((enabledContactEventFlags & ContactEventFlags.OnCollisionExit) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].value as IContactEventReceiver;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = ActiveSender;
                recipient.OnCollisionExit(collision);
                recipient.ActiveSender = null;
            }
        }
        public void OnParticleCollision(GameObject other)
        {
            if ((enabledContactEventFlags & ContactEventFlags.OnParticleCollision) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].value as IContactEventReceiver;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = ActiveSender;
                recipient.OnParticleCollision(other);
                recipient.ActiveSender = null;
            }
        }
        public void OnTriggerEnter(Collider other)
        {
            if ((enabledContactEventFlags & ContactEventFlags.OnTriggerEnter) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].value as IContactEventReceiver;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = ActiveSender;
                recipient.OnTriggerEnter(other);
                recipient.ActiveSender = null;
            }
        }
        public void OnTriggerStay(Collider other)
        {
            if ((enabledContactEventFlags & ContactEventFlags.OnTriggerStay) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].value as IContactEventReceiver;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = ActiveSender;
                recipient.OnTriggerStay(other);
                recipient.ActiveSender = null;
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if ((enabledContactEventFlags & ContactEventFlags.OnTriggerExit) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].value as IContactEventReceiver;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = ActiveSender;
                recipient.OnTriggerExit(other);
                recipient.ActiveSender = null;
            }
        }
        public void OnParticleTrigger()
        {
            if ((enabledContactEventFlags & ContactEventFlags.OnParticleTrigger) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver recipient = receivers[i].value as IContactEventReceiver;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = ActiveSender;
                recipient.OnParticleTrigger();
                recipient.ActiveSender = null;
            }
        }
    }
}
