using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Wing.UI.Auth
{
    public class JwtAuthRequirement : IAuthorizationRequirement
    {
        public Func<HttpContext, bool> ValidatePermission { get; set; }
    }
}
