using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using Wing.APM.Builder;
using Wing.APM.FreeSql;
using Wing.APM.Listeners;
using Wing.APM.SqlSugar;

namespace Wing
{
    public static class WingApmBuilderExtensions
    {
        private static readonly DiagnosticListener SqlSugarListener = new DiagnosticListener("SqlSugarDiagnosticListener");

        public static WingApmBuilder AddSqlSugar(this WingApmBuilder wingApmBuilder)
        {
            wingApmBuilder.ServiceBuilder.Services.AddSingleton<IDiagnosticListener, SqlSugarDiagnosticListener>();
            return wingApmBuilder;
        }

        public static ISqlSugarClient AddWingAPM(this ISqlSugarClient db)
        {
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                var model = new OnLogExecutingModel
                {
                    ContextID = db.ContextID,
                    BeginTime = DateTime.Now,
                    ActionType = db.SugarActionType,
                    Sql = UtilMethods.GetSqlString(db.CurrentConnectionConfig.DbType, sql, pars)
                };
                SqlSugarListener.Write(SqlSugarKey.OnLogExecuting, model);
            };
            db.Aop.OnLogExecuted = (sql, pars) =>
            {
                var model = new OnLogExecutedModel
                {
                    ContextID = db.ContextID,
                    EndTime = DateTime.Now,
                    ExecutionTime = db.Ado.SqlExecutionTime
                };
                SqlSugarListener.Write(SqlSugarKey.OnLogExecuted, model);
            };

            db.Aop.OnError = x =>
            {
                var model = new OnErrorModel
                {
                    ContextID = db.ContextID,
                    Exception = x
                };
                SqlSugarListener.Write(SqlSugarKey.OnError, model);
            };

            return db;
        }
    }
}
