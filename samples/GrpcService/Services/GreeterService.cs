using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Wing.Auth;

namespace GrpcService
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly IAuth _auth;
        public GreeterService(IAuth auth)
        {
            _auth = auth;
        }
        [Authorize]
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "a " + request.Name
            });
        }
        public override Task<TokenReply> GetToken(TokenRequest request, ServerCallContext context)
        {
            var token = _auth.GetToken("byron");
            return Task.FromResult(new TokenReply
            {
                Token = token
            });
        }
    }
}
