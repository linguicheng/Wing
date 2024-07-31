using FreeSql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Wing.Configuration.ServiceBuilder;
using Wing.Filters;
using Wing.Persistence.User;
using Wing.UI;

namespace Wing
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddWingUI(this IWingServiceBuilder wingBuilder, DataType dataType, string connectionString = "")
        {
            wingBuilder.AddPersistence(dataType, connectionString);
            wingBuilder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ApiExceptionFilter));
                options.Filters.Add(typeof(ApiResultFilter));
            });

            // 禁用默认行为
            wingBuilder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            wingBuilder.AppBuilder += new WingStartupFilter().Configure();
            AddDefaultUser(wingBuilder);
            return wingBuilder;
        }

        private static void AddDefaultUser(IWingServiceBuilder wingBuilder)
        {
            try
            {
                var account = App.Configuration["User:Account"];
                var password = App.Configuration["User:Password"];
                if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(password))
                {
                    throw new Exception("请设置默认登录账号和密码！");
                }

                var userService = wingBuilder.Services.BuildServiceProvider().GetService<UserService>();
                userService.Add(new User
                {
                    UserName = account,
                    CreatedAccount = account,
                    CreatedName = account,
                    UserAccount = account,
                    Password = password
                }).GetAwaiter().GetResult();
            }
            catch (AccountExistsException)
            {
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
