using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Wing.APM.FreeSql
{
    public class WingDbFreeSqlDiagnosticListener : FreeSqlDiagnosticListener
    {
        public override string Name => "WingDbFreeSqlDiagnosticListener";

        public WingDbFreeSqlDiagnosticListener(IHttpContextAccessor httpContextAccessor, ILogger<WingDbFreeSqlDiagnosticListener> logger)
            : base(httpContextAccessor, logger)
        {
            DoNotDiagnostic = sql =>
            {
                if (string.IsNullOrWhiteSpace(sql))
                {
                    return false;
                }

                return sql.ToUpper().Contains("APM_");
            };
        }
    }
}
