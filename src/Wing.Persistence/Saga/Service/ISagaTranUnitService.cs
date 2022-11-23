using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wing.Persistence.Saga
{
    public interface ISagaTranUnitService
    {
        Task<int> Add(SagaTranUnit entity);

        Task<int> UpdateStatus(UpdateStatusEvent dto);

        Task<bool> Any(string id);

        List<SagaTranUnit> GetFailedData(string tranId);
    }
}
