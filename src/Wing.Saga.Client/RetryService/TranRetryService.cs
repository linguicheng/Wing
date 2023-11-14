using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.EventBus;
using Wing.Injection;
using Wing.Persistence.Saga;
using Wing.Saga.Client.Persistence;

namespace Wing.Saga.Client
{
    public class TranRetryService : ITranRetryService, ISingleton
    {
        private readonly ILogger<TranRetryService> _logger;
        private readonly ISagaTranAppService _tranAppService;
        private readonly ISagaTranUnitAppService _tranUnitAppService;

        public TranRetryService(ILogger<TranRetryService> logger,
            ISagaTranAppService tranAppService,
            ISagaTranUnitAppService tranUnitAppService)
        {
            _logger = logger;
            _tranAppService = tranAppService;
            _tranUnitAppService = tranUnitAppService;
        }

        public async Task<ResponseData> Commit(RetryData retryData)
        {
            SagaResult previousResult = null;
            var tranEvent = new RetryCommitTranEvent
            {
                Id = retryData.TranId,
                BeginTime = DateTime.Now,
                RetryAction = "Commit"
            };
            ResponseData result = new();
            foreach (var item in retryData.SagaTranUnits)
            {
                SagaResult sagaResult;
                var tranUnitEvent = new RetryCommitTranUnitEvent { Id = item.Id };
                try
                {
                    var unitType = GlobalInjection.GetType(item.UnitNamespace);
                    var unitModelType = GlobalInjection.GetType(item.UnitModelNamespace);
                    var unitObj = Activator.CreateInstance(unitType);
                    var commit = unitType.GetMethod("Commit");
                    var unitModel = DataConverter.BytesToObject(item.ParamsValue, unitModelType);
                    tranUnitEvent.BeginTime = DateTime.Now;
                    sagaResult = await (commit.Invoke(unitObj, new object[] { unitModel, previousResult }) as Task<SagaResult>);
                }
                catch (Exception ex)
                {
                    sagaResult = new SagaResult
                    {
                        Success = false,
                        Msg = ex.Message
                    };
                    _logger.LogError(ex, "Saga重试服务提交事务单元异常");
                }

                tranUnitEvent.RetryAction = "Commit";
                tranUnitEvent.EndTime = DateTime.Now;
                tranUnitEvent.ErrorMsg = sagaResult.Msg;
                tranUnitEvent.UsedMillSeconds = Convert.ToInt64((tranUnitEvent.EndTime - tranUnitEvent.BeginTime).TotalMilliseconds);
                if (sagaResult.Success)
                {
                    previousResult = sagaResult;
                    tranUnitEvent.RetryResult = ExecutedResult.Success;
                    result.Success = await _tranUnitAppService.RetryCommit(tranUnitEvent, "事务单元提交");
                    if (!result.Success)
                    {
                        return result;
                    }

                    continue;
                }

                tranUnitEvent.RetryResult = ExecutedResult.Failed;
                await _tranUnitAppService.RetryCommit(tranUnitEvent, "事务单元提交");
                tranEvent.EndTime = DateTime.Now;
                tranEvent.RetryResult = ExecutedResult.Failed;
                tranEvent.UsedMillSeconds = Convert.ToInt64((tranEvent.EndTime - tranEvent.BeginTime).TotalMilliseconds);
                await _tranAppService.RetryCommit(tranEvent, "事务提交");
                result.Success = false;
                result.Msg = sagaResult.Msg;
                return result;
            }

            tranEvent.EndTime = DateTime.Now;
            tranEvent.RetryResult = ExecutedResult.Success;
            result.Success = await _tranAppService.RetryCommit(tranEvent, "事务提交");
            return result;
        }

        public async Task<ResponseData> Cancel(RetryData retryData)
        {
            SagaResult previousResult = null;
            var tranEvent = new RetryCancelTranEvent
            {
                Id = retryData.TranId,
                BeginTime = DateTime.Now,
                RetryAction = "Cancel"
            };
            ResponseData result = new();
            foreach (var item in retryData.SagaTranUnits)
            {
                SagaResult sagaResult;
                var tranUnitEvent = new RetryCancelTranUnitEvent { Id = item.Id };
                try
                {
                    var unitType = GlobalInjection.GetType(item.UnitNamespace);
                    var unitModelType = GlobalInjection.GetType(item.UnitModelNamespace);
                    var unitObj = Activator.CreateInstance(unitType);
                    var commit = unitType.GetMethod("Cancel");
                    var unitModel = DataConverter.BytesToObject(item.ParamsValue, unitModelType);
                    tranUnitEvent.BeginTime = DateTime.Now;
                    sagaResult = await (commit.Invoke(unitObj, new object[] { unitModel, previousResult }) as Task<SagaResult>);
                }
                catch (Exception ex)
                {
                    sagaResult = new SagaResult
                    {
                        Success = false,
                        Msg = ex.Message
                    };
                    _logger.LogError(ex, "Saga重试服务取消事务单元异常");
                }

                tranUnitEvent.RetryAction = "Cancel";
                tranUnitEvent.EndTime = DateTime.Now;
                tranUnitEvent.ErrorMsg = sagaResult.Msg;
                tranUnitEvent.UsedMillSeconds = Convert.ToInt64((tranUnitEvent.EndTime - tranUnitEvent.BeginTime).TotalMilliseconds);
                if (sagaResult.Success)
                {
                    previousResult = sagaResult;
                    tranUnitEvent.RetryResult = ExecutedResult.Success;
                    result.Success = await _tranUnitAppService.RetryCancel(tranUnitEvent, "事务单元取消");
                    if (!result.Success)
                    {
                        return result;
                    }

                    continue;
                }

                tranUnitEvent.RetryResult = ExecutedResult.Failed;
                await _tranUnitAppService.RetryCancel(tranUnitEvent, "事务单元取消");
                tranEvent.EndTime = DateTime.Now;
                tranEvent.RetryResult = ExecutedResult.Failed;
                tranEvent.UsedMillSeconds = Convert.ToInt64((tranEvent.EndTime - tranEvent.BeginTime).TotalMilliseconds);
                await _tranAppService.RetryCancel(tranEvent, "事务取消");
                result.Success = false;
                result.Msg = sagaResult.Msg;
                return result;
            }

            tranEvent.EndTime = DateTime.Now;
            tranEvent.RetryResult = ExecutedResult.Success;
            result.Success = await _tranAppService.RetryCancel(tranEvent, "事务取消");
            return result;
        }
    }
}
