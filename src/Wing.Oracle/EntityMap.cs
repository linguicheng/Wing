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
                eb.HasIndex(x => x.RequestTime).HasName("IX_Tracer_RequestTime");
                eb.HasIndex(x => x.ServiceName).HasName("IX_Tracer_ServiceName");
                eb.Property(x => x.Id).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.ParentId).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.ServiceName).HasColumnType("NVARCHAR2(200)");
                eb.Property(x => x.ServiceUrl).HasColumnType("VARCHAR2(200)");
                eb.Property(x => x.RequestType).HasColumnType("VARCHAR2(20)");
                eb.Property(x => x.RequestUrl).HasColumnType("VARCHAR2(4000)");
                eb.Property(x => x.RequestMethod).HasColumnType("VARCHAR2(20)");
                eb.Property(x => x.RequestValue).HasColumnType("NCLOB");
                eb.Property(x => x.ClientIp).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.ResponseValue).HasColumnType("NCLOB");
                eb.Property(x => x.Exception).HasColumnType("NCLOB");
            });
            fsql.CodeFirst.Entity<HttpTracer>(eb =>
            {
                eb.ToTable("APM_WS_HttpTracer");
                eb.HasIndex(x => x.RequestTime).HasName("IX_WS_HttpTracer_RequestTime");
                eb.HasIndex(x => x.ServiceName).HasName("IX_WS_HttpTracer_ServiceName");
                eb.Property(x => x.Id).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.ServiceName).HasColumnType("NVARCHAR2(200)");
                eb.Property(x => x.ServiceUrl).HasColumnType("VARCHAR2(200)");
                eb.Property(x => x.RequestType).HasColumnType("VARCHAR2(20)");
                eb.Property(x => x.RequestUrl).HasColumnType("VARCHAR2(4000)");
                eb.Property(x => x.RequestMethod).HasColumnType("VARCHAR2(20)");
                eb.Property(x => x.RequestValue).HasColumnType("NCLOB");
                eb.Property(x => x.ResponseValue).HasColumnType("NCLOB");
                eb.Property(x => x.Exception).HasColumnType("NCLOB");
            });
            fsql.CodeFirst.Entity<SqlTracer>(eb =>
            {
                eb.ToTable("APM_WS_SqlTracer");
                eb.HasIndex(x => x.BeginTime).HasName("IX_WS_SqlTracer_BeginTime");
                eb.HasIndex(x => x.ServiceName).HasName("IX_WS_SqlTracer_ServiceName");
                eb.Property(x => x.Id).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.ServiceName).HasColumnType("NVARCHAR2(200)");
                eb.Property(x => x.ServiceUrl).HasColumnType("VARCHAR2(200)");
                eb.Property(x => x.Action).HasColumnType("NVARCHAR2(50)");
                eb.Property(x => x.Sql).HasColumnType("NCLOB");
                eb.Property(x => x.Exception).HasColumnType("NCLOB");
            });
            fsql.CodeFirst.Entity<HttpTracerDetail>(eb =>
            {
                eb.ToTable("APM_HttpTracerDetail");
                eb.HasIndex(x => x.TraceId).HasName("IX_HttpTracerDtl_TraceId");
                eb.HasIndex(x => x.RequestTime).HasName("IX_HttpTracerDtl_RequestTime");
                eb.Property(x => x.Id).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.TraceId).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.RequestType).HasColumnType("VARCHAR2(20)");
                eb.Property(x => x.RequestUrl).HasColumnType("VARCHAR2(4000)");
                eb.Property(x => x.RequestMethod).HasColumnType("VARCHAR2(20)");
                eb.Property(x => x.RequestValue).HasColumnType("NCLOB");
                eb.Property(x => x.ResponseValue).HasColumnType("NCLOB");
                eb.Property(x => x.Exception).HasColumnType("NCLOB");
            });
            fsql.CodeFirst.Entity<SqlTracerDetail>(eb =>
            {
                eb.ToTable("APM_SqlTracerDetail");
                eb.HasIndex(x => x.TraceId).HasName("IX_SqlTracerDtl_TraceId");
                eb.HasIndex(x => x.BeginTime).HasName("IX_SqlTracerDtl__BeginTime");
                eb.Property(x => x.Id).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.TraceId).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.Action).HasColumnType("NVARCHAR2(50)");
                eb.Property(x => x.Sql).HasColumnType("NCLOB");
                eb.Property(x => x.Exception).HasColumnType("NCLOB");
            });
            #endregion

            #region Gateway entity config
            fsql.CodeFirst.Entity<Log>(eb =>
            {
                eb.ToTable("GateWay_Log");
                eb.HasIndex(x => x.RequestTime).HasName("IX_GateWay_RequestTime");
                eb.HasIndex(x => x.ServiceName).HasName("IX_GateWay_ServiceName");
                eb.Property(x => x.Id).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.ServiceName).HasColumnType("NVARCHAR2(200)");
                eb.Property(x => x.DownstreamUrl).HasColumnType("VARCHAR2(4000)");
                eb.Property(x => x.RequestUrl).HasColumnType("VARCHAR2(4000)");
                eb.Property(x => x.GateWayServerIp).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.ClientIp).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.ServiceAddress).HasColumnType("VARCHAR2(200)");
                eb.Property(x => x.RequestMethod).HasColumnType("VARCHAR2(20)");
                eb.Property(x => x.RequestValue).HasColumnType("NCLOB");
                eb.Property(x => x.ResponseValue).HasColumnType("NCLOB");
                eb.Property(x => x.Policy).HasColumnType("NCLOB");
                eb.Property(x => x.AuthKey).HasColumnType("VARCHAR2(4000)");
                eb.Property(x => x.Token).HasColumnType("VARCHAR2(4000)");
                eb.Property(x => x.Exception).HasColumnType("NCLOB");
            });

            fsql.CodeFirst.Entity<LogDetail>(eb =>
            {
                eb.ToTable("GateWay_LogDetail");
                eb.HasIndex(x => x.RequestTime).HasName("IX_GateWay_RequestTime");
                eb.HasIndex(x => x.ServiceName).HasName("IX_GateWay_ServiceName");
                eb.Property(x => x.Id).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.ServiceName).HasColumnType("NVARCHAR2(200)");
                eb.Property(x => x.RequestUrl).HasColumnType("VARCHAR2(4000)");
                eb.Property(x => x.Key).HasColumnType("VARCHAR2(100)");
                eb.Property(x => x.ServiceAddress).HasColumnType("VARCHAR2(200)");
                eb.Property(x => x.RequestMethod).HasColumnType("VARCHAR2(20)");
                eb.Property(x => x.RequestValue).HasColumnType("NCLOB");
                eb.Property(x => x.ResponseValue).HasColumnType("NCLOB");
                eb.Property(x => x.Policy).HasColumnType("NCLOB");
                eb.Property(x => x.AuthKey).HasColumnType("VARCHAR2(4000)");
                eb.Property(x => x.Token).HasColumnType("VARCHAR2(4000)");
                eb.Property(x => x.Exception).HasColumnType("NCLOB");
            });
            #endregion

            #region Saga entity config
            fsql.CodeFirst.Entity<SagaTran>(eb =>
            {
                eb.ToTable("Saga_Tran");
                eb.HasIndex(x => x.CreatedTime).HasName("IX_SagaTran_CreatedTime");
                eb.HasIndex(x => x.Name).HasName("IX_SagaTran_Name");
                eb.Property(x => x.Id).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.Name).HasColumnType("NVARCHAR2(200)");
                eb.Property(x => x.ServiceName).HasColumnType("NVARCHAR2(200)");
                eb.Property(x => x.Description).HasColumnType("NVARCHAR2(800)");
                eb.Property(x => x.RetryAction).HasColumnType("VARCHAR2(50)");
            });
            fsql.CodeFirst.Entity<SagaTranUnit>(eb =>
            {
                eb.ToTable("Saga_TranUnit");
                eb.HasIndex(x => x.CreatedTime).HasName("IX_SagaTranUnit_CreatedTime");
                eb.HasIndex(x => x.Name).HasName("IX_SagaTranUnit_Name");
                eb.HasIndex(x => x.TranId).HasName("IX_SagaTranUnit_TranId");
                eb.Property(x => x.Id).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.Name).HasColumnType("NVARCHAR2(200)");
                eb.Property(x => x.UnitNamespace).HasColumnType("VARCHAR2(800)");
                eb.Property(x => x.UnitModelNamespace).HasColumnType("VARCHAR2(800)");
                eb.Property(x => x.ParamsValue).HasColumnType("NCLOB");
                eb.Property(x => x.Description).HasColumnType("NVARCHAR2(1000)");
                eb.Property(x => x.ErrorMsg).HasColumnType("NVARCHAR2(2000)");
                eb.Property(x => x.RetryAction).HasColumnType("VARCHAR2(50)");
            });
            fsql.CodeFirst.Entity<SagaTranStatusCount>(eb =>
            {
                eb.ToTable("Saga_TranStatusCount");
                eb.HasIndex(x => x.Name).HasName("IX_SagaTranStatusCount_Name");
                eb.Property(x => x.Id).HasColumnType("VARCHAR2(50)");
                eb.Property(x => x.Name).HasColumnType("NVARCHAR2(200)");
                eb.Property(x => x.ServiceName).HasColumnType("NVARCHAR2(200)");
            });
            #endregion
            return fsql;
        }
    }
}
