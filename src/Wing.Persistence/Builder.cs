using System;
using Microsoft.Extensions.DependencyInjection;
using Wing.Persistence.Apm;
using Wing.Persistence.Gateway;
using Wing.Persistence.Saga;

namespace Wing.Persistence
{
    public class Builder
    {
        public static void Add(IServiceCollection services, Func<string, bool, IFreeSql<WingDbFlag>> addFreeSql, string connectionString = "")
        {
            connectionString = string.IsNullOrWhiteSpace(connectionString) ? App.Configuration["ConnectionStrings:Wing"] : connectionString;
            var autoSyncStructure = !string.IsNullOrWhiteSpace(App.Configuration["UseAutoSyncStructure"]) && Convert.ToBoolean(App.Configuration["UseAutoSyncStructure"]);
            services.AddSingleton(typeof(IFreeSql<WingDbFlag>), serviceProvider => addFreeSql(connectionString, autoSyncStructure));
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<ITracerService, TracerService>();
            services.AddSingleton<ITracerWorkService, TracerWorkService>();
            services.AddScoped<ISagaTranService, SagaTranService>();
            services.AddScoped<ISagaTranUnitService, SagaTranUnitService>();
            services.AddFreeRepository();
        }
    }
}
