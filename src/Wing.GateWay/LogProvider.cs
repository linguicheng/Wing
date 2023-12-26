using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Wing.Converter;
using Wing.EventBus;
using Wing.Gateway.Config;
using Wing.Persistence.Gateway;
using Wing.Persistence.GateWay;

namespace Wing.Gateway
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
            var config = _configuration.GetSection("Gateway:Log").Get<LogConfig>();
            if (!config.IsEnabled)
            {
                return;
            }

            var httpContext = serviceContext.HttpContext;
            var request = httpContext.Request;
            LogAddDto logDto = new();
            try
            {
                var now = DateTime.Now;
                logDto.Log = new Log
                {
                    Id = Guid.NewGuid().ToString(),
                    ClientIp = Tools.RemoteIp,
                    DownstreamUrl = serviceContext.DownstreamPath,
                    Policy = serviceContext.Policy == null ? string.Empty : _json.Serialize(serviceContext.Policy),
                    RequestTime = serviceContext.RequestTime,
                    RequestMethod = serviceContext.IsWebSocket ? "WebSocket" : request.Method,
                    RequestUrl = request.GetDisplayUrl(),
                    ResponseTime = now,
                    ServiceName = serviceContext.ServiceName,
                    StatusCode = serviceContext.StatusCode,
                    ResponseValue = serviceContext.ResponseValue,
                    GateWayServerIp = App.CurrentServiceUrl,
                    ServiceAddress = serviceContext.ServiceAddress,
                    UsedMillSeconds = Convert.ToInt64((now - serviceContext.RequestTime).TotalMilliseconds),
                    Exception = serviceContext.Exception
                };

                if (request.Headers != null && request.Headers.ContainsKey("AuthKey"))
                {
                    logDto.Log.AuthKey = request.Headers["AuthKey"].ToString();
                }

                if (App.GetService<IAuthenticationService>() != null)
                {
                    try
                    {
                        logDto.Log.Token = await httpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, OpenIdConnectParameterNames.AccessToken);
                    }
                    catch
                    {
                    }
                }

                if (!string.IsNullOrEmpty(serviceContext.RequestValue))
                {
                    logDto.Log.RequestValue = serviceContext.RequestValue;
                }
                else if (request.Body != null)
                {
                    using (var reader = new StreamReader(request.Body))
                    {
                        logDto.Log.RequestValue = await reader.ReadToEndAsync();
                    }
                }

                if (config.UseEventBus)
                {
                    App.GetRequiredService<IEventBus>().Publish(logDto);
                }
                else
                {
                    DataProvider.Data.Enqueue(logDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发生异常,请求日志：{0}", _json.Serialize(logDto));
            }
        }

        public async Task Add(LogAddDto logDto, HttpContext httpContext)
        {
            var config = _configuration.GetSection("Gateway:Log").Get<LogConfig>();
            if (!config.IsEnabled)
            {
                return;
            }

            var request = httpContext.Request;
            try
            {
                if (request.Headers != null && request.Headers.ContainsKey("AuthKey"))
                {
                    logDto.Log.AuthKey = request.Headers["AuthKey"].ToString();
                }

                if (App.GetService<IAuthenticationService>() != null)
                {
                    try
                    {
                        logDto.Log.Token = await httpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, OpenIdConnectParameterNames.AccessToken);
                    }
                    catch
                    {
                    }
                }

                if (config.UseEventBus)
                {
                    App.GetRequiredService<IEventBus>().Publish(logDto);
                }
                else
                {
                    DataProvider.Data.Enqueue(logDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发生异常,请求日志：{0}", _json.Serialize(logDto));
            }
        }
    }
}
