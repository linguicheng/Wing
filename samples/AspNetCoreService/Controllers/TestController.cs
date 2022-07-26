using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.AspNetCoreService.EventBus;
using Sample.AspNetCoreService.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Wing.Auth;
using Wing.EventBus;
using System.Net.Http;
using Wing.Injection;
using Wing.ServiceProvider;
using Wing.Saga;

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
        private readonly IEventBus _eventBus;
        private readonly IProduct _product;
        private readonly IAuth _auth;
        private readonly IConfiguration _configuration;
        private readonly ITracerService _tracerService;
        public TestController(IAuth auth,
                            IHttpClientFactory httpClientFactory,
                            IEventBus eventBus,
                            IProduct product,
                            ITracerService tracerService,
                            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _eventBus = eventBus;
            _product = product;
            _auth = auth;
            _configuration = configuration;
            _tracerService = tracerService;
        }

        [HttpGet("SagaTest")]
        [SagaMain]
        public Task<bool> SagaTest(bool aa)
        {
            _product.SageTest(aa);
            _product.SageTest2();
            return Task.FromResult(true);
        }

        [HttpGet("SagaTest2")]
        public bool SagaTest2()
        {
            var type = GlobalInjection.GetType("Sample.AspNetCoreService.Policy.IProduct");
            var mi = type.GetMethod("SageTest");
            var cc = ServiceLocator.GetService(type);
            var dd = mi.Invoke(cc, new object[] { false });

            return _product.SageTest2();
        }

        [HttpGet]
        [Authorize("Wing")]
        public IEnumerable<WeatherForecast> Get()
        {
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
        [HttpGet("Hello")]
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

        [HttpGet("Publish")]
        public void Publish()
        {
            _eventBus.Publish(new User { Name = "byron", Age = DateTime.Now.Millisecond });
        }
    }
}
