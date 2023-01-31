using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wing.Model;
using Wing.Result;

namespace Wing.Persistence.Saga
{
    public class SagaTranService : ISagaTranService
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

        public List<SagaTran> GetFailedData()
        {
            return _fsql.Select<SagaTran>().Where(x => x.Status == TranStatus.Failed).ToList();
        }

        public Task<int> RetryCommit(RetryCommitTranEvent dto)
        {
            return _fsql.Update<SagaTran>(dto.Id)
                 .SetIf(dto.RetryResult == ExecutedResult.Success, x => x.Status, TranStatus.Success)
                 .Set(x => x.BeginTime, dto.BeginTime)
                 .Set(x => x.EndTime, dto.EndTime)
                 .Set(x => x.UsedMillSeconds, dto.UsedMillSeconds)
                 .Set(x => x.RetryResult, dto.RetryResult)
                 .Set(x => x.RetryAction, dto.RetryAction)
                 .Set(x => x.CommittedCount + 1)
                 .ExecuteAffrowsAsync();
        }

        public Task<int> RetryCancel(RetryCancelTranEvent dto)
        {
            return _fsql.Update<SagaTran>(dto.Id)
                 .SetIf(dto.RetryResult == ExecutedResult.Success, x => x.Status, TranStatus.Cancelled)
                 .Set(x => x.BeginTime, dto.BeginTime)
                 .Set(x => x.EndTime, dto.EndTime)
                 .Set(x => x.UsedMillSeconds, dto.UsedMillSeconds)
                 .Set(x => x.RetryResult, dto.RetryResult)
                 .Set(x => x.RetryAction, dto.RetryAction)
                 .Set(x => x.CancelledCount + 1)
                 .ExecuteAffrowsAsync();
        }

        public Task<int> UpdateStatus(UpdateStatusEvent dto)
        {
            return _fsql.Update<SagaTran>(dto.Id)
                 .Set(x => x.Status, dto.Status)
                 .Set(x => x.EndTime, dto.EndTime)
                 .Set(x => x.UsedMillSeconds, dto.UsedMillSeconds)
                 .ExecuteAffrowsAsync();
        }

        public Task<int> UpdateStatus(string id, TranStatus status)
        {
            return _fsql.Update<SagaTran>(id)
                 .Set(x => x.Status, status)
                 .ExecuteAffrowsAsync();
        }

        public async Task<PageResult<List<SagaTran>>> List(PageModel<SagaTranSearchDto> dto)
        {
            var result = await _fsql.Select<SagaTran>()
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.ServiceName), u => u.ServiceName.Contains(dto.Data.ServiceName))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.Name), u => u.Name.Contains(dto.Data.Name))
                    .WhereIf(dto.Data.Status != null, u => u.Status == dto.Data.Status)
                    .WhereIf(dto.Data.CreatedTime != null && dto.Data.CreatedTime.Count == 2, u => u.CreatedTime >= dto.Data.CreatedTime[0] && u.CreatedTime <= dto.Data.CreatedTime[1])
                    .OrderByDescending(x => x.CreatedTime)
                    .Count(out var total)
                    .Page(dto.PageIndex, dto.PageSize)
                    .ToListAsync();
            return new PageResult<List<SagaTran>>
            {
                TotalCount = total,
                Items = result
            };
        }

        public long GetFailedTotal()
        {
            return _fsql.Select<SagaTran>().Where(x => x.Status == TranStatus.Failed).Count();
        }

        public async Task<object> GetTop5FailedData()
        {
            var result = await _fsql.Select<SagaTran>()
                .GroupBy(a => new { a.Name })
                .WithTempQuery(a => new
                {
                    a.Value.Name,
                    FailedLv = Math.Round(_fsql.Select<SagaTran>().Where(b => b.Name == a.Value.Name && b.Status == TranStatus.Failed).Count() * 1.0 / a.Count(), 2)
                })
                .OrderByDescending(a => a.FailedLv)
                .Take(5)
                .From<SagaTran>()
                .InnerJoin((a, b) => a.Name == b.Name)
                .GroupBy((a, b) => new { b.Name, b.Status })
                .ToListAsync(x => new
                {
                    x.Value.Item2.Name,
                    x.Value.Item2.Status,
                    Count = x.Count()
                });
            return result;
        }
    }
}
