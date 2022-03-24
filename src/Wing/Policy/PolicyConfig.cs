namespace Wing.Policy
{
    public class PolicyConfig
    {
        public bool IsEnableBreaker { get; set; }

        public int ExceptionsAllowedBeforeBreaking { get; set; }

        public int MillisecondsOfBreak { get; set; }

        public int MaxRetryTimes { get; set; }

        public int RetryIntervalMilliseconds { get; set; }

        public int TimeOutMilliseconds { get; set; }

        public string CacheKeyPrefix { get; set; }

        public int CacheMilliseconds { get; set; }

        public string CacheKey { get; set; }

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

        public static bool operator ==(PolicyConfig left, PolicyConfig right)
        {
            return left.IsEnableBreaker == right.IsEnableBreaker &&
                left.ExceptionsAllowedBeforeBreaking == right.ExceptionsAllowedBeforeBreaking &&
                left.MillisecondsOfBreak == right.MillisecondsOfBreak &&
                left.TimeOutMilliseconds == right.TimeOutMilliseconds &&
                left.MaxRetryTimes == right.MaxRetryTimes &&
                left.CacheMilliseconds == right.CacheMilliseconds &&
                left.CacheKeyPrefix == right.CacheKeyPrefix &&
                left.CacheKey == right.CacheKey &&
                left.RetryIntervalMilliseconds == right.RetryIntervalMilliseconds;
        }

        public static bool operator !=(PolicyConfig left, PolicyConfig right)
        {
            return left.IsEnableBreaker != right.IsEnableBreaker ||
                left.ExceptionsAllowedBeforeBreaking != right.ExceptionsAllowedBeforeBreaking ||
                left.MillisecondsOfBreak != right.MillisecondsOfBreak ||
                left.TimeOutMilliseconds != right.TimeOutMilliseconds ||
                left.MaxRetryTimes != right.MaxRetryTimes ||
                left.CacheMilliseconds != right.CacheMilliseconds ||
                left.CacheKeyPrefix != right.CacheKeyPrefix ||
                left.CacheKey != right.CacheKey ||
                left.RetryIntervalMilliseconds != right.RetryIntervalMilliseconds;
        }
    }
}
