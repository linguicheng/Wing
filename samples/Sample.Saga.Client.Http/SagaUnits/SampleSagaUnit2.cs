using System.Threading.Tasks;
using Wing.Saga.Client;

namespace Sample.Saga.Client.Http
{
    public class SampleSagaUnit2 : SagaUnit<SampleUnitModel>
    {
        public override Task<SagaResult> Cancel(SampleUnitModel model, SagaResult previousResult)
        {
            return Task.FromResult(new SagaResult { Success = true });
        }

        public override Task<SagaResult> Commit(SampleUnitModel model, SagaResult previousResult)
        {
            return Task.FromResult(new SagaResult { Success = true });
        }
    }
}
