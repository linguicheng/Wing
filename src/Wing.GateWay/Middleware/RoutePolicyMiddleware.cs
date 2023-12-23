using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Bulkhead;
using Polly.RateLimit;
using Polly.Timeout;
using Wing.Converter;
using Wing.Exceptions;
using Wing.Persistence.Gateway;
using Wing.Persistence.GateWay;
using Wing.ServiceProvider;

namespace Wing.Gateway.Middleware
{
    public class RoutePolicyMiddleware
    {
        private static readonly ConcurrentDictionary<string, KeyValuePair<Config.Policy, AsyncPolicy<HttpResponseMessage>>> Policies = new();
        private readonly ServiceRequestDelegate _next;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IServiceFactory _serviceFactory;
        private readonly ILogger<RoutePolicyMiddleware> _logger;
        private readonly ILogProvider _logProvider;
        private readonly IJson _json;

        public RoutePolicyMiddleware(ServiceRequestDelegate next,
            IHttpClientFactory clientFactory,
            IServiceFactory serviceFactory,
            ILogger<RoutePolicyMiddleware> logger,
            ILogProvider logProvider,
            IJson json)
        {
            _next = next;
            _clientFactory = clientFactory;
            _serviceFactory = serviceFactory;
            _logger = logger;
            _logProvider = logProvider;
            _json = json;
        }

        public async Task InvokeAsync(ServiceContext serviceContext)
        {
            if (serviceContext.Route == null || serviceContext.IsWebSocket)
            {
                await _next(serviceContext);
                return;
            }

            HttpResponseMessage downstreamResponse;
            var context = serviceContext.HttpContext;
            var request = context.Request;
            if (serviceContext.DownstreamServices.Count == 1)
            {
                try
                {
                    downstreamResponse = await InvokeDownstreamService(serviceContext, serviceContext.DownstreamServices.First());
                    await context.Response.FromHttpResponseMessage(downstreamResponse, (statusCode, content) =>
                    {
                        serviceContext.StatusCode = statusCode;
                        serviceContext.ResponseValue = content;
                        _logProvider.Add(serviceContext);
                    });
                }
                catch (ServiceNotFoundException ex)
                {
                    serviceContext.StatusCode = (int)HttpStatusCode.NotFound;
                    context.Response.StatusCode = serviceContext.StatusCode;
                    serviceContext.Exception = $"{ex.Message} {ex.StackTrace}";
                    await _logProvider.Add(serviceContext);
                }
                catch (Exception ex)
                {
                    serviceContext.StatusCode = (int)HttpStatusCode.BadGateway;
                    serviceContext.Exception = $"{ex.Message} {ex.StackTrace}";
                    context.Response.StatusCode = serviceContext.StatusCode;

                    await _logProvider.Add(serviceContext);
                }

                return;
            }

            var result = "{";
            LogAddDto logDto = new()
            {
                Log = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    ClientIp = Tools.RemoteIp,
                    RequestTime = serviceContext.RequestTime,
                    RequestMethod = request.Method,
                    RequestUrl = request.GetDisplayUrl(),
                    GateWayServerIp = App.CurrentServiceUrl
                },
                LogDetails = []
            };

            if (request.Body != null)
            {
                using (var reader = new StreamReader(request.Body))
                {
                    logDto.Log.RequestValue = await reader.ReadToEndAsync();
                }
            }

