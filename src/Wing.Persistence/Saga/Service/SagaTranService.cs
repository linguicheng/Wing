using System.Data.Common;
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

        public Task<List<SagaTranStatusCount>> FailedDataRanking()
        {
            return _fsql.Select<SagaTranStatusCount>()
                .OrderByDescending(a => a.FaildCount * 1.0 / (a.SuccessCount + a.FaildCount + a.CancelledCount + a.ExecutingCount))
                .Take(5)
                .ToListAsync();
        }

        public int AddStatusCount()
        {
            var data = _fsql.Select<SagaTran>()
                .GroupBy(a => new { a.Name, a.ServiceName, a.Status })
                .ToList(a => new
                {
                    a.Value.Name,
                    a.Value.ServiceName,
                    a.Value.Status,
                    Count = a.Count()
                });
            if (data == null || data.Count == 0)
            {
                return 0;
            }

            var result = new List<SagaTranStatusCount>();
            data.ForEach(a =>
            {
                var item = result.Where(b => b.Name == a.Name && b.ServiceName == a.ServiceName).FirstOrDefault();
                if (item == null)
                {
                    item = new SagaTranStatusCount
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        Name = a.Name,
                        ServiceName = a.ServiceName,
                        CreatedTime = DateTime.Now
                    };
                    result.Add(item);
                }

                switch (a.Status)
                {
                    case TranStatus.Success: item.SuccessCount = a.Count; break;
                    case TranStatus.Failed: item.FaildCount = a.Count; break;
                    case TranStatus.Cancelled: item.CancelledCount = a.Count; break;
                    case TranStatus.Executing: item.ExecutingCount = a.Count; break;
                }
            });
            _fsql.Delete<SagaTranStatusCount>().Where("1=1").ExecuteAffrows();
            return _fsql.Insert(result).ExecuteAffrows();
        }
    }
}
