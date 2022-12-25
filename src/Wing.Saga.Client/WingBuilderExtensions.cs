using Microsoft.Extensions.DependencyInjection;
using Wing.Configuration.ServiceBuilder;
using Wing.Saga.Client;

namespace Wing
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddSaga(this IWingServiceBuilder wingBuilder)
        {
            wingBuilder.Services.AddSingleton<ITranRetryService, TranRetryService>();
            return wingBuilder;
        }
    }
}
