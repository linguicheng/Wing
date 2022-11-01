using System.Threading.Tasks;
using Wing.Injection;

namespace Wing.Persistence.Saga
{
    public class SagaTranService : ISagaTranService, IScoped
    {
        private readonly IFreeSql<WingDbFlag> _fsql;

        public SagaTranService(IFreeSql<WingDbFlag> fsql)
        {
            _fsql = fsql;
        }

        public Task<int> Add(SagaTran entity)
        {
            return _fsql.Insert(entity).ExecuteAffrowsAsync();
        }

        public Task<bool> Any(string id)
        {
            return _fsql.Select<SagaTran>().AnyAsync(x => x.Id == id);
        }

        public Task<int> UpdateStatus(UpdateStatusDto dto)
        {
           return _fsql.Update<SagaTran>(dto.Id)
                .Set(x => x.Status, dto.Status)
                .Set(x => x.EndTime, dto.EndTime)
                .Set(x => x.UsedMillSeconds, dto.UsedMillSeconds)
                .ExecuteAffrowsAsync();
        }
    }
}
