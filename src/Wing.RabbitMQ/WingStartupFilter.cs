using System;
using Microsoft.AspNetCore.Builder;
using Wing.EventBus;
using Wing.Injection;

namespace Wing.RabbitMQ
{
    internal class WingStartupFilter
    {
        public Action<IApplicationBuilder> Configure(IEventBus eventBus)
        {
            return app =>
            {
                GlobalInjection.CreateSubscribe(eventBus); 
            };
        }
    }
}
