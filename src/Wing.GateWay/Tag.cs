namespace Wing.Gateway
{
    public class Tag
    {
        public const string POLICY_TIP = "触发网关策略";

        public const string SERVICE_NOT_FOUND = $"{POLICY_TIP}，找不到服务";

        public const string TIME_OUT_FALLBACK = $"{POLICY_TIP}，超时异常降级";

        public const string UNKNOWN_FALLBACK = $"{POLICY_TIP}，未知异常降级";

        public const string RATE_LIMIT_FALLBACK = $"{POLICY_TIP}，限流异常降级";

        public const string BULK_HEAD_FALLBACK = $"{POLICY_TIP}，舱壁异常降级";

        public static readonly List<string> DO_NOT_TRANSFORM_HEADERS = new()
        { "accept", "content-type", "content-length" };

        public static readonly List<string> DO_NOT_TRANSFORM_RESPONSE_HEADERS = new()
        { "Date", "Transfer-Encoding", "Server" };

        public static string ExceptionFormat(string tip, Exception exception)
        {
            if (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            return $"{tip}，异常提示：{exception.Message}，{exception.StackTrace}";
        }
    }
}
