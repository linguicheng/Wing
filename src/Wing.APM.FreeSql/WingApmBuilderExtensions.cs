using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Wing.APM.Builder;
using Wing.APM.FreeSql;
using Wing.APM.Listeners;

namespace Wing
{
    public static class WingApmBuilderExtensions
    {
        private static readonly DiagnosticListener FreeSqlListener = new DiagnosticListener("FreeSqlDiagnosticListener");

        public static WingApmBuilder AddFreeSql(this WingApmBuilder wingApmBuilder)
        {
            wingApmBuilder.ServiceBuilder.Services.AddSingleton<IDiagnosticListener, FreeSqlDiagnosticListener>();
            return wingApmBuilder;
        }

        public static WingApmBuilder Build(this WingApmBuilder wingApmBuilder, IFreeSql fsql)
        {
            if (fsql == null)
            {
                throw new ArgumentNullException(nameof(fsql));
            }

            WriteToListener.Write(fsql, FreeSqlListener);
            return wingApmBuilder;
        }
    }
}
