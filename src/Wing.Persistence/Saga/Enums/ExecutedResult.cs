namespace Wing.Persistence.Saga
{
    public enum ExecutedResult : int
    {
        /// <summary>
        /// 执行成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 执行失败
        /// </summary>
        Failed = 2
    }
}
