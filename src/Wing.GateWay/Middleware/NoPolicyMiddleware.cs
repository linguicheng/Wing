using System.Net;
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
            if (string.IsNullOrWhiteSpace(serviceContext.ServiceName) || serviceContext.IsWebSocket)
            {
                await _next(serviceContext);
                return;
            }

            if (serviceContext.Policy != null &&
                ((serviceContext.Policy.Breaker != null && serviceContext.Policy.Breaker.IsEnabled)
                || (serviceContext.Policy.RateLimit != null && serviceContext.Policy.RateLimit.IsEnabled)
                || (serviceContext.Policy.BulkHead != null && serviceContext.Policy.BulkHead.IsEnabled)
                || (serviceContext.Policy.Retry != null && serviceContext.Policy.Retry.IsEnabled)
                || (serviceContext.Policy.TimeOut != null && serviceContext.Policy.TimeOut.IsEnabled)))
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
                    var reqMsg = await context.Request.ToHttpRequestMessage(serviceAddr, serviceContext.DownstreamPath);
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
            catch (ServiceNotFoundException ex)
            {
                serviceContext.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.StatusCode = serviceContext.StatusCode;
                serviceContext.Exception = $"{ex.Message} {ex.StackTrace}";
                await _logProvider.Add(serviceContext);
            }
            catch(Exception ex)
            {
                serviceContext.StatusCode = (int)HttpStatusCode.BadGateway;
                serviceContext.Exception = $"{ex.Message} {ex.StackTrace}";
                context.Response.StatusCode = serviceContext.StatusCode;

                await _logProvider.Add(serviceContext);
            }
        }
    }
}
