using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wing.Saga.Client;

namespace Sample.Saga.Client.Http.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get(string name)
        {
            Wing.Saga.Client.Saga.Start("Saga分布式事务样例", new SagaOptions { BreakerCount = 5, CustomCount = 2, TranPolicy = Wing.Persistence.Saga.TranPolicy.Custom })
                 .Then(new SampleSagaUnit1(), new SampleUnitModel { Name = "事务单元1", HelloName = name })
                 .Then(new SampleSagaUnit2(), new SampleUnitModel { Name = "事务单元2", HelloName = name })
                 .Then(new SampleSagaUnit3(), new SampleUnitModel { Name = "事务单元3", HelloName = name })
                 .End();
            return $"Hello {name}";
        }
    }
}
