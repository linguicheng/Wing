using System.Collections.Concurrent;
using Wing.Gateway.Config;
using Wing.Persistence.GateWay;

namespace Wing.Gateway
{
    public class DataProvider
    {
        public static readonly ConcurrentQueue<LogAddDto> Data = new();

        public static LogConfig LogConfig => App.GetConfig<LogConfig>("Gateway:Log");
    }
}
