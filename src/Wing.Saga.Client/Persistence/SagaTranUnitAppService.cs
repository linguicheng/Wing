using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.Persistence.Saga;

namespace Wing.Saga.Client.Persistence
{
    public class SagaTranUnitAppService : BaseAppService, ISagaTranUnitAppService
    {
        private ISagaTranUnitService _service;

        public SagaTranUnitAppService(ILogger<BaseAppService> logger, IJson json)
            : base(logger, json)
        {
            if (!_useEventBus)
            {
                _service = App.GetRequiredService<ISagaTranUnitService>();
            }
        }

        public async Task<bool> Add(SagaTranUnit sagaTran, string action)
        {
            if (_useEventBus)
            {
                Publish(sagaTran, action);
                return true;
            }

            return await Handler(sagaTran, async x => await _service.Add(sagaTran), action);
        }

        public async Task<bool> RetryCancel(RetryCancelTranUnitEvent eventMessage, string action)
        {
            if (_useEventBus)
            {
                Publish(eventMessage, action);
                return true;
            }

            return await Handler(eventMessage, async x => await _service.RetryCancel(eventMessage), action);
        }

        public async Task<bool> RetryCommit(RetryCommitTranUnitEvent eventMessage, string action)
        {
            if (_useEventBus)
            {
                Publish(eventMessage, action);
                return true;
            }

            return await Handler(eventMessage, async x => await _service.RetryCommit(eventMessage), action);
        }

        public async Task<bool> UpdateStatus(UpdateTranUnitStatusEvent eventMessage, string action)
        {
            if (_useEventBus)
            {
                Publish(eventMessage, action);
                return true;
            }

            return await Handler(eventMessage, async x => await _service.UpdateStatus(eventMessage), action);
        }
    }
}
