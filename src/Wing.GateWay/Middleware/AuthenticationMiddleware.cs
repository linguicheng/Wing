using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Wing.GateWay.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly ServiceRequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;

        public AuthenticationMiddleware(ServiceRequestDelegate next, ILogger<AuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(ServiceContext serviceContext)
        {
            if (serviceContext.Policy == null)
            {
                await _next(serviceContext);
                return;
            }

            var context = serviceContext.HttpContext;
            if (!string.IsNullOrWhiteSpace(serviceContext.Policy.AuthKey))
            {
                _logger.LogInformation($"请求路由：{context.Request.Path}，开始AuthKey权限认证");
                if (context.Request.Headers == null ||
                    !context.Request.Headers.ContainsKey("AuthKey") ||
                    context.Request.Headers["AuthKey"].ToString() != serviceContext.Policy.AuthKey)
                {
                    _logger.LogInformation($"请求路由：{context.Request.Path}，AuthKey权限认证不通过");
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }

                _logger.LogInformation($"请求路由：{context.Request.Path}，AuthKey权限认证通过");
            }

            if (serviceContext.Policy.UseJWTAuth.GetValueOrDefault())
            {
                _logger.LogInformation($"请求路由：{context.Request.Path}，开始JWT权限认证");
                var result = await context.AuthenticateAsync();
                if (!result.Succeeded)
                {
                    _logger.LogInformation($"请求路由：{context.Request.Path}，JWT权限认证不通过");
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }

                context.User = result.Principal;
                if (!context.User.Identity.IsAuthenticated)
                {
                    _logger.LogInformation($"请求路由：{context.Request.Path}，JWT权限认证不通过");
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }

                _logger.LogInformation($"请求路由：{context.Request.Path}，JWT权限认证通过");
            }

            await _next(serviceContext);
        }
    }
}
