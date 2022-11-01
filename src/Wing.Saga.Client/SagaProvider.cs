using System;
using Wing.Converter;
using Wing.EventBus;
using Wing.Persistence.Saga;
using Wing.ServiceProvider;

namespace Wing.Saga.Client
{
    public class SagaProvider
    {
        public SagaResult Result { get; set; }
        private readonly SagaTran Tran;
        private int PreviousOrder;
        private readonly IEventBus _eventBus;
        private readonly IJson _json;
        public SagaProvider(SagaTran tran, int previousOrder = 0)
        {
            Tran = tran;
            PreviousOrder = previousOrder;
            _eventBus = ServiceLocator.GetRequiredService<IEventBus>();
        }

        public SagaProvider Then<TSagaUnit, TUnitModel>(TSagaUnit sagaUnit, TUnitModel unitModel)
            where TSagaUnit : SagaUnit<TUnitModel>, new()
            where TUnitModel : UnitModel, new()
        {
            if (Tran.Status == TranStatus.Failed)
            {
                return new SagaProvider(Tran, PreviousOrder);
            }

            PreviousOrder++;
            var tranUnit = new SagaTranUnit
            {
                Id = Guid.NewGuid().ToString(),
                TranId = Tran.Id,
                CreatedTime = DateTime.Now,
                BeginTime = DateTime.Now,
                Description = unitModel.Description,
                Name = unitModel.Name,
                OrderNo = PreviousOrder,
                ParamsValue = _json.Serialize(unitModel),
                ParamsNamespace = typeof(TUnitModel).FullName
            };
            var result = sagaUnit.Commit(unitModel);
            Tran.Status = tranUnit.Status = result.Success ? TranStatus.Success : TranStatus.Failed;
            tranUnit.ErrorMsg = result.Msg;
            tranUnit.EndTime = DateTime.Now;
            tranUnit.UsedMillSeconds = Convert.ToInt64((tranUnit.EndTime - tranUnit.BeginTime).TotalMilliseconds);
            Result = result;
            _eventBus.Publish(tranUnit);
            // 保存事务单元执行结果到数据库
            return new SagaProvider(Tran, PreviousOrder);
        }

        public SagaResult End()
        {
            if (Tran.Status == TranStatus.Executing)
            {
                return null;
            }
            Tran.EndTime = DateTime.Now;
            Tran.UsedMillSeconds = Convert.ToInt64((Tran.EndTime - Tran.BeginTime).TotalMilliseconds);
            _eventBus.Publish(new UpdateTranStatusDto
            {
                Id = Tran.Id,
                EndTime = DateTime.Now,
                Status = Tran.Status,
                UsedMillSeconds = Convert.ToInt64((Tran.EndTime - Tran.BeginTime).TotalMilliseconds)
            });
            return Result;
        }
    }
}
