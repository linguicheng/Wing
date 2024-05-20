using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Sample.Auth;
using Wing;

namespace Sample.AspNetCoreService2.Controllers
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
        private readonly IHttpContextAccessor _contextAccessor;
        public WeatherForecastController(IHttpClientFactory httpClientFactory, IAuth auth, IHttpContextAccessor context)
        {
            _httpClientFactory = httpClientFactory;
            _auth = auth;
            _contextAccessor = context;
        }

        [HttpGet("{name}")]
        public IEnumerable<WeatherForecast> Get(string name)
        {
            var headers = _contextAccessor.HttpContext.Request.Headers;
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
                Ver = "2.0"
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
        [Authorize]
        [HttpGet("test3")]
        public string Test3()
        {
            return "Hello Wing";
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

        [HttpGet("CustomRoute/{name}")]
        public string CustomRoute(string name)
        {
            return $"自定义路由测试：{name}，客户端IP:{Tools.RemoteIp}";
        }
    }
}
