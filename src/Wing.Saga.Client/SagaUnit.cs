using System.Threading.Tasks;

namespace Wing.Saga.Client
{
    /// <summary>
    /// 事务单元
    /// </summary>
    public abstract class SagaUnit<T>
        where T : UnitModel, new()
    {
        public abstract Task<SagaResult> Commit(T model, SagaResult previousResult);

        public abstract Task<SagaResult> Cancel(T model, SagaResult previousResult);
    }
}
