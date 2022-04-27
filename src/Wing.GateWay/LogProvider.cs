using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Wing.Converter;
using Wing.EventBus;
using Wing.GateWay.Config;
using Wing.Persistence.GateWay;

namespace Wing.GateWay
{
    public class LogProvider : ILogProvider
    {
        private readonly IJson _json;
        private readonly ILogger<LogProvider> _logger;
        private readonly IConfiguration _configuration;

        public LogProvider(IJson json,
            ILogger<LogProvider> logger,
            IConfiguration configuration)
        {
            _json = json;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Add(ServiceContext serviceContext)
        {
            var config = _configuration.GetSection("GateWay:Log").Get<LogConfig>();
            if (!config.IsEnabled)
            {
                return;
            }

            var httpContext = serviceContext.HttpContext;
            var request = httpContext.Request;
            Log log = null;
            try
            {
                var now = DateTime.Now;
                log = new Log
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ClientIp = httpContext.Connection.RemoteIpAddress.ToString(),
                    DownstreamUrl = serviceContext.DownstreamPath,
                    Policy = _json.Serialize(serviceContext.Policy),
                    RequestTime = serviceContext.RequestTime,
                    RequestMethod = request.Method,
                    RequestUrl = request.GetDisplayUrl(),
                    ResponseTime = now,
                    ServiceName = serviceContext.ServiceName,
                    StatusCode = serviceContext.StatusCode,
                    ResponseValue = serviceContext.ResponseValue,
                    GateWayServerIp = httpContext.Connection.LocalIpAddress.ToString(),
                    ServiceAddress = serviceContext.ServiceAddress,
                    UsedMillSeconds = Convert.ToInt64((now - serviceContext.RequestTime).TotalMilliseconds)
                };

                if (request.Headers != null && request.Headers.ContainsKey("AuthKey"))
                {
                    log.AuthKey = request.Headers["AuthKey"].ToString();
                }

                log.Token = await httpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, OpenIdConnectParameterNames.AccessToken);
                if (request.Body != null)
                {
                    request.EnableBuffering();
                    request.Body.Position = 0;
                    using var reader = new StreamReader(request.Body);
                    log.RequestValue = await reader.ReadToEndAsync();
                }

                if (config.UseEventBus)
                {
                    ServiceLocator.GetRequiredService<IEventBus>().Publish(log);
                }
                else
                {
                    var result = await ServiceLocator.GetRequiredService<ILogService>().Add(log);
                    if (result <= 0)
                    {
                        _logger.LogInformation($"数据库保存失败，请求日志：{_json.Serialize(log)}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发生异常,请求日志：{0}", log != null ? _json.Serialize(log) : string.Empty);
            }
        }
    }
}
