using Wing.EventBus;

namespace Wing.Saga.Server.Tran
{
    [Subscribe(QueueMode.DLX)]
    public class RetryCommitConsumerFail : RetryCommitConsumer
    {
    }
}
