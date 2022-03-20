using Polly;

namespace Wing.Policy
{
    public class WingAsyncPolicy
    {
        public AsyncPolicy Policy { get; set; }

        public PolicyConfig Config { get; set; }
    }
}
