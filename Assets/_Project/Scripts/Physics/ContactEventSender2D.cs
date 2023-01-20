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

    public interface IContactEventReceiver2D
    {
        /// <summary>
        /// Only set this if you absolutely know what you're doing!
        /// </summary>
        public ContactEventSender2D ActiveSender { get; set; }

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

    public class ContactEventSender2D : MonoBehaviour, IContactEventReceiver2D
    {
        public ContactEventSender2D ActiveSender { get; set; }

        public ContactEventFlags2D enabledContactEvents;
        public List<TypeRestrictedObjectReference<IContactEventReceiver2D>> receivers;

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if ((enabledContactEvents & ContactEventFlags2D.OnCollisionEnter) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver2D recipient = receivers[i].value as IContactEventReceiver2D;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = ActiveSender;
                recipient.OnCollisionEnter2D(collision);
                recipient.ActiveSender = null;
            }
        }
        public void OnCollisionStay2D(Collision2D collision)
        {
            if ((enabledContactEvents & ContactEventFlags2D.OnCollisionStay) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver2D recipient = receivers[i].value as IContactEventReceiver2D;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = ActiveSender;
                recipient.OnCollisionStay2D(collision);
                recipient.ActiveSender = null;
            }
        }
        public void OnCollisionExit2D(Collision2D collision)
        {
            if ((enabledContactEvents & ContactEventFlags2D.OnCollisionExit) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver2D recipient = receivers[i].value as IContactEventReceiver2D;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = ActiveSender;
                recipient.OnCollisionExit2D(collision);
                recipient.ActiveSender = null;
            }
        }
        public void OnTriggerEnter2D(Collider2D other)
        {
            if ((enabledContactEvents & ContactEventFlags2D.OnTriggerEnter) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver2D recipient = receivers[i].value as IContactEventReceiver2D;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = ActiveSender;
                recipient.OnTriggerEnter2D(other);
                recipient.ActiveSender = null;
            }
        }
        public void OnTriggerStay2D(Collider2D other)
        {
            if ((enabledContactEvents & ContactEventFlags2D.OnTriggerStay) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver2D recipient = receivers[i].value as IContactEventReceiver2D;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = ActiveSender;
                recipient.OnTriggerStay2D(other);
                recipient.ActiveSender = null;
            }
        }
        public void OnTriggerExit2D(Collider2D other)
        {
            if ((enabledContactEvents & ContactEventFlags2D.OnTriggerExit) == 0)
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                IContactEventReceiver2D recipient = receivers[i].value as IContactEventReceiver2D;
                if (recipient == null)
                    continue;

                recipient.ActiveSender = ActiveSender;
                recipient.OnTriggerExit2D(other);
                recipient.ActiveSender = null;
            }
        }
    }
}
