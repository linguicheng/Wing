using System.Collections.Generic;

namespace Wing.GateWay
{
    public class Config
    {
        public List<Policy> Policies { get; set; }

        public Global Global { get; set; }
    }

    public class Global
    {
        public Policy Policy { get; set; }
    }

    public class Policy
    {
        public string ServiceName { get; set; }

        public bool IsEnableBreaker { get; set; }

        public int ExceptionsAllowedBeforeBreaking { get; set; }

        public int MillisecondsOfBreak { get; set; }

        public int MaxRetryTimes { get; set; }

        public int RetryIntervalMilliseconds { get; set; }

        public int TimeOutMilliseconds { get; set; }

        public string AuthKey { get; set; }

        public bool? UseJWTAuth { get; set; }
    }
}
