using System.Threading.Tasks;

namespace Wing.Policy
{
    public class WingPolicy
    {
        public Polly.Policy<Task> Policy { get; set; }

        public PolicyConfig Config { get; set; }
    }
}
