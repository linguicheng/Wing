namespace Wing.Persistence.Saga
{
    public enum TranPolicy : int
    {
        /// <summary>
        /// 向前恢复
        /// </summary>
        Forward = 0,
        /// <summary>
        /// 向后恢复
        /// </summary>
        Backward = 1,
        /// <summary>
        /// 先前再后(向前恢复指定次数，如果失败，则向后恢复)
        /// </summary>
        Custom = 2
    }
}
