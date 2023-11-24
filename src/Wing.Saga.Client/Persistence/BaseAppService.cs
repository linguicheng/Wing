using Microsoft.Extensions.Logging;
using System;
using Wing.Converter;
using Wing.EventBus;

namespace Wing.Saga.Client.Persistence
{
    public abstract class BaseAppService
    {
        protected readonly IEventBus _eventBus;

        protected readonly ILogger<BaseAppService> _logger;

        protected readonly IJson _json;

        protected readonly bool _useEventBus;

        public BaseAppService(ILogger<BaseAppService> logger, IJson json)
        {
            _logger = logger;
            _json = json;
            _useEventBus = App.Configuration["Saga:UseEventBus"] == "True";
            if (_useEventBus)
            {
                _eventBus = App.GetRequiredService<IEventBus>();
            }
        }

        protected void Publish(EventMessage message, string errorMsg)
        {
            try
            {
                _eventBus.Publish(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Saga发布{0}消息异常，内容为：{1}", errorMsg, _json.Serialize(message));
                throw;
            }
        }

        protected async Task<bool> Handler(EventMessage message, Func<EventMessage, Task<int>> func, string errorMsg)
        {
            try
            {
                var result = await func(message);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Saga{0}持久化异常，内容为：{1}", errorMsg, _json.Serialize(message));
                throw;
            }
        }
    }
}
