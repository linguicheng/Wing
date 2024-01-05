using Microsoft.AspNetCore.Mvc;
using Sample.Auth;

namespace Sample.AspNetCoreService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAuth _auth;
        public WeatherForecastController(IHttpClientFactory httpClientFactory, IAuth auth)
        {
            _httpClientFactory = httpClientFactory;
            _auth = auth;
        }

        [HttpGet("{name}")]
        //[Authorize("Wing")]
        public IEnumerable<WeatherForecast> Get(string name)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
                Ver = $"自定义路由测试：{name}"
            })
            .ToArray();
        }

        [HttpGet("HelloWing")]
        public string HelloWing()
        {
            return "Hello Wing";
        }

        [HttpPost]
        public WeatherForecast Post(WeatherForecast model)
        {
            return model;
        }

        [HttpGet("CustomRoute/{name}")]
        public string CustomRoute(string name)
        {
            return $"自定义路由测试：{name}" ;
        }
    }
}
