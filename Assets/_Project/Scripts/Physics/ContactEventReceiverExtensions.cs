using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    public static class ContactEventReceiverExtensions
    {
        public static ContactEventSender GetEventInstigator(this IContactEventReceiver contactEventReceiver)
        {
            ContactEventSender sender = contactEventReceiver.ActiveSender;

            while ( sender && sender.ActiveSender )
                sender = sender.ActiveSender;

            return sender;
        }
        public static ContactEventSender2D GetEventInstigator(this IContactEventReceiver2D contactEventReceiver)
        {
            ContactEventSender2D sender = contactEventReceiver.ActiveSender;

            while (sender && sender.ActiveSender)
                sender = sender.ActiveSender;

            return sender;
        }
    }
}
