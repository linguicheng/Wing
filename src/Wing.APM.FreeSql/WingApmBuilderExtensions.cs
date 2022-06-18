using System;
using System.Diagnostics;
using FreeSql.Aop;
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

            WriteToListener(fsql);
            return wingApmBuilder;
        }

        public static void WriteToListener(IFreeSql fsql)
        {
            fsql.Aop.CurdBefore += new EventHandler<CurdBeforeEventArgs>((s, e) =>
            {
                FreeSqlListener.Write(FreeSqlKey.CurdBefore, e);
            });
            fsql.Aop.CurdAfter += new EventHandler<CurdAfterEventArgs>((s, e) =>
            {
                FreeSqlListener.Write(FreeSqlKey.CurdAfter, e);
            });

            fsql.Aop.SyncStructureBefore += new EventHandler<SyncStructureBeforeEventArgs>((s, e) =>
            {
                FreeSqlListener.Write(FreeSqlKey.SyncStructureBefore, e);
            });
            fsql.Aop.SyncStructureAfter += new EventHandler<SyncStructureAfterEventArgs>((s, e) =>
            {
                FreeSqlListener.Write(FreeSqlKey.SyncStructureAfter, e);
            });

            fsql.Aop.CommandBefore += new EventHandler<CommandBeforeEventArgs>((s, e) =>
            {
                FreeSqlListener.Write(FreeSqlKey.CommandBefore, e);
            });
            fsql.Aop.CommandAfter += new EventHandler<CommandAfterEventArgs>((s, e) =>
            {
                FreeSqlListener.Write(FreeSqlKey.CommandAfter, e);
            });

            fsql.Aop.TraceBefore += new EventHandler<TraceBeforeEventArgs>((s, e) =>
            {
                FreeSqlListener.Write(FreeSqlKey.TraceBefore, e);
            });
            fsql.Aop.TraceAfter += new EventHandler<TraceAfterEventArgs>((s, e) =>
            {
                FreeSqlListener.Write(FreeSqlKey.TraceAfter, e);
            });
        }
    }
}
