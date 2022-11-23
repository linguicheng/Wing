namespace Wing.Persistence.Saga
{
    public class RetryCommitTranUnitEvent : RetryEvent
    {
        public string ErrorMsg { get; set; }
    }
}
