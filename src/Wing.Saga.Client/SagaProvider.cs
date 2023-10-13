using System;
using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.EventBus;
using Wing.Persistence.Saga;

namespace Wing.Saga.Client
{
    public class SagaProvider
    {
        private readonly SagaTran _tran;

        private readonly IEventBus _eventBus;

        private readonly ILogger<SagaProvider> _logger;

        private readonly IJson _json;

        private SagaResult _previousResult;

        private int _previousOrder;

        public SagaProvider(SagaTran tran, int previousOrder, SagaResult previousResult)
        {
            _tran = tran;
            _previousOrder = previousOrder;
            _eventBus = App.GetRequiredService<IEventBus>();
            _logger = App.GetRequiredService<ILogger<SagaProvider>>();
            _json = App.GetRequiredService<IJson>();
            _previousResult = previousResult;
        }

        public SagaProvider Then<TSagaUnit, TUnitModel>(TSagaUnit sagaUnit, TUnitModel unitModel)
            where TSagaUnit : SagaUnit<TUnitModel>, new()
            where TUnitModel : UnitModel, new()
        {
            _previousOrder++;
            var tranUnit = new SagaTranUnit
            {
                Id = Guid.NewGuid().ToString(),
                TranId = _tran.Id,
                CreatedTime = DateTime.Now,
                BeginTime = DateTime.Now,
                Description = unitModel.Description,
                Name = unitModel.Name,
                OrderNo = _previousOrder,
                ParamsValue = DataConverter.ObjectToBytes(unitModel),
                UnitNamespace = typeof(TSagaUnit).FullName,
                UnitModelNamespace = typeof(TUnitModel).FullName
            };

            if (_tran.Status == TranStatus.Failed)
            {
                tranUnit.EndTime = DateTime.Now;
                tranUnit.UsedMillSeconds = 0;
                tranUnit.Status = TranStatus.Failed;
                Publish(tranUnit, "事务单元");
                return new SagaProvider(_tran, _previousOrder, _previousResult);
            }

            SagaResult result;
            try
            {
                result = sagaUnit.Commit(unitModel, _previousResult).Result;
            }
            catch (Exception ex)
            {
                result = new SagaResult
                {
                    Success = false,
                    Msg = ex.Message
                };
                _logger.LogError(ex, "Saga提交事务单元异常");
            }

            _tran.Status = tranUnit.Status = result.Success ? TranStatus.Success : TranStatus.Failed;
            tranUnit.ErrorMsg = result.Msg;
            tranUnit.EndTime = DateTime.Now;
            tranUnit.UsedMillSeconds = Convert.ToInt64((tranUnit.EndTime - tranUnit.BeginTime).TotalMilliseconds);
            _previousResult = result;
            Publish(tranUnit, "事务单元");
            return new SagaProvider(_tran, _previousOrder, _previousResult);
        }

        public SagaResult End()
        {
            if (_tran.Status == TranStatus.Executing)
            {
                return null;
            }

            _tran.EndTime = DateTime.Now;
            _tran.UsedMillSeconds = Convert.ToInt64((_tran.EndTime - _tran.BeginTime).TotalMilliseconds);
            Publish(new UpdateTranStatusEvent
            {
                Id = _tran.Id,
                EndTime = DateTime.Now,
                Status = _tran.Status,
                UsedMillSeconds = Convert.ToInt64((_tran.EndTime - _tran.BeginTime).TotalMilliseconds)
            }, "事务");
            return _previousResult;
        }

        private void Publish(EventMessage message, string errorMsg)
        {
            try
            {
                _eventBus.Publish(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Saga发布{0}消息异常，内容为：{1}", errorMsg, _json.Serialize(message));
                throw;
            }
        }
    }
}
