using Wing.EventBus;

namespace Wing.Saga.Server
{
    [Subscribe(QueueMode.DLX)]
    public class TranConsumerFail : TranConsumer
    {
    }
}
