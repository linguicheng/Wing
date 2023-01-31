using System.Collections.Generic;

namespace Wing.Gateway.Config
{
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

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(Policy left, Policy right)
        {
            return left.IsEnableBreaker == right.IsEnableBreaker &&
                left.ExceptionsAllowedBeforeBreaking == right.ExceptionsAllowedBeforeBreaking &&
                left.MillisecondsOfBreak == right.MillisecondsOfBreak &&
                left.TimeOutMilliseconds == right.TimeOutMilliseconds &&
                left.MaxRetryTimes == right.MaxRetryTimes &&
                left.RetryIntervalMilliseconds == right.RetryIntervalMilliseconds;
        }

        public static bool operator !=(Policy left, Policy right)
        {
            return left.IsEnableBreaker != right.IsEnableBreaker ||
                left.ExceptionsAllowedBeforeBreaking != right.ExceptionsAllowedBeforeBreaking ||
                left.MillisecondsOfBreak != right.MillisecondsOfBreak ||
                left.TimeOutMilliseconds != right.TimeOutMilliseconds ||
                left.MaxRetryTimes != right.MaxRetryTimes ||
                left.RetryIntervalMilliseconds != right.RetryIntervalMilliseconds;
        }
    }

    public class PolicyConfig
    {
        public List<Policy> Policies { get; set; }

        public Policy Global { get; set; }
    }
}
