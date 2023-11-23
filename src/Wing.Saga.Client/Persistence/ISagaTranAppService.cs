using System.Data.Common;
using Wing.Persistence.Saga;

namespace Wing.Saga.Client.Persistence
{
    public interface ISagaTranAppService
    {
        Task<bool> Add(SagaTran sagaTran, string action, DbTransaction transaction);

        Task<bool> RetryCancel(RetryCancelTranEvent eventMessage, string action);

        Task<bool> RetryCommit(RetryCommitTranEvent eventMessage, string action);

        Task<bool> UpdateStatus(UpdateTranStatusEvent eventMessage, string action);
    }
}
