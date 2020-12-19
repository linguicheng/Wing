using System;
using System.Collections.Generic;
using System.Linq;
using Wing.Exceptions;
using Wing.ServiceProvider;

namespace Wing.LoadBalancer
{
    public class WeightRoundRobin : BaseLoadBalancer
    {
        private int _totalWeight = 0;

        public WeightRoundRobin(List<Service> services)
            : base(services)
        {
            Services.ForEach(s =>
            {
                _totalWeight += s.EffectiveWeight;
            });
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
                Services.ForEach(s =>
                {
                    s.CurrentWeight += s.EffectiveWeight;
                });
                var service = Services.OrderByDescending(s => s.CurrentWeight).FirstOrDefault();
                if (service == null)
                {
                    return null;
                }

                service.CurrentWeight -= _totalWeight;
                return service.ServiceAddress;
            }
        }

        /// <summary>
        /// 调用服务成功后调用此方法
        /// </summary>
        /// <param name="serviceAddress">服务地址</param>
        public void AddWeight(ServiceAddress serviceAddress)
        {
            var service = Services.Where(s => s.ServiceAddress == serviceAddress).FirstOrDefault();
            lock (_lock)
            {
                if (service == null || service.EffectiveWeight == service.Weight)
                {
                    return;
                }

                service.EffectiveWeight++;
            }
        }

        /// <summary>
        /// 调用服务失败后调用此方法
        /// </summary>
        /// <param name="serviceAddress">服务地址</param>
        public void ReduceWeight(ServiceAddress serviceAddress)
        {
            var service = Services.Where(s => s.ServiceAddress == serviceAddress).FirstOrDefault();
            if (service == null)
            {
                return;
            }

            lock (_lock)
            {
                if (service.EffectiveWeight <= 0)
                {
                    Services.Remove(service);
                }

                service.EffectiveWeight--;
            }
        }

        protected override void UpdateServiceAfter()
        {
            _totalWeight = 0;
            Services.ForEach(s =>
            {
                _totalWeight += s.EffectiveWeight;
            });
        }
    }
}
