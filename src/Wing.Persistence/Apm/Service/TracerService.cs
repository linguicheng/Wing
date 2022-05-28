using System.Threading.Tasks;
using Wing.Persistence.APM;

namespace Wing.Persistence.Apm
{
    public class TracerService : ITracerService
    {
        private readonly IFreeSql _fsql;

        public TracerService(IFreeSql fsql)
        {
            _fsql = fsql;
        }

        public async Task Add(TracerDto tracerDto)
        {
            using var uow = _fsql.CreateUnitOfWork();
            await uow.Orm.Insert(tracerDto.Tracer).ExecuteAffrowsAsync();
            await uow.Orm.Insert(tracerDto.HttpTracerDetails).ExecuteAffrowsAsync();
            await uow.Orm.Insert(tracerDto.SqlTracerDetails).ExecuteAffrowsAsync();
            uow.Commit();
        }
    }
}
