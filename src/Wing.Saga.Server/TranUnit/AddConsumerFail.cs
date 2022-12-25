using Wing.EventBus;

namespace Wing.Saga.Server.TranUnit
{
    [Subscribe(QueueMode.DLX)]
    public class AddConsumerFail : AddConsumer
    {
    }
}
