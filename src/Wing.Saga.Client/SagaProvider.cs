using System.Data.Common;
using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.Persistence.Saga;
using Wing.Saga.Client.Persistence;

namespace Wing.Saga.Client
{
    public class SagaProvider
    {
        private readonly SagaTran _tran;

        private readonly ILogger<SagaProvider> _logger;

        private readonly ISagaTranAppService _tranAppService;

        private readonly ISagaTranUnitAppService _tranUnitAppService;

        private SagaResult _previousResult;

        private int _previousOrder;

        private DbTransaction _transaction;

        public SagaProvider(SagaTran tran, int previousOrder, SagaResult previousResult, DbTransaction transaction)
        {
            _tran = tran;
            _previousOrder = previousOrder;
            _logger = App.GetRequiredService<ILogger<SagaProvider>>();
            _previousResult = previousResult;
            _tranAppService = App.GetRequiredService<ISagaTranAppService>();
            _tranUnitAppService = App.GetRequiredService<ISagaTranUnitAppService>();
            _transaction = transaction;
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
                _tranUnitAppService.Add(tranUnit, "事务单元", _transaction).GetAwaiter().GetResult();
                return new SagaProvider(_tran, _previousOrder, _previousResult, _transaction);
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
            _tranUnitAppService.Add(tranUnit, "事务单元", _transaction).GetAwaiter().GetResult();
            return new SagaProvider(_tran, _previousOrder, _previousResult, _transaction);
        }

        public SagaResult End()
        {
            if (_tran.Status == TranStatus.Executing)
            {
                return null;
            }

            _tran.EndTime = DateTime.Now;
            _tran.UsedMillSeconds = Convert.ToInt64((_tran.EndTime - _tran.BeginTime).TotalMilliseconds);
            _tranAppService.UpdateStatus(new UpdateTranStatusEvent
            {
                Id = _tran.Id,
                EndTime = DateTime.Now,
                Status = _tran.Status,
                UsedMillSeconds = Convert.ToInt64((_tran.EndTime - _tran.BeginTime).TotalMilliseconds)
            }, "事务").GetAwaiter().GetResult();
            return _previousResult;
        }
    }
}
