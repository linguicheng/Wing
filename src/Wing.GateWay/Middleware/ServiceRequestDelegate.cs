using System.Threading.Tasks;

namespace Wing.GateWay.Middleware
{
    public delegate Task ServiceRequestDelegate(ServiceContext serviceContext);
}
