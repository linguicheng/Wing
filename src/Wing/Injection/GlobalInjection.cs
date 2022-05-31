﻿using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Wing.EventBus;
using Wing.Policy;

namespace Wing.Injection
{
    public class GlobalInjection
    {
        public static readonly Assembly[] Assemblies;

        static GlobalInjection()
        {
            Assemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        public static void CreateSubscribe(IEventBus eventBus)
        {
            var types = Assemblies
                .SelectMany(a =>
                    a.GetTypes()
                    .Where(t =>
                        t.GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubscribe<>))
                        .Any()))
                   .ToArray();
            var subscribeMethod = typeof(IEventBus).GetMethod("Subscribe");
            foreach (Type consumerType in types)
            {
                if (consumerType.IsInterface)
                {
                    continue;
                }

                var eventMessageType = consumerType.GetInterfaces()
                                       .Where(type =>
                                            type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(ISubscribe<>))
                                       .Select(type =>
                                           type.GetGenericArguments()
                                           .Single())
                                       .First();
                MethodInfo curMethod = subscribeMethod.MakeGenericMethod(eventMessageType, consumerType);
                curMethod.Invoke(eventBus, null);
            }
        }

        public static void Injection(IServiceCollection services)
        {
            foreach (var type in Assemblies.SelectMany(a =>
            a.GetTypes()
            .Where(t => !t.IsInterface && t.GetInterfaces().Any(u => u == typeof(IScoped)
                || u == typeof(ISingleton)
                || u == typeof(ITransient))))
            .ToArray())
            {
                var interfaces = type.GetInterfaces();
                if (interfaces.Count() == 1)
                {
                    if (interfaces[0] == typeof(IScoped))
                    {
                        services.AddScoped(type);
                    }
                    else if (interfaces[0] == typeof(ISingleton))
                    {
                        services.AddSingleton(type);
                    }
                    else if (interfaces[0] == typeof(ITransient))
                    {
                        services.AddTransient(type);
                    }
                }
                else if (interfaces.Count() == 2)
                {
                    if (interfaces[1] == typeof(IScoped))
                    {
                        services.AddScoped(interfaces[0], type);
                    }
                    else if (interfaces[1] == typeof(ISingleton))
                    {
                        services.AddSingleton(interfaces[0], type);
                    }
                    else if (interfaces[1] == typeof(ITransient))
                    {
                        services.AddTransient(interfaces[0], type);
                    }
                }
            }
        }
    }
}