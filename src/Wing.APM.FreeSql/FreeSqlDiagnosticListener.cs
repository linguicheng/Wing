using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Wing.APM.FreeSql
{
    public class FreeSqlDiagnosticListener : IDiagnosticListener
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FreeSqlDiagnosticListener> _logger;

        public virtual string Name => "FreeSqlDiagnosticListener";

        public FreeSqlDiagnosticListener(IHttpContextAccessor httpContextAccessor, ILogger<FreeSqlDiagnosticListener> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            try
            {
                switch (value.Key)
                {
                    case FreeSqlKey.CurdBefore:
                        break;
                    case FreeSqlKey.CurdAfter:
                        break;
                    case FreeSqlKey.TraceBefore:
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FreeSql监听异常");
            }
        }
    }
}
