using Microsoft.Extensions.DependencyInjection;

namespace Wing.APM
{
    public class WingApmBuilder
    {
        public IServiceCollection Services { get; }

        public WingApmBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
