using System;
using System.Diagnostics;
using FreeSql.Aop;

namespace Wing.APM.FreeSql
{
    public class WriteToListener
    {
        public static void Write(IFreeSql fsql, DiagnosticListener freeSqlListener)
        {
            fsql.Aop.CurdBefore += new EventHandler<CurdBeforeEventArgs>((s, e) =>
            {
                freeSqlListener.Write(FreeSqlKey.CurdBefore, e);
            });
            fsql.Aop.CurdAfter += new EventHandler<CurdAfterEventArgs>((s, e) =>
            {
                freeSqlListener.Write(FreeSqlKey.CurdAfter, e);
            });

            fsql.Aop.SyncStructureBefore += new EventHandler<SyncStructureBeforeEventArgs>((s, e) =>
            {
                freeSqlListener.Write(FreeSqlKey.SyncStructureBefore, e);
            });
            fsql.Aop.SyncStructureAfter += new EventHandler<SyncStructureAfterEventArgs>((s, e) =>
            {
                freeSqlListener.Write(FreeSqlKey.SyncStructureAfter, e);
            });

            fsql.Aop.CommandBefore += new EventHandler<CommandBeforeEventArgs>((s, e) =>
            {
                freeSqlListener.Write(FreeSqlKey.CommandBefore, e);
            });
            fsql.Aop.CommandAfter += new EventHandler<CommandAfterEventArgs>((s, e) =>
            {
                freeSqlListener.Write(FreeSqlKey.CommandAfter, e);
            });

            fsql.Aop.TraceBefore += new EventHandler<TraceBeforeEventArgs>((s, e) =>
            {
                freeSqlListener.Write(FreeSqlKey.TraceBefore, e);
            });
            fsql.Aop.TraceAfter += new EventHandler<TraceAfterEventArgs>((s, e) =>
            {
                freeSqlListener.Write(FreeSqlKey.TraceAfter, e);
            });
        }
    }
}
