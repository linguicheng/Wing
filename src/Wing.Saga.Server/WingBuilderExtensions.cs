using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wing.Configuration.ServiceBuilder;
using Wing.Converter;
using Wing.Saga.Server;
using Wing.ServiceProvider;

namespace Wing
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddSaga(this IWingServiceBuilder wingBuilder, Func<IServiceProvider, SagaOptions> sagaOptions = null)
        {
            var tranRetryService = new TranRetryHostedService(App.GetRequiredService<ILogger<TranRetryHostedService>>(),
                App.GetRequiredService<IServiceFactory>(),
                App.GetRequiredService<IHttpClientFactory>(),
                App.GetRequiredService<IJson>(),
                sagaOptions?.Invoke(wingBuilder.Services.BuildServiceProvider()));

            wingBuilder.Services.AddSingleton(typeof(IHostedService), tranRetryService);
            return wingBuilder;
        }
    }
}
