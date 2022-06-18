﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Wing.Configuration.ServiceBuilder;

namespace Wing
{
    public static class ServiceCollectionExtensions
    {
        private static WingServiceBuilder builder;

        public static IWingServiceBuilder AddWing(this IServiceCollection services)
        {
            services.AddTransient<IStartupFilter, WingStartupFilter>();
            builder = new WingServiceBuilder(services)
            {
                App = app => app.UseHealthChecks("/health")
            };
            return builder;
        }

        internal class WingStartupFilter : IStartupFilter
        {
            public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
            {
                return x =>
                {
                    builder.App(x);
                    next(x);
                };
            }
        }
    }
}
