using System.Data.Common;
using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.Persistence.Saga;

namespace Wing.Saga.Client.Persistence
{
    public class SagaTranAppService : BaseAppService, ISagaTranAppService
    {
        private ISagaTranService _sagaTranService;

        public SagaTranAppService(ILogger<BaseAppService> logger, IJson json)
            : base(logger, json)
        {
            if (!_useEventBus)
            {
                _sagaTranService = App.GetRequiredService<ISagaTranService>();
            }
        }

        public async Task<bool> Add(SagaTran sagaTran, string action, DbTransaction transaction)
        {
            if (_useEventBus)
            {
                Publish(sagaTran, action);
                return true;
            }

            var result = await _sagaTranService.Add(sagaTran, transaction);
            return result > 0;
        }

        public async Task<bool> RetryCancel(RetryCancelTranEvent eventMessage, string action)
        {
            if (_useEventBus)
            {
                Publish(eventMessage, action);
                return true;
            }

            var result = await _sagaTranService.RetryCancel(eventMessage);
            return result > 0;
        }

        public async Task<bool> RetryCommit(RetryCommitTranEvent eventMessage, string action)
        {
            if (_useEventBus)
            {
                Publish(eventMessage, action);
                return true;
            }

            var result = await _sagaTranService.RetryCommit(eventMessage);
            return result > 0;
        }

        public async Task<bool> UpdateStatus(UpdateTranStatusEvent eventMessage, string action)
        {
            if (_useEventBus)
            {
                Publish(eventMessage, action);
                return true;
            }

            var result = await _sagaTranService.UpdateStatus(eventMessage);
            return result > 0;
        }
    }
}
