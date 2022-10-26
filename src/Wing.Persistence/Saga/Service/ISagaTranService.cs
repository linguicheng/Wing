using System.Threading.Tasks;

namespace Wing.Persistence.Saga
{
    public interface ISagaTranService
    {
        Task<int> Add(SagaTran entity);

        Task<int> UpdateStatus(UpdateStatusDto dto);

        Task<bool> Any(string id);
    }
}
