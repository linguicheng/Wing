using Microsoft.Extensions.Logging;
using System.Data.Common;
using Wing.Converter;
using Wing.EventBus;
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

        public async Task<bool> Add(SagaTranUnit sagaTran, string action, DbTransaction transaction)
        {
            if (_useEventBus)
            {
                Publish(sagaTran, action);
                return true;
            }

            var result = await _service.Add(sagaTran, transaction);
            return result > 0;
        }

        public async Task<bool> RetryCancel(RetryCancelTranUnitEvent eventMessage, string action)
        {
            if (_useEventBus)
            {
                Publish(eventMessage, action);
                return true;
            }

            var result = await _service.RetryCancel(eventMessage);
            return result > 0;
        }

        public async Task<bool> RetryCommit(RetryCommitTranUnitEvent eventMessage, string action)
        {
            if (_useEventBus)
            {
                Publish(eventMessage, action);
                return true;
            }

            var result = await _service.RetryCommit(eventMessage);
            return result > 0;
        }

        public async Task<bool> UpdateStatus(UpdateTranUnitStatusEvent eventMessage, string action)
        {
            if (_useEventBus)
            {
                Publish(eventMessage, action);
                return true;
            }

            var result = await _service.UpdateStatus(eventMessage);
            return result > 0;
        }
    }
}
