using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Polly;
using Polly.Timeout;
using Wing.Exceptions;
using Wing.Logger;
using Wing.ServiceProvider;

namespace Wing.GateWay.Middleware
{
    public class PolicyMiddleware
    {
        private static readonly ConcurrentDictionary<string, KeyValuePair<Policy, AsyncPolicy<HttpResponseMessage>>> Policies = new ConcurrentDictionary<string, KeyValuePair<Policy, AsyncPolicy<HttpResponseMessage>>>();
        private readonly ServiceRequestDelegate _next;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IServiceFactory _serviceFactory;
        private readonly IWingLogger<PolicyMiddleware> _logger;

        public PolicyMiddleware(ServiceRequestDelegate next, IHttpClientFactory clientFactory, IServiceFactory serviceFactory, IWingLogger<PolicyMiddleware> logger)
        {
            _next = next;
            _clientFactory = clientFactory;
            _serviceFactory = serviceFactory;
            _logger = logger;
        }

        public async Task InvokeAsync(ServiceContext serviceContext)
        {
            if (string.IsNullOrWhiteSpace(serviceContext.ServiceName))
            {
                await _next(serviceContext);
                return;
            }

            await InvokeWithPolicy(serviceContext.ServiceName, serviceContext.HttpContext, serviceContext.DownstreamPath, serviceContext.Policy);
        }

        private bool PolicyConfigIsChange(Policy oldConfig, Policy config)
        {
            return oldConfig == null ||
                oldConfig.IsEnableBreaker != config.IsEnableBreaker ||
                oldConfig.ExceptionsAllowedBeforeBreaking != config.ExceptionsAllowedBeforeBreaking ||
                oldConfig.MillisecondsOfBreak != config.MillisecondsOfBreak ||
                oldConfig.TimeOutMilliseconds != config.TimeOutMilliseconds ||
                oldConfig.MaxRetryTimes != config.MaxRetryTimes ||
                oldConfig.RetryIntervalMilliseconds != config.RetryIntervalMilliseconds;
        }

        private async Task InvokeWithPolicy(string serviceName, HttpContext context, string path, Policy config)
        {
            Policies.TryGetValue(serviceName, out KeyValuePair<Policy, AsyncPolicy<HttpResponseMessage>> policyPair);
            var policy = policyPair.Value;
            if (policy == null || PolicyConfigIsChange(policyPair.Key, config))
            {
                policy = Polly.Policy.NoOpAsync<HttpResponseMessage>();
                if (config.TimeOutMilliseconds > 0)
                {
                    policy = Policy<HttpResponseMessage>
                   .Handle<TimeoutRejectedException>()
                   .Or<TimeoutException>()
                   .FallbackAsync(t =>
                   {
                       return Task.FromResult(new HttpResponseMessage(HttpStatusCode.GatewayTimeout));
                   }).WrapAsync<HttpResponseMessage>(Polly.Policy.TimeoutAsync<HttpResponseMessage>(() => TimeSpan.FromMilliseconds(config.TimeOutMilliseconds), TimeoutStrategy.Pessimistic));
                }

                if (config.IsEnableBreaker)
                {
                    policy = policy.WrapAsync<HttpResponseMessage>(Polly.Policy.Handle<ServiceNotFoundException>().Or<Exception>().CircuitBreakerAsync(config.ExceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(config.MillisecondsOfBreak)));
                }

                if (config.MaxRetryTimes > 0)
                {
                    policy = policy.WrapAsync<HttpResponseMessage>(Polly.Policy.Handle<ServiceNotFoundException>().Or<Exception>().WaitAndRetryAsync(config.MaxRetryTimes, i => TimeSpan.FromMilliseconds(config.RetryIntervalMilliseconds)));
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
                        _logger.Error(d.Exception, "触发异常降级");
                        return Task.CompletedTask;
                    });

                var serviceNotFoundFallBack = Policy<HttpResponseMessage>
                    .Handle<ServiceNotFoundException>()
                    .FallbackAsync(t =>
                    {
                        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
                    });

                policy = unknownErrorFallBack.WrapAsync(serviceNotFoundFallBack).WrapAsync(policy);
                Policies.AddOrUpdate(serviceName, KeyValuePair.Create(config, policy), (key, value) =>
                {
                    return KeyValuePair.Create(config, policy);
                });
            }

            var resMsg = await policy.ExecuteAsync(async () =>
            {
                return await _serviceFactory.HttpServiceInvoke(serviceName, async serviceAddr =>
                {
                    var reqMsg = context.Request.ToHttpRequestMessage(serviceAddr, path);
                    var client = _clientFactory.CreateClient(serviceName);
                    return await client.SendAsync(reqMsg);
                });
            });
            await context.Response.FromHttpResponseMessage(resMsg);
        }
    }
}
