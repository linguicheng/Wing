using System;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using Wing.Configuration.ServiceBuilder;
using Wing.Persistence;
using Wing.Persistence.Apm;
using Wing.Persistence.Gateway;
using Wing.Persistence.Saga;

namespace Wing
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddPersistence(this IWingServiceBuilder wingBuilder, string connectionString = "")
        {
            connectionString = string.IsNullOrWhiteSpace(connectionString) ? App.Configuration["ConnectionStrings:Wing"] : connectionString;
            var autoSyncStructure = !string.IsNullOrWhiteSpace(App.Configuration["UseAutoSyncStructure"]) && Convert.ToBoolean(App.Configuration["UseAutoSyncStructure"]);
            var fsql = new FreeSqlBuilder()
                              .UseConnectionString(DataType.SqlServer, connectionString)
                              .UseAutoSyncStructure(autoSyncStructure) // 自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
                              .Build<WingDbFlag>().Map();
            WingDbApmBuilder.AddFreeSql(wingBuilder.Services, fsql);
            wingBuilder.Services.AddSingleton(typeof(IFreeSql<WingDbFlag>), serviceProvider => fsql);
            wingBuilder.Services.AddSingleton<ILogService, LogService>();
            wingBuilder.Services.AddSingleton<ITracerService, TracerService>();
            wingBuilder.Services.AddSingleton<ITracerWorkService, TracerWorkService>();
            wingBuilder.Services.AddScoped<ISagaTranService, SagaTranService>();
            wingBuilder.Services.AddScoped<ISagaTranUnitService, SagaTranUnitService>();
            wingBuilder.Services.AddFreeRepository();
            return wingBuilder;
        }
    }
}
