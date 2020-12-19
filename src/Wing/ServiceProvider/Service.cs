using System.Collections.Generic;
using Wing.Configuration;

namespace Wing.ServiceProvider
{
    public class Service
    {
        public Service(
            string id,
            string name,
            IEnumerable<string> tags,
            ServiceAddress serviceAddress)
        {
            Id = id;
            Name = name;
            Tags = tags;
            ServiceAddress = serviceAddress;
            ServiceTool.GetServiceTagConfig(tags, ServiceTag.WEIGHT, w =>
            {
                int.TryParse(w, out int weight);
                EffectiveWeight = Weight = weight;
            });
        }

        public string Id { get; }

        public string Name { get; }

        public IEnumerable<string> Tags { get; }

        public ServiceAddress ServiceAddress { get; }

        public int Weight { get; private set; }

        internal int EffectiveWeight { get; set; }

        internal int CurrentWeight { get; set; }
    }
}
