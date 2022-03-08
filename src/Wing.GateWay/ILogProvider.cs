using System.Threading.Tasks;

namespace Wing.GateWay
{
    public interface ILogProvider
    {
        Task Add(ServiceContext serviceContext);
    }
}
