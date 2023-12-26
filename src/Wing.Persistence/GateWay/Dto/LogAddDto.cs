using Wing.EventBus;
using Wing.Persistence.Gateway;

namespace Wing.Persistence.GateWay
{
    public class LogAddDto : EventMessage
    {
        public Log Log { get; set; }

        public List<LogDetail> LogDetails { get; set; }
    }
}
