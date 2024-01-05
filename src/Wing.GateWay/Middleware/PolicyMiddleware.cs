namespace Wing.Gateway.Middleware
{
    public class PolicyMiddleware
    {
        private readonly ServiceRequestDelegate _next;
        private readonly ILogProvider _logProvider;

        public PolicyMiddleware(ServiceRequestDelegate next,
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

            await DataProvider.InvokeWithPolicy(serviceContext);
            var context = serviceContext.HttpContext;
            await _logProvider.Add(serviceContext);
            await context.Response.Response(serviceContext);
        }
    }
}
