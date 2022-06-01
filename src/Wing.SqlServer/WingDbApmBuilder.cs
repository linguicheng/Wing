using System;
using System.Diagnostics;
using FreeSql.Aop;
using Microsoft.Extensions.DependencyInjection;
using Wing.APM;
using Wing.APM.FreeSql;

namespace Wing.Persistence
{
    public class WingDbApmBuilder
    {
        private static readonly DiagnosticListener FreeSqlListener = new DiagnosticListener("WingDbFreeSqlDiagnosticListener");

        public static void AddFreeSql(IServiceCollection services, IFreeSql<WingDbFlag> fsql)
        {
            if (fsql == null)
            {
                throw new ArgumentNullException(nameof(fsql));
            }

            services.AddSingleton<IDiagnosticListener, WingDbFreeSqlDiagnosticListener>();
            WriteToListener(fsql);
        }

        public static void WriteToListener(IFreeSql<WingDbFlag> fsql)
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
