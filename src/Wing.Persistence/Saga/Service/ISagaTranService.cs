using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wing.Persistence.Saga
{
    public interface ISagaTranService
    {
        Task<int> Add(SagaTran entity);

        Task<int> UpdateStatus(UpdateStatusEvent dto);

        Task<bool> Any(string id);

        List<SagaTran> GetFailedData();

        Task<int> UpdateStatus(string id, TranStatus status);
    }
}
