using Grpc.Net.Client;
using Sample.Saga.Client.Grpc;
using System.Threading.Tasks;
using Wing.Injection;
using Wing.Policy;
using Wing.ServiceProvider;

namespace Sample.AspNetCoreService.Policy
{
    public interface IProduct
    {
        //[Policy]
        Task<string> InvokeHello(string name);
        Task<string> InvokeHelloFallback(string name);
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
            //var token = await _serviceFactory.GrpcServiceInvokeAsync("grpctest", async serviceAddr =>
            //{
            //    var channel = GrpcChannel.ForAddress(serviceAddr.ToString());
            //    var greeterClient = new Greeter.GreeterClient(channel);
            //    return await greeterClient.GetTokenAsync(new TokenRequest { Name = name });
            //});
            return await _serviceFactory.GrpcServiceInvokeAsync("Sample.Saga.Client.Grpc", async serviceAddr =>
            {
                //var headers = new Metadata
                //{
                //    { "Authorization", $"Bearer {token.Token}" }
                //};
                var channel = GrpcChannel.ForAddress(serviceAddr.ToString());
                var greeterClient = new Greeter.GreeterClient(channel);
                var result = await greeterClient.SayHelloAsync(new HelloRequest { Name = name });
                return result.Message;
            });

        }

        public Task<string> InvokeHelloFallback(string name)
        {
            return Task.FromResult("test-fallback");
        }
    }
}
