using System;

namespace Wing.Persistence
{
    public class Config
    {
        public static long GatewayTimeOut => string.IsNullOrWhiteSpace(App.Configuration["Home:Timeout:Gateway"]) ? 60 * 1000 : Convert.ToInt64(App.Configuration["Home:Timeout:Gateway"]);

        public static long ApmTimeOut => string.IsNullOrWhiteSpace(App.Configuration["Home:Timeout:Apm:Http"]) ? 60 * 1000 : Convert.ToInt64(App.Configuration["Home:Timeout:Apm:Http"]);

        public static long ApmWorkServiceHttpTimeOut => string.IsNullOrWhiteSpace(App.Configuration["Home:Timeout:Apm:WorkServiceHttp"]) ? 60 * 1000 : Convert.ToInt64(App.Configuration["Home:Timeout:Apm:WorkServiceHttp"]);

        public static long ApmWorkServiceSqlTimeOut => string.IsNullOrWhiteSpace(App.Configuration["Home:Timeout:Apm:WorkServiceSql"]) ? 60 * 1000 : Convert.ToInt64(App.Configuration["Home:Timeout:Apm:WorkServiceSql"]);

        public static DateTime SearchTime => string.IsNullOrWhiteSpace(App.Configuration["Home:Timeout:SearchTime"]) ? DateTime.Now.AddMonths(-1) : Convert.ToDateTime(App.Configuration["Home:Timeout:SearchTime"]);
    }
}
