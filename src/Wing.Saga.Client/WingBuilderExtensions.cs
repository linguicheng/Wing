using Microsoft.Extensions.DependencyInjection;
using Wing.Configuration.ServiceBuilder;
using Wing.Saga.Client;
using Wing.Saga.Client.Persistence;

namespace Wing
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddSaga(this IWingServiceBuilder wingBuilder)
        {
            wingBuilder.Services.AddScoped<ITranRetryService, TranRetryService>();
            wingBuilder.Services.AddScoped<ISagaTranAppService, SagaTranAppService>();
            wingBuilder.Services.AddScoped<ISagaTranUnitAppService, SagaTranUnitAppService>();
            return wingBuilder;
        }
    }
}
