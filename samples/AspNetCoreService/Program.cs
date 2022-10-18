using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Wing.Configuration.HostBuilder;
using Wing.Consul;

namespace Sample.AspNetCoreService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>();
             }).AddWing(builder=> builder.AddConsul());
    }
}
