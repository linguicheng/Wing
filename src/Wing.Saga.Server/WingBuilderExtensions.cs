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
        public static IWingServiceBuilder AddSaga(this IWingServiceBuilder wingBuilder, SagaOptions sagaOptions = null)
        {
            var tranRetryService = new TranRetryHostedService(App.GetRequiredService<ILogger<TranRetryHostedService>>(),
                App.GetRequiredService<IServiceFactory>(),
                App.GetRequiredService<IHttpClientFactory>(),
                App.GetRequiredService<IJson>(),
                sagaOptions);

            wingBuilder.Services.AddSingleton(typeof(IHostedService), tranRetryService);
            wingBuilder.Services.AddSingleton<IHostedService, TranReportHostedService>();
            return wingBuilder;
        }
    }
}
