using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Wing.Gateway.Config;

namespace Wing.Gateway.Middleware
{
    public class RouteMapMiddleware
    {
        private readonly ServiceRequestDelegate _next;
        private readonly IConfiguration _configuration;

        public RouteMapMiddleware(ServiceRequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(ServiceContext serviceContext)
        {
            serviceContext.RequestTime = DateTime.Now;
            var fullPath = serviceContext.HttpContext.Request.Path.ToString();
            var paths = fullPath.Split("/");
            if (paths == null || paths.Length <= 2)
            {
                await _next(serviceContext);
                return;
            }

            var serviceName = paths[1];
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                await _next(serviceContext);
                return;
            }

            serviceContext.ServiceName = serviceName;
            string[] downstreamPaths = new string[paths.Length - 2];
            for (var i = 0; i < downstreamPaths.Length; i++)
            {
                downstreamPaths[i] = paths[i + 2];
            }

            serviceContext.DownstreamPath = "/" + string.Join('/', downstreamPaths);
            var config = _configuration.GetSection("Gateway:Policy").Get<PolicyConfig>();
            if (config != null)
            {
                if (config.Policies != null && config.Policies.Count > 0)
                {
                    serviceContext.Policy = config.Policies.Where(p => p.ServiceName == serviceContext.ServiceName).FirstOrDefault();
                }

                serviceContext.Policy ??= config.Global;
            }

            await _next(serviceContext);
        }
    }
}
