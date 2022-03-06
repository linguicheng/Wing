using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Wing.Configuration.ServiceBuilder;
using Wing.Dashboard.Filters;

namespace Wing.Dashboard
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddDashboard(this IWingServiceBuilder wingBuilder)
        {
            wingBuilder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ApiExceptionFilter));
                options.Filters.Add(typeof(ApiResultFilter));
            });
            //禁用默认行为
            wingBuilder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            wingBuilder.App += new WingStartupFilter().Configure();
            return wingBuilder;
        }
    }
}
