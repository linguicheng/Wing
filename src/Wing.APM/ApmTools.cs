using System.Data.Common;

namespace Wing.APM
{
    public class ApmTools
    {
        public const string Exception = "wing-apm-exception";
        public const string TraceId = "wing-apm-traceId";
        public const string TraceDetailId = "wing-apm-traceDetailId";
        public const string GrpcRequest = "wing-apm-grpcRequest";
        public const string GrpcResponse = "wing-apm-grpcResponse";
        public const string Sql_Action_SyncStructure = "表结构同步";
        public const string Sql_Action_Select = "查询";
        public const string Sql_Action_Delete = "删除";
        public const string Sql_Action_Update = "修改";
        public const string Sql_Action_Insert = "新增";
        public const string Sql_Action_InsertOrUpdate = "新增或修改";
        public const string Http = "http";
        public const string Grpc = "grpc";

        public static string SqlFormat(string sql, DbParameter[] parameters)
        {
            for (var i = parameters.Length - 1; i >= 0; i--)
            {
                SqlFormat(ref sql, parameters[i]);
            }

            return sql;
        }

        public static string SqlFormat(string sql, DbParameterCollection parameters)
        {
            foreach (DbParameter parameter in parameters)
            {
                SqlFormat(ref sql, parameter);
            }

            return sql;
        }

        public static long UsedMillSeconds(DateTime bdate, DateTime edate)
        {
            return Convert.ToInt64((edate - bdate).TotalMilliseconds);
        }

        private static void SqlFormat(ref string sql, DbParameter parameter)
        {
            if (parameter.DbType == System.Data.DbType.String
                   || parameter.DbType == System.Data.DbType.DateTime
                   || parameter.DbType == System.Data.DbType.Date
                   || parameter.DbType == System.Data.DbType.Time
                   || parameter.DbType == System.Data.DbType.DateTime2
                   || parameter.DbType == System.Data.DbType.DateTimeOffset
                   || parameter.DbType == System.Data.DbType.Guid
                   || parameter.DbType == System.Data.DbType.AnsiStringFixedLength
                   || parameter.DbType == System.Data.DbType.AnsiString
                   || parameter.DbType == System.Data.DbType.StringFixedLength)
            {
                sql = sql.Replace(parameter.ParameterName, "'" + parameter.Value?.ToString() + "'");
            }
            else if (parameter.DbType == System.Data.DbType.Boolean)
            {
                sql = sql.Replace(parameter.ParameterName, Convert.ToBoolean(parameter.Value) ? "1" : "0");
            }
            else
            {
                sql = sql.Replace(parameter.ParameterName, parameter.Value?.ToString());
            }
        }
    }
}
