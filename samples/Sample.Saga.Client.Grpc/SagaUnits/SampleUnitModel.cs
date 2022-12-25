using System;
using Wing.Saga.Client;

namespace Sample.Saga.Client.Grpc
{
    [Serializable]
    public class SampleUnitModel : UnitModel
    {
        public string HelloName { get; set; }
    }
}
