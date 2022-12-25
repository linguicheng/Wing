using System.Collections.Generic;

namespace Wing.Persistence.Saga
{
    public class RetryData
    {
        public string TranId { get; set; }

        public List<RetryTranUnit> SagaTranUnits { get; set; }
    }
}
