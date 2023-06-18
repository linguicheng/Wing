using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Wing.Exceptions;
using Wing.ServiceProvider;

namespace Wing.LoadBalancer
{
    public class LeastConnection : BaseLoadBalancer
    {
        private readonly List<Lease> _leases;

        public LeastConnection(List<Service> services)
            : base(services)
        {
            _leases = new List<Lease>();
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
                UpdateServices();
                var leaseConnection = GetLease();
                _leases.Remove(leaseConnection);
                leaseConnection = new Lease(leaseConnection.ServiceAddress, leaseConnection.Connections + 1);
                _leases.Add(leaseConnection);
                return leaseConnection.ServiceAddress;
            }
        }

        /// <summary>
        /// 调用服务成功后调用此方法
        /// </summary>
        /// <param name="serviceAddress">服务地址</param>
        public void ReLease(ServiceAddress serviceAddress)
        {
            lock (_lock)
            {
                var matchingLease = _leases.FirstOrDefault(l => l.ServiceAddress == serviceAddress);

                if (matchingLease != null)
                {
                    var replacementLease = new Lease(serviceAddress, matchingLease.Connections - 1);

                    _leases.Remove(matchingLease);

                    _leases.Add(replacementLease);
                }
            }
        }

        private void UpdateServices()
        {
            if (_leases.Count > 0)
            {
                var leasesToRemove = new List<Lease>();

                foreach (var lease in _leases)
                {
                    var match = Services.FirstOrDefault(s => s.ServiceAddress == lease.ServiceAddress);

                    if (match == null)
                    {
                        leasesToRemove.Add(lease);
                    }
                }

                foreach (var lease in leasesToRemove)
                {
                    _leases.Remove(lease);
                }

                foreach (var service in Services)
                {
                    var exists = _leases.FirstOrDefault(l => l.ServiceAddress == service.ServiceAddress);

                    if (exists == null)
                    {
                        _leases.Add(new Lease(service.ServiceAddress, 0));
                    }
                }
            }
            else
            {
                foreach (var service in Services)
                {
                    _leases.Add(new Lease(service.ServiceAddress, 0));
                }
            }
        }

        private Lease GetLease()
        {
            Lease leaseWithLeastConnections = null;

            for (var i = 0; i < _leases.Count; i++)
            {
                if (i == 0)
                {
                    leaseWithLeastConnections = _leases[i];
                }
                else
                {
                    if (_leases[i].Connections < leaseWithLeastConnections.Connections)
                    {
                        leaseWithLeastConnections = _leases[i];
                    }
                }
            }

            return leaseWithLeastConnections;
        }

        private class Lease
        {
            public Lease(ServiceAddress serviceAddress, int connections)
            {
                ServiceAddress = serviceAddress;
                Connections = connections;
            }

            public ServiceAddress ServiceAddress { get; private set; }

            public int Connections { get; private set; }
        }
    }
}
