using System.Collections.Concurrent;
using Wing.EventBus;
using Wing.Persistence.Gateway;

namespace Wing.Persistence.GateWay
{
    public class LogAddDto : EventMessage
    {
        public Log Log { get; set; }

        public ConcurrentBag<LogDetail> LogDetails { get; set; }
    }
}
