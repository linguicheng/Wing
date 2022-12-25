namespace Wing.Persistence.Saga
{
    public class RetryTranUnit
    {
        public string Id { get; set; }

        public byte[] ParamsValue { get; set; }

        public string UnitNamespace { get; set; }
    }
}
