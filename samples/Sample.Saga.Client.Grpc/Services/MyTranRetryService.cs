using Microsoft.AspNetCore.Authorization;
using Wing.Saga.Client;

namespace Sample.Saga.Client.Grpc.Services
{
    [Authorize]
    public class MyTranRetryService : TranRetryGrpcService
    {
        public MyTranRetryService(ITranRetryService tranRetryService) : base(tranRetryService)
        {
        }
    }
}
