namespace Wing.Saga.Client
{
    /// <summary>
    /// 事务单元
    /// </summary>
    public abstract class SagaUnit<T> where T : UnitModel, new()
    {
        public abstract SagaResult Commit(T model);
        public abstract SagaResult Cancel(T model);
    }
}
