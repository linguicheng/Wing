using System.Collections.Generic;

namespace Wing.Gateway.Config
{
    public class Policy
    {
        public string Key { get; set; }

        public Breaker Breaker { get; set; }

        public Retry Retry { get; set; }

        public Bulkhead BulkHead { get; set; }

        public Ratelimit RateLimit { get; set; }

        public Timeout TimeOut { get; set; }

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
            if (left is null)
            {
                return right is null;
            }

            if (right is null)
            {
                return left is null;
            }

            return left.Breaker == right.Breaker &&
               left.Retry == right.Retry &&
               left.BulkHead == right.BulkHead &&
               left.RateLimit == right.RateLimit &&
               left.TimeOut == right.TimeOut;
        }

        public static bool operator !=(Policy left, Policy right)
        {
            if (left is null)
            {
                return !(right is null);
            }

            if (right is null)
            {
                return !(left is null);
            }

            return left.Breaker != right.Breaker ||
                  left.Retry != right.Retry ||
                  left.BulkHead != right.BulkHead ||
                  left.RateLimit != right.RateLimit ||
                  left.TimeOut != right.TimeOut;
        }

        public List<Policy> MethodPolicies { get; set; }
    }

    public class Breaker
    {
        public bool IsEnabled { get; set; }

        public int ExceptionsAllowedBeforeBreaking { get; set; }

        public int MillisecondsOfBreak { get; set; }

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

        public static bool operator ==(Breaker left, Breaker right)
        {
            if (left is null)
            {
                return right is null;
            }

            if (right is null)
            {
                return left is null;
            }

            return left.IsEnabled == right.IsEnabled &&
                left.ExceptionsAllowedBeforeBreaking == right.ExceptionsAllowedBeforeBreaking &&
                left.MillisecondsOfBreak == right.MillisecondsOfBreak;
        }

        public static bool operator !=(Breaker left, Breaker right)
        {
            if (left is null)
            {
                return !(right is null);
            }

            if (right is null)
            {
                return !(left is null);
            }

            return left.IsEnabled != right.IsEnabled ||
                left.ExceptionsAllowedBeforeBreaking != right.ExceptionsAllowedBeforeBreaking ||
                left.MillisecondsOfBreak != right.MillisecondsOfBreak;
        }
    }

    public class Retry
    {
        public bool IsEnabled { get; set; }

        public int MaxTimes { get; set; }

        public int IntervalMilliseconds { get; set; }

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

        public static bool operator ==(Retry left, Retry right)
        {
            if (left is null)
            {
                return right is null;
            }

            if (right is null)
            {
                return left is null;
            }

            return left.IsEnabled == right.IsEnabled &&
                left.MaxTimes == right.MaxTimes &&
                left.IntervalMilliseconds == right.IntervalMilliseconds;
        }

        public static bool operator !=(Retry left, Retry right)
        {
            if (left is null)
            {
                return !(right is null);
            }

            if (right is null)
            {
                return !(left is null);
            }

            return left.IsEnabled != right.IsEnabled ||
                left.MaxTimes != right.MaxTimes ||
                left.IntervalMilliseconds != right.IntervalMilliseconds;
        }
    }

    public class Bulkhead
    {
        public bool IsEnabled { get; set; }

        public int MaxParallelization { get; set; }

        public int MaxQueuingActions { get; set; }

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

        public static bool operator ==(Bulkhead left, Bulkhead right)
        {
            if (left is null)
            {
                return right is null;
            }

            if (right is null)
            {
                return left is null;
            }

            return left.IsEnabled == right.IsEnabled &&
                left.MaxParallelization == right.MaxParallelization &&
                left.MaxQueuingActions == right.MaxQueuingActions;
        }

        public static bool operator !=(Bulkhead left, Bulkhead right)
        {
            if (left is null)
            {
                return !(right is null);
            }

            if (right is null)
            {
                return !(left is null);
            }

            return left.IsEnabled != right.IsEnabled ||
                left.MaxParallelization != right.MaxParallelization ||
                left.MaxQueuingActions != right.MaxQueuingActions;
        }
    }

    public class Ratelimit
    {
        public bool IsEnabled { get; set; }

        public int NumberOfExecutions { get; set; }

        public int PerSeconds { get; set; }

        public int MaxBurst { get; set; }

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

        public static bool operator ==(Ratelimit left, Ratelimit right)
        {
            if (left is null)
            {
                return right is null;
            }

            if (right is null)
            {
                return left is null;
            }

            return left.IsEnabled == right.IsEnabled &&
                left.NumberOfExecutions == right.NumberOfExecutions &&
                left.PerSeconds == right.PerSeconds &&
                left.MaxBurst == right.MaxBurst;
        }

        public static bool operator !=(Ratelimit left, Ratelimit right)
        {
            if (left is null)
            {
                return !(right is null);
            }

            if (right is null)
            {
                return !(left is null);
            }

            return left.IsEnabled != right.IsEnabled ||
                left.NumberOfExecutions != right.NumberOfExecutions ||
                left.PerSeconds != right.PerSeconds ||
                left.MaxBurst != right.MaxBurst;
        }
    }

    public class Timeout
    {
        public bool IsEnabled { get; set; }

        public int TimeOutMilliseconds { get; set; }

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

        public static bool operator ==(Timeout left, Timeout right)
        {
            if (left is null)
            {
                return right is null;
            }

            if (right is null)
            {
                return left is null;
            }

            return left.IsEnabled == right.IsEnabled &&
                left.TimeOutMilliseconds == right.TimeOutMilliseconds;
        }

        public static bool operator !=(Timeout left, Timeout right)
        {
            if (left is null)
            {
                return !(right is null);
            }

            if (right is null)
            {
                return !(left is null);
            }

            return left.IsEnabled != right.IsEnabled ||
                left.TimeOutMilliseconds != right.TimeOutMilliseconds;
        }
    }

    public class PolicyConfig
    {
        public List<Policy> Policies { get; set; }

        public Policy Global { get; set; }
    }
}
