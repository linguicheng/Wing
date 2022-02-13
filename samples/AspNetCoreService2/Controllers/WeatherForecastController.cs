using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wing.Auth;
using Wing.HttpTransport;

namespace AspNetCoreService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IRequest _request;
        private readonly IAuth _auth;
        public WeatherForecastController(IRequest request, IAuth auth)
        {
            _request = request;
            _auth = auth;
        }
        [Authorize("Wing")]
        [HttpGet]
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
        public async Task<string> Test1()
        {
            return await _request.Get<string>("http://192.168.56.99:5002/api/values");
        }
        [HttpGet("test2")]
        public async Task<string> Test2()
        {
            return await _request.Get<string>("http://192.168.56.98:5002/api/values");
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

    }
}
