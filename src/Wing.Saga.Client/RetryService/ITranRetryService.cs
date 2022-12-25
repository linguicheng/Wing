using System.Threading.Tasks;
using Wing.Persistence.Saga;

namespace Wing.Saga.Client
{
    public interface ITranRetryService
    {
        Task<ResponseData> Commit(RetryData retryData);

        Task<ResponseData> Cancel(RetryData retryData);
    }
}
