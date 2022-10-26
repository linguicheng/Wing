namespace Wing.Persistence.Saga
{
    public enum TranStatus : int
    {
        /// <summary>
        /// 执行成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 执行中
        /// </summary>
        Executing = 1,
        /// <summary>
        /// 执行失败
        /// </summary>
        Failed = 2
    }
}
