using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wing.GateWay.Middleware
{
    public class MiddlewareBuilder : IMiddlewareBuilder
    {
        private readonly IList<Func<ServiceRequestDelegate, ServiceRequestDelegate>> _components = new List<Func<ServiceRequestDelegate, ServiceRequestDelegate>>();

        public IServiceProvider ApplicationServices { get; }

        public MiddlewareBuilder(IServiceProvider serviceProvider)
        {
            ApplicationServices = serviceProvider;
        }

        public MiddlewareBuilder(IMiddlewareBuilder middlewareBuilder)
        {
            ApplicationServices = middlewareBuilder.ApplicationServices;
        }

        public ServiceRequestDelegate Build()
        {
            ServiceRequestDelegate app = serviceContext =>
            {
                serviceContext.HttpContext.Response.StatusCode = 404;
                return Task.CompletedTask;
            };

            foreach (var component in _components.Reverse())
            {
                app = component(app);
            }

            return app;
        }

        public IMiddlewareBuilder New()
        {
            return new MiddlewareBuilder(this);
        }

        public IMiddlewareBuilder Use(Func<ServiceRequestDelegate, ServiceRequestDelegate> middleware)
        {
            _components.Add(middleware);
            return this;
        }
    }
}
