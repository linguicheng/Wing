namespace Wing.Persistence.Saga
{
    public class RetryCancelTranUnitEvent : RetryEvent
    {
        public string ErrorMsg { get; set; }
    }
}
