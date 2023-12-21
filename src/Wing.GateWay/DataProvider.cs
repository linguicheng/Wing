using System.Collections.Concurrent;
using Wing.Persistence.GateWay;

namespace Wing.Gateway
{
    public class DataProvider
    {
        public static readonly ConcurrentQueue<LogAddDto> Data = new();
    }
}
