using System;
using System.Collections.Generic;
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
                if (db.TempItems == null)
                {
                    db.TempItems = new Dictionary<string, object>();
                }

                var model = new OnLogExecutingModel
                {
                    Id = Guid.NewGuid().ToString(),
                    BeginTime = DateTime.Now,
                    ActionType = db.SugarActionType,
                    Sql = UtilMethods.GetSqlString(db.CurrentConnectionConfig.DbType, sql, pars)
                };
                var contextId = db.ContextID.ToString();
                if (db.TempItems.ContainsKey(contextId))
                {
                    db.TempItems[contextId] = model.Id;
                }
                else
                {
                    db.TempItems.Add(contextId, model.Id);
                }

                SqlSugarListener.Write(SqlSugarKey.OnLogExecuting, model);
            };
            db.Aop.OnLogExecuted = (sql, pars) =>
            {
                var contextId = db.ContextID.ToString();
                if (!db.TempItems.ContainsKey(contextId))
                {
                    return;
                }

                var model = new OnLogExecutedModel
                {
                    Id = db.TempItems[contextId].ToString(),
                    EndTime = DateTime.Now,
                    ExecutionTime = db.Ado.SqlExecutionTime
                };

                SqlSugarListener.Write(SqlSugarKey.OnLogExecuted, model);
            };

            db.Aop.OnError = x =>
            {
                var contextId = db.ContextID.ToString();
                if (!db.TempItems.ContainsKey(contextId))
                {
                    return;
                }

                var model = new OnErrorModel
                {
                    Id = db.TempItems[contextId].ToString(),
                    Exception = x
                };
                SqlSugarListener.Write(SqlSugarKey.OnError, model);
            };

            return db;
        }
    }
}
