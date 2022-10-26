using System.Threading.Tasks;

namespace Wing.Persistence.Saga
{
    public interface ISagaTranUnitService
    {
        Task<int> Add(SagaTranUnit entity);

        Task<int> UpdateStatus(UpdateStatusDto dto);

        Task<bool> Any(string id);
    }
}
