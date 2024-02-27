using FreeSql;
using Wing.APM.FreeSql;
using Wing.Configuration.ServiceBuilder;
using Wing.Persistence;

namespace Wing
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddPersistence(this IWingServiceBuilder wingBuilder, string connectionString = "")
        {
            Builder.Add(wingBuilder.Services, (conn, autoSyncStructure) =>
            {
                var fsql = new FreeSqlBuilder()
                           .UseConnectionString(DataType.Sqlite, conn)
                           .UseAutoSyncStructure(autoSyncStructure) // 自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
                           .Build<WingDbFlag>().Map();
                WingDbApmBuilder.AddFreeSql(wingBuilder.Services, fsql);
                return fsql;
            }, connectionString);
            return wingBuilder;
        }
    }
}
