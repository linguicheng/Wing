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
            if (serviceContext.Policy is null)
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
                    await Unauthorized(serviceContext);
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
            }

            await _next(serviceContext);
        }

        private async Task Unauthorized(ServiceContext serviceContext)
        {
            serviceContext.StatusCode = (int)HttpStatusCode.Unauthorized;
            serviceContext.HttpContext.Response.StatusCode = serviceContext.StatusCode;
            await _logProvider.Add(serviceContext);
        }
    }
}
