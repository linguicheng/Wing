using System.Threading.Tasks;

namespace Wing.Gateway
{
    public interface ILogProvider
    {
        Task Add(ServiceContext serviceContext);
    }
}
