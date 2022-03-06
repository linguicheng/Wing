using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wing.Configuration.ServiceBuilder
{
    public interface IWingServiceBuilder
    {
        IServiceCollection Services { get; }

        IConfiguration Configuration { get; }

        Action<IApplicationBuilder> App { get; set; }

        T GetConfig<T>(string key);
    }
}
