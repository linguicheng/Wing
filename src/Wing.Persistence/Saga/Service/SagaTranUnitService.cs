using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Wing.Persistence.Saga
{
    public class SagaTranUnitService : ISagaTranUnitService
    {
        private readonly IFreeSql<WingDbFlag> _fsql;

        public SagaTranUnitService(IFreeSql<WingDbFlag> fsql)
        {
            _fsql = fsql;
        }

        public Task<int> Add(SagaTranUnit entity, DbTransaction transaction = null)
        {
            return _fsql.Insert(entity).WithTransaction(transaction).ExecuteAffrowsAsync();
        }

        public Task<bool> Any(string id)
        {
            return _fsql.Select<SagaTranUnit>().AnyAsync(x => x.Id == id);
        }

        public List<SagaTranUnit> GetFailedData(string tranId)
        {
            return _fsql.Select<SagaTranUnit>()
                .Where(x => x.TranId == tranId && x.Status == TranStatus.Failed)
                .OrderBy(x => x.OrderNo)
                .ToList();
        }

        public List<SagaTranUnit> GetSuccessData(string tranId)
        {
            return _fsql.Select<SagaTranUnit>()
               .Where(x => x.TranId == tranId && x.Status == TranStatus.Success)
               .OrderByDescending(x => x.OrderNo)
               .ToList();
        }

        public Task<int> RetryCommit(RetryCommitTranUnitEvent dto)
        {
            return _fsql.Update<SagaTranUnit>(dto.Id)
                 .SetIf(dto.RetryResult == ExecutedResult.Success, x => x.Status, TranStatus.Success)
                 .Set(x => x.BeginTime, dto.BeginTime)
                 .Set(x => x.EndTime, dto.EndTime)
                 .Set(x => x.UsedMillSeconds, dto.UsedMillSeconds)
                 .Set(x => x.ErrorMsg, dto.ErrorMsg)
                 .Set(x => x.RetryResult, dto.RetryResult)
                 .Set(x => x.RetryAction, dto.RetryAction)
                 .Set(x => x.CommittedCount + 1)
                 .ExecuteAffrowsAsync();
        }

        public Task<int> RetryCancel(RetryCancelTranUnitEvent dto)
        {
            return _fsql.Update<SagaTranUnit>(dto.Id)
                 .SetIf(dto.RetryResult == ExecutedResult.Success, x => x.Status, TranStatus.Cancelled)
                 .Set(x => x.BeginTime, dto.BeginTime)
                 .Set(x => x.EndTime, dto.EndTime)
                 .Set(x => x.UsedMillSeconds, dto.UsedMillSeconds)
                 .Set(x => x.ErrorMsg, dto.ErrorMsg)
                 .Set(x => x.RetryResult, dto.RetryResult)
                 .Set(x => x.RetryAction, dto.RetryAction)
                 .Set(x => x.CancelledCount + 1)
                 .ExecuteAffrowsAsync();
        }

        public Task<int> UpdateStatus(UpdateStatusEvent dto)
        {
            return _fsql.Update<SagaTranUnit>(dto.Id).Set(x => x.Status, dto.Status).ExecuteAffrowsAsync();
        }

        public Task<int> UpdateStatus(string tranId, TranStatus status)
        {
            return _fsql.Update<SagaTranUnit>()
                 .Set(x => x.Status, status)
                 .Where(x => x.TranId == tranId)
                 .ExecuteAffrowsAsync();
        }

        public Task<List<SagaTranUnit>> List(string tranId)
        {
            return _fsql.Select<SagaTranUnit>()
                .Where(x => x.TranId == tranId)
                .OrderBy(x => x.OrderNo)
                .ToListAsync();
        }
    }
}
