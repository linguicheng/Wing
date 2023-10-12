using System;
using System.Collections.Generic;

namespace Wing.Persistence.Saga
{
    public class SagaTranSearchDto
    {
        /// <summary>
        /// 事务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        public TranStatus? Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public List<DateTime> CreatedTime { get; set; }
    }
}
