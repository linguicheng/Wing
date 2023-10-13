using Wing.Persistence.Apm;
using Wing.Persistence.Gateway;
using Wing.Persistence.Saga;

namespace Wing.Persistence
{
    public static class EntityMap
    {
        public static IFreeSql<WingDbFlag> Map(this IFreeSql<WingDbFlag> fsql)
        {
            #region Apm entity config
            fsql.CodeFirst.Entity<Tracer>(eb =>
            {
                eb.ToTable("APM_Tracer");
                eb.HasIndex(x => x.RequestTime).HasName("IX_RequestTime");
                eb.HasIndex(x => x.ServiceName).HasName("IX_ServiceName");
                eb.Property(x => x.Id).HasColumnType("varchar(50)");
                eb.Property(x => x.ParentId).HasColumnType("varchar(50)");
                eb.Property(x => x.ServiceName).HasColumnType("nvarchar(200)");
                eb.Property(x => x.ServiceUrl).HasColumnType("varchar(200)");
                eb.Property(x => x.RequestType).HasColumnType("varchar(20)");
                eb.Property(x => x.RequestUrl).HasColumnType("varchar(4000)");
                eb.Property(x => x.RequestMethod).HasColumnType("varchar(20)");
                eb.Property(x => x.RequestValue).HasColumnType("nvarchar(max)");
                eb.Property(x => x.ClientIp).HasColumnType("varchar(50)");
                eb.Property(x => x.ResponseValue).HasColumnType("nvarchar(max)");
                eb.Property(x => x.Exception).HasColumnType("nvarchar(max)");
            });
            fsql.CodeFirst.Entity<HttpTracer>(eb =>
            {
                eb.ToTable("APM_WS_HttpTracer");
                eb.HasIndex(x => x.RequestTime).HasName("IX_RequestTime");
                eb.HasIndex(x => x.ServiceName).HasName("IX_ServiceName");
                eb.Property(x => x.Id).HasColumnType("varchar(50)");
                eb.Property(x => x.ServiceName).HasColumnType("nvarchar(200)");
                eb.Property(x => x.ServiceUrl).HasColumnType("varchar(200)");
                eb.Property(x => x.RequestType).HasColumnType("varchar(20)");
                eb.Property(x => x.RequestUrl).HasColumnType("varchar(4000)");
                eb.Property(x => x.RequestMethod).HasColumnType("varchar(20)");
                eb.Property(x => x.RequestValue).HasColumnType("nvarchar(max)");
                eb.Property(x => x.ServerIp).HasColumnType("varchar(50)");
                eb.Property(x => x.ResponseValue).HasColumnType("nvarchar(max)");
                eb.Property(x => x.Exception).HasColumnType("nvarchar(max)");
            });
            fsql.CodeFirst.Entity<SqlTracer>(eb =>
            {
                eb.ToTable("APM_WS_SqlTracer");
                eb.HasIndex(x => x.BeginTime).HasName("IX_BeginTime");
                eb.HasIndex(x => x.ServiceName).HasName("IX_ServiceName");
                eb.Property(x => x.Id).HasColumnType("varchar(50)");
                eb.Property(x => x.ServiceName).HasColumnType("nvarchar(200)");
                eb.Property(x => x.ServiceUrl).HasColumnType("varchar(200)");
                eb.Property(x => x.Action).HasColumnType("nvarchar(50)");
                eb.Property(x => x.Sql).HasColumnType("nvarchar(max)");
                eb.Property(x => x.ServerIp).HasColumnType("varchar(50)");
                eb.Property(x => x.Exception).HasColumnType("nvarchar(max)");
            });
            fsql.CodeFirst.Entity<HttpTracerDetail>(eb =>
            {
                eb.ToTable("APM_HttpTracerDetail");
                eb.HasIndex(x => x.TraceId).HasName("IX_TraceId");
                eb.HasIndex(x => x.RequestTime).HasName("IX_RequestTime");
                eb.Property(x => x.Id).HasColumnType("varchar(50)");
                eb.Property(x => x.TraceId).HasColumnType("varchar(50)");
                eb.Property(x => x.RequestType).HasColumnType("varchar(20)");
                eb.Property(x => x.RequestUrl).HasColumnType("varchar(4000)");
                eb.Property(x => x.RequestMethod).HasColumnType("varchar(20)");
                eb.Property(x => x.RequestValue).HasColumnType("nvarchar(max)");
                eb.Property(x => x.ResponseValue).HasColumnType("nvarchar(max)");
                eb.Property(x => x.Exception).HasColumnType("nvarchar(max)");
            });
            fsql.CodeFirst.Entity<SqlTracerDetail>(eb =>
            {
                eb.ToTable("APM_SqlTracerDetail");
                eb.HasIndex(x => x.TraceId).HasName("IX_TraceId");
                eb.HasIndex(x => x.BeginTime).HasName("IX_BeginTime");
                eb.Property(x => x.Id).HasColumnType("varchar(50)");
                eb.Property(x => x.TraceId).HasColumnType("varchar(50)");
                eb.Property(x => x.Action).HasColumnType("nvarchar(50)");
                eb.Property(x => x.Sql).HasColumnType("nvarchar(max)");
                eb.Property(x => x.Exception).HasColumnType("nvarchar(max)");
            });
            #endregion

            #region Gateway entity config
            fsql.CodeFirst.Entity<Log>(eb =>
            {
                eb.ToTable("GateWay_Log");
                eb.HasIndex(x => x.RequestTime).HasName("IX_RequestTime");
                eb.HasIndex(x => x.ServiceName).HasName("IX_ServiceName");
                eb.Property(x => x.Id).HasColumnType("varchar(50)");
                eb.Property(x => x.ServiceName).HasColumnType("nvarchar(200)");
                eb.Property(x => x.DownstreamUrl).HasColumnType("varchar(4000)");
                eb.Property(x => x.RequestUrl).HasColumnType("varchar(4000)");
                eb.Property(x => x.GateWayServerIp).HasColumnType("varchar(50)");
                eb.Property(x => x.ClientIp).HasColumnType("varchar(50)");
                eb.Property(x => x.ServiceAddress).HasColumnType("varchar(200)");
                eb.Property(x => x.RequestMethod).HasColumnType("varchar(20)");
                eb.Property(x => x.RequestValue).HasColumnType("nvarchar(max)");
                eb.Property(x => x.ResponseValue).HasColumnType("nvarchar(max)");
                eb.Property(x => x.Policy).HasColumnType("nvarchar(max)");
                eb.Property(x => x.AuthKey).HasColumnType("varchar(4000)");
                eb.Property(x => x.Token).HasColumnType("varchar(4000)");
            });
            #endregion

            #region Saga entity config
            fsql.CodeFirst.Entity<SagaTran>(eb =>
            {
                eb.ToTable("Saga_Tran");
                eb.HasIndex(x => x.CreatedTime).HasName("IX_CreatedTime");
                eb.HasIndex(x => x.Name).HasName("IX_Name");
                eb.Property(x => x.Id).HasColumnType("varchar(50)");
                eb.Property(x => x.Name).HasColumnType("nvarchar(200)");
                eb.Property(x => x.ServiceName).HasColumnType("nvarchar(200)");
                eb.Property(x => x.Description).HasColumnType("nvarchar(800)");
                eb.Property(x => x.RetryAction).HasColumnType("varchar(50)");
            });
            fsql.CodeFirst.Entity<SagaTranUnit>(eb =>
            {
                eb.ToTable("Saga_TranUnit");
                eb.HasIndex(x => x.CreatedTime).HasName("IX_CreatedTime");
                eb.HasIndex(x => x.Name).HasName("IX_Name");
                eb.HasIndex(x => x.TranId).HasName("IX_TranId");
                eb.Property(x => x.Id).HasColumnType("varchar(50)");
                eb.Property(x => x.Name).HasColumnType("nvarchar(200)");
                eb.Property(x => x.UnitNamespace).HasColumnType("varchar(800)");
                eb.Property(x => x.UnitModelNamespace).HasColumnType("varchar(800)");
                eb.Property(x => x.ParamsValue).HasColumnType("varbinary(max)");
                eb.Property(x => x.Description).HasColumnType("nvarchar(800)");
                eb.Property(x => x.ErrorMsg).HasColumnType("nvarchar(2000)");
                eb.Property(x => x.RetryAction).HasColumnType("varchar(50)");
            });
            fsql.CodeFirst.Entity<SagaTranStatusCount>(eb =>
            {
                eb.ToTable("Saga_TranStatusCount");
                eb.HasIndex(x => x.Name).HasName("IX_Name");
                eb.Property(x => x.Id).HasColumnType("varchar(50)");
                eb.Property(x => x.Name).HasColumnType("nvarchar(200)");
                eb.Property(x => x.ServiceName).HasColumnType("nvarchar(200)");
            });
            #endregion
            return fsql;
        }
    }
}