            foreach (var downstreamService in serviceContext.DownstreamServices)
            {
                var logDetail = new LogDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    Policy = downstreamService.Policy == null ? string.Empty : _json.Serialize(downstreamService.Policy),
                    Key = downstreamService.Downstream.Key,
                    LogId = logDto.Log.Id,
                    RequestMethod = downstreamService.Downstream.Method,
                    RequestTime = DateTime.Now,
                    RequestUrl = downstreamService.Downstream.Url,
                    ServiceName = downstreamService.Downstream.ServiceName
                };
                try
                {
                    downstreamResponse = await InvokeDownstreamService(serviceContext, downstreamService);
                    logDetail.StatusCode = (int)downstreamResponse.StatusCode;
                    string content = string.Empty;
                    if (downstreamResponse.Content != null)
                    {
                        if (downstreamResponse.Content.Headers.Contains("Content-Type"))
                        {
                            context.Response.ContentType = downstreamResponse.Content.Headers.GetValues("Content-Type").Single();
                        }

                        content = await downstreamResponse.Content.ReadAsStringAsync();
                    }

                    logDetail.ResponseTime = DateTime.Now;
                    logDetail.ResponseValue = content;
                    logDetail.UsedMillSeconds = Convert.ToInt64((logDetail.ResponseTime - logDetail.RequestTime).TotalMilliseconds);
                    logDetail.ServiceAddress = serviceContext.ServiceAddress;
                    content = "\"" + logDetail.Key + "\":" + content + ",";
                    result += content;
                }
                catch (ServiceNotFoundException ex)
                {
                    logDetail.StatusCode = (int)HttpStatusCode.NotFound;
                    logDetail.Exception = $"{ex.Message} {ex.StackTrace}";
                }
                catch (Exception ex)
                {
                    logDetail.StatusCode = (int)HttpStatusCode.BadGateway;
                    logDetail.Exception = $"{ex.Message} {ex.StackTrace}";
                }
                finally
                {
                    logDto.LogDetails.Add(logDetail);
                }
            }

