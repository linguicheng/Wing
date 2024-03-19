using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Wing.UI.Auth
{
    public class JwtAuthHandler : AuthorizationHandler<JwtAuthRequirement>
    {
        private readonly IHttpContextAccessor _accessor;

        public JwtAuthHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtAuthRequirement requirement)
        {
            var httpContext = _accessor.HttpContext;
            var result = await httpContext.AuthenticateAsync();
            if (result.Succeeded)
            {
                httpContext.User = result.Principal;
                if (httpContext.User.Identity.IsAuthenticated && requirement.ValidatePermission(httpContext))
                {
                    context.Succeed(requirement);
                    return;
                }
            }

            context.Fail();
        }
    }
}
