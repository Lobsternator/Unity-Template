namespace Template.Physics
{
    /// <summary>
    /// Extension class for <see cref="IContactEventReceiver"/>.
    /// </summary>
    public static class ContactEventReceiverExtensions
    {
        public static ContactEventSender GetCurrentContactEventInstigator(this IContactEventReceiver contactEventReceiver)
        {
            ContactEventSender sender = contactEventReceiver.CurrentContactEventSender;

            while ( sender && sender.CurrentContactEventSender )
                sender = sender.CurrentContactEventSender;

            return sender;
        }
        public static ContactEventSender2D GetCurrentContactEventInstigator(this IContactEventReceiver2D contactEventReceiver)
        {
            ContactEventSender2D sender = contactEventReceiver.CurrentContactEventSender;

            while (sender && sender.CurrentContactEventSender)
                sender = sender.CurrentContactEventSender;

            return sender;
        }
    }
}
