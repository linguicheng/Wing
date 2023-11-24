namespace Wing.Persistence.Saga
{
    public class RetryTranUnit
    {
        public string Id { get; set; }

        public string ParamsValue { get; set; }

        public string UnitNamespace { get; set; }

        public string UnitModelNamespace { get; set; }
    }
}
