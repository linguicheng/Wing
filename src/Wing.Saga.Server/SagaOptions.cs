using System.Collections.Generic;

namespace Wing.Saga.Server
{
    public class SagaOptions
    {
        /// <summary>
        /// 设置请求头
        /// </summary>
        public Func<Dictionary<string, string>> Headers { get; set; }
    }
}
