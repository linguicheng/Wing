using Wing.Exceptions;
using Wing.ServiceProvider;

namespace Wing.LoadBalancer
{
    public class RoundRobin : BaseLoadBalancer
    {
        private int _last;

        public RoundRobin(List<Service> services)
            : base(services)
        {
        }

        public override ServiceAddress GetServiceAddress()
        {
            if (Services == null)
            {
                throw new ArgumentNullException(nameof(Services));
            }

            if (Services.Count == 0)
            {
                throw new ArgumentEmptyException(nameof(Services));
            }

            lock (_lock)
            {
                if (_last >= Services.Count)
                {
                    _last = 0;
                }

                var next = Services[_last];
                _last++;
                return next.ServiceAddress;
            }
        }

        public override ServiceAddress GetServiceAddress(string key)
        {
            throw new NotImplementedException();
        }
    }
}
