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
            var context = serviceContext.HttpContext;
            var fullPath = context.Request.Path.ToString();
            var paths = fullPath.Split("/", StringSplitOptions.RemoveEmptyEntries);
            if (paths == null || paths.Length <= 0)
            {
                await _next(serviceContext);
                return;
            }

            var routes = _configuration.GetSection("Gateway:Routes").Get<List<Route>>();
            foreach (var route in routes)
            {
                var keys = route.Upstream.Url.Split("/", StringSplitOptions.RemoveEmptyEntries);
                if (paths.Length == keys.Length)
                {
                    int count = 0;
                    for (var i = 0; i < keys.Length; i++)
                    {
                        if (keys[i].StartsWith('{') && keys[i].EndsWith('}'))
                        {
                            count++;
                            continue;
                        }

                        if (keys[i].Equals(paths[i], StringComparison.OrdinalIgnoreCase))
                        {
                            count++;
                            continue;
                        }
                    }

                    if (count == keys.Length)
                    {
                        serviceContext.Route = route;
                        serviceContext.UpstreamPath = fullPath;
                        break;
                    }
                }
            }

            string[] downstreamPaths = null;
            if (serviceContext.Route == null)
            {
                if (paths.Length <= 1)
                {
                    await _next(serviceContext);
                    return;
                }

                serviceContext.ServiceName = paths[0];
                downstreamPaths = new string[paths.Length - 1];
                for (var i = 0; i < downstreamPaths.Length; i++)
                {
                    downstreamPaths[i] = paths[i + 1];
                }

                serviceContext.UpstreamPath = serviceContext.DownstreamPath = "/" + string.Join('/', downstreamPaths);
            }

            serviceContext.IsWebSocket = context.WebSockets.IsWebSocketRequest;
            WebSocketAuth(serviceContext);
            GetPolicy(serviceContext, downstreamPaths);
            await _next(serviceContext);
        }

        private void WebSocketAuth(ServiceContext serviceContext)
        {
            if (!serviceContext.IsWebSocket)
            {
                return;
            }

            var context = serviceContext.HttpContext;
            var token = context.Request.Query["token"].ToString();
            if (!string.IsNullOrWhiteSpace(token))
            {
                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Request.Headers["Authorization"] = $"Bearer {token}";
                }
                else
                {
                    context.Request.Headers.Add("Authorization", $"Bearer {token}");
                }
            }
            else
            {
                var authKey = context.Request.Query["key"].ToString();
                if (context.Request.Headers.ContainsKey("AuthKey"))
                {
                    context.Request.Headers["AuthKey"] = authKey;
                }
                else
                {
                    context.Request.Headers.Add("AuthKey", authKey);
                }
            }
        }

        private void GetPolicy(ServiceContext serviceContext, string[] downstreamPaths)
        {
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
                            if (keys.Length == downstreamPaths.Length)
                            {
                                int count = 0;
                                for (var i = 0; i < keys.Length; i++)
                                {
                                    if (keys[i].StartsWith('{') && keys[i].EndsWith('}'))
                                    {
                                        count++;
                                        continue;
                                    }

                                    if (keys[i].Equals(downstreamPaths[i], StringComparison.OrdinalIgnoreCase))
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
        }
    }
}
