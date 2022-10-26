using System;
using System.Collections.Generic;
using System.Text;
using Wing.EventBus;
using Wing.Persistence.Saga;
using Wing.ServiceProvider;

namespace Wing.Saga.Client
{
    public class SagaProvider
    {
        private SagaTran Tran;
        private int PreviousOrder;
        public SagaProvider(SagaTran tran, int previousOrder = 0)
        {
            Tran = tran;
            PreviousOrder = previousOrder;
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
            unitModel.TranId = Tran.Id;
            var result = sagaUnit.Commit(unitModel);
            ServiceLocator.GetRequiredService<IEventBus>().Publish(log);
            // 保存事务单元执行结果到数据库
            return new SagaProvider(Tran, PreviousOrder);
        }

        public SagaResult End()
        {
            if (Tran.Status == TranStatus.Executing)
            {
                return null;
            }
            // 保存事务执行结果到数据库
            return PreviousResult;
        }
    }
}
