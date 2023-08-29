using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.APM.EFCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly EFCoreDemoContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, EFCoreDemoContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public Task<int> Get()
        {
            _context.Add(new EFCoreDemo { Id = Guid.NewGuid().ToString(), Name = "EFCore" });
            return _context.SaveChangesAsync();
        }
    }
}