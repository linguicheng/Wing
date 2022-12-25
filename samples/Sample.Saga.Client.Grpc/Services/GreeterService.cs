using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Wing.Saga.Client;

namespace Sample.Saga.Client.Grpc
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            Wing.Saga.Client.Saga.Start("Saga分布式事务样例", new SagaOptions { BreakerCount = 5, CustomCount=2, TranPolicy = Wing.Persistence.Saga.TranPolicy.Custom })
                .Then(new SampleSagaUnit1(), new SampleUnitModel { Name = "事务单元1", HelloName = request.Name })
                .Then(new SampleSagaUnit2(), new SampleUnitModel { Name = "事务单元2", HelloName = request.Name })
                .End();
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
