using System.Data.Common;
using System.Transactions;
using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.EventBus;
using Wing.Persistence.Saga;
using Wing.ServiceProvider;

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

        public async Task<bool> Add(SagaTran sagaTran, string action)
        {
            if (_useEventBus)
            {
                Publish(sagaTran, action);
                return true;
            }

            return await Handler(sagaTran, async x => await _sagaTranService.Add(sagaTran), action);
        }

        public async Task<bool> RetryCancel(RetryCancelTranEvent eventMessage, string action)
        {
            if (_useEventBus)
            {
                Publish(eventMessage, action);
                return true;
            }

            return await Handler(eventMessage, async x => await _sagaTranService.RetryCancel(eventMessage), action);
        }

        public async Task<bool> RetryCommit(RetryCommitTranEvent eventMessage, string action)
        {
            if (_useEventBus)
            {
                Publish(eventMessage, action);
                return true;
            }

            return await Handler(eventMessage, async x => await _sagaTranService.RetryCommit(eventMessage), action);
        }

        public async Task<bool> UpdateStatus(UpdateTranStatusEvent eventMessage, string action)
        {
            if (_useEventBus)
            {
                Publish(eventMessage, action);
                return true;
            }

            return await Handler(eventMessage, async x => await _sagaTranService.UpdateStatus(eventMessage), action);
        }
    }
}
