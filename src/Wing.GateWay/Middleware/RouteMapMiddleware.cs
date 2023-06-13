using System;
using System.Collections.Generic;
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
            string[] upstreamPaths = new string[paths.Length - 2];
            for (var i = 0; i < upstreamPaths.Length; i++)
            {
                upstreamPaths[i] = paths[i + 2];
            }

            serviceContext.DownstreamPath = "/" + string.Join('/', upstreamPaths);
            var config = _configuration.GetSection("Gateway:Policy").Get<PolicyConfig>();
            if (config != null)
            {
                if (config.Policies != null && config.Policies.Count > 0)
                {
                    // 服务策略
                    serviceContext.Policy = config.Policies.Where(p => p.Key == serviceContext.ServiceName).FirstOrDefault();
                    if (serviceContext.Policy != null
                        && serviceContext.Policy.MethodPolicies != null
                        && serviceContext.Policy.MethodPolicies.Count > 0)
                    {
                        // 服务方法策略
                        Policy methodPolicy = null;
                        foreach (var p in serviceContext.Policy.MethodPolicies)
                        {
                            var keys = p.Key.Split('/');
                            if (keys.Length == upstreamPaths.Length)
                            {
                                int count = 0;
                                for (var i = 0; i < keys.Length; i++)
                                {
                                    if (keys[i].StartsWith("{") && keys[i].EndsWith("}"))
                                    {
                                        count++;
                                        continue;
                                    }

                                    if (keys[i].ToLower() == upstreamPaths[i].ToLower())
                                    {
                                        count++;
                                        continue;
                                    }
                                }

                                if (count == keys.Length)
                                {
                                    methodPolicy = p;
                                    break;
                                }
                            }
                        }

                        if (methodPolicy != null)
                        {
                            serviceContext.Policy = methodPolicy;
                        }
                    }
                }

                serviceContext.Policy ??= config.Global;
            }

            await _next(serviceContext);
        }
    }
}
