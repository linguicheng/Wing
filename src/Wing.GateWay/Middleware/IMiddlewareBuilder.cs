namespace Wing.Gateway.Middleware
{
    public interface IMiddlewareBuilder
    {
        IServiceProvider ApplicationServices { get; }

        IMiddlewareBuilder Use(Func<ServiceRequestDelegate, ServiceRequestDelegate> middleware);

        ServiceRequestDelegate Build();

        IMiddlewareBuilder New();
    }
}
