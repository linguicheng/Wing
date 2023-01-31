using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Wing.Exceptions;
using Wing.ServiceProvider;

namespace Wing.Gateway.Middleware
{
    public class NoPolicyMiddleware
    {
        private readonly ServiceRequestDelegate _next;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IServiceFactory _serviceFactory;
        private readonly ILogProvider _logProvider;

        public NoPolicyMiddleware(ServiceRequestDelegate next,
            IHttpClientFactory clientFactory,
            IServiceFactory serviceFactory,
            ILogProvider logProvider)
        {
            _next = next;
            _clientFactory = clientFactory;
            _serviceFactory = serviceFactory;
            _logProvider = logProvider;
        }

        public async Task InvokeAsync(ServiceContext serviceContext)
        {
            if (string.IsNullOrWhiteSpace(serviceContext.ServiceName))
            {
                await _next(serviceContext);
                return;
            }

            if (!(serviceContext.Policy is null) && (serviceContext.Policy.IsEnableBreaker || serviceContext.Policy.MaxRetryTimes > 0 || serviceContext.Policy.TimeOutMilliseconds > 0))
            {
                await _next(serviceContext);
                return;
            }

            var context = serviceContext.HttpContext;
            try
            {
                var resMsg = await _serviceFactory.InvokeAsync(serviceContext.ServiceName, async serviceAddr =>
                {
                    serviceContext.ServiceAddress = serviceAddr.ToString();
                    var reqMsg = context.Request.ToHttpRequestMessage(serviceAddr, serviceContext.DownstreamPath);
                    var client = _clientFactory.CreateClient(serviceContext.ServiceName);
                    return await client.SendAsync(reqMsg);
                });
                await context.Response.FromHttpResponseMessage(resMsg, (statusCode, content) =>
                {
                    serviceContext.StatusCode = statusCode;
                    serviceContext.ResponseValue = content;
                    _logProvider.Add(serviceContext);
                });
            }
            catch (ServiceNotFoundException)
            {
                serviceContext.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.StatusCode = serviceContext.StatusCode;
                await _logProvider.Add(serviceContext);
            }
            catch
            {
                serviceContext.StatusCode = (int)HttpStatusCode.BadGateway;
                context.Response.StatusCode = serviceContext.StatusCode;
                await _logProvider.Add(serviceContext);
            }
        }
    }
}
