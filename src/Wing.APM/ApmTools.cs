using System;
using System.Data.Common;
using Wing.APM.Listeners;
using Wing.Persistence.APM;
using Wing.ServiceProvider.Config;

namespace Wing.APM
{
    public class ApmTools
    {
        public const string Exception = "wing-apm-exception";
        public const string TraceId = "wing-apm-traceId";
        public const string TraceDetailId = "wing-apm-traceDetailId";
        public const string Sql_Action_SyncStructure = "表结构同步";
        public const string Sql_Action_Select = "查询";
        public const string Sql_Action_Delete = "删除";
        public const string Sql_Action_Update = "修改";
        public const string Sql_Action_Insert = "新增";
        public const string Sql_Action_InsertOrUpdate = "新增或修改";

        public static string SqlFormat(string sql, DbParameter[] parameters)
        {
            for (var i = parameters.Length - 1; i >= 0; i--)
            {
                if (parameters[i].DbType == System.Data.DbType.String
                    || parameters[i].DbType == System.Data.DbType.DateTime
                    || parameters[i].DbType == System.Data.DbType.Date
                    || parameters[i].DbType == System.Data.DbType.Time
                    || parameters[i].DbType == System.Data.DbType.DateTime2
                    || parameters[i].DbType == System.Data.DbType.DateTimeOffset
                    || parameters[i].DbType == System.Data.DbType.Guid
                    || parameters[i].DbType == System.Data.DbType.AnsiStringFixedLength
                    || parameters[i].DbType == System.Data.DbType.AnsiString
                    || parameters[i].DbType == System.Data.DbType.StringFixedLength)
                {
                    sql = sql.Replace(parameters[i].ParameterName, "'" + parameters[i].Value?.ToString() + "'");
                }
                else if (parameters[i].DbType == System.Data.DbType.Boolean)
                {
                    sql = sql.Replace(parameters[i].ParameterName, Convert.ToBoolean(parameters[i].Value) ? "1" : "0");
                }
                else
                {
                    sql = sql.Replace(parameters[i].ParameterName, parameters[i].Value?.ToString());
                }
            }

            return sql;
        }

        public static long UsedMillSeconds(DateTime bdate, DateTime edate)
        {
            return Convert.ToInt64((edate - bdate).TotalMilliseconds);
        }

        public static string GetServiceUrl(ServiceData service)
        {
            return $"{service.Scheme}://{service.Host}:{service.Port}";
        }
    }
}
