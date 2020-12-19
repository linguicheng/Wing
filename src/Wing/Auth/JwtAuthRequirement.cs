using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Wing.Auth
{
    public class JwtAuthRequirement : IAuthorizationRequirement
    {
        public Func<HttpContext, bool> ValidatePermission { get; set; }
    }
}
