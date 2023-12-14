namespace Wing.EventBus
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class SubscribeAttribute : Attribute
    {
        public QueueMode[] QueueModes { get; }

        public SubscribeAttribute(params QueueMode[] queueModes)
        {
            QueueModes = queueModes.Any() ? queueModes : new QueueMode[] { QueueMode.Normal };
        }
    }
}
