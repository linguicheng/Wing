namespace Wing.GateWay.Config
{
    public enum LogProviderEnum
    {
        /// <summary>
        /// Channel发布订阅
        /// </summary>
        Channel = 0,
        /// <summary>
        /// 直连数据库
        /// </summary>
        DataBase = 1,
        /// <summary>
        /// 分布式消息总线
        /// </summary>
        EventBus = 2
    }
}
