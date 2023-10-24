using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Wing.APM.Listeners;
using Wing.Persistence;

namespace Wing.APM.FreeSql
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
            WriteToListener.Write(fsql, FreeSqlListener);
        }
    }
}
