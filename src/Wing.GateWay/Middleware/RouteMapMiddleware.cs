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
                    var lastWildchar = keys[^1];
                    if (keys.Length > 1 && (keys[0] == "{*}" || lastWildchar.StartsWith("{*}")))
                    {
                        serviceContext.FirstWildcardMatchPath = new();
                        serviceContext.LastWildcardMatchPath = new();
                        var fixedKeys = new List<string>();
                        if (keys[0] == "{*}")
                        {
                            if (lastWildchar.StartsWith("{*}") && keys.Length > 2)
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
                                                        var suffix = lastWildchar[3..];
                                                        if (!paths[paths.Length - 1].EndsWith(suffix))
                                                        {
                                                            break;
                                                        }
                                                    }

                                                    serviceContext.ServiceName = paths[0];
                                                    // 匹配到了
                                                    for (var h = 1; h < j; h++)
                                                    {
                                                        serviceContext.FirstWildcardMatchPath.Add(paths[h]);
                                                    }

                                                    for (var g = j + fixedKeys.Count; g < paths.Length; g++)
                                                    {
                                                        serviceContext.LastWildcardMatchPath.Add(paths[g]);
                                                    }

                                                    serviceContext.Route = route;
                                                    serviceContext.UpstreamPath = fullPath;
                                                    break;
                                                }
                                            }
                                        }

                                        if (serviceContext.Route != null)
                                        {
                                            break;
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 结尾无通配符
                                for (var i = 1; i < keys.Length; i++)
                                {
                                    fixedKeys.Add(keys[i]);
                                }

                                if (fixedKeys.Count > 0)
                                {
                                    if (paths[^1] == fixedKeys.Last() && paths.Length > fixedKeys.Count)
                                    {
                                        int i = paths.Length - fixedKeys.Count;
                                        var isMatch = true;
                                        foreach (var key in fixedKeys)
                                        {
                                            if (key != paths[i])
                                            {
                                                isMatch = false;
                                                break;
                                            }

                                            i++;
                                        }

                                        if (isMatch)
                                        {
                                            serviceContext.ServiceName = paths[0];
                                            for (var j = 1; j < paths.Length - fixedKeys.Count; j++)
                                            {
                                                serviceContext.FirstWildcardMatchPath.Add(paths[j]);
                                            }

                                            serviceContext.Route = route;
                                            serviceContext.UpstreamPath = fullPath;
                                            break;
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        else if (lastWildchar.StartsWith("{*}"))
                        {
                            for (var i = 0; i < keys.Length - 1; i++)
                            {
                                fixedKeys.Add(keys[i]);
                            }

                            if (fixedKeys.Count > 0)
                            {
                                if (paths[0] == fixedKeys.First() && paths.Length > fixedKeys.Count)
                                {
                                    int i = 0;
                                    var isMatch = true;
                                    foreach (var key in fixedKeys)
                                    {
                                        if (key != paths[i])
                                        {
                                            isMatch = false;
                                            break;
                                        }

                                        i++;
                                    }

                                    if (isMatch)
                                    {
                                        if (lastWildchar != "{*}")
                                        {
                                            var suffix = lastWildchar[3..];
                                            if (!paths[^1].EndsWith(suffix))
                                            {
                                                break;
                                            }
                                        }

                                        serviceContext.ServiceName = paths[0];
                                        for (var j = fixedKeys.Count; j < paths.Length; j++)
                                        {
                                            serviceContext.LastWildcardMatchPath.Add(paths[j]);
                                        }

                                        serviceContext.Route = route;
                                        serviceContext.UpstreamPath = fullPath;
                                        break;
                                    }
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
                if (x.ServiceName == "{*}")
                {
                    x.ServiceName = serviceContext.ServiceName;
                }

                if (!string.IsNullOrEmpty(serviceContext.TemplateParameterName))
                {
                    x.Url = x.Url.Replace(serviceContext.TemplateParameterName, serviceContext.TemplateParameterValue);
                }

                if (serviceContext.FirstWildcardMatchPath != null && serviceContext.FirstWildcardMatchPath.Count > 0)
                {
                    var paths = x.Url.Split("/", StringSplitOptions.RemoveEmptyEntries);
                    var wildcardMatchPath = string.Join('/', serviceContext.FirstWildcardMatchPath);
                    if (paths[0] == "{*}")
                    {
                        x.Url = wildcardMatchPath;
                        for (var i = 1; i < paths.Length; i++)
                        {
                            x.Url += "/" + paths[i];
                        }
                    }
                }

                if (serviceContext.LastWildcardMatchPath != null && serviceContext.LastWildcardMatchPath.Count > 0)
                {
                    var paths = x.Url.Split("/", StringSplitOptions.RemoveEmptyEntries);
                    var wildcardMatchPath = string.Join('/', serviceContext.LastWildcardMatchPath);
                    if (paths[^1].StartsWith("{*}"))
                    {
                        x.Url = string.Empty;
                        for (var i = 0; i < paths.Length - 1; i++)
                        {
                            x.Url += paths[i] + "/";
                        }

                        x.Url += wildcardMatchPath;
                    }
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
