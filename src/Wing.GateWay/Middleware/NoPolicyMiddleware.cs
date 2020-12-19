using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Wing.Exceptions;
using Wing.ServiceProvider;

namespace Wing.GateWay.Middleware
{
    public class NoPolicyMiddleware
    {
        private readonly ServiceRequestDelegate _next;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IServiceFactory _serviceFactory;

        public NoPolicyMiddleware(ServiceRequestDelegate next, IHttpClientFactory clientFactory, IServiceFactory serviceFactory)
        {
            _next = next;
            _clientFactory = clientFactory;
            _serviceFactory = serviceFactory;
        }

        public async Task InvokeAsync(ServiceContext serviceContext)
        {
            if (string.IsNullOrWhiteSpace(serviceContext.ServiceName))
            {
                await _next(serviceContext);
                return;
            }

            if (serviceContext.Policy != null && (serviceContext.Policy.IsEnableBreaker || serviceContext.Policy.MaxRetryTimes > 0 || serviceContext.Policy.TimeOutMilliseconds > 0))
            {
                await _next(serviceContext);
                return;
            }

            var context = serviceContext.HttpContext;
            try
            {
                var resMsg = await _serviceFactory.HttpServiceInvoke(serviceContext.ServiceName, async serviceAddr =>
                {
                    var reqMsg = context.Request.ToHttpRequestMessage(serviceAddr, serviceContext.DownstreamPath);
                    var client = _clientFactory.CreateClient(serviceContext.ServiceName);
                    return await client.SendAsync(reqMsg);
                });
                await context.Response.FromHttpResponseMessage(resMsg);
            }
            catch (ServiceNotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
            }
        }
    }
}
