using Microsoft.Extensions.DependencyInjection;
using Wing.APM.Builder;
using Wing.APM.EFCore;
using Wing.APM.Listeners;

namespace Wing
{
    public static class WingApmBuilderExtensions
    {
        public static WingApmBuilder AddEFCore(this WingApmBuilder wingApmBuilder)
        {
            wingApmBuilder.ServiceBuilder.Services.AddSingleton<IDiagnosticListener, EFCoreDiagnosticListener>();
            return wingApmBuilder;
        }
    }
}
