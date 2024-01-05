using Sample.AspNetCoreService.EventBus;
using Sample.AspNetCoreService.Policy;
using Microsoft.AspNetCore.Mvc;
using Sample.Auth;
using Wing.EventBus;

namespace Sample.AspNetCoreService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IProduct _product;
        private readonly IAuth _auth;
        private readonly IConfiguration _configuration;
        private readonly ITracerService _tracerService;
        public TestController(IAuth auth,
                            IHttpClientFactory httpClientFactory,
                            IProduct product,
                            ITracerService tracerService,
                            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _product = product;
            _auth = auth;
            _configuration = configuration;
            _tracerService = tracerService;
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

        [HttpGet("test1")]
        public async Task<ActionResult> Test1()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://192.168.56.99:5002");
            string result = await client.GetStringAsync("/api/values");
            return Ok(result);
        }
        [HttpGet("test2")]
        public async Task<ActionResult> Test2()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://192.168.56.98:5002");
            string result = await client.GetStringAsync("/api/values");
            return Ok(result);
        }
        [HttpGet("Hello/{name}")]
        public async Task<string> Hello(string name)
        {
            return await _product.InvokeHello(name);
        }

        [HttpGet("test3")]
        public string Test3()
        {
            return "Hello Wing";
        }

        [HttpGet("AddTracer")]
        public Task AddTracer()
        {
            return _tracerService.Add(new Tracer
            {
                Id = Guid.NewGuid().ToString(),
                RequestTime = DateTime.Now,
                Exception = "测试AddTracer"
            });
        }

        [HttpGet("test4")]
        public IActionResult Test4(string key)
        {
            return Ok(_configuration[key]);
        }

        [HttpGet("gettoken")]
        public string GetToken()
        {
            return _auth.GetToken("byron");
        }

        [HttpPost]
        public WeatherForecast Post(WeatherForecast model)
        {
            return model;
        }
    }
}
