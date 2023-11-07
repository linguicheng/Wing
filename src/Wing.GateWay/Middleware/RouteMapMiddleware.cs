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

            serviceContext.IsWebSocket = context.WebSockets.IsWebSocketRequest;
            serviceContext.DownstreamPath = "/" + string.Join('/', upstreamPaths);
            WebSocketAuth(serviceContext);
            GetPolicy(serviceContext, upstreamPaths);
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

        private void GetPolicy(ServiceContext serviceContext, string[] upstreamPaths)
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
        }
    }
}
