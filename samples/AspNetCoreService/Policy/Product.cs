using Grpc.Core;
using Grpc.Net.Client;
using GrpcService;
using System.Threading.Tasks;
using Wing.DynamicProxy;
using Wing.Injection;
using Wing.Policy;
using Wing.Saga;
using Wing.ServiceProvider;

namespace Sample.AspNetCoreService.Policy
{
    public interface IProduct
    {
        [Policy]
        Task<string> InvokeHello(string name);
        Task<string> InvokeHelloFallback(string name);
        [SagaMain]
        Task<bool> SageTest(bool aa);
        [Saga]
        bool SageTest2();
    }
    public class Product : IProduct, ITransient
    {
        private readonly IServiceFactory _serviceFactory;
        public Product(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public async Task<string> InvokeHello(string name)
        {
            var token = await _serviceFactory.GrpcServiceInvoke("grpctest", async serviceAddr =>
            {
                var channel = GrpcChannel.ForAddress(serviceAddr.ToString());
                var greeterClient = new Greeter.GreeterClient(channel);
                return await greeterClient.GetTokenAsync(new TokenRequest { Name = name });
            });
            return await _serviceFactory.GrpcServiceInvoke("grpctest", async serviceAddr =>
            {
                var headers = new Metadata
                {
                    { "Authorization", $"Bearer {token.Token}" }
                };
                var channel = GrpcChannel.ForAddress(serviceAddr.ToString());
                var greeterClient = new Greeter.GreeterClient(channel);
                var result = await greeterClient.SayHelloAsync(new HelloRequest { Name = name }, headers);
                return result.Message;
            });

        }

        public Task<string> InvokeHelloFallback(string name)
        {
            return Task.FromResult("test-fallback");
        }

        public Task<bool> SageTest(bool aa)
        {
            return Task.FromResult(aa);
        }

        public bool SageTest2()
        {
            return true;
        }
    }
}
