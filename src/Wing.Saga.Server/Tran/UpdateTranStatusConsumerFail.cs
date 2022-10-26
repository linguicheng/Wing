using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Wing.Converter;
using Wing.EventBus;
using Wing.Injection;
using Wing.Persistence.Saga;

namespace Wing.Saga.Server
{
    [Subscribe(QueueMode.DLX)]
    public class UpdateTranStatusConsumerFail : UpdateTranStatusConsumer
    {
    }
}
