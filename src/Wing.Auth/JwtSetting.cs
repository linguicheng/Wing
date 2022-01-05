namespace Wing.Auth
{
    public class JwtSetting
    {
        public string Secret { get; set; }

        public string Iss { get; set; }

        public string Aud { get; set; }

        public uint Expire { get; set; } = 5;

        public string PolicyName { get; set; } = "Wing";
    }
}
