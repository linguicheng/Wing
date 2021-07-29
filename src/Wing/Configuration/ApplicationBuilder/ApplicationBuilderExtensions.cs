using Microsoft.AspNetCore.Builder;

namespace Wing.Configuration.ApplicationBuilder
{
    public static class ApplicationBuilderExtensions
    {
         public static IWingApplicationBuilder UseWing(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseHealthChecks("/health");
            return new WingApplicationBuilder(applicationBuilder);
        }
    }
}