            result = result.TrimEnd(',');
            result += "}";
            logDto.Log.ResponseValue = result;
            logDto.Log.ResponseTime = DateTime.Now;
            logDto.Log.UsedMillSeconds = Convert.ToInt64((logDto.Log.ResponseTime - logDto.Log.RequestTime).TotalMilliseconds);
            logDto.Log.StatusCode = (int)HttpStatusCode.OK;
            await _logProvider.Add(logDto, context);
            await context.Response.WriteAsync(result);
        }

        private async Task<HttpResponseMessage> InvokeDownstreamService(ServiceContext serviceContext, DownstreamService downstreamService)
        {
            serviceContext.ServiceName = downstreamService.Downstream.ServiceName;
            serviceContext.DownstreamPath = downstreamService.Downstream.Url;
            serviceContext.Method = downstreamService.Downstream.Method;
            serviceContext.Policy = downstreamService.Policy;
            HttpResponseMessage downstreamResponse;
            if (serviceContext.Policy != null &&
                  ((serviceContext.Policy.Breaker != null && serviceContext.Policy.Breaker.IsEnabled)
                  || (serviceContext.Policy.RateLimit != null && serviceContext.Policy.RateLimit.IsEnabled)
                  || (serviceContext.Policy.BulkHead != null && serviceContext.Policy.BulkHead.IsEnabled)
                  || (serviceContext.Policy.Retry != null && serviceContext.Policy.Retry.IsEnabled)
                  || (serviceContext.Policy.TimeOut != null && serviceContext.Policy.TimeOut.IsEnabled)))
            {
                downstreamResponse = await InvokeWithPolicy(serviceContext);
            }
            else
            {
                downstreamResponse = await InvokeWithNoPolicy(serviceContext);
            }

            return downstreamResponse;
        }

        private async Task<HttpResponseMessage> InvokeWithNoPolicy(ServiceContext serviceContext)
        {
            var context = serviceContext.HttpContext;
            return await _serviceFactory.InvokeAsync(serviceContext.ServiceName, async serviceAddr =>
            {
                serviceContext.ServiceAddress = serviceAddr.ToString();
                var reqMsg = await context.Request.ToHttpRequestMessage(serviceAddr, serviceContext.DownstreamPath, serviceContext.Method);
                var client = _clientFactory.CreateClient(serviceContext.ServiceName);
                return await client.SendAsync(reqMsg);
            });
        }

        private async Task<HttpResponseMessage> InvokeWithPolicy(ServiceContext serviceContext)
        {
            var config = serviceContext.Policy;
            Policies.TryGetValue(serviceContext.ServiceName, out KeyValuePair<Config.Policy, AsyncPolicy<HttpResponseMessage>> policyPair);
            var policy = policyPair.Value;
            if (policy == null || policyPair.Key != config)
            {
                policy = Policy.NoOpAsync<HttpResponseMessage>();

                if (config.Breaker != null && config.Breaker.IsEnabled)
                {
                    policy = policy.WrapAsync<HttpResponseMessage>(Policy.Handle<ServiceNotFoundException>()
                        .Or<Exception>()
                        .CircuitBreakerAsync(config.Breaker.ExceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(config.Breaker.MillisecondsOfBreak)));
                }

                if (config.Retry != null && config.Retry.IsEnabled)
                {
                    policy = policy.WrapAsync<HttpResponseMessage>(Policy.Handle<ServiceNotFoundException>()
                        .Or<Exception>()
                        .WaitAndRetryAsync(config.Retry.MaxTimes, i => TimeSpan.FromMilliseconds(config.Retry.IntervalMilliseconds)));
                }

                if (config.BulkHead != null && config.BulkHead.IsEnabled)
                {
                    policy = policy.WrapAsync<HttpResponseMessage>(Policy.BulkheadAsync<HttpResponseMessage>(config.BulkHead.MaxParallelization, config.BulkHead.MaxQueuingActions));
                }

                if (config.RateLimit != null && config.RateLimit.IsEnabled)
                {
                    policy = policy.WrapAsync<HttpResponseMessage>(Policy.RateLimitAsync<HttpResponseMessage>(config.RateLimit.NumberOfExecutions, TimeSpan.FromSeconds(config.RateLimit.PerSeconds), config.RateLimit.MaxBurst));
                }

                if (config.TimeOut != null && config.TimeOut.IsEnabled)
                {
                    policy = policy.WrapAsync<HttpResponseMessage>(Policy.TimeoutAsync<HttpResponseMessage>(() => TimeSpan.FromMilliseconds(config.TimeOut.TimeOutMilliseconds), TimeoutStrategy.Pessimistic));
                }

                var unknownErrorFallBack = Policy<HttpResponseMessage>
                .Handle<Exception>()
                .FallbackAsync((d, cxt, t) =>
                {
                    if (d.Exception.InnerException is ServiceNotFoundException)
                    {
                        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
                    }

                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadGateway));
                }, (d, cxt) =>
                {
                    _logger.LogError(d.Exception, "触发异常降级");
                    return Task.CompletedTask;
                });

                var serviceNotFoundFallBack = Policy<HttpResponseMessage>
                    .Handle<ServiceNotFoundException>()
                    .FallbackAsync(t =>
                    {
                        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
                    });

                var timeoutFallBack = Policy<HttpResponseMessage>
                   .Handle<TimeoutRejectedException>()
                   .Or<TimeoutException>()
                   .FallbackAsync(t =>
                   {
                       _logger.LogError("触发超时异常降级");
                       return Task.FromResult(new HttpResponseMessage(HttpStatusCode.GatewayTimeout));
                   });

                var rateLimitFallBack = Policy<HttpResponseMessage>
                   .Handle<RateLimitRejectedException>()
                   .FallbackAsync(t =>
                   {
                       _logger.LogError("触发限流异常降级");
                       return Task.FromResult(new HttpResponseMessage(HttpStatusCode.TooManyRequests));
                   });

                var bulkHeadFallBack = Policy<HttpResponseMessage>
                   .Handle<BulkheadRejectedException>()
                   .FallbackAsync(t =>
                   {
                       _logger.LogError("触发舱壁异常降级");
                       return Task.FromResult(new HttpResponseMessage(HttpStatusCode.TooManyRequests));
                   });

                policy = unknownErrorFallBack.WrapAsync(serviceNotFoundFallBack)
                    .WrapAsync(timeoutFallBack)
                    .WrapAsync(rateLimitFallBack)
                    .WrapAsync(bulkHeadFallBack)
                    .WrapAsync(policy);
                Policies.AddOrUpdate(serviceContext.ServiceName, KeyValuePair.Create(config, policy), (key, value) =>
                {
                    return KeyValuePair.Create(config, policy);
                });
            }

            return await policy.ExecuteAsync(async () =>
            {
                return await _serviceFactory.InvokeAsync(serviceContext.ServiceName, async serviceAddr =>
                {
                    serviceContext.ServiceAddress = serviceAddr.ToString();
                    var reqMsg = await serviceContext.HttpContext.Request.ToHttpRequestMessage(serviceAddr, serviceContext.DownstreamPath);
                    var client = _clientFactory.CreateClient(serviceContext.ServiceName);
                    return await client.SendAsync(reqMsg);
                });
            });
        }
    }
}
