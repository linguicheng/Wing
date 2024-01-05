using System.Net;
using Wing.Exceptions;

namespace Wing.Gateway.Middleware
{
    public class NoPolicyMiddleware
    {
        private readonly ServiceRequestDelegate _next;
        private readonly ILogProvider _logProvider;

        public NoPolicyMiddleware(ServiceRequestDelegate next,
            ILogProvider logProvider)
        {
            _next = next;
            _logProvider = logProvider;
        }

        public async Task InvokeAsync(ServiceContext serviceContext)
        {
            if (serviceContext.Route != null
                || string.IsNullOrWhiteSpace(serviceContext.ServiceName)
                || serviceContext.IsWebSocket)
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
                await DataProvider.InvokeWithNoPolicy(serviceContext);
            }
            catch (ServiceNotFoundException ex)
            {
                serviceContext.StatusCode = (int)HttpStatusCode.NotFound;
                serviceContext.Exception = $"{ex.Message} {ex.StackTrace}";
            }
            catch (Exception ex)
            {
                serviceContext.StatusCode = (int)HttpStatusCode.BadGateway;
                serviceContext.Exception = $"{ex.Message} {ex.StackTrace}";
            }
            finally
            {
                await _logProvider.Add(serviceContext);
                await context.Response.Response(serviceContext);
            }
        }
    }
}
