using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wing.Persistence.Apm;
using Wing.Persistence.Gateway;
using Wing.Persistence.Saga;
using Wing.ServiceProvider;

namespace Wing.UI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IDiscoveryServiceProvider _discoveryService;
        private readonly ILogService _gatewayLogService;
        private readonly ITracerService _tracerService;
        private readonly ITracerWorkService _tracerWorkService;
        private readonly ISagaTranService _sagaTranService;

        public HomeController(ILogService gatewayLogService,
                              ITracerService tracerService,
                              ITracerWorkService tracerWorkService,
                              ISagaTranService sagaTranService)
        {
            _discoveryService = App.DiscoveryService;
            _gatewayLogService = gatewayLogService;
            _tracerService = tracerService;
            _tracerWorkService = tracerWorkService;
            _sagaTranService = sagaTranService;
        }

        /// <summary>
        /// 指标统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object TargetCount()
        {
            int serviceCriticalTotal = 0;
            long gatewayTimeoutTotal = 0;
            long apmTimeoutTotal = 0;
            long apmWorkHttpTimeoutTotal = 0;
            long apmWorkSqlTimeoutTotal = 0;
            long sagaFailedTotal = 0;
            Parallel.Invoke(() =>
            {
                var serviceDetail = _discoveryService.Get().GetAwaiter().GetResult();
                serviceCriticalTotal = serviceDetail.Count(x => x.Status == HealthStatus.Critical);
            },
             () => gatewayTimeoutTotal = _gatewayLogService.TimeoutTotal(),
             () => apmTimeoutTotal = _tracerService.TimeoutTotal(),
             () => apmWorkHttpTimeoutTotal = _tracerWorkService.HttpTimeoutTotal(),
             () => apmWorkSqlTimeoutTotal = _tracerWorkService.SqlTimeoutTotal(),
             () => sagaFailedTotal = _sagaTranService.GetFailedTotal());
            return new
            {
                serviceCriticalTotal,
                gatewayTimeoutTotal,
                apmTimeoutTotal,
                apmWorkHttpTimeoutTotal,
                apmWorkSqlTimeoutTotal,
                sagaFailedTotal
            };
        }
    }

    public class IndexController : Controller
    {
        [Route("wing")]
        public RedirectResult Index()
        {
            return new RedirectResult(url: "/wing/index.html", permanent: true);
        }
    }
}
