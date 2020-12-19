using System;

namespace Wing.EventBus
{
    public abstract class EventMessage
    {
        public EventMessage()
        {
            EventId = Guid.NewGuid();
            CreateTime = DateTime.Now;
        }

        public Guid EventId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
