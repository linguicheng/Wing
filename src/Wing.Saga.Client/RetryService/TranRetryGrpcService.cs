using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Wing.Saga.Grpc;

namespace Wing.Saga.Client
{
    public class TranRetryGrpcService : TranRetry.TranRetryBase
    {
        private readonly ITranRetryService _tranRetryService;

        public TranRetryGrpcService(ITranRetryService tranRetryService)
        {
            _tranRetryService = tranRetryService;
        }

        public override async Task<ResponseData> Commit(RetryData retryData, ServerCallContext context)
        {
            var requestData = new Persistence.Saga.RetryData
            {
                TranId = retryData.TranId,
                SagaTranUnits = new List<Persistence.Saga.RetryTranUnit>()
            };
            retryData.SagaTranUnits.ToList().ForEach(x => requestData.SagaTranUnits.Add(new Persistence.Saga.RetryTranUnit
            {
                Id = x.Id,
                ParamsValue = x.ParamsValue.ToByteArray(),
                UnitNamespace = x.UnitNamespace
            }));
            var result = await _tranRetryService.Commit(requestData);
            return new ResponseData { Success = result.Success, Msg = result.Msg };
        }

        public override async Task<ResponseData> Cancel(RetryData retryData, ServerCallContext context)
        {
            var requestData = new Persistence.Saga.RetryData
            {
                TranId = retryData.TranId,
                SagaTranUnits = new List<Persistence.Saga.RetryTranUnit>()
            };
            retryData.SagaTranUnits.ToList().ForEach(x => requestData.SagaTranUnits.Add(new Persistence.Saga.RetryTranUnit
            {
                Id = x.Id,
                ParamsValue = x.ParamsValue.ToByteArray(),
                UnitNamespace = x.UnitNamespace
            }));
            var result = await _tranRetryService.Cancel(requestData);
            return new ResponseData { Success = result.Success, Msg = result.Msg };
        }
    }
}
