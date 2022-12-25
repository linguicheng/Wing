namespace Wing.Saga.Client
{
    public class SagaResult
    {
        /// <summary>
        /// 事务执行结果
        /// </summary>
        public bool Success { get; set; } = true;
        /// <summary>
        /// 消息提示
        /// </summary>
        public string Msg { get; set; } = string.Empty;

        public dynamic Data { get; set; }
    }
}
