using System.Threading.Tasks;
using Wing.Injection;

namespace Wing.Persistence.Saga
{
    public class SagaTranUnitService : ISagaTranUnitService, IScoped
    {
        private readonly IFreeSql<WingDbFlag> _fsql;

        public SagaTranUnitService(IFreeSql<WingDbFlag> fsql)
        {
            _fsql = fsql;
        }

        public Task<int> Add(SagaTranUnit entity)
        {
            return _fsql.Insert(entity).ExecuteAffrowsAsync();
        }

        public Task<bool> Any(string id)
        {
            return _fsql.Select<SagaTranUnit>().AnyAsync(x => x.Id == id);
        }

        public Task<int> UpdateStatus(UpdateStatusDto dto)
        {
            return _fsql.Update<SagaTranUnit>(dto.Id).Set(x => x.Status, dto.Status).ExecuteAffrowsAsync();
        }
    }
}
