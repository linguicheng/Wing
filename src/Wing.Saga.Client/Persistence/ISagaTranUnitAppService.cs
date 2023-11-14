using Wing.Persistence.Saga;

namespace Wing.Saga.Client.Persistence
{
    public interface ISagaTranUnitAppService
    {
        Task<bool> Add(SagaTranUnit tranUnit, string action);

        Task<bool> RetryCancel(RetryCancelTranUnitEvent eventMessage, string action);

        Task<bool> RetryCommit(RetryCommitTranUnitEvent eventMessage, string action);

        Task<bool> UpdateStatus(UpdateTranUnitStatusEvent eventMessage, string action);
    }
}
