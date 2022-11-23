using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.EventBus;
using Wing.Injection;
using Wing.Persistence.Saga;
using Wing.Saga;

namespace Wing.Saga.Client
{
    public class TranRetryService : TranRetry.TranRetryBase
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<TranRetryService> _logger;
        private readonly IJson _json;

        public TranRetryService(IEventBus eventBus, ILogger<TranRetryService> logger, IJson json)
        {
            _eventBus = eventBus;
            _logger = logger;
            _json = json;
        }

        public override async Task<ResponseData> Commit(RetryData retryData, ServerCallContext context)
        {
            SagaResult previousResult = null;
            var tranEvent = new RetryCommitTranEvent
            {
                Id = retryData.TranId,
                BeginTime = DateTime.Now
            };
            ResponseData result = new ResponseData();
            foreach (var item in retryData.SagaTranUnits)
            {
                SagaResult sagaResult;
                var tranUnitEvent = new RetryCommitTranUnitEvent { Id = item.Id };
                try
                {
                    var unitType = GlobalInjection.GetType(item.UnitNamespace);
                    var unitObj = Activator.CreateInstance(unitType);
                    var commit = unitType.GetMethod("Commit");
                    var unitModel = DataConverter.BytesToObject(item.ParamsValue.ToByteArray());
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

                tranUnitEvent.EndTime = DateTime.Now;
                tranUnitEvent.ErrorMsg = sagaResult.Msg;
                if (sagaResult.Success)
                {
                    previousResult = sagaResult;
                    tranUnitEvent.Status = TranStatus.Success;
                    result = Publish(tranUnitEvent, "事务单元提交");
                    if (!result.Success)
                    {
                        return result;
                    }

                    continue;
                }

                tranUnitEvent.Status = TranStatus.Failed;
                Publish(tranUnitEvent, "事务单元提交");
                tranEvent.EndTime = DateTime.Now;
                tranEvent.Status = TranStatus.Failed;
                Publish(tranEvent, "事务提交");
                result.Success = false;
                result.Msg = sagaResult.Msg;
                return result;
            }

            tranEvent.EndTime = DateTime.Now;
            tranEvent.Status = TranStatus.Success;
            return Publish(tranEvent, "事务提交");
        }

        public override async Task<ResponseData> Cancel(RetryData retryData, ServerCallContext context)
        {
            SagaResult previousResult = null;
            var tranEvent = new RetryCancelTranEvent
            {
                Id = retryData.TranId,
                BeginTime = DateTime.Now
            };
            ResponseData result = new ResponseData();
            foreach (var item in retryData.SagaTranUnits)
            {
                SagaResult sagaResult;
                var tranUnitEvent = new RetryCancelTranUnitEvent { Id = item.Id };
                try
                {
                    var unitType = GlobalInjection.GetType(item.UnitNamespace);
                    var unitObj = Activator.CreateInstance(unitType);
                    var commit = unitType.GetMethod("Cancel");
                    var unitModel = DataConverter.BytesToObject(item.ParamsValue.ToByteArray());
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

                tranUnitEvent.EndTime = DateTime.Now;
                tranUnitEvent.ErrorMsg = sagaResult.Msg;
                if (sagaResult.Success)
                {
                    previousResult = sagaResult;
                    tranUnitEvent.Status = TranStatus.Cancelled;
                    result = Publish(tranUnitEvent, "事务单元取消");
                    if (!result.Success)
                    {
                        return result;
                    }

                    continue;
                }

                tranUnitEvent.Status = TranStatus.Failed;
                Publish(tranUnitEvent, "事务单元取消");
                tranEvent.EndTime = DateTime.Now;
                tranEvent.Status = TranStatus.Failed;
                Publish(tranEvent, "事务取消");
                result.Success = false;
                result.Msg = sagaResult.Msg;
                return result;
            }

            tranEvent.EndTime = DateTime.Now;
            tranEvent.Status = TranStatus.Cancelled;
            return Publish(tranEvent, "事务取消");
        }

        private ResponseData Publish(EventMessage message, string errorMsg)
        {
            var result = new ResponseData() { Success = true };
            try
            {
                _eventBus.Publish(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Saga重试服务发布{0}消息异常，内容为：{1}", errorMsg, _json.Serialize(message));
                result.Success = false;
                result.Msg = ex.Message;
            }

            return result;
        }
    }
}
