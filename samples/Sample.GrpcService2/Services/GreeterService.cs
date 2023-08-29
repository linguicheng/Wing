using System.Threading.Tasks;
using Grpc.Core;
using GrpcService;
using Microsoft.Extensions.Logging;

namespace Sample.GrpcService2
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
            return Task.FromResult(new HelloReply
            {
                Message = "b " + request.Name
            });
        }
    }
}
