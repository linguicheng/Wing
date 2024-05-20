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
            if (routes != null)
            {
                foreach (var route in routes)
                {
                    if (route.Upstream == null)
                    {
                        continue;
                    }

                    var keys = route.Upstream.Url.Split("/", StringSplitOptions.RemoveEmptyEntries);
                    if (keys[0] == "{*}" || keys[^1].StartsWith("{*}"))
                    {
                        var fixedKeys = new List<string>();
                        if (keys[0] == "{*}")
                        {
                            var lastWildchar = keys[^1];
                            // 开头有通配符
                            if (lastWildchar.StartsWith("{*}"))
                            {
                                #region 结尾有通配符
                                for (var i = 1; i < keys.Length - 1; i++)
                                {
                                    fixedKeys.Add(keys[i]);
                                }

                                if (fixedKeys.Count > 0)
                                {
                                    var firstFixedKey = fixedKeys.First();
                                    var firstFixedKeyArray = fixedKeys.ToArray();
                                    if (paths.Any(x => x == firstFixedKey))
                                    {
                                        for (var j = 1; j < paths.Length - 1; j++)
                                        {
                                            if (paths[j] == firstFixedKey)
                                            {
                                                var isMatch = true;
                                                for (var k = j; k < j + fixedKeys.Count; k++)
                                                {
                                                    if (k >= paths.Length - 1 || paths[k] != firstFixedKeyArray[k - j])
                                                    {
                                                        isMatch = false;
                                                        break;
                                                    }
                                                }

                                                if (isMatch)
                                                {
                                                    if (lastWildchar != "{*}")
                                                    {
                                                        var suffix = lastWildchar.Substring(3, lastWildchar.Length - 3);
                                                        if (!paths[paths.Length - 1].EndsWith(suffix))
                                                        {
                                                            break;
                                                        }
                                                    }

                                                    serviceContext.ServiceName = paths[0];
                                                    // 匹配到了
                                                    for (var h = 0; h < j; h++)
                                                    {
                                                        serviceContext.FirstWildcardMatchPath.Add(paths[h]);
                                                    }

                                                    for (var g = j + fixedKeys.Count; g < paths.Length; g++)
                                                    {
                                                        serviceContext.FirstWildcardMatchPath.Add(paths[g]);
                                                    }

                                                    serviceContext.Route = route;
                                                    serviceContext.UpstreamPath = fullPath;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                for (var j = 1; j < keys.Length; j++)
                                {

                                }
                            }
                        }
                    }
                    else if (paths.Length == keys.Length)
                    {
                        var count = UrlTemplateMatch(keys, paths, (key, path) =>
                        {
                            serviceContext.TemplateParameterName = key;
                            serviceContext.TemplateParameterValue = path;
                        });

                        if (count == keys.Length
                            && route.Downstreams != null
                            && route.Downstreams.Count > 0)
                        {
                            //route.Downstreams.ForEach(x =>
                            //{
                            //    if (x.ServiceName.StartsWith("{") && x.ServiceName.EndsWith("}"))
                            //    {
                            //        x.ServiceName = paths[0];
                            //    }
                            //});
                            serviceContext.Route = route;
                            serviceContext.UpstreamPath = fullPath;
                            break;
                        }
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
                GetServicePolicy(serviceContext, downstreamPaths);
            }
            else
            {
                GetRoutePolicy(serviceContext);
            }

            serviceContext.IsWebSocket = context.WebSockets.IsWebSocketRequest;
            WebSocketAuth(serviceContext);
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

        private void GetServicePolicy(ServiceContext serviceContext, string[] downstreamPaths)
        {
            var config = _configuration.GetSection("Gateway:Policy").Get<PolicyConfig>();
            if (config != null)
            {
                if (config.Policies != null && config.Policies.Count > 0)
                {
                    GetPolicy(serviceContext, downstreamPaths, config);
                }

                serviceContext.Policy ??= config.Global;
            }
        }

        private void GetRoutePolicy(ServiceContext serviceContext)
        {
            var config = _configuration.GetSection("Gateway:Policy").Get<PolicyConfig>();
            serviceContext.DownstreamServices = new();
            if (config != null)
            {
                if (config.Policies != null && config.Policies.Count > 0)
                {
                    DownstreamsForEach(serviceContext, x =>
                    {
                        serviceContext.ServiceName = x.ServiceName;
                        GetPolicy(serviceContext, x.Url.Split('/'), config);
                        serviceContext.Policy ??= config.Global;
                        serviceContext.DownstreamServices.Add(new DownstreamService
                        {
                            Downstream = x,
                            Policy = serviceContext.Policy
                        });
                    });
                    return;
                }

                DownstreamsForEach(serviceContext, x => serviceContext.DownstreamServices.Add(new DownstreamService
                {
                    Downstream = x,
                    Policy = config.Global
                }));
                return;
            }

            DownstreamsForEach(serviceContext, x => serviceContext.DownstreamServices.Add(new DownstreamService
            {
                Downstream = x,
            }));
        }

        private void DownstreamsForEach(ServiceContext serviceContext, Action<Downstream> action)
        {
            serviceContext.Route.Downstreams.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(serviceContext.TemplateParameterName))
                {
                    x.Url = x.Url.Replace(serviceContext.TemplateParameterName, serviceContext.TemplateParameterValue);
                }

                action(x);
            });
        }

        private void GetPolicy(ServiceContext serviceContext, string[] downstreamPaths, PolicyConfig config)
        {
            // 服务方法策略
            Policy methodPolicy = null;
            serviceContext.Policy = config.Policies.Where(p => p.Key == serviceContext.ServiceName).FirstOrDefault();
            if (serviceContext.Policy != null
                && serviceContext.Policy.MethodPolicies != null
                && serviceContext.Policy.MethodPolicies.Count > 0)
            {
                foreach (var p in serviceContext.Policy.MethodPolicies)
                {
                    var keys = p.Key.Split('/');
                    if (keys.Length == downstreamPaths.Length)
                    {
                        var count = UrlTemplateMatch(keys, downstreamPaths);
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

        private int UrlTemplateMatch(string[] keys, string[] paths, Action<string, string> action = null)
        {
            int count = 0;
            for (var i = 0; i < keys.Length; i++)
            {
                if (keys[i].StartsWith('{') && keys[i].EndsWith('}'))
                {
                    count++;
                    action?.Invoke(keys[i], paths[i]);
                    continue;
                }

                if (keys[i].Equals(paths[i], StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                    continue;
                }
            }

            return count;
        }
    }
}
