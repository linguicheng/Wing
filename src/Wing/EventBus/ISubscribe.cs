namespace Wing.EventBus
{
    public interface ISubscribe<TEventMessage>
        where TEventMessage : EventMessage
    {
        Task<bool> Consume(TEventMessage eventMessage);
    }
}
