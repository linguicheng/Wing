using Wing.Exceptions;
using Wing.ServiceProvider;

namespace Wing.LoadBalancer
{
    public class ConsistentHash : BaseLoadBalancer
    {
        private readonly SortedList<long, VirtualNode> _hashRing = new ();

        public ConsistentHash(List<Service> services)
            : base(services)
        {
        }

        public override ServiceAddress GetServiceAddress(string key)
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
                Services.ForEach(x => AddNode(x.ServiceAddress, 3));
                var hashcode = Tools.GetHashCode(key);
                foreach (var item in _hashRing)
                {
                    if (hashcode < item.Key)
                    {
                        return item.Value.PhysicalNode;
                    }
                }

                return _hashRing.First().Value.PhysicalNode;
            }
        }

        public override ServiceAddress GetServiceAddress()
        {
            throw new NotImplementedException();
        }

        private void AddNode(ServiceAddress physicalNode, int virtualNodeCount)
        {
            for (int i = 0; i < virtualNodeCount; i++)
            {
                var vNode = new VirtualNode(physicalNode, i);
                _hashRing.Add(Tools.GetHashCode(vNode.ToString()), vNode);
            }
        }
    }

    public class VirtualNode
    {
        public ServiceAddress PhysicalNode { get; private set; }

        public int VirtualNodeCount { get; set; }

        public VirtualNode(ServiceAddress physicalNode, int virtualNodeCount)
        {
            PhysicalNode = physicalNode;
            VirtualNodeCount = virtualNodeCount;
        }

        public bool IsExists(ServiceAddress serviceAddress)
        {
            return PhysicalNode == serviceAddress;
        }

        public override string ToString()
        {
            return $"{PhysicalNode.ToString().ToLower()}:{VirtualNodeCount}";
        }
    }
}
