using Wing.EventBus;

namespace AspNetCoreService.EventBus
{
    public class User: EventMessage
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
