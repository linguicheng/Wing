using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wing.ServiceProvider;

namespace Wing.Dashboard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase
    {

        private readonly IDiscoveryServiceProvider _discoveryServiceProvider;

        public ServiceController(IDiscoveryServiceProvider discoveryServiceProvider)
        {
            _discoveryServiceProvider = discoveryServiceProvider;
        }
        public class ServiceModel
        {

            public string Id { get; }

            public string Name { get; }

            public IEnumerable<string> Tags { get; }

            public ServiceAddress ServiceAddress { get; }

            public int Weight { get; private set; }

            internal int EffectiveWeight { get; set; }

            internal int CurrentWeight { get; set; }
        }
        [HttpGet]
        public async Task<List<Service>> Get([FromQuery]ServiceModel serviceModel)
        {
            return await _discoveryServiceProvider.Get();
        }
    }
}
