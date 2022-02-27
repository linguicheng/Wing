using Wing.Configuration.ServiceBuilder;

namespace Wing.Persistence
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddPersistence(this IWingServiceBuilder wingBuilder, string gateWayConnectionString = "name=ConnectionStrings:WingGateWay", string mqConnectionString = "name=ConnectionStrings:WingMQ")
        {
            DynamicRegister.RegisterContext(wingBuilder.Services, gateWayConnectionString, mqConnectionString);
            return wingBuilder;
        }
    }
}
