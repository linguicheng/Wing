using System;
using Microsoft.Extensions.DependencyInjection;
using Wing.Persistence.Apm;
using Wing.Persistence.Gateway;
using Wing.Persistence.Saga;
using Wing.Persistence.User;

namespace Wing.Persistence
{
    public class Builder
    {
        public static void Add(IServiceCollection services, Func<string, bool, IFreeSql<WingDbFlag>> addFreeSql, string connectionString = "")
        {
            connectionString = string.IsNullOrWhiteSpace(connectionString) ? App.Configuration["ConnectionStrings:Wing"] : connectionString;
            var autoSyncStructure = !string.IsNullOrWhiteSpace(App.Configuration["UseAutoSyncStructure"]) && Convert.ToBoolean(App.Configuration["UseAutoSyncStructure"]);
            var fsql = addFreeSql(connectionString, autoSyncStructure);
            services.AddSingleton(typeof(IFreeSql<WingDbFlag>), serviceProvider => fsql);
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<ITracerService, TracerService>();
            services.AddSingleton<ITracerWorkService, TracerWorkService>();
            services.AddScoped<ISagaTranService, SagaTranService>();
            services.AddScoped<ISagaTranUnitService, SagaTranUnitService>();
            services.AddScoped<IUserService, UserService>();
            services.AddFreeRepository();
        }
    }
}
