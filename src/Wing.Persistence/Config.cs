namespace Wing.Persistence
{
    public class Config
    {
        public static long GatewayTimeOut => string.IsNullOrWhiteSpace(App.Configuration["Home:Timeout:Gateway"]) ? 60 * 1000 : Convert.ToInt64(App.Configuration["Home:Timeout:Gateway"]);

        public static DateTime SearchTime => string.IsNullOrWhiteSpace(App.Configuration["Home:Timeout:SearchTime"]) ? DateTime.Now.AddMonths(-1) : Convert.ToDateTime(App.Configuration["Home:Timeout:SearchTime"]);
    }
}
