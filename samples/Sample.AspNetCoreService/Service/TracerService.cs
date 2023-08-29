using System.Threading.Tasks;

namespace Sample.AspNetCoreService
{
    public class TracerService : ITracerService
    {
        private readonly IFreeSql<SampleWingDbFlag> _fsql;

        public TracerService(IFreeSql<SampleWingDbFlag> fsql)
        {
            _fsql = fsql;
        }

        public async Task Add(Tracer tracer)
        {
            await _fsql.Insert(tracer).ExecuteAffrowsAsync();
        }
    }
}
