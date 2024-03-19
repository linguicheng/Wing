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
                eb.HasIndex(x => x.RequestTime).HasName("IX_APM_Tracer_RequestTime");
                eb.HasIndex(x => x.ServiceName).HasName("IX_APM_Tracer_ServiceName");
                eb.Property(x => x.Id).HasColumnType("VARCHAR");
                eb.Property(x => x.ParentId).HasColumnType("NVARCHAR");
                eb.Property(x => x.ServiceName).HasColumnType("NVARCHAR");
                eb.Property(x => x.ServiceUrl).HasColumnType("VARCHAR");
                eb.Property(x => x.RequestType).HasColumnType("VARCHAR");
                eb.Property(x => x.RequestUrl).HasColumnType("VARCHAR");
                eb.Property(x => x.RequestMethod).HasColumnType("VARCHAR");
                eb.Property(x => x.RequestValue).HasColumnType("NVARCHAR");
                eb.Property(x => x.ClientIp).HasColumnType("VARCHAR");
                eb.Property(x => x.ResponseValue).HasColumnType("NVARCHAR");
                eb.Property(x => x.Exception).HasColumnType("NVARCHAR");
            });
            fsql.CodeFirst.Entity<HttpTracer>(eb =>
            {
                eb.ToTable("APM_WS_HttpTracer");
                eb.HasIndex(x => x.RequestTime).HasName("IX_APM_WS_HttpTracer_RequestTime");
                eb.HasIndex(x => x.ServiceName).HasName("IX_APM_WS_HttpTracer_ServiceName");
                eb.Property(x => x.Id).HasColumnType("VARCHAR");
                eb.Property(x => x.ServiceName).HasColumnType("NVARCHAR");
                eb.Property(x => x.ServiceUrl).HasColumnType("VARCHAR");
                eb.Property(x => x.RequestType).HasColumnType("VARCHAR");
                eb.Property(x => x.RequestUrl).HasColumnType("VARCHAR");
                eb.Property(x => x.RequestMethod).HasColumnType("VARCHAR");
                eb.Property(x => x.RequestValue).HasColumnType("NVARCHAR");
                eb.Property(x => x.ResponseValue).HasColumnType("NVARCHAR");
                eb.Property(x => x.Exception).HasColumnType("NVARCHAR");
            });
            fsql.CodeFirst.Entity<SqlTracer>(eb =>
            {
                eb.ToTable("APM_WS_SqlTracer");
                eb.HasIndex(x => x.BeginTime).HasName("IX_APM_WS_SqlTracer_BeginTime");
                eb.HasIndex(x => x.ServiceName).HasName("IX_APM_WS_SqlTracer_ServiceName");
                eb.Property(x => x.Id).HasColumnType("VARCHAR");
                eb.Property(x => x.ServiceName).HasColumnType("NVARCHAR");
                eb.Property(x => x.ServiceUrl).HasColumnType("VARCHAR");
                eb.Property(x => x.Action).HasColumnType("NVARCHAR");
                eb.Property(x => x.Sql).HasColumnType("NVARCHAR");
                eb.Property(x => x.Exception).HasColumnType("NVARCHAR");
            });
            fsql.CodeFirst.Entity<HttpTracerDetail>(eb =>
            {
                eb.ToTable("APM_HttpTracerDetail");
                eb.HasIndex(x => x.TraceId).HasName("IX_APM_HttpTracerDetail_TraceId");
                eb.HasIndex(x => x.RequestTime).HasName("IX_APM_HttpTracerDetail_RequestTime");
                eb.Property(x => x.Id).HasColumnType("VARCHAR");
                eb.Property(x => x.TraceId).HasColumnType("VARCHAR");
                eb.Property(x => x.RequestType).HasColumnType("VARCHAR");
                eb.Property(x => x.RequestUrl).HasColumnType("VARCHAR");
                eb.Property(x => x.RequestMethod).HasColumnType("VARCHAR");
                eb.Property(x => x.RequestValue).HasColumnType("NVARCHAR");
                eb.Property(x => x.ResponseValue).HasColumnType("NVARCHAR");
                eb.Property(x => x.Exception).HasColumnType("NVARCHAR");
            });
            fsql.CodeFirst.Entity<SqlTracerDetail>(eb =>
            {
                eb.ToTable("APM_SqlTracerDetail");
                eb.HasIndex(x => x.TraceId).HasName("IX_APM_SqlTracerDetail_TraceId");
                eb.HasIndex(x => x.BeginTime).HasName("IX_APM_SqlTracerDetail_BeginTime");
                eb.Property(x => x.Id).HasColumnType("VARCHAR");
                eb.Property(x => x.TraceId).HasColumnType("VARCHAR");
                eb.Property(x => x.Action).HasColumnType("NVARCHAR");
                eb.Property(x => x.Sql).HasColumnType("NVARCHAR");
                eb.Property(x => x.Exception).HasColumnType("NVARCHAR");
            });
            #endregion

            #region Gateway entity config
            fsql.CodeFirst.Entity<Log>(eb =>
            {
                eb.ToTable("GateWay_Log");
                eb.HasIndex(x => x.RequestTime).HasName("IX_GateWay_Log_RequestTime");
                eb.HasIndex(x => x.ServiceName).HasName("IX_GateWay_Log_ServiceName");
                eb.Property(x => x.Id).HasColumnType("VARCHAR");
                eb.Property(x => x.ServiceName).HasColumnType("NVARCHAR");
                eb.Property(x => x.DownstreamUrl).HasColumnType("NVARCHAR");
                eb.Property(x => x.RequestUrl).HasColumnType("NVARCHAR");
                eb.Property(x => x.GateWayServerIp).HasColumnType("VARCHAR");
                eb.Property(x => x.ClientIp).HasColumnType("VARCHAR");
                eb.Property(x => x.ServiceAddress).HasColumnType("VARCHAR");
                eb.Property(x => x.RequestMethod).HasColumnType("VARCHAR");
                eb.Property(x => x.RequestValue).HasColumnType("NVARCHAR");
                eb.Property(x => x.ResponseValue).HasColumnType("NVARCHAR");
                eb.Property(x => x.Policy).HasColumnType("NVARCHAR");
                eb.Property(x => x.AuthKey).HasColumnType("VARCHAR");
                eb.Property(x => x.Token).HasColumnType("VARCHAR");
                eb.Property(x => x.Exception).HasColumnType("NVARCHAR");
            });

            fsql.CodeFirst.Entity<LogDetail>(eb =>
            {
                eb.ToTable("GateWay_LogDetail");
                eb.HasIndex(x => x.RequestTime).HasName("IX_GateWay_LogDetail_RequestTime");
                eb.HasIndex(x => x.ServiceName).HasName("IX_GateWay_LogDetail_ServiceName");
                eb.Property(x => x.Id).HasColumnType("VARCHAR");
                eb.Property(x => x.ServiceName).HasColumnType("NVARCHAR");
                eb.Property(x => x.RequestUrl).HasColumnType("NVARCHAR");
                eb.Property(x => x.Key).HasColumnType("VARCHAR");
                eb.Property(x => x.ServiceAddress).HasColumnType("VARCHAR");
                eb.Property(x => x.RequestMethod).HasColumnType("VARCHAR");
                eb.Property(x => x.ResponseValue).HasColumnType("NVARCHAR");
                eb.Property(x => x.Policy).HasColumnType("NVARCHAR");
                eb.Property(x => x.Exception).HasColumnType("NVARCHAR");
            });
            #endregion

            #region Saga entity config
            fsql.CodeFirst.Entity<SagaTran>(eb =>
            {
                eb.ToTable("Saga_Tran");
                eb.HasIndex(x => x.CreatedTime).HasName("IX_Saga_Tran_CreatedTime");
                eb.HasIndex(x => x.Name).HasName("IX_Saga_Tran_Name");
                eb.Property(x => x.Id).HasColumnType("VARCHAR");
                eb.Property(x => x.Name).HasColumnType("NVARCHAR");
                eb.Property(x => x.ServiceName).HasColumnType("NVARCHAR");
                eb.Property(x => x.Description).HasColumnType("NVARCHAR");
                eb.Property(x => x.RetryAction).HasColumnType("VARCHAR");
            });
            fsql.CodeFirst.Entity<SagaTranUnit>(eb =>
            {
                eb.ToTable("Saga_TranUnit");
                eb.HasIndex(x => x.CreatedTime).HasName("IX_Saga_TranUnit_CreatedTime");
                eb.HasIndex(x => x.Name).HasName("IX_Saga_TranUnit_Name");
                eb.HasIndex(x => x.TranId).HasName("IX_Saga_TranUnit_TranId");
                eb.Property(x => x.Id).HasColumnType("VARCHAR");
                eb.Property(x => x.Name).HasColumnType("NVARCHAR");
                eb.Property(x => x.UnitNamespace).HasColumnType("VARCHAR");
                eb.Property(x => x.UnitModelNamespace).HasColumnType("VARCHAR");
                eb.Property(x => x.ParamsValue).HasColumnType("NVARCHAR");
                eb.Property(x => x.Description).HasColumnType("NVARCHAR");
                eb.Property(x => x.ErrorMsg).HasColumnType("NVARCHAR");
                eb.Property(x => x.RetryAction).HasColumnType("VARCHAR");
            });
            fsql.CodeFirst.Entity<SagaTranStatusCount>(eb =>
            {
                eb.ToTable("Saga_TranStatusCount");
                eb.HasIndex(x => x.Name).HasName("IX_Saga_TranStatusCount_Name");
                eb.Property(x => x.Id).HasColumnType("VARCHAR");
                eb.Property(x => x.Name).HasColumnType("NVARCHAR");
                eb.Property(x => x.ServiceName).HasColumnType("NVARCHAR");
            });
            #endregion

            #region User entity config
            fsql.CodeFirst.Entity<Wing.Persistence.User.User>(eb =>
            {
                eb.ToTable("Sys_User");
                eb.HasIndex(x => x.UserName).HasName("IX_User_UserName");
                eb.HasIndex(x => x.UserAccount).HasName("IX_User_UserAccount");
                eb.Property(x => x.Id).HasColumnType("VARCHAR");
                eb.Property(x => x.UserName).HasColumnType("NVARCHAR");
                eb.Property(x => x.UserAccount).HasColumnType("VARCHAR");
                eb.Property(x => x.Password).HasColumnType("VARCHAR");
                eb.Property(x => x.CreatedName).HasColumnType("NVARCHAR");
                eb.Property(x => x.CreatedAccount).HasColumnType("VARCHAR");
                eb.Property(x => x.ModifiedName).HasColumnType("NVARCHAR");
                eb.Property(x => x.ModifiedAccount).HasColumnType("VARCHAR");
                eb.Property(x => x.Dept).HasColumnType("NVARCHAR");
                eb.Property(x => x.Station).HasColumnType("NVARCHAR");
                eb.Property(x => x.Phone).HasColumnType("VARCHAR");
                eb.Property(x => x.Remark).HasColumnType("NVARCHAR");
            });
            #endregion
            return fsql;
        }
    }
}
