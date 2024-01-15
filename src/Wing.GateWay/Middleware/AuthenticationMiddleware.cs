using System.Net;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Wing.Gateway.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly ServiceRequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;
        private readonly ILogProvider _logProvider;

        public AuthenticationMiddleware(ServiceRequestDelegate next,
            ILogger<AuthenticationMiddleware> logger,
            ILogProvider logProvider)
        {
            _next = next;
            _logger = logger;
            _logProvider = logProvider;
        }

        public async Task InvokeAsync(ServiceContext serviceContext)
        {
            string authKey;
            bool useJWTAuth;

            if (serviceContext.Route == null)
            {
                if (serviceContext.Policy is null)
                {
                    await _next(serviceContext);
                    return;
                }

                authKey = serviceContext.Policy.AuthKey;
                useJWTAuth = serviceContext.Policy.UseJWTAuth.GetValueOrDefault();
            }
            else
            {
                authKey = serviceContext.Route.AuthKey;
                useJWTAuth = serviceContext.Route.UseJWTAuth.GetValueOrDefault();
            }

            var context = serviceContext.HttpContext;
            if (!string.IsNullOrWhiteSpace(authKey))
            {
                _logger.LogInformation($"请求路由：{context.Request.Path}，开始AuthKey权限认证");
                if (context.Request.Headers == null ||
                    !context.Request.Headers.ContainsKey("AuthKey") ||
                    context.Request.Headers["AuthKey"].ToString() != authKey)
                {
                    _logger.LogInformation($"请求路由：{context.Request.Path}，AuthKey权限认证不通过");
                    await Unauthorized(serviceContext);
                    return;
                }

                _logger.LogInformation($"请求路由：{context.Request.Path}，AuthKey权限认证通过");
            }

            if (useJWTAuth)
            {
                _logger.LogInformation($"请求路由：{context.Request.Path}，开始JWT权限认证");
                var result = await context.AuthenticateAsync();
                if (!result.Succeeded)
                {
                    _logger.LogInformation($"请求路由：{context.Request.Path}，JWT权限认证不通过");
                    await Unauthorized(serviceContext);
                    return;
                }

                context.User = result.Principal;
                if (!context.User.Identity.IsAuthenticated)
                {
                    _logger.LogInformation($"请求路由：{context.Request.Path}，JWT权限认证不通过");
                    await Unauthorized(serviceContext);
                    return;
                }

                _logger.LogInformation($"请求路由：{context.Request.Path}，JWT权限认证通过");

                if (serviceContext.Authorization != null)
                {
                    var downStreamUrl = new List<string>();
                    if (serviceContext.Route == null)
                    {
                        downStreamUrl.Add(serviceContext.DownstreamPath);
                    }
                    else
                    {
                        foreach (var item in serviceContext.DownstreamServices)
                        {
                            downStreamUrl.Add(item.Downstream.Url);
                        }
                    }

                    var authResult = await serviceContext.Authorization.Invoke(downStreamUrl, serviceContext.HttpContext);
                    if (!authResult)
                    {
                        _logger.LogInformation($"请求路由：{context.Request.Path}，JWT策略鉴权不通过");
                        await Forbidden(serviceContext);
                        return;
                    }

                    _logger.LogInformation($"请求路由：{context.Request.Path}，JWT策略鉴权通过");
                }
            }

            await _next(serviceContext);
        }

        private async Task Unauthorized(ServiceContext serviceContext)
        {
            var context = serviceContext.HttpContext;
            if (serviceContext.IsWebSocket)
            {
                WebSocketCloseStatus status = WebSocketCloseStatus.PolicyViolation;
                serviceContext.StatusCode = (int)status;
                serviceContext.Exception = "权限认证不通过";
                await _logProvider.Add(serviceContext);
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await webSocket.CloseAsync(status, null, CancellationToken.None);
                return;
            }

            serviceContext.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.StatusCode = serviceContext.StatusCode;
            await _logProvider.Add(serviceContext);
        }

        private async Task Forbidden(ServiceContext serviceContext)
        {
            var context = serviceContext.HttpContext;
            if (serviceContext.IsWebSocket)
            {
                WebSocketCloseStatus status = WebSocketCloseStatus.PolicyViolation;
                serviceContext.StatusCode = (int)status;
                serviceContext.Exception = "权限认证不通过";
                await _logProvider.Add(serviceContext);
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await webSocket.CloseAsync(status, null, CancellationToken.None);
                return;
            }

            serviceContext.StatusCode = (int)HttpStatusCode.Forbidden;
            context.Response.StatusCode = serviceContext.StatusCode;
            await _logProvider.Add(serviceContext);
        }
    }
}
