using System;

namespace Wing.EventBus
{
    public interface IEventBus
    {
        void Publish(EventMessage message, Action<bool> confirm);

        void Publish(EventMessage message);

        void Subscribe<TEventMessage, TConsumer>()
           where TEventMessage : EventMessage
           where TConsumer : ISubscribe<TEventMessage>, new();
    }
}
