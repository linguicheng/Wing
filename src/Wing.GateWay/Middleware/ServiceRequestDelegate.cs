using System.Threading.Tasks;

namespace Wing.Gateway.Middleware
{
    public delegate Task ServiceRequestDelegate(ServiceContext serviceContext);
}
