using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.Auth;
using System.Collections.Generic;
using Wing;
using Wing.Saga.Server;

namespace Sample.Saga.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddWing()
                    .AddJwt()
                    .AddPersistence()
                    .AddEventBus()
                    .AddSaga(serviceProvider =>
                    {
                        var token = $"Bearer {serviceProvider.GetRequiredService<IAuth>().GetToken()}";
                        return new SagaOptions
                        {
                            Headers = new Dictionary<string, string> { { "Authorization", token }}
                        };
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
