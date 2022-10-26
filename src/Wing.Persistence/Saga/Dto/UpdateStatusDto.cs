using Wing.EventBus;

namespace Wing.Persistence.Saga
{
    public class UpdateStatusDto : EventMessage
    {
        public string Id { get; set; }

        public TranStatus Status { get; set; }
    }
}
