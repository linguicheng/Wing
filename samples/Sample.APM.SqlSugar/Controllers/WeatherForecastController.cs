using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Sample.APM.SqlSugar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ISqlSugarDemoService _sqlSugarDemoService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISqlSugarDemoService sqlSugarDemoService)
        {
            _logger = logger;
            _sqlSugarDemoService = sqlSugarDemoService;
        }

        [HttpGet]
        public Task<int> Get(string name = "SqlSugar")
        {
            return _sqlSugarDemoService.Add(new SqlSugarDemo { Id = Guid.NewGuid().ToString(), Name = name });
        }
    }
}
