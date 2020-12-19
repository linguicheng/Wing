namespace Wing.Redis
{
    public class Config
    {
        public string[] Sentinels { get; set; }

        public string ConnectionString { get; set; }
    }
}
