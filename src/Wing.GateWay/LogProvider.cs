using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Wing.Convert;
using Wing.Persistence.GateWay;

namespace Wing.GateWay
{
    public class LogProvider : ILogProvider
    {
        private readonly ILogService _logService;
        private readonly IJson _json;

        public LogProvider(ILogService logService, IJson json)
        {
            _logService = logService;
            _json = json;
        }

        public async Task Add(ServiceContext serviceContext)
        {
            var httpContext = serviceContext.HttpContext;
            var request = httpContext.Request;
            var response = httpContext.Response;
            var log = new Log
            {
                Id = Guid.NewGuid().ToString("N"),
                ClientIp = httpContext.Connection.RemoteIpAddress.ToString(),
                DownstreamUrl = serviceContext.DownstreamPath,
                Policy = _json.Serialize(serviceContext.Policy),
                RequestTime = serviceContext.RequestTime,
                RequestMethod = request.Method,
                RequestUrl = request.Path.ToString(),
                ResponseTime = serviceContext.ResponseTime,
                ServiceName = serviceContext.ServiceName,
                StatusCode = response.StatusCode
            };
            if (request.Headers != null && request.Headers.ContainsKey("AuthKey"))
            {
                log.AuthKey = request.Headers["AuthKey"].ToString();
            }

            log.Token = await httpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, OpenIdConnectParameterNames.AccessToken);
            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body);
            log.RequestValue = await reader.ReadToEndAsync();
            response.Body.Position = 0;
            using var responseReader = new StreamReader(response.Body);
            log.ResponseValue = await responseReader.ReadToEndAsync();
            response.Body.Position = 0;
            await _logService.Add(log);
        }
    }
}
