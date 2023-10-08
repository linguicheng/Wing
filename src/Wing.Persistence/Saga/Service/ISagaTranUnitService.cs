using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wing.Persistence.Saga
{
    public interface ISagaTranUnitService
    {
        Task<int> Add(SagaTranUnit entity);

        Task<int> UpdateStatus(UpdateStatusEvent dto);

        Task<int> UpdateStatus(string tranId, TranStatus status);

        Task<bool> Any(string id);

        List<SagaTranUnit> GetFailedData(string tranId);

        List<SagaTranUnit> GetSuccessData(string tranId);

        Task<List<SagaTranUnit>> List(string tranId);

        Task<int> RetryCommit(RetryCommitTranUnitEvent dto);

        Task<int> RetryCancel(RetryCancelTranUnitEvent dto);
    }
}
