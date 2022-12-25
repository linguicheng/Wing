using System.Collections.Generic;
using System.Threading.Tasks;
using Wing.Model;
using Wing.Result;

namespace Wing.Persistence.Saga
{
    public interface ISagaTranService
    {
        Task<int> Add(SagaTran entity);

        Task<int> UpdateStatus(UpdateStatusEvent dto);

        Task<bool> Any(string id);

        List<SagaTran> GetFailedData();

        Task<int> RetryCommit(RetryCommitTranEvent dto);

        Task<int> RetryCancel(RetryCancelTranEvent dto);

        Task<int> UpdateStatus(string id, TranStatus status);

        Task<PageResult<List<SagaTran>>> List(PageModel<SagaTranSearchDto> dto);
    }
}
