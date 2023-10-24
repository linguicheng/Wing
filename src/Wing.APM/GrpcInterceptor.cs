using Grpc.Core;
using Grpc.Core.Interceptors;
using Wing.Converter;

namespace Wing.APM
{
    public class GrpcInterceptor : Interceptor
    {
        private readonly IJson _json;

        public GrpcInterceptor(IJson json)
        {
            _json = json;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var response = await base.UnaryServerHandler(request, context, continuation);
            var httpContext = context.GetHttpContext();
            httpContext.Items.Add(ApmTools.GrpcRequest, request == null ? string.Empty : _json.Serialize(request));
            httpContext.Items.Add(ApmTools.GrpcResponse, response == null ? string.Empty : _json.Serialize(response));
            return response;
        }
    }
}
