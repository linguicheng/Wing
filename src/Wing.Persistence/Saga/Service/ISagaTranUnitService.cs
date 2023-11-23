using System.Data.Common;

namespace Wing.Persistence.Saga
{
    public interface ISagaTranUnitService
    {
        Task<int> Add(SagaTranUnit entity, DbTransaction transaction = null);

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
