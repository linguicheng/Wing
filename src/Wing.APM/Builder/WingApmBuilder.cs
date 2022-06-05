using Wing.Configuration.ServiceBuilder;

namespace Wing.APM.Builder
{
    public class WingApmBuilder
    {
        public IWingServiceBuilder ServiceBuilder { get; }

        public WingApmBuilder(IWingServiceBuilder serviceBuilder)
        {
            ServiceBuilder = serviceBuilder;
        }
    }
}
